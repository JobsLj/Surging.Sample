namespace Surging.Core.CPlatform.Filters.Implementation
{
    public class AuthorizationAttribute : AuthorizationFilterAttribute
    {
        public AuthorizationType AuthType { get; set; }
    }
}