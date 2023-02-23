using Microsoft.AspNetCore.Mvc.RazorPages;
using Project_ServerSide.Models.DAL;
using System.Numerics;
using System.Xml.Linq;

namespace Project_ServerSide.Models
{
    public class Student
    {
        Type student;

        double password;
        int studentId;
        string firstName;
        string lastName;
        double phone;
        string email;
        double parentPhone;
        string pictureUrl;
        int groupId { get; set; };
        DateTime startDate { get; set; };
        DateTime endDate { get; set; };

        public double Password { get => password; set => password = value; }
        public int StudentId { get => studentId; set => studentId = value; }
        public string FirstName { get => firstName; set => firstName = value; }
        public string LastName { get => lastName; set => lastName = value; }
        public double Phone { get => phone; set => phone = value; }
        public string Email { get => email; set => email = value; }
        public double ParentPhone { get => parentPhone; set => parentPhone = value; }
        public string PictureUrl { get => pictureUrl; set => pictureUrl = value; }

        static List<Student> StudentList = new List<Student>();

        public Student(double password,int studentId,string firstName,string lastName,double phone,string email,double parentPhone)
        {
            Password = password;
            StudentId = studentId;
            FirstName= firstName;
            LastName=lastName;
            Phone=phone;
            Email=email;
            ParentPhone=parentPhone;
        }

        public int Insert()//insetrt new students to DB
        {
            Students_DBservices dbs = new Students_DBservices();
            return dbs.Insert(this);
        }
        public List<Student> Read()
        {
            Students_DBservices dbs = new Students_DBservices();
            return dbs.Read();
        }

    }
}

