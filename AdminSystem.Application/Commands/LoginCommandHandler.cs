using AdminSystem.Domain.AggregatesModel.UserAggregate;
using MediatR;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AdminSystem.Application.Commands
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, ResultData<string>>
    {
        private IApplicationUserRepository _applicationUserRepository { get; set; }
        public LoginCommandHandler(IApplicationUserRepository applicationUserRepository)
        {
            this._applicationUserRepository = applicationUserRepository;
        }
        public async Task<ResultData<string>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Mobile) ||string.IsNullOrWhiteSpace(request.ValidateCode))
            {
                return ResultData<string>.CreateResultDataFail("手机号码或验证码不能为空");
            }
            var user = await _applicationUserRepository.GetUserByMobileAsync(request.Mobile);
            if (user == null)
            {
                return ResultData<string>.CreateResultDataFail("不存在改手机号码");
            }
            if (request.ValidateCode != user.SmsCode|| user.IsUseSms)
            {
                return ResultData<string>.CreateResultDataFail("验证码不符合或者已使用,请重新发送验证码");
            }
            //已使用验证码 设置为失效
            user.UseSms();


            var claim = new Claim[]
            {
                  new Claim("Mobile",request.Mobile),
                  new Claim("UserId",user.UserId),
                  new Claim("DeptCode",user.DeptCode),
                  new Claim("TrueName",user.TrueName),
            };

            await _applicationUserRepository.UnitOfWork.SaveEntitiesAsync();

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Hello-key-----wyt"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken("self", "selfA", claim, DateTime.Now, DateTime.Now.AddMinutes(30), creds);

            return ResultData<string>.CreateResultDataSuccess("成功", new JwtSecurityTokenHandler().WriteToken(token));
        }
    }
}
