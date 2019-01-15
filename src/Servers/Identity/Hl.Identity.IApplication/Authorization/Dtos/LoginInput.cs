using System;
using System.Collections.Generic;
using System.Text;

namespace Hl.Identity.IApplication.Authorization.Dtos
{
    public class LoginInput
    {
        public string UserName { get; set; }

        public string Password { get; set; }
    }
}
