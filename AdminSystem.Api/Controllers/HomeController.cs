using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using AdminSystem.Application;
using AdminSystem.Application.Commands;
using Hangfire;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Swagger;
namespace AdminSystem.Api.Controllers
{
    //[Produces("application/json")]
    [Route("api/[controller]/[action]")]
    public class HomeController : Controller
    {
        IMediator _mediator;
        public HomeController(IMediator mediator)
        {
            _mediator = mediator;
        }
       
        [HttpGet]
        public IActionResult Index()
        {
            //每天一点钟执行
             RecurringJob.AddOrUpdate("同步用户信息", () =>this.SynchronizeUser(), "0 1 * * *", TimeZoneInfo.Local);
            //同步配置数据
            RecurringJob.AddOrUpdate("同步配置信息", () => this.SynchronizeConfig(), "15 1 * * *", TimeZoneInfo.Local);

            return new RedirectResult("~/homeIndex.html");
        }
        /// <summary>
        /// 同步用户数据
        /// </summary>
        [HttpPost]
        
        public ResultData<string> SynchronizeUser()
        {
            var a= _mediator.Send(new SynchronizeUserCommand()).Result;
            return ResultData<string>.CreateResultDataSuccess("成功");
        }
        /// <summary>
        /// 同步配置数据
        /// </summary>
        [HttpPost]
        public ResultData<string> SynchronizeConfig()
        {
            var a = _mediator.Send(new SynchronizeAttributeConfigCommand()).Result;
            return ResultData<string>.CreateResultDataSuccess("成功");
        }


        [HttpPost]
        public async Task<bool> CreateUser()
        {
            CreateUserCommand createUserCommand = new CreateUserCommand("115516","小就","女");
            return await _mediator.Send(createUserCommand);
        }

    }
}