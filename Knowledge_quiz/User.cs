
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

        public DateTime RegistrationDate { get; }

        public LPass LoginPass { get; }

        public User(LPass lPass,string name, DateTime date)
        {
            if (string.IsNullOrEmpty(name)) throw new ApplicationException(" Не вірне ім'я користувача");
            LoginPass = lPass;
            Name = name;
            Date = date;
            RegistrationDate = DateTime.Now;
        }

        public User(SerializationInfo info, StreamingContext context)
        {
            LoginPass = info.GetValue("LogPass", typeof(LPass)) as LPass ?? new(string.Empty, string.Empty);
            Name = info.GetString("name") ?? string.Empty;
            Date = info.GetDateTime("date") ;
            RegistrationDate = info.GetDateTime("RegistrationDate");
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("LogPass", LoginPass);
            info.AddValue("name", Name);   
            info.AddValue ("date", Date);
            info.AddValue("RegistrationDate", RegistrationDate);

        }

        public override string ToString()
        {
           return $"{("\""+LoginPass?.Login+ "\""),-Quiz.loginMaxLenght}  {Name}";
        }
    }
}
