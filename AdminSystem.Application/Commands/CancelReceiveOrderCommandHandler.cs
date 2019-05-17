using AdminSystem.Application.Services;
using AdminSystem.Domain.AggregatesModel.JsdOrderAggregate;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AdminSystem.Application.Commands
{
    public class CancelReceiveOrderCommandHandler : IRequestHandler<CancelReceiveOrderCommand, ResultData<string>>
    {
        private IIdentityService _identityService;
        private IJsdOrderRepository _jsdOrderRepository;
        private HttpOmsService _httpOmsService;
        private ILogger<CancelReceiveOrderCommandHandler> _logger;
        public CancelReceiveOrderCommandHandler(IIdentityService identityService, IJsdOrderRepository jsdOrderRepository
            , HttpOmsService httpOmsService, ILogger<CancelReceiveOrderCommandHandler> logger)
        {
            this._identityService = identityService;
            this._jsdOrderRepository = jsdOrderRepository;
            this._httpOmsService = httpOmsService;
            this._logger = logger;
        }

        public async Task<ResultData<string>> Handle(CancelReceiveOrderCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.JsdOrderCode))
            {
                return ResultData<string>.CreateResultDataFail("JsdOrderId 字段不能为空");
            }
            if (string.IsNullOrWhiteSpace(request.Mobile))
            {
                return ResultData<string>.CreateResultDataFail("下单人手机号码不能为空");
            }
            if (string.IsNullOrWhiteSpace(request.ReceiveCode))
            {
                return ResultData<string>.CreateResultDataFail("签收码不能为空");
            }

            var jsdOrder = await _jsdOrderRepository.GetJsdByJsdOrderCodeAsync(request.JsdOrderCode);
            if (jsdOrder == null)
            {
                return ResultData<string>.CreateResultDataFail("该订单未找到");
            }

            if (jsdOrder.Status != "0" && jsdOrder.Status != "1")
            {
                return ResultData<string>.CreateResultDataFail("订单只能在派单中或者派送才能签收");
            }
            //先查询验证合法性
            var queryResult = await _httpOmsService.QueryReceiveCodeAsync(new Models.HttpQueryReceiveCodeParameter() { serial = jsdOrder.OrderCode, receiveCode = request.ReceiveCode });
            if (!queryResult.IsSuccess())
            {
                return queryResult;
            }

            jsdOrder.CancelReceive(request.Mobile, _identityService.GetUserId(), _identityService.GetTrueName(), request.Remark,request.ReceiveCode);

            //调用签收接口
            var checkResult = await _httpOmsService.CheckReceiveCodeAsync(new Models.HttpCheckReceiveCodeParameter() { serial = jsdOrder.OrderCode, receiveCode = request.ReceiveCode, status = "2" });
            if (!checkResult.IsSuccess())
            {
                this._logger.LogInformation($"拒签接口失败，订单号{jsdOrder.OrderCode} ,原因：{checkResult.resultMessage}");
                return checkResult;
            }
            else
            {
                this._logger.LogInformation($"拒签接口成功，订单号{jsdOrder.OrderCode} ");
            }
            var cancelOrderResult = await _httpOmsService.CancelOrder(new Models.HttpCancelOrderParameter() { OrderCode = jsdOrder.OrderCode, cancelBy = _identityService.GetTrueName() });
            if (!cancelOrderResult.IsSuccess())
            {
                this._logger.LogWarning($"取消订单接口失败，订单号：{jsdOrder.OrderCode} 原因：{cancelOrderResult.resultMessage}");
            }
            else
            {
                this._logger.LogInformation($"取消订单接口成功，订单号{jsdOrder.OrderCode} ");
            }
            //更新数据库
            await _jsdOrderRepository.UnitOfWork.SaveEntitiesAsync();


            return ResultData<string>.CreateResultDataSuccess("成功");
        }
    }
}
