using AdminSystem.Domain.Events;
using AdminSystem.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdminSystem.Domain.AggregatesModel.UserAggregate
{
    public class ApplicationUser : Entity, IAggregateRoot
    {

        /// <summary>
        /// 原来主表的用户Id
        /// </summary>
        public string UserId { get; private set; }
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
        public ApplicationUser(string userId, string trueName, string phone, string deptCode)
        {
            this.UserId = userId;
            this.TrueName = trueName;
            this.Phone = phone;
            this.DeptCode = deptCode;
        }

        public void UseSms()
        {
            this.IsUseSms = true;
        }
        public void SendSms()
        {
            var code= this.CreateValidateCode(5);
            this.SmsCode = code;
            this.IsUseSms = false;
            this.SmsSendDatetime = DateTime.Now;
            AddDomainEvent(new SendSmsDomainEvent(this.Phone, $"你的验证码为{code}，请小心保管"));
        }

        /// <summary>
        /// 生成验证码
        /// </summary>
        /// <param name="length">指定验证码的长度</param>
        /// <returns></returns>
        private string CreateValidateCode(int length)
        {
            int[] randMembers = new int[length];
            int[] validateNums = new int[length];
            string validateNumberStr = "";
            //生成起始序列值
            int seekSeek = unchecked((int)DateTime.Now.Ticks);
            Random seekRand = new Random(seekSeek);
            int beginSeek = (int)seekRand.Next(0, Int32.MaxValue - length * 10000);
            int[] seeks = new int[length];
            for (int i = 0; i < length; i++)
            {
                beginSeek += 10000;
                seeks[i] = beginSeek;
            }
            //生成随机数字
            for (int i = 0; i < length; i++)
            {
                Random rand = new Random(seeks[i]);
                int pownum = 1 * (int)Math.Pow(10, length);
                randMembers[i] = rand.Next(pownum, Int32.MaxValue);
            }
            //抽取随机数字
            for (int i = 0; i < length; i++)
            {
                string numStr = randMembers[i].ToString();
                int numLength = numStr.Length;
                Random rand = new Random();
                int numPosition = rand.Next(0, numLength - 1);
                validateNums[i] = Int32.Parse(numStr.Substring(numPosition, 1));
            }
            //生成验证码
            for (int i = 0; i < length; i++)
            {
                validateNumberStr += validateNums[i].ToString();
            }
            return validateNumberStr;
        }

    }
}
