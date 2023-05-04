
using System.Runtime.Serialization;


namespace KnowledgeQuiz
{
    [KnownType(typeof(LPass))]
    [KnownType(typeof(User))]
    [KnownType(typeof(Dictionary<string, User>))]
    [Serializable]
    public sealed class Users : ISerializable
    {
        private readonly Dictionary<string, User> users;

        private const string defAdminLogin = "admin";

        private const string defAdminPass = "admin";

        public LPass AdminLogPass { get; private set; }

        public void AddUser(User user) => users.Add(user.LoginPass.Login, user);
       
        public bool DellUser(string userLogin) => users.Remove(userLogin);
       
        public User? GetUser(string login)
        {
            users.TryGetValue(login, out User? user);
            return user ;
        }

        public int Count => users.Count;

        public IEnumerable<string> Logins => users.Keys;

        public IEnumerable<User> AllUsers => users.Values;

        public Users()
        {
            users = new ();
            AdminLogPass  = new(defAdminLogin, defAdminPass);
        }

        public Users(SerializationInfo info, StreamingContext context)
        {
            users = info.GetValue("Users", typeof(Dictionary<string, User>)) as Dictionary<string, User> ?? new();
            AdminLogPass = info.GetValue("AdminPassLog", typeof(LPass)) as LPass ?? new(defAdminLogin, defAdminPass);
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Users", users);
            info.AddValue("AdminPassLog", AdminLogPass);
        }
    }
}
