namespace Numaka.FSharp.Functions.FunctionApp

open Numaka.FSharp.Functions.Infrastructure
open Microsoft.Azure.WebJobs
open Microsoft.Azure.WebJobs.Extensions.Http
open Microsoft.Extensions.Logging
open System.Net.Http
open System.Net
open System.Security.Claims

module Program =

    let errorHandler : ErrorHandler =
        fun context ex ->
            context.Logger.LogError(ex.Message)

            if ex :? ValidateBearerTokenException 
            then context.Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message)
            else context.Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex)

    let validateBearerToken : ValidateBearerToken =
        fun token ->
            let claims = [Claim(ClaimTypes.Name, "Test User")]                
            let identity = ClaimsIdentity(claims, "Bearer")

            ClaimsPrincipal(identity) |> Async.singleton

    [<FunctionName("HelloWorld")>]
    let helloWorld ([<HttpTrigger(AuthorizationLevel.Anonymous, "get", "options")>] request : HttpRequestMessage) (logger : ILogger) = 
        async {
            
            let helloWorldHandler : HttpHandler =
                fun context -> 
                    async {
                        context.Logger.LogInformation("Handling HelloWorld request...")

                        let response = context.Request.CreateResponse(HttpStatusCode.OK, "Hello World!")

                        return { context with Response = Some response }
                    }

            return!
                HttpFunctionContext.bootstrap logger request
                |> MiddlewarePipeline.execute errorHandler [ Middleware.cors; helloWorldHandler ]
        } |> Async.StartAsTask

    [<FunctionName("HelloLaz")>]
    let helloLaz ([<HttpTrigger(AuthorizationLevel.Anonymous, "get", "options")>] request : HttpRequestMessage) (logger : ILogger) = 
        async {
            
            let helloLazHandler : HttpHandler =
                fun context -> 
                    async {
                        context.Logger.LogInformation("Handling HelloLaz request...")

                        let response = context.Request.CreateResponse(HttpStatusCode.OK, "Hello Laz!")

                        return { context with Response = Some response }
                    }

            return!
                HttpFunctionContext.bootstrap logger request
                |> MiddlewarePipeline.execute errorHandler [ Middleware.cors; helloLazHandler ]
        } |> Async.StartAsTask    

    [<FunctionName("CurrentUser")>]
    let currentUser ([<HttpTrigger(AuthorizationLevel.Anonymous, "get", "options")>] request : HttpRequestMessage) (logger : ILogger) = 
        async {
            
            let currentUserHandler : HttpHandler =
                fun context -> 
                    async {
                        context.Logger.LogInformation("Handling CurrentUser request...")

                        let user = 
                            context.ClaimsPrincipal 
                            |> Option.map (fun principal -> principal.Identity.Name)
                            |> Option.defaultWith (fun _ -> invalidOp "ClaimsPrincipal not available")

                        let response = context.Request.CreateResponse(HttpStatusCode.OK, sprintf "The current user is: %s" user)

                        return { context with Response = Some response }
                    }

            let pipeline = [ Middleware.cors; Middleware.security validateBearerToken; currentUserHandler ]

            return!
                HttpFunctionContext.bootstrap logger request
                |> MiddlewarePipeline.execute errorHandler pipeline
        } |> Async.StartAsTask      