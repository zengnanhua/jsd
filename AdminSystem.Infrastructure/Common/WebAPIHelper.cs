using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AdminSystem.Infrastructure.Common
{
    public static class WebAPIHelper
    {
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
