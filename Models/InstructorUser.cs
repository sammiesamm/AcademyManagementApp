namespace AcademyManager.Models
{
    public class InstructorUser : User
    {
        public string Certificate { get; set; }
        public string Speciality { get; set; }
        public async Task<bool> UpdateScore(string termID, string courseID, string classID, Dictionary<string, StudentRecord> score)
        {
            DatabaseManager database = new DatabaseManager();
            bool success = await database.UpdateScoreAsync(termID, courseID, classID, score);
            if (success) return true;
            return false;
        }
        public void UpdateInfo(InstructorUser user)
        {
            if (user == null) return;
            if (this.Fullname != user.Fullname) this.Fullname = user.Fullname;
            if (this.Email != user.Email) this.Email = user.Email;
            if (this.Birthday != user.Birthday) this.Birthday = user.Birthday;
            if (this.Faculty != user.Faculty) this.Faculty = user.Faculty;
            if (this.AvatarBase64 != user.AvatarBase64) this.AvatarBase64 = user.AvatarBase64;
            if (this.Certificate != user.Certificate) this.Certificate = user.Certificate;
            if (this.Speciality != user.Speciality) this.Speciality = user.Speciality;
        }
        public InstructorUser(string id, string fullname, string email, DateOnly birthday, string faculty, string avt, string cert, string spec)
            : base(id, fullname, email, birthday, faculty, avt)
        {
            Certificate = cert;
            Speciality = spec;
        }
    }
}
