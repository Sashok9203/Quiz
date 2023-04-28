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
        private readonly Dictionary<string, User>? users;

        public void AddUser(User? user)
        {
            if (user == null || ( users?.ContainsKey(user?.LoginPass?.Login ?? "") ?? false)) throw new ApplicationException(" Не можливо дотати користувача");
            if (user != null) users?.Add(user.LoginPass?.Login ?? "", user);
        }

        public bool DellUser(string? userName) => users?.Remove(userName ?? "") ?? false;
       
        public User? GetUser(string? login)
        {
            User? user = null;
            users?.TryGetValue(login ?? "",out user);
            return user ;
        }

        public IEnumerable<string>? Logins => users?.Keys;

        public IEnumerable<User>? AllUsers => users?.Values;

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
