using Microsoft.AspNetCore.Mvc.RazorPages;
using Project_ServerSide.Models.DAL;
using System.Numerics;
using System.Xml.Linq;

namespace Project_ServerSide.Models
{
    //תיקונים:  //    לבדוק אם הסיסמא והמייל קיים במערכת
    //    אם כן פצצה ולשרשר את שאר הפרטים
    //    להוסיף TYPE למחלקה!
    public class Student
    {
        Type Student;

        double password;
        int studentId;
        string firstName;
        string lastName;
        double phone;
        string email;
        double parentPhone;
        string pictureUrl { get; set; };
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

        //static List<Student> StudentList = new List<Student>();

        //public List<Student> Read()
        //{
        //    DBservices dbs = new DBservices();
        //    return dbs.ReadStudent();
        //}

        public Student
        (double password,
        int studentId,
        string firstName,
        string lastName,
        double phone,
        string email,
        double parentPhone)//פונקציית הזנה של המורה 
        {
            Password = password;
            StudentId = studentId;
            FirstName= firstName;
            LastName=lastName;
            Phone=phone;
            Email=email;
            ParentPhone=parentPhone;
        }

        //public Student(string name, double age, int id)
        //{
        //    Name = name;
        //    Age = age;
        //    Id = id;
        //}


        public int Insert()//insetrt new students to DB
        {
            Students_DBservices dbs = new Students_DBservices();
            return dbs.Insert(this);
        }

        public int Update()
        {
            Students_DBservices dbs = new Students_DBservices();
            return dbs.Update(this);

        }

        public List<Student> Read()
        {
            Students_DBservices dbs = new Students_DBservices();
            //return dbs.Read();
            return null;
        }

        public void Init()
        {
            //SmartRec_DBservices dbs = new SmartRec_DBservices();
            //dbs.Init();
        }

    }
}

