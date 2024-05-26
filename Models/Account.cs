using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;

namespace AcademyManager.Models
{
    public class Account
    {
        public string UserID { get; set; }
        public string UUID { get; set; }
        public string Email { get; set; }
        public int Type { get; set; }
        private string Password { get; set; }
        public Account(string stdid, string email, string password = null, int type = 0)
        {
            UserID = stdid;
            Email = email;
            Guid uid = Guid.NewGuid();
            UUID = uid.ToString();
            if (password != null) Password = SHA256Hash(password);
            Type = type;
        }
        [JsonConstructor]
        public Account(string stdid, string uuid, string email, string password, int type)
        {
            UserID = stdid;
            Email = email;
            UUID = uuid;
            Password = password;
            Type = type;
        }
        public void ChangePassword(string password)
        {
            Password = SHA256Hash(password);
        }
        public bool IsActivated()
        {
            return Password != null;
        }
        public bool Match(string pass, string id)
        {
            return UserID == id && SHA256Hash(pass) == Password;
        }
        public async Task SetPassword()
        {
            DatabaseManager db = new DatabaseManager();
            await db.SetPasswordAsync(UserID, Password, Type);
        }
        public string SHA256Hash(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder encodedpass = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    encodedpass.Append(bytes[i].ToString("x2"));
                }
                return encodedpass.ToString();
            }
        }
    }
}
