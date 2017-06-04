using System.Security.Cryptography;
using System.Text;

namespace test.common
{
    internal class Coder
    {
        public static string StrToMd5(string str)
        {
            var data = Encoding.GetEncoding("GB2312").GetBytes(str);
            MD5 md5 = new MD5CryptoServiceProvider();
            var outBytes = md5.ComputeHash(data);

            var outString = "";
            for (var i = 0; i < outBytes.Length; i++)
                outString += outBytes[i].ToString("x2");
            return outString.ToLower();
        }
    }
}