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
    }
}
