using AdminSystem.Application.Models;
using AdminSystem.Application.Queries;
using AdminSystem.Infrastructure.Common;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AdminSystem.Application.Services
{
    public class HttpOmsService
    {
        IRmsDataBaseQuery _rmsDataBaseQuery;

        readonly string OMSUrl = string.Empty;
        readonly string ECOUrl = string.Empty;
        readonly string eco_app_token = string.Empty;
        readonly string JsdUpdateOrderUrl = string.Empty;
        readonly string JsdUpdateOrderAppSecret = string.Empty;
        readonly ILogger<HttpOmsService> _logger;
        public HttpOmsService(IRmsDataBaseQuery rmsDataBaseQuery, ILogger<HttpOmsService> logger)
        {
            this._rmsDataBaseQuery = rmsDataBaseQuery;
            this._logger = logger;

            this.OMSUrl = _rmsDataBaseQuery.GetZmd_Base_ConfigyValueByKeyCacheAsync("OMSUrl").Result;
            this.ECOUrl = _rmsDataBaseQuery.GetZmd_Base_ConfigyValueByKeyCacheAsync("ECOUrl").Result;
            this.eco_app_token = _rmsDataBaseQuery.GetZmd_Base_ConfigyValueByKeyCacheAsync("eco_app_token").Result;
            this.JsdUpdateOrderUrl = _rmsDataBaseQuery.GetZmd_Base_ConfigyValueByKeyCacheAsync("JsdUpdateOrderUrl").Result;
            this.JsdUpdateOrderAppSecret = _rmsDataBaseQuery.GetZmd_Base_ConfigyValueByKeyCacheAsync("JsdUpdateOrderAppSecret").Result;
        }

        /// <summary>
        /// 获取极速达订单
        /// </summary>
        /// <returns></returns>
        public async Task<HttpGetJsdOrderPayedDetailResult> GetJsdOrderPayedDetailAsync(DateTime datetime)
        {
            var intertfeceUrl = "/orders/web/ecoSync/orders/getJsdOrderPayedDetail";
            var headurl = JsdUpdateOrderUrl.Trim('/');

            var dateStr = datetime.ToString("yyyy-MM-dd");

            var requestObj = new
            {
                appid = "3",
                start_date = dateStr+" 00:00:00",
                end_date = dateStr+" 23:59:59",
                sign = MD5Utils.MD5Encrypt(headurl + intertfeceUrl + JsdUpdateOrderAppSecret)
            };

            var returnStr = await WebAPIHelper.HttpPostAsync(headurl + intertfeceUrl, JsonConvert.SerializeObject(requestObj));

            var result = JsonConvert.DeserializeObject<HttpGetJsdOrderPayedDetailResult>(returnStr);
            return result;
        }

        /// <summary>
        /// 签收码查询(极速达)
        /// 曾南华 20190326
        /// </summary>
        /// <returns></returns>
        public async Task<ResultData<string>> QueryReceiveCodeAsync(HttpQueryReceiveCodeParameter param)
        {
            var str = JsonConvert.SerializeObject(param);

            var returnStr = await WebAPIHelper.HttpPostAsync(OMSUrl + "/orders/web/trade/queryReceiveCode", str);
            var result = JsonConvert.DeserializeObject<HttpReceiveResult>(returnStr);
            if (result.Code == "0")
            {
                return ResultData<string>.CreateResultDataSuccess("成功");
            }

            return ResultData<string>.CreateResultDataFail(result.Msg);
        }
        /// <summary>
        /// 签收核销码(极速达)
        /// 曾南华 20190326
        /// </summary>
        /// <returns></returns>
        public async Task<ResultData<string>> CheckReceiveCodeAsync(HttpCheckReceiveCodeParameter param)
        {
            var str = JsonConvert.SerializeObject(param);

            var returnStr = await WebAPIHelper.HttpPostAsync(OMSUrl + "/orders/web/trade/checkReceiveCode", str);
            var result = JsonConvert.DeserializeObject<HttpReceiveResult>(returnStr);
            if (result.Code == "0")
            {
                return ResultData<string>.CreateResultDataSuccess("成功");
            }

            return ResultData<string>.CreateResultDataFail(result.Msg);

        }

        /// <summary>
        /// 极速达取消订单
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<ResultData<string>> CancelOrder(HttpCancelOrderParameter param)
        {
            Dictionary<string, string> dirList = new Dictionary<string, string>();
            dirList.Add("OrderCode",param.OrderCode);
            dirList.Add("cancelBy", param.cancelBy);

            dirList.Add("app_id", "jsd");
            dirList.Add("timestamp", StaticFunction.GetTimestamp(0).ToString() + "000");
            dirList.Add("ecOrderNumber", param.OrderCode);
            var str = StaticFunction.GetEcoParamSrc(dirList);
            var sign = MD5Utils.MD5Encrypt(eco_app_token + str + eco_app_token).ToUpper();


            dirList.Add("sign", sign);

            var returnStr = await WebAPIHelper.HttpPostAsync(ECOUrl + "/api/order/cancelOrder", JsonConvert.SerializeObject(dirList));
            var result = JsonConvert.DeserializeObject<HttpCancelOrderResult>(returnStr);
            if (result.success)
            {
                this._logger.LogInformation("订单号为："+ param.OrderCode+"取消成功");
                return ResultData<string>.CreateResultDataSuccess("成功");
            }
            this._logger.LogWarning("订单号为：" + param.OrderCode + "取消失败，原因："+ (result.errorMsg ?? "调用极速达取消接口失败"));
            return ResultData<string>.CreateResultDataFail(result.errorMsg ?? "调用极速达取消接口失败");
        }

        /// <summary>
        /// 更新订单为已签收
        /// 曾南华 20190327
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<ResultData<string>> UpdateOrderSign(HttpUpdateOrderSignParameter param)
        {
            var intertfeceUrl = "/orders/web/ecoSync/orders/updateOrderSign";
            var headurl = JsdUpdateOrderUrl.Trim('/');

            var requestObj = new
            {
                appid = "3",
                order_deliveries = new[]
                {
                    new  {
                        order_no=param.OrderCode,
                        serial="",
                        shipping_id="",
                        shipping_name="",
                        sign_at=DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"),
                        status="8"
                    },
                },
                sign = MD5Utils.MD5Encrypt(headurl + intertfeceUrl + JsdUpdateOrderAppSecret)
            };


            var returnStr = await WebAPIHelper.HttpPostAsync(headurl + intertfeceUrl, JsonConvert.SerializeObject(requestObj));

            var result = JsonConvert.DeserializeObject<HttpUpdateOrderSignResult>(returnStr);
            if (result != null && result.data != null && result.data.state == "1")
            {
                _logger.LogInformation($"签收更新订单接口成功 订单号{param.OrderCode}");
                return ResultData<string>.CreateResultDataSuccess("成功");
            }
            if (result != null && result.data != null && (!string.IsNullOrEmpty(result.data.errorMsg)))
            {
                _logger.LogWarning($"签收更新订单接口失败 订单号{param.OrderCode} 原因：{result.data.errorMsg}");
                return ResultData<string>.CreateResultDataFail(result.data.errorMsg);
            }
            _logger.LogWarning($"签收更新订单接口失败 订单号{param.OrderCode} ");
            return ResultData<string>.CreateResultDataFail("调用订单更新为已签收失败");
        }
    } 
}
