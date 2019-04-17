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
    public class GetJsdOrderListPageAsyncDtoInput
    {
        public string Page { get; set; }
        public string PageSize { get; set; }
    }

    public class JsdOrderDto
    {
        public int Id { get; set; }
        /// <summary>
        /// 订单编号
        /// </summary>
        public string OrderCode { get; set; }
        /// <summary>
        /// 部门编码
        /// </summary>
        public string DeptCode { get; set; }
        /// <summary>
        /// 日期
        /// </summary>
        public string OprDate { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 0 等待派送  1 派送中 2 已取消  3 签收中 4 已完成 5.退款中 6.已退货
        /// </summary>
        public string Status { get; set; }
        public string Mobile { get; set; }
        /// <summary>
        /// 数据更新或插入时间
        /// </summary>
        public DateTime LastUpDate { get; set; }
        /// <summary>
        /// 下单人核销码
        /// </summary>
        public string WeightCode { get; set; }
        public string DeliveryUserId { get; set; }
        public string DeliveryUserName { get; set; }
        /// <summary>
        /// 用户姓名
        /// </summary>
        public string TrueName { get; set; }
        /// <summary>
        /// 下单人手机号码
        /// </summary>
        public string CreateOrderMobile { get; set; }
        /// <summary>
        /// 取消人用户IdUserId
        /// </summary>
        public string CancelUserId { get; set; }
        /// <summary>
        /// 取消人用户姓名
        /// </summary>
        public string CancelTureName { get; set; }
        public string Address { get; set; }
        /// <summary>
        /// 经度
        /// </summary>
        public string Longitude { get; set; }
        /// <summary>
        /// 维度
        /// </summary>
        public string Latitude { get; set; }
        /// <summary>
        /// 订单金额
        /// </summary>
        public string Amount { get; set; }
        /// <summary>
        /// 是否是本系统更改了
        /// </summary>
        public bool IsThisSystemChange { get; set; }
    }
    public class JsdOrderItemDot
    {
        /// <summary>
        /// 商品编码
        /// </summary>
        public string ProductCode { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public string Qty { get; set; }
        /// <summary>
        /// 单价
        /// </summary>
        public string Price { get; set; }
        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName { get; set; }
        /// <summary>
        /// 订单类别
        /// </summary>
        public string Category { get; set; }
        /// <summary>
        /// 物料编码
        /// </summary>
        public string MaterialCode { get; set; }
    }


    public class GetJsdOrderListPageAsyncDtoOutput: JsdOrderDto
    {
   
        /// <summary>
        /// 商品编码
        /// </summary>
        public string ProductCode { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public string Qty { get; set; }
        /// <summary>
        /// 单价
        /// </summary>
        public string Price { get;  set; }
        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName { get;  set; }
        /// <summary>
        /// 订单类别
        /// </summary>
        public string Category { get;  set; }
        /// <summary>
        /// 物料编码
        /// </summary>
        public string MaterialCode { get;  set; }
    }

    public class GetJsdOrderListPageAsyncDtoResult
    {
        public JsdOrderDto Head { get; set; }
        public List<JsdOrderItemDot> DetailList { get; set; }
    }
}
