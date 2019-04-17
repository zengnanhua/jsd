using AdminSystem.Application.Models;
using AdminSystem.Application.Queries;
using AdminSystem.Infrastructure.Common;
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
        public HttpOmsService(IRmsDataBaseQuery rmsDataBaseQuery)
        {
            this._rmsDataBaseQuery = rmsDataBaseQuery;

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
    } 
}
