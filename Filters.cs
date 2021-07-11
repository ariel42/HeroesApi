using log4net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HeroesApi.Filters
{
    public class ReadableBodyStreamFilter : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            context.HttpContext.Request.EnableBuffering();
        }
    }

    public class RequestLoggerActionFilter : IAsyncActionFilter
    {
        private readonly ILog logger;
        public RequestLoggerActionFilter(ILoggerManager loggerManager)
        {
            logger = loggerManager.Logger;
        }

        public async Task OnActionExecutionAsync(
            ActionExecutingContext context,
            ActionExecutionDelegate next)
        {
            var request = context.HttpContext.Request;
            string body = "";

            if (request.Method != "GET" && request.Path != "/api/users/login")
            {
                request.Body.Seek(0, SeekOrigin.Begin);
                using (StreamReader stream = new StreamReader(request.Body))
                {
                    body = await stream.ReadToEndAsync();
                }
            }

            logger.Info(context.HttpContext.TraceIdentifier + "\t" + request.Method + "\t" + request.Path + "\t" + body);

            var result = await next();
            if (result.Exception != null) 
            {
                logger.Error(context.HttpContext.TraceIdentifier, result.Exception);
            }
        }
    }
}
