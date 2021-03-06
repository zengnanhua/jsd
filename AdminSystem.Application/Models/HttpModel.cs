﻿using Newtonsoft.Json;
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


    public class HttpQueryReceiveCodeParameter
    {
        public string serial { get; set; }
        public string receiveCode { get; set; }
    }
    public class HttpReceiveResult
    {
        public string Code { get; set; }
        public string Msg { get; set; }
    }

    public class HttpCheckReceiveCodeParameter
    {
        public string serial { get; set; }
        public string receiveCode { get; set; }
        /// <summary>
        /// 签收（1） 拒签（2）
        /// </summary>
        public string status { get; set; }
    }
    public class HttpSmdModelResult
    {
        public object message { get; set; }
        public string code { get; set; }
    }

    public class HttpCancelOrderParameter
    {
        public string OrderCode { get; set; }
        public string cancelBy { get; set; }
    }
    public class HttpCancelOrderResult
    {
        public bool success { get; set; }
        public string errorCode { get; set; }
        public string errorMsg { get; set; }
    }
    public class HttpUpdateOrderSignParameter
    {
        public string OrderCode { get; set; }
    }
    public class HttpUpdateOrderSignResult
    {
        public string code { get; set; }

        public Data data { get; set; }
        public class Data
        {
            public string sucCode { get; set; }
            public string state { get; set; }
            public string sucMsg { get; set; }
            public string errorMsg { get; set; }
        }
    }

}
