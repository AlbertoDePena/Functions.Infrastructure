namespace Numaka.FSharp.Functions.Infrastructure

open System
open System.Net.Http
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
            |> Async.singleton

[<RequireQualifiedAccess>]
module MiddlewarePipeline =

    /// **Description**
    /// 
    /// Execute HttpHandler resulting in a HttpResponseMessage.
    /// Throws MiddlewarePipelineException when HttpHandler HttpResponseMessage is None
    /// 
    /// **Parameters**
    /// 
    /// errorHandler: ErrorHandler
    /// 
    /// handler: HttpHandler
    /// 
    /// context: HttpFunctionContext
    let execute : ExecutePipeline = 
        fun errorHandler handler context ->
            async {
                try
                    let enrichWithCorsOrigin (response : HttpResponseMessage) =
                        response.Headers.Add("Access-Control-Allow-Origin", "*"); response

                    let execute mapper context =
                        async {
                            let! handlerResponse = handler context
                            match handlerResponse with
                            | Some response -> return mapper response
                            | None -> return raise (MiddlewarePipelineException("HTTP handler did not yield a response"))
                        }

                    let! corsResponse = Middleware.cors context

                    match corsResponse with
                    | Some response -> return response
                    | None -> return! execute enrichWithCorsOrigin context
                with
                | ex -> return errorHandler context ex
            }