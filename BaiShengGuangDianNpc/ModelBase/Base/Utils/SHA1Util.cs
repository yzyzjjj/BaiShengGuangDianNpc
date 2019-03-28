using System.Security.Cryptography;
using System.Text;

namespace ModelBase.Base.Utils
{
    public class SHA1Util
    {

        public static string GetSHA1Hash(string input)
        {
            var buffer = Encoding.UTF8.GetBytes(input);
            var data = SHA1.Create().ComputeHash(buffer);
            StringBuilder sub = new StringBuilder();
            foreach (var t in data)
            {
                sub.Append(t.ToString("x2"));
            }

            return sub.ToString();
        }
    }

}