using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace AdminSystem.Infrastructure.Common
{
    public static class WebAPIHelper
    {
        public static string ConvertCurrentTimeStamp()
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            DateTime dtNow =DateTime.Now;
            TimeSpan toNow = dtNow.Subtract(dtStart);
            string timeStamp = toNow.Ticks.ToString();
            timeStamp = timeStamp.Substring(0, timeStamp.Length - 7);
            return timeStamp;
        }
        public static string SmsSign(IDictionary<string, string> parameters, string url, string secret)
        {
            var builder = new StringBuilder();
            builder.Append(url);
            builder.Append("\n");

            foreach (var param in parameters.OrderBy(t => t.Key))
            {
                builder.Append(string.Format("{0}={1}\n", param.Key, param.Value));
            }

            builder.Append(secret);
            return MD5Utils.SmsGetMd5(builder.ToString());
        }
        public static string SmsBuildEncodeUrl(IDictionary<string, string> dict)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var param in dict.OrderBy(t => t.Key))
            {
                string encode = HttpUtility.UrlEncode(param.Value, Encoding.UTF8);
                sb.Append(param.Key).Append("=").Append(encode).Append("&");
            }

            return sb.ToString().Substring(0, sb.ToString().LastIndexOf("&"));
        }
        public static async　Task<string> HttpPostAsync(string url, string postData = null,string contentType = "application/json")
        {
            using (HttpClient client = new HttpClient())
            {
                using (HttpContent httpContent = new StringContent(postData, Encoding.UTF8))
                {
                    httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(contentType);
                    HttpResponseMessage response = await client.PostAsync(url, httpContent);
                    return await response.Content.ReadAsStringAsync();
                }
                
            }
        }
    }
}
