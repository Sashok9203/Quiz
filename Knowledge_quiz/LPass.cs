
using System.Runtime.Serialization;

namespace KnowledgeQuiz
{
    [Serializable]
    public class LPass : ISerializable
    {
        public string Login { get; private set; }

        private string passHash;

        public LPass(string login,string password)
        {
            passHash = Utility.GetHash(password);
            Login = login;
        }

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
                passHash = Utility.GetHash(newPassword);
                return true;
            }
            return false;
        }

        public bool ChackPassword(string password) => Utility.HashCompare(password, passHash);

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("login", Login);
            info.AddValue("PassHash", passHash);
        }
    }
}
