
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Expression;

namespace WebApplication1.Middleware
{
    public class ErrorHandlingMiddleware : IMiddleware
    {
        private readonly ILogger<ErrorHandlingMiddleware> _loger;

        public ErrorHandlingMiddleware(ILogger<ErrorHandlingMiddleware> loger)
        {
            _loger = loger;
  
        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next.Invoke(context);
            }
            catch(NotFoundException nfe)
            {
                context.Response.StatusCode = 404;
                await context.Response.WriteAsync(nfe.Message);
            }
            catch (BadRequestException bre)
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync(bre.Message);
            }
            catch (ForbidException e)
            {
                context.Response.StatusCode = 403;
                await context.Response.WriteAsync(e.Message);
            }
            catch (Exception e )
            {
                _loger.LogError(e, e.Message);
                context.Response.StatusCode = 500;
                await context.Response.WriteAsync("something went wrong");
            }
           
        }
    }
}
