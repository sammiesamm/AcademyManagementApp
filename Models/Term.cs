using Newtonsoft.Json;

namespace AcademyManager.Models
{
    public class Term
    {
        public string TermID { get; set; }
        public Dictionary<string, Course> Courses { get; set; }
        public Term(string id)
        {
            TermID = id;
            Courses = new Dictionary<string, Course>();
        }
        [JsonConstructor]
        public Term(string id, Dictionary<string, Course> c)
        {
            TermID = id;
            Courses = c;
        }
    }
}
