using AcademyManager.UCViews;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using System.Net.NetworkInformation;

namespace AcademyManager.Models
{
    public class DatabaseManager
    {
        private static IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = Authentication.DatabaseSecret,
            BasePath = Authentication.DatabaseURL,
        };

        FireSharp.FirebaseClient client = new(config);
        private bool NetworkConnection()
        {
            try
            {
                using (Ping ping = new Ping())
                {
                    PingReply reply = ping.Send("www.google.com", 3000);
                    return reply.Status == IPStatus.Success;
                }
            }
            catch
            {
                return false;
            }
        }
        public DatabaseManager()
        {
            if (client == null || !NetworkConnection())
            {
                CustomedMessageBox error = new CustomedMessageBox();
                error.ShowDialog();
            }
        }
        #region Account
        public async Task<bool> UpdateAccountAsync(Account acc, int type)
        {
            try
            {
                SetResponse response;
                if (type == 1)
                {
                    response = await client.SetAsync("Accounts/InstructorAccounts/" + acc.UserID, acc);
                }
                else
                {
                    response = await client.SetAsync("Accounts/StudentAccounts/" + acc.UserID, acc);
                }
                if (response == null) return false;
                return true;
            } catch
            {
                return false;
            }
        }
        public async Task SetPasswordAsync(string id, string pass, int type)
        {
            try
            {
                SetResponse response;
                if (type == 1)
                {
                    response = await client.SetAsync($"Accounts/InstructorAccounts/{id}/Password", pass);
                }
                else
                {
                    response = await client.SetAsync($"Accounts/StudentAccounts/{id}/Password", pass);
                }
                if (response == null) return;
            }
            catch
            {
                return;
            }
        }
        public async Task<Account> GetAccountAsync(string id, int type)
        {
            try
            {
                FirebaseResponse response;
                if (type == 1)
                {
                    response = await client.GetAsync("Accounts/InstructorAccounts/" + id);
                }
                else
                {
                    response = await client.GetAsync("Accounts/StudentAccounts/" + id);
                }
                Account result = response.ResultAs<Account>();
                return result;
            }
            catch
            {
                return null;
            }
        }
        #endregion
        #region Student
        public async Task<bool> UpdateStudentAsync(string uuid, StudentUser user)
        {
            try
            {
                SetResponse response = await client.SetAsync("Students/" + uuid, user);
                if (response == null) return false;
                return true;
            } catch
            {
                return false;
            }
        }
        public async Task<StudentUser> GetStudentAsync(string id)
        {
            try
            {
                Account account = await GetAccountAsync(id, 2);
                string uid = "";
                if (account != null) uid = account.UUID;
                else return null;
                FirebaseResponse response = await client.GetAsync("Students/" + uid);
                StudentUser result = response.ResultAs<StudentUser>();
                return result;
            } catch
            {
                return null;
            }
        }
        public async Task<StudentUser> GetStudentByUUIDAsync(string uuid)
        {
            try
            {
                FirebaseResponse response = await client.GetAsync("Students/" + uuid);
                StudentUser result = response.ResultAs<StudentUser>();
                return result;
            }
            catch
            {
                return null;
            }
        }
        #endregion
        #region Instructors
        public async Task<bool> UpdateInstructorAsync(string uuid, InstructorUser user)
        {
            try
            {
                SetResponse response = await client.SetAsync("Instructors/" + uuid, user);
                if (response != null) return true;
                return false;
            } catch
            {
                return false;
            }
        }
        public async Task<InstructorUser> GetInstructorAsync(string id)
        {
            try
            {
                Account account = await GetAccountAsync(id, 1);
                string uid = "";
                if (account != null) uid = account.UUID;
                else return null;
                FirebaseResponse response = await client.GetAsync("Instructors/" + uid);
                InstructorUser result = response.ResultAs<InstructorUser>();
                return result;
            }
            catch
            {
                return null;
            }
        }
        public async Task<InstructorUser> GetInstructorByUUIDAsync(string uuid)
        {
            try
            {
                FirebaseResponse response = await client.GetAsync("Instructors/" + uuid);
                InstructorUser result = response.ResultAs<InstructorUser>();
                return result;
            }
            catch
            {
                return null;
            }
        }
        #endregion
        #region Term
        public async Task<bool> UpdateTermAsync(Term term)
        {
            try
            {
                SetResponse response = await client.SetAsync("Terms/" + term.TermID, term);
                if (response != null) return true;
                return false;
            }
            catch
            {
                return false;
            }
        }
        public async Task<Term> GetTermAsync(string id)
        {
            try
            {
                FirebaseResponse response = await client.GetAsync("Terms/" + id);
                Term result = response.ResultAs<Term>();
                return result;
            }
            catch
            {
                return null;
            }
        }
        public async Task<string> GetCurrentTermAsync()
        {
            try
            {
                FirebaseResponse response = await client.GetAsync("CurrentTerm");
                string result = response.ResultAs<string>();
                return result;
            } catch
            {
                return null;
            }
        }
        #endregion
        #region Course
        public async Task<Course> GetCourseAsync(string termID, string courseId)
        {
            try
            {
                FirebaseResponse response = await client.GetAsync($"Terms/{termID}/Courses/{courseId}");
                Course result = response.ResultAs<Course>();
                return result;
            }
            catch
            {
                return null;
            }
        }
        #endregion
        #region Class
        public async Task<Class> GetClassAsync(string termID, string courseId, string classID)
        {
           try
           {
                FirebaseResponse response = await client.GetAsync($"Terms/{termID}/Courses/{courseId}/Classes/{classID}");
                Class result = response.ResultAs<Class>();
                return result;
           }
            catch
           {
                return null;
           }
        }
        public async Task<bool> UpdateScoreAsync(string termid, string courseid, string classid, Dictionary<string, StudentRecord> score)
        {
           try
           {
                string path = $"Terms/{termid}/Courses/{courseid}/Classes/{classid}/Students";
                SetResponse res = await client.SetAsync(path, score);
                if (res != null) return true;
                return false;
           }
            catch
           {
                return false;
            }
        }
        #endregion
        #region Admin
        public async Task<Admin> GetAdminAsync(string uuid)
        {
            try
            {
                FirebaseResponse response = await client.GetAsync($"Admin/{uuid}");
                Admin result = response.ResultAs<Admin>();
                return result;
            }
            catch
            {
                return null;
            }
        }
        public async Task<bool> UpdateAdminPassword(Admin ad)
        {
            try
            {
                SetResponse response = await client.SetAsync($"Admin/{ad.UUID}/Password", ad.Password);
                if (response != null) return true;
                return false;
            }
            catch
            {
                return false;
            }
        }
        #endregion
        #region Student List
        public async Task<bool> AddStudentAsync(string termid, string courseid, string classid, string stid, StudentRecord rec)
        {
            try
            {
                string path = $"Terms/{termid}/Courses/{courseid}/Classes/{classid}/Students/{stid}";
                SetResponse response = await client.SetAsync(path, rec);
                if (response != null) return true;
                return false;
            }
            catch
            {
                return false;
            }
        }
        public async Task<bool> RemoveStudentAsync(string termid, string courseid, string classid, string stid)
        {
            try
            {
                string path = $"Terms/{termid}/Courses/{courseid}/Classes/{classid}/Students/{stid}";
                FirebaseResponse response = await client.DeleteAsync(path);
                if (response != null) return true;
                return false;
            }
            catch
            {
                return false;
            }
        }
        #endregion
        #region Notifications
        public async Task RemoveNotificationAsync(string uuid, int type, int nid)
        {
            try
            {
                if (type == 1)
                {
                    string path = $"Instructors/{uuid}/Notifications/{nid}";
                    FirebaseResponse res = await client.DeleteAsync(path);
                }
                else
                {
                    string path = $"Students/{uuid}/Notifications/{nid}";
                    FirebaseResponse res = await client.DeleteAsync(path);
                }
            }
            catch
            {
                return;
            }
        }
        public async Task<bool> SendNotificationAsync(string receiverid, int type, Notification n)
        {
            try
            {
                Account acc = await GetAccountAsync(receiverid, type);
                if (acc == null) { return false; }
                string path;
                if (type == 1) path = $"Instructors/{acc.UUID}/Notifications/{n.ID}";
                else path = $"Students/{acc.UUID}/Notifications/{n.ID}";
                SetResponse response = await client.SetAsync(path, n);
                if (response != null) return true;
                return false;
            }
            catch
            {
                return false;
            }
        }
        #endregion
        #region Documents
        public async Task<bool> UploadDocumentAsync(string termid, string courseid, string classid, KeyValuePair<string, string> doc)
        {
            try
            {
                string path = $"Terms/{termid}/Courses/{courseid}/Classes/{classid}/Documents/{doc.Key}";
                SetResponse response = await client.SetAsync(path, doc.Value);
                if (response != null) return true;
                return false;
            }
            catch
            {
                return false;
            }
        }
        #endregion
    }
}
