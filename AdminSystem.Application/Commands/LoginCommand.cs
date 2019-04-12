using MediatR;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace AdminSystem.Application.Commands
{
    /// <summary>
    /// 用户订单Command
    /// </summary>
    public class LoginCommand:IRequest<ResultData<string>>
    {
        [DataMember]
        public string Mobile { get; private set; }
        [DataMember]
        public string ValidateCode { get; private set; }

        public LoginCommand(string Mobile, string ValidateCode)
        {
            this.Mobile = !string.IsNullOrWhiteSpace(Mobile)?Mobile : throw new InvalidOperationException("mobile 不能为空");
            this.ValidateCode = !string.IsNullOrWhiteSpace(ValidateCode) ? ValidateCode : throw new InvalidOperationException("ValidateCode 不能为空"); ;
        }
    }
}
