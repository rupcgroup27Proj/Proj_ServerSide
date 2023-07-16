using Project_ServerSide.Models.DAL;

namespace Project_ServerSide.Models
{
    public class Submission
    {

        int id;
        string firstName;
        string lastName;
        string fileURL;
        string description;
        DateTime submittedAt;
        int grade;
        int taskId;
        int submissionId;

        public int Id { get => id; set => id = value; }
        public string FirstName { get => firstName; set => firstName = value; }
        public string LastName { get => lastName; set => lastName = value; }
        public string FileURL { get => fileURL; set => fileURL = value; }
        public string Description { get => description; set => description = value; }
        public DateTime SubmittedAt { get => submittedAt; set => submittedAt = value; }
        public int Grade { get => grade; set => grade = value; }
        public int TaskId { get => taskId; set => taskId = value; }
        public int SubmissionId { get => submissionId; set => submissionId = value; }


        public static List<Submission> ReadSubList(int taskId)
        {
            Submissions_DBservice dbs = new Submissions_DBservice();
            return dbs.ReadSubList(taskId);
        }

        //public bool SubmitTaskByStudent()
        //{
        //    Submissions_DBservice dbs = new Submissions_DBservice();
        //    return (dbs.SubmitTaskByStudent(this) == 2) ? true : false;
        //}

        public int UpdateSubmittion()
        {
            Submissions_DBservice dbs = new Submissions_DBservice();
            return dbs.UpdateSubmittion(this);
        }

        public int DeleteSubmittion(int submissionId)
        {
            Submissions_DBservice dbs = new Submissions_DBservice();
            return dbs.DeleteSubmittion(submissionId);
        }
    }
}

