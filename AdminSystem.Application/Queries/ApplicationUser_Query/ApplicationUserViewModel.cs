using System;
using System.Collections.Generic;
using System.Text;

namespace AdminSystem.Application.Queries
{
    public class ApplicationUserViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class GetJsdOrderOutput
    {
        public int Id { get; set; }
        /// <summary>
        /// jsd订单id
        /// </summary>
        public string OrderCode { get; set; }
        /// <summary>
        /// 门店编码
        /// </summary>
        public string DeptCode { get; set; }
        /// <summary>
        /// 操作日期
        /// </summary>
        public string OprDate { get; set; }
        /// <summary>
        /// 0 等待派送  1 派送中 2 已取消  3 签收中 4 已完成 5.退款中 6.已退货
        /// </summary>
        public string Status { get; set; }
        public bool IsThisSystemChange { get;  set; }
    }
}
