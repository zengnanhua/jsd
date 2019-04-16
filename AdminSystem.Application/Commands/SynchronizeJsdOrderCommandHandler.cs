using AdminSystem.Application.Queries;
using AdminSystem.Application.Services;
using AdminSystem.Domain.AggregatesModel.JsdOrderAggregate;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static AdminSystem.Application.Models.HttpGetJsdOrderPayedDetailResult;

namespace AdminSystem.Application.Commands
{
    public class SynchronizeJsdOrderCommandHandler : IRequestHandler<SynchronizeJsdOrderCommand, bool>
    {
        private HttpOmsService _httpOmsService;
        private IJsdOrderRepository _iJsdOrderRepository;
        private IRmsDataBaseQuery _rmsDataBaseQuery;
        private IApplicationUserQuery _applicationUserQuery;
        public SynchronizeJsdOrderCommandHandler(HttpOmsService httpOmsService, IJsdOrderRepository iJsdOrderRepository, 
            IRmsDataBaseQuery rmsDataBaseQuery, IApplicationUserQuery applicationUserQuery)
        {
            this._httpOmsService = httpOmsService;
            this._iJsdOrderRepository = iJsdOrderRepository;
            this._rmsDataBaseQuery = rmsDataBaseQuery;
            this._applicationUserQuery = applicationUserQuery;
        }
        public async Task<bool> Handle(SynchronizeJsdOrderCommand request, CancellationToken cancellationToken)
        {
            DateTime date;
            if (!DateTime.TryParse(request.Date, out date))
            {
                throw new Exception($"字段 date 值：‘{request.Date}’不是 yyyy-MM-dd格式");
            }

            var remoteJsdOrderList= await _httpOmsService.GetJsdOrderPayedDetailAsync(date);//接口获取极速达订单

            var zmd_oms_headList= await _rmsDataBaseQuery.GetZmd_oms_headAsync(date.ToString("yyyy-MM-dd"));//rms极速达订单

            var getJsdOrderOutputList= await _applicationUserQuery.GetJsdOrderListAsync(date.ToString("yyyy-MM-dd"));//临时极速达订单

            //如果官网没有任何数据那就不要同步
            if (!(remoteJsdOrderList != null && remoteJsdOrderList.data != null && remoteJsdOrderList.data.Count > 0))
            {
                return false;
            }

            foreach (var remoteItem in remoteJsdOrderList.data)
            {
                remoteItem.OrderStatus = "0";
                var zmd_oms_headItem = zmd_oms_headList.Where(c => c.OrderCode == remoteItem.OrderCode).FirstOrDefault();
                var getJsdOrderOutputItem = getJsdOrderOutputList.Where(c => c.OrderCode == remoteItem.OrderCode).FirstOrDefault();

                if (zmd_oms_headItem != null && zmd_oms_headItem.Status != "0") //同步rms更新状态
                {
                    remoteItem.OrderStatus = zmd_oms_headItem.Status;
                }


                //刚下的单 还没有到各个系统里面
                if ( getJsdOrderOutputItem == null)
                {
                    await CreateJsdOrder(remoteItem); //在临时表中增加数据
                }
                //rms 更新了状态  同步到临时库里面
                else if (getJsdOrderOutputItem != null && getJsdOrderOutputItem.IsThisSystemChange == false
                    && getJsdOrderOutputItem.Status != remoteItem.OrderStatus
                    )
                {
                    if (zmd_oms_headItem != null)
                    {
                        var entity = await _iJsdOrderRepository.GetAsync(getJsdOrderOutputItem.Id);
                        entity.SetStatus(remoteItem.OrderStatus);  //同步Rms状态
                        _iJsdOrderRepository.Update(entity);
                    }
                }

               

            }

            return await _iJsdOrderRepository.UnitOfWork.SaveEntitiesAsync();
        }
        public async Task CreateJsdOrder(HttpJsdOrder temp)
        {

            JsdOrder jsdOrder = new JsdOrder(temp.OrderCode, temp.DeptCode, temp.OprDate, temp.Mobile, temp.TrueName, temp.Address, temp.Longitude, temp.Latitude, temp.Amount);
            jsdOrder.SetStatus(temp.OrderStatus);
            var detailInfoList = temp.GetOrderItems();
            foreach (var tempitem in detailInfoList)
            {
                jsdOrder.AddJsdOrderItem(temp.OrderCode, tempitem.ProductCode, tempitem.ProductName, tempitem.Qty, tempitem.Price, "", tempitem.ProductCode);
            }
            await _iJsdOrderRepository.AddJsdOrderAsync(jsdOrder);
        }
    }
}
