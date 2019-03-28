using System.Security.Cryptography;
using System.Text;

namespace ModelBase.Base.Utils
{
    public class MD5Util
    {
        public static string GetMd5Hash(string input)
        {
            var md5Hash = MD5.Create();
            // Convert the input string to a byte array and compute the hash.
            var data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            var sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            foreach (byte d in data)
                sBuilder.Append(d.ToString("x2")); //lower, X2 ToUper

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

    }

}