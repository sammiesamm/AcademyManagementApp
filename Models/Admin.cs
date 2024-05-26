using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;

namespace AcademyManager.Models
{
    public class Admin
    {
        public string UUID { get; set; }
        public string Password { get; set; }
        private string MD5Hash(string password)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(password);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to a hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("x2"));
                }
                return sb.ToString();
            }
        }
        public bool Match(string uuid, string password)
        {
            string hash = MD5Hash(password);
            return uuid == UUID && hash == Password;
        }
        public bool Activated()
        {
            return Password != null && Password != String.Empty;
        }
        public Admin(string uuid, string password)
        {
            UUID = uuid;
            if (password != null) Password = MD5Hash(password);
        }
        [JsonConstructor]
        public Admin(string uuid, string password, int a = 0)
        {
            UUID = uuid;
            Password = password;
        }
    }
}
