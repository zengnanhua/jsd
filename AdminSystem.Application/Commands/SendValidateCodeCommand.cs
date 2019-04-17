using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdminSystem.Application.Commands
{
    /// <summary>
    /// 发送验证码
    /// </summary>
    public class SendValidateCodeCommand: IRequest<ResultData<string>>
    {
        public string Mobile { get; private set; }
        public  SendValidateCodeCommand(string mobile)
        {
            this.Mobile = mobile;
        }
    }
}
