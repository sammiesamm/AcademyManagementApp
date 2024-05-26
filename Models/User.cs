namespace AcademyManager.Models
{
    public class User
    {
        public string ID { get; set; }
        public string Fullname { get; set; }
        public string Email { get; set; }
        public DateOnly Birthday { get; set; }
        public string Faculty { get; set; }
        public string AvatarBase64 { get; set; }
        public Dictionary<int, Notification> Notifications { get; set; }
        public List<ClassIdentifier> StudyElements { get; set; }
        private bool InSchedule(DateOnly date, Class cls)
        {
            bool inPeriod = cls.BeginDate <= date && cls.EndDate >= date;
            bool inDate = cls.Weekday == date.DayOfWeek;
            return inPeriod && inDate;
        }
        public List<Class> GetSchedule(DateOnly date, List<Class> classlist)
        {
            var list = new List<Class>();
            foreach (Class c in classlist)
            {
                if (InSchedule(date, c)) list.Add(c);
            }
            return list;
        }
        public User(string id, string fullname, string email, DateOnly birthday, string faculty, string avt)
        {
            ID = id;
            Fullname = fullname;
            Email = email;
            Birthday = birthday;
            Faculty = faculty;
            AvatarBase64 = avt;
            StudyElements = new List<ClassIdentifier>();
            Notifications = new Dictionary<int, Notification>();
        }
    }
}
