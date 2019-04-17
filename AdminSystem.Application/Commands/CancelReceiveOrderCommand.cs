using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdminSystem.Application.Commands
{
    public class CancelReceiveOrderCommand : IRequest<ResultData<string>>
    {
        public string JsdOrderCode { get; private set; }
        public string ReceiveCode { get; private set; }
        public string Mobile { get; private set; }
        public string Remark { get; private set; }

        public CancelReceiveOrderCommand(string jsdOrderCode, string receiveCode, string mobile, string remark)
        {
            this.JsdOrderCode = jsdOrderCode;
            this.ReceiveCode = receiveCode;
            this.Mobile = mobile;
            this.Remark = remark;
        }
    }
}
