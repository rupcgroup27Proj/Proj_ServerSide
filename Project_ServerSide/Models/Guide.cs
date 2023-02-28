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





    public class Guide
    {
        string type;
        string password;
        int guideId;
        string firstName;
        string lastName;
        double phone;
        string email;
        string pictureUrl;
        int groupId;
        DateTime startDate;
        DateTime endDate;
        bool isAdmin;
        public string Password { get => password; set => password = value; }
        public int GuideId { get => guideId; set => guideId = value; }
        public string FirstName { get => firstName; set => firstName = value; }
        public string LastName { get => lastName; set => lastName = value; }
        public double Phone { get => phone; set => phone = value; }
        public string Email { get => email; set => email = value; }
        public string PictureUrl { get => pictureUrl; set => pictureUrl = value; }
        public int GroupId { get => groupId; set => groupId = value; }
        public DateTime StartDate { get => startDate; set => startDate = value; }
        public DateTime EndDate { get => endDate; set => endDate = value; }
        public string Type { get => type; set => type = value; }
        public bool IsAdmin { get => isAdmin; set => isAdmin = value; }


        static List<Guide>  GuidetList = new List<Guide>();

        public Guide Login()
        {
            Guides_DBservices dbs = new Guides_DBservices();
            return dbs.Login(this);
        }

        public bool Insert()//insetrt new Guide to DB
        {
            Guides_DBservices dbs = new Guides_DBservices();

            if (dbs.Insert(this) == 1)
            {
                GuidetList.Add(this);
                return true;
            }
            return false;
        }
        //public List<Guide> Read()
        //{
        //    Guides_DBservices dbs = new Guides_DBservices();
        //    return dbs.Read();
        //}

    }
}

