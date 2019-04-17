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
            if (!(request.Mobile == "nanhua" && request.ValidateCode == "123"))
            {
                return ResultData<string>.CreateResultDataFail("用户名或者验证码不对");
            }

            var claim = new Claim[]
            {
                  new Claim("Mobile",request.Mobile),
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Hello-key-----wyt"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken("self", "selfA", claim, DateTime.Now, DateTime.Now.AddMinutes(30), creds);

            return ResultData<string>.CreateResultDataSuccess("成功", new JwtSecurityTokenHandler().WriteToken(token));
        }
    }
}
