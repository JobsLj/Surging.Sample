using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Surging.Core.CPlatform.Routing.Template
{
    public class RoutePatternParser
    {
        private const string routeRepalceReg = "/{2,}";
        public static string Parse(string routeTemplet, string service, string method)
        {
            var sb = new StringBuilder();
            var parameters = routeTemplet.Split(@"/");
            foreach (var parameter in parameters)
            {
                var param = GetParameters(parameter).FirstOrDefault();
                if (param == null)
                {
                    sb.Append(parameter);
                }
                else if (service.EndsWith(param))
                {
                    sb.Append(service.Substring(1, service.Length - param.Length - 1));
                }
                else if (param == "Method")
                {
                    sb.Append(method);
                }

                sb.Append("/");

            }

            var result = sb.Append(method).ToString().TrimStart('/').ToLower();


            return Regex.Replace(result, routeRepalceReg, "/");
        }

        public static string Parse(string routeTemplet, string service)
        {
            var sb = new StringBuilder();
            var parameters = routeTemplet.Split(@"/");
            foreach (var parameter in parameters)
            {
                var param = GetParameters(parameter).FirstOrDefault();
                if (param == null)
                {
                    sb.Append(parameter);
                }
                else if (service.EndsWith(param))
                {
                    sb.Append(service.Substring(1, service.Length - param.Length - 1));
                }
                sb.Append("/");
            }

            var result = sb.ToString().TrimStart('/').TrimEnd('/').ToLower();
            return Regex.Replace(result, routeRepalceReg, "/");
        }

        private static List<string> GetParameters(string text)
        {
            var matchVale = new List<string>();
            string Reg = @"(?<={)[^{}]*(?=})";
            string key = string.Empty;
            foreach (Match m in Regex.Matches(text, Reg))
            {
                matchVale.Add(m.Value);
            }
            return matchVale;
        }
    }
}