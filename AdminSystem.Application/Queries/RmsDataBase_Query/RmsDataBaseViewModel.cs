using System;
using System.Collections.Generic;
using System.Text;

namespace AdminSystem.Application.Queries
{
    public class zmd_ac_usersTab
    {
        /// <summary>
        /// 原来主表的用户Id
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// 真实姓名
        /// </summary>
        public string TrueName { get; set; }
        /// <summary>
        /// 手机号码
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 所属部门
        /// </summary>
        public string DeptCode { get; set; }
    }
    public class Zmd_Base_ConfigTab
    {
        public string ConfigValue { get; set; }
        public string ConfigText { get; set; }
        public string ConfigDesc { get; set; }
        public string Enabled { get; set; }
    }

    public class zmd_oms_head
    {
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
    }
}
