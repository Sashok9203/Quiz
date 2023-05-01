
using System.Runtime.Serialization;


namespace KnowledgeQuiz
{
    [KnownType(typeof(LPass))]
    [Serializable]
    public class User :ISerializable
    {
        private DateTime date;

        public string Name { get; }
        
        public DateTime Date 
        {
            get => date;
            set
            {
                if (value > DateTime.Now) throw new ApplicationException($" Невірна дата народження {value}...");
                date = value;
            }
        }

        public LPass LoginPass { get; }

        public User(LPass lPass,string name, DateTime date)
        {
            if (string.IsNullOrEmpty(name)) throw new ApplicationException(" Не вірне ім'я користувача");
            LoginPass = lPass;
            Name = name;
            Date = date;
        }

        public User(SerializationInfo info, StreamingContext context)
        {
            LoginPass = info.GetValue("LogPass", typeof(LPass)) as LPass ?? new(string.Empty, string.Empty);
            Name = info.GetString("name") ?? string.Empty;
            Date = info.GetDateTime("date") ;
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("LogPass", LoginPass);
            info .AddValue("name", Name);   
            info.AddValue ("date", Date);
            
        }

        public override string ToString()
        {
            return $"\"{LoginPass?.Login}\"  {Name}";
        }
    }
}
