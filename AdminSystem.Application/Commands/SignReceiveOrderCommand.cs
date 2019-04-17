using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdminSystem.Application.Commands
{
    public class SignReceiveOrderCommand:IRequest<ResultData<string>>
    {
        public string JsdOrderCode { get;private set; }
        public string ReceiveCode { get;private set; }
        public string Mobile { get; private set; }
        public string Imeis { get; private set; }

        public SignReceiveOrderCommand(string jsdOrderCode, string receiveCode,string mobile,string imeis)
        {
            this.JsdOrderCode = jsdOrderCode;
            this.ReceiveCode = receiveCode;
            this.Mobile = mobile;
            this.Imeis = imeis;
        }
    }
}
