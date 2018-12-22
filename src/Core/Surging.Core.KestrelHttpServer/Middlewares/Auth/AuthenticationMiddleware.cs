using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Surging.Core.CPlatform;
using Surging.Core.CPlatform.Exceptions;
using Surging.Core.CPlatform.Messages;
using Surging.Core.CPlatform.Routing;
using Surging.Core.CPlatform.Utilities;
using Surging.Core.KestrelHttpServer.Extensions;
using System;
using System.Collections.Generic;
using System.Security.Authentication;
using System.Threading.Tasks;

namespace Surging.Core.KestrelHttpServer.Middlewares
{
    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthenticationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await OnAuthorize(context);
                await _next(context);
            }
            catch (AuthenticationException ex)
            {
                var noAuthResponseContent = new HttpResultMessage()
                { IsSucceed = false, Message = ex.Message, StatusCode = StatusCode.UnAuthentication };
                await context.Response.WriteAsync(JsonConvert.SerializeObject(noAuthResponseContent));
            }
            catch (CPlatformException ex)
            {
                var resultMessage = new HttpResultMessage()
                { IsSucceed = false, Message = ex.Message, StatusCode = ex.ExceptionCode };
                await context.Response.WriteAsync(JsonConvert.SerializeObject(resultMessage));
            }
            catch (Exception ex)
            {
                var resultMessage = new HttpResultMessage()
                { IsSucceed = false, Message = ex.StackTrace, StatusCode = StatusCode.RequestError };
                await context.Response.WriteAsync(JsonConvert.SerializeObject(resultMessage));
            }
        }

        private async Task OnAuthorize(HttpContext context)
        {
            try
            {
                var serviceRouteProvider = ServiceLocator.GetService<IServiceRouteProvider>();
                var routPath = GetRoutePath(context.Request.Path.ToString());
                var commandInfo = await serviceRouteProvider.GetRouteByPath(routPath);
                if (commandInfo == null)
                {
                    throw new CPlatformException($"系统中不存在{routPath}的路由信息");
                }

                if (!commandInfo.ServiceDescriptor.EnableAuthorization())
                {
                    return;
                }

                var token = context.Request.GetTokenFromHeader();
                var authorizationServerProvider = ServiceLocator.GetService<IAuthorizationServerProvider>();
                var isSuccess = await authorizationServerProvider.ValidateClientAuthentication(token);

                if (isSuccess)
                {
                    if (!string.IsNullOrEmpty(AppConfig.SwaggerOptions.Authorization.AuthorizationRoutePath))
                    {
                        var apiPath = context.Request.Path.ToString().TrimStart('/');
                        await authorizationServerProvider.Authorize(apiPath, new Dictionary<string, object>()
                        {
                            {"apiPath", apiPath}
                        });
                    }
                }
                else
                {
                    throw new AuthenticationException("不合法的身份凭证");
                }
            }
            catch (Exception ex)
            {
                throw new AuthenticationException(ex.Message);
            }
        }

        private string GetRoutePath(string path)
        {
            string routePath = "";
            var urlSpan = path.AsSpan();

            var len = urlSpan.IndexOf("?");
            if (len == -1)
                routePath = urlSpan.TrimStart("/").ToString().ToLower();
            else

                routePath = urlSpan.Slice(0, len).TrimStart("/").ToString().ToLower();
            return routePath;
        }
    }

    public static class AuthenticationMiddlewareExtensions
    {
        public static IApplicationBuilder UseAuthentication(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AuthenticationMiddleware>();
        }
    }
}