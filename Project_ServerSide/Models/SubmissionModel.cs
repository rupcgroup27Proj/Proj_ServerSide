using Project_ServerSide.Models.DAL;

namespace Project_ServerSide.Models
{
    public class SubmissionModel
    {
        public int Id { get; set; }
        public IFormFile Document { get; set; }
        public int TaskId { get; set; }
        public string Description { get; set; }
        public string SubmittedAt { get; set; }

        static public int addSubmission(string uniqueFileName, string description, int id, int taskId, string submittedAt)
        {
            Submissions_DBservice dbs = new Submissions_DBservice();
            return dbs.addSubmission(uniqueFileName, description, id, taskId, submittedAt);
        }

    }
}
