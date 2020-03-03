using Functions.Infrastructure.Contracts;
using Functions.Infrastructure.Exceptions;
using Functions.Infrastructure.Middlewares;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Functions.Infrastructure
{
    public class MiddlewarePipeline : IMiddlewarePipeline
    {
        private readonly List<HttpMiddleware> _pipeline;

        public MiddlewarePipeline()
        {
            _pipeline = new List<HttpMiddleware>();
        }

        public void Register(HttpMiddleware middleware)
        {
            if (_pipeline.Count > 0)
            {
                _pipeline[_pipeline.Count - 1].Next = middleware;
            }

            _pipeline.Add(middleware);
        }

        public async Task<HttpResponseMessage> ExecuteAsync(IHttpFunctionContext context)
        {
            try
            {
                if (_pipeline.Count > 0)
                {
                    await _pipeline[0].InvokeAsync(context);

                    if (context.Response != null)
                    {
                        return context.Response;
                    }
                }

                throw new MiddlewarePipelineException();
            }
            catch (Exception e)
            {
                context.Logger.LogError(e, e.Message);

                return context.Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }
    }
}
