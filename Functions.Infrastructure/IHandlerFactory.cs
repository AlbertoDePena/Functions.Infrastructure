namespace Numaka.Functions.Infrastructure
{
    /// <summary>
    /// Handler factory interface
    /// </summary>
    public interface IHandlerFactory 
    {
        /// <summary>
        /// Create a HTTP handler
        /// </summary>
        /// <typeparam name="THandler"></typeparam>
        THandler Create<THandler>() where THandler : HttpMiddleware;
    }
}