using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using AdminSystem.Application;
using AdminSystem.Application.Commands;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace AdminSystem.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        IMediator _mediator;
        public AccountController(IMediator mediator)
        {
            this._mediator = mediator;
        }
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ResultData<string>> Login(LoginCommand param)
        {
            return await _mediator.Send(param);
        }
        /// <summary>
        /// 发送验证码
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ResultData<string>> SendValidateCode(SendValidateCodeCommand param)
        {
            return await _mediator.Send(param);
        }
    }
}