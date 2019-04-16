using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdminSystem.Application.Models
{
    public class HttpGetJsdOrderPayedDetailResult
    {
        public string code { get; set; }

        public List<HttpJsdOrder> data { get; set; }

        public class HttpJsdOrder
        {
            public string Amount { get; set; }
            public string Address { get; set; }
            public string OprDate { get; set; }
            public string Latitude { get; set; }
            public string Mobile { get; set; }
            public string OrderStatus { get; set; }
            public string Remark { get; set; }
            public string TrueName { get; set; }
            public string OrderCode { get; set; }
            public string DeptCode { get; set; }
            public string Longitude { get; set; }
            public string DetailInfo { get; set; }

            public List<HttpJsdOrderItem> GetOrderItems()
            {
                if (!string.IsNullOrWhiteSpace(this.DetailInfo))
                {
                    return JsonConvert.DeserializeObject<List<HttpJsdOrderItem>>(this.DetailInfo);
                }
                return new List<HttpJsdOrderItem>();
               
            }

        }
        public class HttpJsdOrderItem
        {
            public string ProductCode { get; set; }
            public string Price { get; set; }
            public string Qty { get; set; }
            public string ProductName { get; set; }
        }
    }

    
}
