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


        public Student Login()
        {
            Students_DBservices dbs = new Students_DBservices();
            return dbs.Login(this);
        }

        public int Update()
        {
            Students_DBservices dbs = new Students_DBservices();
            return dbs.Update(this);
        }

        public bool Insert()
        {
            Students_DBservices dbs = new Students_DBservices();
            return (dbs.Insert(this) == 2) ? true : false;
        }

        public List<Student> Read(int groupId)
        {
            Students_DBservices dbs = new Students_DBservices();
            return dbs.Read(groupId);
        }

        public Student pullSpecificStudent()
        {
            Students_DBservices dbs = new Students_DBservices();
            return dbs.pullSpecificStudent(this);
        }

        public int DeleteFromGroupe(int groupId)
        {
            Students_DBservices dbs = new Students_DBservices();
            return dbs.DeleteFromGroupe(groupId);
        }
    }
}

