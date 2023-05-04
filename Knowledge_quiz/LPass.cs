using System.Security.Cryptography;
using System.Runtime.Serialization;
using System.Text;

namespace KnowledgeQuiz
{
    [Serializable]
    public class LPass : ISerializable
    {
        private string passHash;
        private static string GetHash(string str)
        {
            byte[] data = SHA256.HashData(Encoding.UTF8.GetBytes(str));
            var sBuilder = new StringBuilder();
            foreach (var item in data)
                sBuilder.Append(item.ToString("x2"));
            return sBuilder.ToString();
        }

        public LPass(string login,string password)
        {
            passHash = GetHash(password);
            Login = login;
        }
        public static bool HashCompare(string str, string hash) => hash.CompareTo(GetHash(str)) == 0;
        public string Login { get; private set; }
        public LPass(SerializationInfo info, StreamingContext context)
        {
            passHash = info.GetString("PassHash") ?? string.Empty;
            Login = info.GetString("login") ?? string.Empty;
        }
        public bool ChangeLogin(string login, string password)
        {
            if (ChackPassword(password)) Login = login;
            return login == Login;
        }
        public bool ChangePassword(string newPassword, string oldPassword)
        {
            if (ChackPassword(oldPassword))
            {
                passHash = GetHash(newPassword);
                return true;
            }
            return false;
        }
        public bool ChackPassword(string password) => passHash.CompareTo(GetHash(password)) == 0;
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("login", Login);
            info.AddValue("PassHash", passHash);
        }
    }
}
