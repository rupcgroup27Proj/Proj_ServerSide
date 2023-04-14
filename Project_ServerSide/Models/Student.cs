using Microsoft.AspNetCore.Mvc.RazorPages;
using Project_ServerSide.Models.DAL;
using System.Numerics;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace Project_ServerSide.Models
{
    public class Student
    {
        string type;
        string password;
        int studentId;
        string firstName;
        string lastName;
        double phone;
        string email;
        double parentPhone;
        string pictureUrl;
        int groupId;
        DateTime startDate;
        DateTime endDate;


        public string Password { get => password; set => password = value; }
        public int StudentId { get => studentId; set => studentId = value; }
        public string FirstName { get => firstName; set => firstName = value; }
        public string LastName { get => lastName; set => lastName = value; }
        public double Phone { get => phone; set => phone = value; }
        public string Email { get => email; set => email = value; }
        public double ParentPhone { get => parentPhone; set => parentPhone = value; }
        public string PictureUrl { get => pictureUrl; set => pictureUrl = value; }
        public int GroupId { get => groupId; set => groupId = value; }
        public DateTime StartDate { get => startDate; set => startDate = value; }
        public DateTime EndDate { get => endDate; set => endDate = value; }
        public string Type { get => type; set => type = value; }


        public List<Student> GetGroupStudents(int groupId)
        {
            Students_DBservices dbs = new Students_DBservices();
            return dbs.GetGroupStudents(groupId);
        }

        public Student GetSpecificStudent()
        {
            Students_DBservices dbs = new Students_DBservices();
            return dbs.GetSpecificStudent(this);
        }

        public bool InsertStudent()
        {
            Students_DBservices dbs = new Students_DBservices();
            return (dbs.InsertStudent(this) == 2) ? true : false;
        }
  
        public int UpdateStudent()
        {
            Students_DBservices dbs = new Students_DBservices();
            return dbs.UpdateStudent(this);
        }

        public int DeleteFromGroup(int studentId)
        {
            Students_DBservices dbs = new Students_DBservices();
            return dbs.DeleteFromGroup(studentId);
        }
    }
}

