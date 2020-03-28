namespace Numaka.FSharp.Functions.Infrastructure

open System
open System.Net.Http
open System.Security.Claims
open Microsoft.Extensions.Logging

type MiddlewarePipelineException(message : string) =
    inherit Exception(message)

type ValidateBearerTokenException(message : string) =
    inherit Exception(message)

type BearerToken = string

type ValidateBearerToken = BearerToken -> Async<ClaimsPrincipal>

type HttpFunctionContext = {
    Logger : ILogger
    Request : HttpRequestMessage
    Response : HttpResponseMessage option
    ClaimsPrincipal : ClaimsPrincipal option 
} 

type HttpHandler = HttpFunctionContext -> Async<HttpFunctionContext>

type ErrorHandler = HttpFunctionContext -> exn -> HttpResponseMessage

type ExecutePipeline = ErrorHandler -> HttpHandler list -> HttpFunctionContext -> Async<HttpResponseMessage>

[<RequireQualifiedAccess>]
module HttpFunctionContext =

    let bootstrap logger request = {
        Logger = logger
        Request = request
        Response = None
        ClaimsPrincipal = None
    }
