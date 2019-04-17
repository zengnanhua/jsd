using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AdminSystem.Application.Commands
{
    public class SignReceiveOrderCommandHandler : IRequestHandler<SignReceiveOrderCommand, ResultData<string>>
    {
        public async Task<ResultData<string>> Handle(SignReceiveOrderCommand request, CancellationToken cancellationToken)
        {
            return ResultData<string>.CreateResultDataSuccess("成功");
        }
    }
}
