using System.Security.Cryptography;
using System.Text;

namespace MoneyMap.Utility
{
    public static class PasswordHelper
    {
        public static string EncodePasswordMd5(string pass) //Encrypt using MD5   
        {
            MD5 md5;
            //Instantiate MD5CryptoServiceProvider, get bytes for original password and compute hash (encoded password)   
            md5 = new MD5CryptoServiceProvider();
            Byte[] originalBytes = ASCIIEncoding.Default.GetBytes(pass);
            Byte[] encodedBytes = md5.ComputeHash(originalBytes);
            //Convert encoded bytes back to a 'readable' string   
            return BitConverter.ToString(encodedBytes);
        }

    }
}
