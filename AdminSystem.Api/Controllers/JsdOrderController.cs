using AdminSystem.Application;
using AdminSystem.Application.Commands;
using AdminSystem.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdminSystem.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class JsdOrderController : ControllerBase
    {
        IMediator _mediator;
        IApplicationUserQuery _applicationUserQuery;
        public JsdOrderController(IMediator mediator, IApplicationUserQuery applicationUserQuery)
        {
            this._mediator = mediator;
            this._applicationUserQuery = applicationUserQuery;
        }
        /// <summary>
        /// 获取订单列表
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ResultData<List<GetJsdOrderListPageAsyncDtoResult>>> GetJsdOrderListPage(GetJsdOrderListPageAsyncDtoInput param)
        {
            return await _applicationUserQuery.GetJsdOrderListPageAsync(param);
        }
        /// <summary>
        /// 签收订单列表
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task SignReceiveOrder(SignReceiveOrderCommand param)
        {
            
        }
    }
}
