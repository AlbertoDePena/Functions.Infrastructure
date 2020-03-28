namespace Numaka.FSharp.Functions.FunctionApp

open Numaka.FSharp.Functions.Infrastructure
open Microsoft.Azure.WebJobs
open Microsoft.Azure.WebJobs.Extensions.Http
open Microsoft.Extensions.Logging
open System
open System.Net.Http
open System.Net
open System.Security.Claims

module Program =

    [<FunctionName("test")>]
    let run ([<HttpTrigger(AuthorizationLevel.Anonymous, "get", "options")>] request : HttpRequestMessage) (logger : ILogger) = 
        async {
            
            let handleError : HandleError =
                fun context ex ->
                    context.Logger.LogError(ex.Message)
                    context.Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex)

            let testHandler : HttpHandler =
                fun context -> 
                    async {
                        let user = 
                            context.ClaimsPrincipal 
                            |> Option.map (fun principal -> principal.Identity.Name)
                            |> Option.defaultWith (fun _ -> invalidOp "ClaimsPrincipal not available")

                        let response = context.Request.CreateResponse(HttpStatusCode.OK, sprintf "Hello %s" user)

                        return { context with Response = Some response }
                    }

            let validateBearerToken : ValidateBearerToken =
                fun token ->
                    async {
                        let claims = [Claim(ClaimTypes.Name, "Test User")]
                        
                        let identity = ClaimsIdentity(claims, "Bearer")

                        return ClaimsPrincipal(identity)
                    }

            let pipeline = [ Middleware.cors; Middleware.security validateBearerToken; testHandler ]

            let! response =
                HttpFunctionContext.bootstrap logger request
                |> MiddlewarePipeline.execute handleError pipeline

            return response
        } |> Async.StartAsTask