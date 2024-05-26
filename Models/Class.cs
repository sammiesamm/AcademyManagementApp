using Newtonsoft.Json;

namespace AcademyManager.Models
{
    public class Class
    {
        public string ClassID { get; set; }
        public string InstructorID { get; set; }
        public string InstructorName { get; set; }
        public string TermID { get; set; }
        public string CourseID { get; set; }
        public string CourseName { get; set; }
        public int CourseCredits { get; set; }
        public Dictionary<string, string> Documents { get; set; }
        public DateOnly ExamDate { get; set; }
        public string ExamRoom { get; set; }
        public TimeOnly ExamTime { get; set; }
        public DayOfWeek Weekday { get; set; }
        public TimeOnly BeginTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public DateOnly BeginDate { get; set; }
        public DateOnly EndDate { get; set; }
        public string Room { get; set; }
        public Dictionary<string, StudentRecord> Students { get; set; }
        public Class(string classID, string instructorID, string instructorName, string termID, string courseID, string courseName, int credits,
            Dictionary<string, string> documents, DayOfWeek weekday, TimeOnly beginTime, TimeOnly endTime,
            DateOnly beginDate, DateOnly endDate, string room, DateOnly examDate, string examRoom, TimeOnly examTime)
        {
            Students = new Dictionary<string, StudentRecord>();
            ClassID = classID;
            InstructorID = instructorID;
            InstructorName = instructorName;
            TermID = termID;
            CourseID = courseID;
            CourseName = courseName;
            CourseCredits = credits;
            Documents = documents;
            Weekday = weekday;
            BeginTime = beginTime;
            EndTime = endTime;
            BeginDate = beginDate;
            EndDate = endDate;
            Room = room;
            ExamDate = examDate;
            ExamRoom = examRoom;
            ExamTime = examTime;
        }
        public Class(string classID, string instructorID, string instructorName, string termID, string courseID, string courseName, int credits,
            DayOfWeek day, TimeOnly bgT, TimeOnly endTime, DateOnly beginDate, DateOnly endDate, string room,
            DateOnly examDate, string examRoom, TimeOnly examTime)
        {
            Students = new Dictionary<string, StudentRecord>();
            ClassID = classID;
            InstructorID = instructorID;
            InstructorName = instructorName;
            TermID = termID;
            CourseID = courseID;
            CourseName = courseName;
            CourseCredits = credits;
            Weekday = day;
            BeginTime = bgT;
            EndTime = endTime;
            BeginDate = beginDate;
            EndDate = endDate;
            Room = room;
            ExamDate = examDate;
            ExamRoom = examRoom;
            ExamTime = examTime;
        }
        [JsonConstructor]
        public Class(string classID, string instructorID, string instructorName, string termID, string courseID, string courseName, int credits,
            Dictionary<string, string> documents, DayOfWeek weekday, TimeOnly beginTime, TimeOnly endTime,
            DateOnly beginDate, DateOnly endDate, string room, Dictionary<string, StudentRecord> students,
            DateOnly examDate, string examRoom, TimeOnly examTime)
            : this(classID, instructorID, instructorName, termID, courseID, courseName, credits, documents, weekday, beginTime, endTime, beginDate, endDate, room,
                  examDate, examRoom, examTime)
        {
            Students = students;
        }
    }
}
