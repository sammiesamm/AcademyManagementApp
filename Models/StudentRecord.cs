namespace AcademyManager.Models
{
    public class StudentRecord
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public double DailyTestScore { get; set; }
        public double Project { get; set; }
        public double Mid_Term { get; set; }
        public double Final { get; set; }
        public double GPA { get; set; }
        public StudentRecord(string studentID, string name, double dailyTestScore, double project, double mid_Term, double final, double gPA)
        {
            ID = studentID;
            Name = name;
            DailyTestScore = dailyTestScore;
            Project = project;
            Mid_Term = mid_Term;
            Final = final;
            GPA = gPA;
        }
    }
}
