using Microsoft.AspNetCore.Mvc.RazorPages;
using Project_ServerSide.Models.DAL;
using System.Numerics;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace Project_ServerSide.Models
{
    //Type: x,
    //GroupId: x,
    //UserId: x,
    //Password: x,
    //FirstName: x,
    //LastName: x,
    //Phone: x,
    //Email: x,
    //PictureUrl: x,
    //ParentPhone: (יכול להיות נאל, כי לא לכולם יש)
    //IsAdmin: (יכול להיות נאל, רק למדריכים יש)
    //   StartDate: מקבל תאריך
    //EndDate: מקבל תאריך
    //type יכול להיות Admin, Student, Guide, Teacher





    public class Teacher
    {
        string type;
        string password;
        int teacherId;
        string firstName;
        string lastName;
        double phone;
        string email;
        string pictureUrl;
        int groupId;
        DateTime startDate;
        DateTime endDate;

        public string Password { get => password; set => password = value; }
        public int TeacherId { get => teacherId; set => teacherId = value; }
        public string FirstName { get => firstName; set => firstName = value; }
        public string LastName { get => lastName; set => lastName = value; }
        public double Phone { get => phone; set => phone = value; }
        public string Email { get => email; set => email = value; }
        public string PictureUrl { get => pictureUrl; set => pictureUrl = value; }
        public int GroupId { get => groupId; set => groupId = value; }
        public DateTime StartDate { get => startDate; set => startDate = value; }
        public DateTime EndDate { get => endDate; set => endDate = value; }
        public string Type { get => type; set => type = value; }

        static List<Teacher> StudentList = new List<Teacher>();

        public Teacher Login()
        {
            Teachers_DBservices dbs = new Teachers_DBservices();
            return dbs.Login(this);
        }

        public bool Insert()//insetrt new Teacher to DB
        {
            Teachers_DBservices dbs = new Teachers_DBservices();

            if (dbs.Insert(this) == 1)
            {
                StudentList.Add(this);
                return true;
            }
            return false;
        }
      
    }
}

