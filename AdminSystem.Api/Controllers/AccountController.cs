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
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace AdminSystem.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        IMediator _mediator;

        private readonly ILogger<AccountController> _logger = null;
        public AccountController(IMediator mediator, ILogger<AccountController> logger)
        {
            this._mediator = mediator;
            this._logger = logger;
        }

        public string Test()
        {
            _logger.LogWarning("这这这这这这这这这这这这这这这这vvv");
            try
            {
                throw new Exception("除数不能为零");
            }
            catch (Exception e)
            {
                _logger.LogError(e, "错误消息");
            }
            return "哈哈";
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