namespace Numaka.FSharp.Functions.Infrastructure

open System
open System.Net.Http
open Microsoft.Extensions.Logging
open System.Net

[<RequireQualifiedAccess>]        
module Middleware =

    let private getCorsResponse (request : HttpRequestMessage) =
        if String.Equals(request.Method.Method, "OPTIONS", StringComparison.OrdinalIgnoreCase) then
            let response = request.CreateResponse(HttpStatusCode.OK, "Hello from the other side")

            if request.Headers.Contains("Origin") then
                response.Headers.Add("Access-Control-Allow-Credentials", "true")
                response.Headers.Add("Access-Control-Allow-Origin", "*")
                response.Headers.Add("Access-Control-Allow-Methods", "GET, HEAD, OPTIONS, PUT, PATCH, POST, DELETE")
                response.Headers.Add("Access-Control-Allow-Headers", "Origin, X-Requested-With, Content-Type, Accept")
               
            Some response
        else None

    /// **Description**
    /// 
    /// CORS: handle HTTP OPTIONS requests by setting appropriate access headers
    let cors : HttpHandler =
        fun context ->
            getCorsResponse context.Request
            |> Option.map (fun response -> { context with Response = Some response })
            |> Option.defaultValue context
            |> Async.singleton

    /// **Description**
    /// 
    /// Security: assigns a ClaimsPrincipal to the HttpFunctionContext using ValidateBearerToken
    let security (validateBearerToken : ValidateBearerToken) : HttpHandler =
        fun context ->
            async {
                let bearerToken = 
                    context.Request.Headers 
                    |> Seq.tryFind (fun q -> q.Key = "Authorization")
                    |> Option.map (fun q -> if Seq.isEmpty q.Value then String.Empty else q.Value |> Seq.head)
                    |> Option.map (fun h -> h.Substring("Bearer ".Length).Trim())
                    |> Option.defaultWith (fun _ -> invalidOp "Bearer token is required")

                let! claimsPrincipal = validateBearerToken bearerToken

                return { context with ClaimsPrincipal = Some claimsPrincipal }
            }
 
[<RequireQualifiedAccess>]
module MiddlewarePipeline =

    /// **Description**
    /// 
    /// Execute each HttpHandler in order until the HttpFunctionContext.Response is Some HttpResponseMessage.
    /// Return InternalServerError response when HttpFunctionContext.Response is None
    /// 
    /// Order of HTTP handlers matters
    /// 
    /// **Parameters**
    /// 
    /// errorHandler: transform exception to HttpResponseMessage
    /// 
    /// pipeline: HttpHandler list
    /// 
    /// context: HttpFunctionContext
    let execute : ExecutePipeline = 
        fun errorHandler pipeline context ->
            async {
                try
                    let enrichWithCorsOrigin (response : HttpResponseMessage) =
                        response.Headers.Add("Access-Control-Allow-Origin", "*"); response

                    let errorReponse = 
                        context.Request.CreateErrorResponse(
                            HttpStatusCode.InternalServerError, "Invoked HTTP middleware did not yield a response")

                    let rec execute pipeline context =
                        async {
                            match pipeline with
                            | [] -> return context
                            | httpFunc::tail ->
                                let! context = httpFunc context
                                match context.Response with
                                | Some _ -> return context
                                | None -> return! execute tail context 
                        }

                    let! context = execute pipeline context

                    match context.Response with
                    | None -> return errorReponse
                    | Some response -> return enrichWithCorsOrigin response
                with
                | ex -> return errorHandler context ex
            }