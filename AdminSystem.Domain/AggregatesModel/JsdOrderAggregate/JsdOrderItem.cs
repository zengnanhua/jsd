using AdminSystem.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdminSystem.Domain.AggregatesModel.JsdOrderAggregate
{
    public class JsdOrderItem : Entity
    {
        /// <summary>
        /// 主表id
        /// </summary>
        public int JsdOrderId { get; private set; }
        /// <summary>
        /// 订单编号
        /// </summary>
        public string OrderCode { get; private set; }
        /// <summary>
        /// 商品编码
        /// </summary>
        public string ProductCode { get; set;  }
        /// <summary>
        /// 数量
        /// </summary>
        public string Qty { get; set; }
        /// <summary>
        /// 单价
        /// </summary>
        public string Price { get; private set; }
        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName { get; private set; }
        /// <summary>
        /// 订单类别
        /// </summary>
        public string Category { get; private set; }
        /// <summary>
        /// 物料编码
        /// </summary>
        public string MaterialCode { get; private set; }

        protected JsdOrderItem() { }

        public JsdOrderItem(int jsdOrderId, string orderCode,string productCode,string productName,string qty
            ,string price,string category,string materialCode)
        {
            this.JsdOrderId = jsdOrderId;
            this.OrderCode = orderCode;
            this.ProductCode = productCode;
            this.ProductName = ProductName;
            this.Qty = qty;
            this.Price = price;
            this.Category = category;
            this.MaterialCode = materialCode;
        }
    }
}
