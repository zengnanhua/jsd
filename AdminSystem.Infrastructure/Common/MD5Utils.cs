using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace AdminSystem.Infrastructure.Common
{
    public static class MD5Utils
    {
        public static string MD5Encrypt(string pwd)
        {
            if (pwd == null)
            {
                throw new NullReferenceException("不能加密空字符串");
            }
            byte[] datas = MD5Bytes(pwd);
            string password = BitConverter.ToString(datas).Replace("-", "");
            return password.ToLower();
        }
        public static byte[] MD5Bytes(string pwd)
        {
            MD5 md5 = new MD5CryptoServiceProvider();

            return md5.ComputeHash(new UTF8Encoding(false).GetBytes(pwd));
        }
        public static string SmsGetMd5(string src)
        {
            var buffer = new StringBuilder();
            using (var md5 = MD5.Create())
            {
                var md5Bytes = md5.ComputeHash(Encoding.UTF8.GetBytes(src));
                for (var i = 0; i < md5Bytes.Length; i++)
                {
                    var val = Convert.ToInt32(md5Bytes[i] & 0xff);
                    if (val < 16)
                    {
                        buffer.Append("0");
                    }
                    buffer.Append(string.Format("{0:X}", val));
                }
            }

            return buffer.ToString();
        }
    }
}
