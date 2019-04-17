using AdminSystem.Domain.AggregatesModel.UserAggregate;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AdminSystem.Application.Commands
{
    public class SendValidateCodeCommandHandler : IRequestHandler<SendValidateCodeCommand, ResultData<string>>
    {
        private IApplicationUserRepository _applicationUserRepository;
        public SendValidateCodeCommandHandler(IApplicationUserRepository applicationUserRepository)
        {
            this._applicationUserRepository = applicationUserRepository;
        }
        public async Task<ResultData<string>> Handle(SendValidateCodeCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Mobile))
            {
                return ResultData<string>.CreateResultDataFail("手机号码不能为空");
            }
            var user = await this._applicationUserRepository.GetUserByMobileAsync(request.Mobile);
            if (user == null)
            {
                return ResultData<string>.CreateResultDataFail("该手机号码不是系统用户，请联系系统管理员");
            }
            user.SendSms();
            await _applicationUserRepository.UnitOfWork.SaveEntitiesAsync();

            return ResultData<string>.CreateResultDataSuccess("成功");
        }
    }
}
