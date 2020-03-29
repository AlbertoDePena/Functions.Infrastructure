namespace Numaka.FSharp.Functions.Infrastructure

open System
open System.Net.Http
open System.Security.Claims
open Microsoft.Extensions.Logging

type MiddlewarePipelineException(message : string) =
    inherit Exception(message)

type ValidateBearerTokenException(message : string) =
    inherit Exception(message)

type HttpFunctionContext = {
    Logger : ILogger
    Request : HttpRequestMessage
    GetClaimsPrincipal : HttpFunctionContext -> Async<ClaimsPrincipal option> } 

type GetClaimsPrincipal = HttpFunctionContext -> Async<ClaimsPrincipal option>

type HttpHandler = HttpFunctionContext -> Async<HttpResponseMessage option>

type ErrorHandler = HttpFunctionContext -> exn -> HttpResponseMessage

type ExecutePipeline = ErrorHandler -> HttpHandler -> HttpFunctionContext -> Async<HttpResponseMessage>

[<RequireQualifiedAccess>]
module HttpFunctionContext =

    /// **Description**
    /// 
    /// Bootstrap HttpFunctionContext without GetClaimsPrincipal function
    /// 
    /// Note: the ClaimsPrincipal will not be available
    let bootstrap logger request = {
        Logger = logger
        Request = request
        GetClaimsPrincipal = fun _ -> Async.singleton None }

    // **Description**
    /// 
    /// Bootstrap HttpFunctionContext with GetClaimsPrincipal function
    /// 
    /// Note: the ClaimsPrincipal will be available
    let bootstrapWithSecurity logger request getClaimsPrincipal = {
        Logger = logger
        Request = request
        GetClaimsPrincipal = getClaimsPrincipal }
