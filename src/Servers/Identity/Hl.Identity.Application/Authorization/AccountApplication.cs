using Hl.Identity.IApplication.Authorization;
using Hl.Identity.IApplication.Authorization.Dtos;
using Surging.Core.CPlatform.Ioc;
using Surging.Core.ProxyGenerator;
using System;
using System.Threading.Tasks;

namespace Hl.Identity.Application.Authorization
{
    [ModuleName("v1identity",Version = "v1")]
    public class AccountApplication : ProxyServiceBase, IAccountApplication
    {
        public Task<PayloadOutput> Login(LoginInput input)
        {
            throw new NotImplementedException();
        }
    }
}
