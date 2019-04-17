using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdminSystem.Application.Commands
{
    public class SignReceiveOrderCommand:IRequest<ResultData<string>>
    {
        public string JsdOrderId { get;private set; }
        public string Mobile { get; private set; }
        public string Imeis { get; private set; }
        public string UserId { get; private set; }
    }
}
