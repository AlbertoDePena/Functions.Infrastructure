namespace Numaka.Functions.Infrastructure
{
    /// <summary>
    /// HTTP middleware factory interface
    /// </summary>
    public interface IHttpMiddlewareFactory 
    {
        /// <summary>
        /// Create a HTTP middleware
        /// </summary>
        /// <typeparam name="THttpMiddleware"></typeparam>
        THttpMiddleware Create<THttpMiddleware>() where THttpMiddleware : HttpMiddleware;
    }
}