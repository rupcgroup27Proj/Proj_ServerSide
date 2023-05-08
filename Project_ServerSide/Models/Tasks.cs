using Microsoft.AspNetCore.Mvc.RazorPages;
using Project_ServerSide.Models.DAL;
using System.Numerics;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace Project_ServerSide.Models
{
    public class Tasks
    {
        int groupId;
        string name;
        string description;
        DateTime startingAt;
        DateTime createdAt;
        DateTime due;
        string fileURL;
        string descriptionStu;
        DateTime submittedAt;
        int grade;




        public int GroupId { get => groupId; set => groupId = value; }
        public string Name { get => name; set => name = value; }
        public string Description { get => description; set => description = value; }
        public DateTime StartingAt { get => startingAt; set => startingAt = value; }
        public DateTime CreatedAt { get => createdAt; set => createdAt = value; }
        public string FileURL { get => fileURL; set => fileURL = value; }
        public DateTime Due { get => due; set => due = value; }
        public string DescriptionStu { get => descriptionStu; set => descriptionStu = value; }
        public DateTime SubmittedAt { get => submittedAt; set => submittedAt = value; }
        public int Grade { get => grade; set => grade = value; }



        public List<Tasks> ReadTaskList(int groupId)
        {
            Tasks_DBservices dbs = new Tasks_DBservices();
            return dbs.ReadTaskList(groupId);
        }

        public bool InsertTasksbyTeacher()
        {
            Tasks_DBservices dbs = new Tasks_DBservices();
            return (dbs.InsertTasksbyTeacher(this) == 2) ? true : false;
        }
        

        //public int DeletePhone(int id)
        //{
        //    Phones_DBservices dbs = new Phones_DBservices();
        //    return dbs.DeletePhone(id);
        //}      
    }
}

