using AdminSystem.Domain.Events;
using AdminSystem.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdminSystem.Domain.AggregatesModel.UserAggregate
{
    public class ApplicationUser: Entity, IAggregateRoot
    {
        public ApplicationUser(string userId,string trueName,string phone,string deptCode)
        {
            this.UserId = userId;
            this.TrueName = trueName;
            this.Phone = phone;
            this.DeptCode = deptCode;
        }

      
        /// <summary>
        /// 原来主表的用户Id
        /// </summary>
        public string UserId { get;private set; }
        /// <summary>
        /// 真实姓名
        /// </summary>
        public string TrueName { get; private set; }
        /// <summary>
        /// 手机号码
        /// </summary>
        public string Phone { get; private set; }
        /// <summary>
        /// 所属部门
        /// </summary>
        public string DeptCode { get; private set; }
        /// <summary>
        /// 短信验证码
        /// </summary>
        public string SmsCode { get; private set; }
        /// <summary>
        /// 短信发送时间
        /// </summary>
        public DateTime SmsSendDatetime { get; private set; }
        /// <summary>
        /// 是否使用了短信
        /// </summary>
        public bool IsUseSms { get; private set; }

    }
}
