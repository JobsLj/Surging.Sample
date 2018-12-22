using Newtonsoft.Json;
using Surging.Core.Caching;
using Surging.Core.CPlatform;
using Surging.Core.CPlatform.Cache;
using Surging.Core.CPlatform.Routing;
using Surging.Core.ProxyGenerator;
using Surging.Core.System.SystemType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Surging.Core.ApiGateWay.OAuth
{
    /// <summary>
    /// 安全验证服务提供类
    /// </summary>
    public class AuthorizationServerProvider : IAuthorizationServerProvider
    {
        private readonly IServiceProxyProvider _serviceProxyProvider;//务代理
        private readonly IServiceRouteProvider _serviceRouteProvider;//用务路由
        private readonly CPlatformContainer _serviceProvider;//服务
        private readonly ICacheProvider _cacheProvider;//缓存

        /// <summary>
        /// 安全验证服务提供类构造
        /// </summary>
        /// <param name="configInfo">配置信息</param>
        /// <param name="serviceProxyProvider">服务代理</param>
        /// <param name="serviceRouteProvider">用务路由</param>
        /// <param name="serviceProvider">用务</param>
        public AuthorizationServerProvider(ConfigInfo configInfo, IServiceProxyProvider serviceProxyProvider
           , IServiceRouteProvider serviceRouteProvider
            , CPlatformContainer serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _serviceProxyProvider = serviceProxyProvider;
            _serviceRouteProvider = serviceRouteProvider;
            _cacheProvider = CacheContainer.GetService<ICacheProvider>(AppConfig.CacheMode);
        }

        /// <summary>
        /// 生成TOKEN凭证
        /// </summary>
        /// <param name="parameters">字典参数</param>
        /// <param name="accessSystemType"></param>
        /// <returns></returns>
        public async Task<string> GenerateTokenCredential(Dictionary<string, object> parameters, AccessSystemType accessSystemType = AccessSystemType.Inner)
        {
            string result = null;

            var tokenEndpointPath = accessSystemType == AccessSystemType.Inner ? AppConfig.AuthenticationRoutePath : AppConfig.ThirdPartyAuthenticationRoutePath;

            var payload = await _serviceProxyProvider.Invoke<object>(parameters, tokenEndpointPath, AppConfig.AuthorizationServiceKey);
            if (payload != null && !payload.Equals("null"))
            {
                var jwtHeader = JsonConvert.SerializeObject(new JWTSecureDataHeader() { TimeStamp = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") });
                var base64Payload = ConverBase64String(JsonConvert.SerializeObject(payload));
                var encodedString = $"{ConverBase64String(jwtHeader)}.{base64Payload}";
                var route = await _serviceRouteProvider.GetRouteByPath(tokenEndpointPath);
                var signature = HMACSHA256(encodedString, route.ServiceDescriptor.Token);
                result = $"{encodedString}.{signature}";
                _cacheProvider.Add(base64Payload, result, AppConfig.AccessTokenExpireTimeSpan);
            }
            return result;
        }

        public async Task<bool> Authorize(string apiPath, Dictionary<string, object> parameters)
        {
            var route = await _serviceRouteProvider.GetRouteByPath(apiPath);

            if (AppConfig.WhiteList.Contains(apiPath))
            {
                return true;
            }

            if (route.ServiceDescriptor.AllowPermission())
            {
                return true;
            }

            return await _serviceProxyProvider.Invoke<bool>(parameters, AppConfig.AuthorizationRoutePath,
                AppConfig.AuthorizationServiceKey);
        }

        /// <summary>
        /// 得到TOKEN内容，token包含三部份（头/内容/验证密文）
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public string GetPayloadString(string token)
        {
            string result = null;
            var jwtToken = token.Split('.');
            if (jwtToken.Length == 3)
            {
                result = Encoding.UTF8.GetString(Convert.FromBase64String(jwtToken[1]));
            }
            return result;
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