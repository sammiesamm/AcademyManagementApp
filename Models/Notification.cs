namespace AcademyManager.Models
{
    public class Notification
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public DateTime UpdateDate { get; set; }
        public Notification(int id, string title, string message, DateTime updateDate)
        {
            ID = id;
            Title = title;
            Message = message;
            UpdateDate = updateDate;
        }
    }
}
