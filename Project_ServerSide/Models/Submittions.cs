using Microsoft.AspNetCore.Mvc.RazorPages;
using Project_ServerSide.Models.DAL;
using System.Numerics;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace Project_ServerSide.Models
{
    public class Submittion
    {

        int id;
        string firstName;
        string lastName;
        string fileURL;
        string descriptionStu;
        DateTime submittedAt;
        int grade;
        int taskId;
        int submissionId;

        public int Id { get => id; set => id = value; }
        public string FirstName { get => firstName; set => firstName = value; }
        public string LastName { get => lastName; set => lastName = value; }
        public string FileURL { get => fileURL; set => fileURL = value; }
        public string DescriptionStu { get => descriptionStu; set => descriptionStu = value; }
        public DateTime SubmittedAt { get => submittedAt; set => submittedAt = value; }
        public int Grade { get => grade; set => grade = value; }
        public int TaskId { get => taskId; set => taskId = value; }
        public int SubmissionId { get => submissionId; set => submissionId = value; }




        public List<Submittion> ReadSubList(int taskId)
        {
            Tasks_DBservices dbs = new Tasks_DBservices();
            return dbs.ReadSubList(taskId);
        }


        public bool SubmitTaskByStudent()
        {
            Tasks_DBservices dbs = new Tasks_DBservices();
            return (dbs.SubmitTaskByStudent(this) == 2) ? true : false;
        }

        public int UpdateSubmittion()
        {
            Tasks_DBservices dbs = new Tasks_DBservices();
            return dbs.UpdateSubmittion(this);
        }
        public int DeleteSubmittion(int submissionId)
        {
            Tasks_DBservices dbs = new Tasks_DBservices();
            return dbs.DeleteSubmittion(submissionId);
        }
    }
}

