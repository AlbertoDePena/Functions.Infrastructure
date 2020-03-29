namespace Numaka.FSharp.Functions.FunctionApp

open Numaka.FSharp.Functions.Infrastructure
open Microsoft.Azure.WebJobs
open Microsoft.Azure.WebJobs.Extensions.Http
open Microsoft.Extensions.Logging
open System.Net.Http
open System.Net
open System
open System.Security.Claims

module Program =

    let errorHandler : ErrorHandler =
        fun context ex ->
            context.Logger.LogError(ex.Message)

            if ex :? ValidateBearerTokenException 
            then context.Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message)
            else context.Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex)

    let getClaimsPrincipal : GetClaimsPrincipal =
        fun context ->
            async {
                let bearerToken = 
                    context.Request.Headers 
                    |> Seq.tryFind (fun q -> q.Key = "Authorization")
                    |> Option.map (fun q -> if Seq.isEmpty q.Value then String.Empty else q.Value |> Seq.head)
                    |> Option.map (fun h -> h.Substring("Bearer ".Length).Trim())
                    |> Option.defaultWith (fun _ -> raise (ValidateBearerTokenException("Bearer token is required")))

                //TODO: validate bearerToken
                context.Logger.LogInformation(sprintf "Bearer Token: %s" bearerToken)

                let claims = [Claim(ClaimTypes.Name, "Test User")]                
                let identity = ClaimsIdentity(claims, "Bearer")

                return identity |> ClaimsPrincipal |> Some 
            }

    [<FunctionName("HelloWorld")>]
    let helloWorld ([<HttpTrigger(AuthorizationLevel.Anonymous, "get", "options")>] request : HttpRequestMessage) (logger : ILogger) = 
        async {
            
            let helloWorldHandler : HttpHandler =
                fun context -> 
                    async {
                        context.Logger.LogInformation("Handling HelloWorld request...")

                        let response = context.Request.CreateResponse(HttpStatusCode.OK, "Hello World!")

                        return Some response
                    }

            return!
                HttpFunctionContext.bootstrap logger request
                |> MiddlewarePipeline.execute errorHandler helloWorldHandler
        } |> Async.StartAsTask

    [<FunctionName("HelloLaz")>]
    let helloLaz ([<HttpTrigger(AuthorizationLevel.Anonymous, "get", "options")>] request : HttpRequestMessage) (logger : ILogger) = 
        async {
            
            let helloLazHandler : HttpHandler =
                fun context -> 
                    async {
                        context.Logger.LogInformation("Handling HelloLaz request...")

                        let response = context.Request.CreateResponse(HttpStatusCode.OK, "Hello Laz!")

                        return Some response
                    }

            return!
                HttpFunctionContext.bootstrap logger request
                |> MiddlewarePipeline.execute errorHandler helloLazHandler
        } |> Async.StartAsTask    

    [<FunctionName("CurrentUser")>]
    let currentUser ([<HttpTrigger(AuthorizationLevel.Anonymous, "get", "options")>] request : HttpRequestMessage) (logger : ILogger) = 
        async {
            
            let currentUserHandler : HttpHandler =
                fun context -> 
                    async {
                        context.Logger.LogInformation("Handling CurrentUser request...")

                        let! claimsPrincipal = context.GetClaimsPrincipal context

                        let user = 
                            claimsPrincipal
                            |> Option.map (fun principal -> principal.Identity.Name)
                            |> Option.defaultWith (fun _ -> invalidOp "ClaimsPrincipal not available")

                        let response = context.Request.CreateResponse(HttpStatusCode.OK, sprintf "The current user is: %s" user)

                        return Some response
                    }

            return!
                HttpFunctionContext.bootstrapWithSecurity logger request getClaimsPrincipal
                |> MiddlewarePipeline.execute errorHandler currentUserHandler
        } |> Async.StartAsTask      