using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeQuiz
{
    [Serializable]
    public class LPass : ISerializable
    {
        public string? Login { get; private set; }

        private string? passHash;

        public LPass(string? login,string? passwordHash)
        {
            passHash = passwordHash;
            Login = login;
        }

        public LPass(SerializationInfo info, StreamingContext context)
        {
            passHash = info.GetString("PassHash");
            Login = info.GetString("login");
        }

        public bool ChangeLogin(string? login, string? password)
        {
            if (ChackPassword(password)) Login = login;
            return login == Login;
        }

        public bool ChangePassword(string? newPassword, string? oldPassword)
        {
            if (ChackPassword(oldPassword))
            {
                passHash = Utility.GetHash(newPassword);
                return true;
            }
            return false;
        }

        public bool ChackPassword(string? password) => Utility.HashCompare(password ?? "", passHash ?? "");

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("login", Login);
            info.AddValue("PassHash", passHash);
        }
    }
}
