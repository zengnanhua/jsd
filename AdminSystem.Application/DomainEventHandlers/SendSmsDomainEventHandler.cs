using AdminSystem.Application.Models;
using AdminSystem.Application.Queries;
using AdminSystem.Domain.Events;
using AdminSystem.Infrastructure.Common;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AdminSystem.Application.DomainEventHandlers
{
    public class SendSmsDomainEventHandler : INotificationHandler<SendSmsDomainEvent>
    {
        ILogger<SendSmsDomainEventHandler> _logger;
        IRmsDataBaseQuery _rmsDataBaseQuery;
        private string Url = string.Empty;
        private string Secret = string.Empty;
        private string AppId = string.Empty;
        private string smsAppKey = string.Empty;

        public SendSmsDomainEventHandler(ILogger<SendSmsDomainEventHandler> logger, IRmsDataBaseQuery rmsDataBaseQuery)
        {
            this._logger = logger;
            this._rmsDataBaseQuery = rmsDataBaseQuery;
            this.Url = _rmsDataBaseQuery.GetZmd_Base_ConfigyValueByKeyCacheAsync("ESB_OCSM").Result;
            this.Secret = _rmsDataBaseQuery.GetZmd_Base_ConfigyValueByKeyCacheAsync("ESB_Secret").Result;
            this.AppId= _rmsDataBaseQuery.GetZmd_Base_ConfigyValueByKeyCacheAsync("ESB_AppId").Result;
            this.smsAppKey= _rmsDataBaseQuery.GetZmd_Base_ConfigyValueByKeyCacheAsync("smsAppKey").Result;
        }
        public async Task Handle(SendSmsDomainEvent notification, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(notification.Content))
            {
                throw new Exception("短信内容不能为空");
            }
            if (string.IsNullOrEmpty(notification.Mobile))
            {
                throw new Exception("短信手机号不能为空");
            }
            string timestamp = WebAPIHelper.ConvertCurrentTimeStamp();
            string dataJson = JsonConvert.SerializeObject(new { phone = notification.Mobile, content = notification.Content, inner = false });
            string uri = "/oppo/sms/singleSend";
          
            IDictionary<string, string> paramsMap = new Dictionary<string, string>();
            paramsMap.Add("smsAppId", AppId);
            paramsMap.Add("smsAppKey", smsAppKey);
            paramsMap.Add("app_id", AppId);
            paramsMap.Add("timestamp", timestamp);
            paramsMap.Add("data", dataJson);
            //生成签名
            string sign = WebAPIHelper.SmsSign(paramsMap, uri, Secret);
            paramsMap.Add("sign", sign);

            //暂用GET方式
            //POST 方式：遇到特殊字符会被转义，导致签名不能通过
            string res = await WebAPIHelper.HttpPostAsync(Url + uri, WebAPIHelper.SmsBuildEncodeUrl(paramsMap), contentType: "application/x-www-form-urlencoded");

            if (!string.IsNullOrWhiteSpace(res))
            {
                HttpSmdModelResult model = JsonConvert.DeserializeObject<HttpSmdModelResult>(res);
                if (model.code != null&& model.code == "0")
                {

                    return;
                }
            }

            _logger.LogWarning($"短信发送失败,mobile:{notification.Mobile} 内容“{notification.Content}”");
        }
       
    }
}
