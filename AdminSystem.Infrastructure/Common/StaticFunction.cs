using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdminSystem.Infrastructure.Common
{
    public class StaticFunction
    {
        /// <summary>
        /// C# 日期时间格式转换成为Unix时间戳格式
        /// </summary>
        /// <param name="timespan">时间差，分钟</param>
        /// <returns>long</returns>
        public static long GetTimestamp(int timespan)
        {
            double intResult = 0;
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            intResult = (DateTime.Now.AddMinutes(timespan) - startTime).TotalSeconds;
            return Convert.ToInt64(intResult);
        }
        /// <summary>
        /// ECO排序
        /// </summary>
        /// <param name="paramsMap"></param>
        /// <returns></returns>
        public static string GetEcoParamSrc(Dictionary<string, string> paramsMap)
        {
            var comList = paramsMap.Keys.ToList();
            comList.Sort((a, b) => string.CompareOrdinal(a, b));
            StringBuilder str = new StringBuilder();
            foreach (var key in comList)
            {
                string pkey = key;
                string pvalue = paramsMap[key];
                str.Append(pkey + pvalue);
            }
            return str.ToString();
        }
    }
}
