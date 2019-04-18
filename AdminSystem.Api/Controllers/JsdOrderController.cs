﻿using AdminSystem.Application;
using AdminSystem.Application.Commands;
using AdminSystem.Application.Queries;
using AdminSystem.Application.Services;
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
        IIdentityService _identityService;
        public JsdOrderController(IMediator mediator, IApplicationUserQuery applicationUserQuery, IIdentityService identityService)
        {
            this._mediator = mediator;
            this._applicationUserQuery = applicationUserQuery;
            this._identityService = identityService;
        }
        /// <summary>
        /// 获取订单列表
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ResultData<List<GetJsdOrderListPageAsyncDtoResult>>> GetJsdOrderListPage(GetJsdOrderListPageAsyncDtoInput param)
        {
            return await _applicationUserQuery.GetJsdOrderListPageAsync(param,_identityService.GetDeptCode());
        }
        /// <summary>
        /// 签收订单
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ResultData<string>> SignReceiveOrder(SignReceiveOrderCommand param)
        {
            return await _mediator.Send(param);
        }
        /// <summary>
        /// 取消订单
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ResultData<string>> CancelReceiveOrder(CancelReceiveOrderCommand param)
        {
            return await _mediator.Send(param);
        }
    }
}
