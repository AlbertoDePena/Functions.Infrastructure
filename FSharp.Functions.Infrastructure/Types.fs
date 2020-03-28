namespace Numaka.FSharp.Functions.Infrastructure

open System.Net.Http
open System.Security.Claims
open Microsoft.Extensions.Logging

type BearerToken = string

type ValidateBearerToken = BearerToken -> Async<ClaimsPrincipal>

type HttpFunctionContext = {
    Logger : ILogger
    Request : HttpRequestMessage
    Response : HttpResponseMessage option
    ClaimsPrincipal : ClaimsPrincipal option 
} 

type HttpHandler = HttpFunctionContext -> Async<HttpFunctionContext>

type ExecutePipeline = HttpHandler list -> HttpFunctionContext -> Async<HttpResponseMessage>

[<RequireQualifiedAccess>]
module HttpFunctionContext =

    let bootstrap logger request = {
        Logger = logger
        Request = request
        Response = None
        ClaimsPrincipal = None
    }
