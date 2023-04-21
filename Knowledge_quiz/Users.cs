using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeQuiz
{
    [KnownType(typeof(User))]
    [KnownType(typeof(Dictionary<string, User>))]
    [Serializable]
    public class Users : ISerializable
    {
        private Dictionary<string, User> users;

        public void AddUser(User? user)
        {
            if (user == null || users.ContainsKey(user.LoginPass.Login)) throw new ApplicationException(" Не можливо дотати користувача");
            users.Add(user.LoginPass.Login, user);
        }

        public void DellUser(string? userName)
        {
            if (userName == null || users.ContainsKey(userName)) throw new ApplicationException($" Не існує користувача з каким логіном {userName ?? "null"}");
            users.Remove(userName);
        }

        public void UpdateUser(User? user)
        {
            DellUser(user?.LoginPass.Login);
            AddUser(user);
        }

        public User GetUser(string login)
        {
            users.TryGetValue(login,out User? user);
            return user;
        }

        public IEnumerable<KeyValuePair<string, User>> AllUsers => users;

        public IEnumerable<string> Logins => users.Keys;

        public IEnumerable<User> UsersInfo => users.Values;

        public Users(){ users = new (); }

        public Users(SerializationInfo info, StreamingContext context)
        {
            users = info.GetValue("Users", typeof(Dictionary<string, User>)) as Dictionary<string, User>;
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Users", users);
        }
    }
}
