using Surging.Core.Caching;
using Surging.Core.CPlatform;
using Surging.Core.CPlatform.Cache;
using Surging.Core.CPlatform.Routing;
using Surging.Core.ProxyGenerator;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Surging.Core.KestrelHttpServer.Middlewares
{
    public class AuthorizationServerProvider : IAuthorizationServerProvider
    {
        private readonly IServiceProxyProvider _serviceProxyProvider;//务代理
        private readonly IServiceRouteProvider _serviceRouteProvider;//用务路由
        private readonly ICacheProvider _cacheProvider;//缓存

        /// <summary>
        /// 安全验证服务提供类构造
        /// </summary>
        /// <param name="serviceProxyProvider">服务代理</param>
        /// <param name="serviceRouteProvider">用务路由</param>
        public AuthorizationServerProvider(IServiceProxyProvider serviceProxyProvider
           , IServiceRouteProvider serviceRouteProvider)
        {
            _serviceProxyProvider = serviceProxyProvider;
            _serviceRouteProvider = serviceRouteProvider;
            _cacheProvider = CacheContainer.GetService<ICacheProvider>(AppConfig.SwaggerOptions.Authorization.CacheMode);
        }

        public async Task<bool> Authorize(string apiPath, Dictionary<string, object> parameters)
        {
            var route = await _serviceRouteProvider.GetRouteByPath(apiPath);

            //if (AppConfig.WhiteList.Contains(apiPath))
            //{
            //    return true;
            //}

            if (route.ServiceDescriptor.AllowPermission())
            {
                return true;
            }

            return await _serviceProxyProvider.Invoke<bool>(parameters, AppConfig.SwaggerOptions.Authorization.AuthorizationRoutePath,
                AppConfig.SwaggerOptions.Authorization.AuthorizationServiceKey ?? "");
        }

        /// <summary>
        /// 得到TOKEN内容，token包含三部份（头/内容/验证密文）
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public Task<string> GetPayloadString(string token)
        {
            string result = null;
            var jwtToken = token.Split('.');
            if (jwtToken.Length == 3)
            {
                result = Encoding.UTF8.GetString(Convert.FromBase64String(jwtToken[1]));
            }
            return Task.FromResult(result);
        }

        /// <summary>
        /// 验证客户端权限
        /// </summary>
        /// <param name="token">客户端TOKEN值</param>
        /// <returns></returns>
        public async Task<bool> ValidateClientAuthentication(string token)
        {
            bool isSuccess = false;
            var jwtToken = token.Split('.');
            if (jwtToken.Length == 3)
            {
                isSuccess = await _cacheProvider.GetAsync<string>(jwtToken[1]) == token;
            }
            return isSuccess;
        }

        /// <summary>
        /// 将字符串转换为BASE64编码的字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private string ConverBase64String(string str)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(str));
        }

        /// <summary>
        /// SHA256加密（根据KEY计算指定串的HASH值）
        /// </summary>
        /// <param name="message">待加密的串</param>
        /// <param name="secret">KYE值</param>
        /// <returns></returns>
        private string HMACSHA256(string message, string secret)
        {
            secret = secret ?? "";
            byte[] keyByte = Encoding.UTF8.GetBytes(secret);
            byte[] messageBytes = Encoding.UTF8.GetBytes(message);
            using (var hmacsha256 = new HMACSHA256(keyByte))
            {
                byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);
                return Convert.ToBase64String(hashmessage);
            }
        }
    }
}