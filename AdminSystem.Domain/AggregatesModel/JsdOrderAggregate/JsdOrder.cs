using AdminSystem.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdminSystem.Domain.AggregatesModel.JsdOrderAggregate
{
    public class JsdOrder : Entity, IAggregateRoot
    {
        /// <summary>
        /// 订单编号
        /// </summary>
        public string OrderCode { get; private set; }
        /// <summary>
        /// 部门编码
        /// </summary>
        public string DeptCode { get; private set; }
        /// <summary>
        /// 日期
        /// </summary>
        public string OprDate { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; private set; }
        /// <summary>
        /// 0 等待派送  1 派送中 2 已取消  3 签收中 4 已完成 5.退款中 6.已退货
        /// </summary>
        public string Status { get; private set; }
        public string Mobile { get; private set; }
        /// <summary>
        /// 数据更新或插入时间
        /// </summary>
        public DateTime LastUpDate { get; private set; }
        /// <summary>
        /// 下单人核销码
        /// </summary>
        public string WeightCode { get; private set; }
        public string DeliveryUserId { get; private set; }
        public string DeliveryUserName { get; private set; }
        /// <summary>
        /// 用户姓名
        /// </summary>
        public string TrueName { get; private set; }
        /// <summary>
        /// 下单人手机号码
        /// </summary>
        public string CreateOrderMobile { get; private set; }
        /// <summary>
        /// 取消人用户IdUserId
        /// </summary>
        public string CancelUserId { get; private set; }
        /// <summary>
        /// 取消人用户姓名
        /// </summary>
        public string CancelTureName { get; private set; }
        public string Address { get; set; }
        /// <summary>
        /// 经度
        /// </summary>
        public string Longitude { get; private set; }
        /// <summary>
        /// 维度
        /// </summary>
        public string Latitude { get; private set; }
        /// <summary>
        /// 订单金额
        /// </summary>
        public string Amount { get; private set; }
        public string Imeis { get; set; }
        /// <summary>
        /// 是否是本系统更改了
        /// </summary>
        public bool IsThisSystemChange { get; private set; }

        public List<JsdOrderItem> OrderItem { get; private set; }

        protected JsdOrder()
        {
            OrderItem = new List<JsdOrderItem>();
        }
        public JsdOrder(string orderCode, string deptCode, string oprDate, string mobile, string trueName, string address
            , string longitude, string latitude, string amount) : this()
        {
            this.OrderCode = orderCode;
            this.DeptCode = deptCode;
            this.OprDate = oprDate;
            this.Mobile = mobile;
            this.TrueName = trueName;
            this.Address = address;
            this.Longitude = Longitude;
            this.Latitude = latitude;
            this.Amount = amount;
            this.LastUpDate = DateTime.Now;
            this.Status = "0";
            //创建新订单不是本系统操作
            this.SetIsThisSystemChange(false);
        }
        public void SetIsThisSystemChange(bool status)
        {
            this.IsThisSystemChange = status;
        }
        public void SetStatus(string status)
        {
            this.Status = status;
        }
        /// <summary>
        /// 签收订单
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="deliveryUserId"></param>
        /// <param name="deliveryUserName"></param>
        /// <param name="imeis"></param>
        public void SignReceive(string mobile,string deliveryUserId,string deliveryUserName,string imeis)
        {
            this.CreateOrderMobile = mobile;
            this.DeliveryUserId = deliveryUserId;
            this.DeliveryUserName = deliveryUserName;
            this.Imeis = imeis;
            this.Status = "3";
            this.SetIsThisSystemChange(true);
        }
        /// <summary>
        /// 取消订单
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="cancelUserId"></param>
        /// <param name="cancelTureName"></param>
        /// <param name="remark"></param>
        public void CancelReceive(string mobile, string cancelUserId , string cancelTureName,string remark )
        {
            this.CreateOrderMobile = mobile;
            this.CancelUserId = cancelUserId;
            this.CancelTureName = cancelTureName;
            this.Remark = remark;
            this.Status = "2";
            this.SetIsThisSystemChange(true);
        }

        public void AddJsdOrderItem(string orderCode, string productCode, string productName, string qty
            , string price, string category, string materialCode)
        {
            var temp = new JsdOrderItem(Id, orderCode, productCode, productName, qty, price, category, materialCode);
            this.OrderItem.Add(temp);
        }
    }
}
