using System;
using System.Collections.Generic;
using System.Text;

namespace AdminSystem.Infrastructure.Common
{
    public static class StringExtensions
    {
        /// <summary>
        /// 会员信息掩码
        /// 曾南华 20190307
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string toMemberNameMask(this string value)
        {
            int first = 1; //第几位开始掩码
            string mask = "*";
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }
            var str = value.Substring(0, first);
            for (var i = first; i < value.Length; i++)
            {
                str += mask;
            }
            return str;
        }
        /// <summary>
        /// 会员信息掩码
        /// 曾南华 20190307
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string toMobileMask(this string value)
        {
            int first = 3; //第几位开始掩码
            int length = 4;//掩码长度
            string mask = "*";
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }
            var str = value.Substring(0, first);
            for (var i = first; i < (first + length); i++)
            {
                str += mask;
            }
            if ((first + length - 1) < value.Length)
            {
                str += value.Substring(first + length - 1);
            }
            return str;
        }
    }
}
