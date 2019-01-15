using System;
using System.Collections.Generic;
using System.Text;

namespace Hl.Identity.IApplication.Authorization.Dtos
{
    public class PayloadOutput
    {
        public string UserId { get; set; }

        public string UserName { get; set; }


        // :todo 其他用户字段,但是请不要把用户密码等敏感的信息存放在该处
    }
}
