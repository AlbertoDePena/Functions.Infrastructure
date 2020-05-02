using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace Numaka.Functions.Infrastructure
{
    /// <inheritdoc />
    public class MiddlewarePipeline : IMiddlewarePipeline
    {
        private readonly List<HttpMiddleware> _pipeline;

        /// <summary>
        /// Constructor
        /// </summary>
        public MiddlewarePipeline() => _pipeline = new List<HttpMiddleware>();

        /// <inheritdoc />
        public void Register(HttpMiddleware middleware)
        {
            if (_pipeline.Count > 0)
            {
                _pipeline[_pipeline.Count - 1].Next = middleware;
            }

            _pipeline.Add(middleware);
        }

        /// <inheritdoc />
        public async Task<IActionResult> ExecuteAsync(IHttpFunctionContext context)
        {
            try
            {
                if (_pipeline.Count > 0)
                {
                    await _pipeline[0].InvokeAsync(context);

                    if (context.ActionResult != null)
                    {
                        return context.ActionResult;
                    }
                }

                throw new MiddlewarePipelineException();
            }
            catch (Exception e)
            {
                context.Logger.LogError(e, e.Message);

                return new InternalServerErrorResult();
            }
        }
    }
}
