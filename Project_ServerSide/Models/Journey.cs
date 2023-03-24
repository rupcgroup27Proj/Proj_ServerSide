using Microsoft.AspNetCore.Mvc.RazorPages;
using Project_ServerSide.Models.DAL;
using System.Numerics;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace Project_ServerSide.Models
{

    //שם מורה
    //טלפון מורה
    //מייל
    //שם מדריך
    //תז מדריך
    //מייל מדריך
    //טלפון מדריך

    public class Journey
    {

        int groupId;
        DateTime startDate;
        DateTime endDate;
        string schoolName;
        string teacherName;
        double phoneTeacher;
        string teacherEmail;
        string guideName;
        int guideId;
        double phoneGuide;
        string guideEmail;



        public string SchoolName { get => schoolName; set => schoolName = value; }
        public DateTime StartDate { get => startDate; set => startDate = value; }
        public DateTime EndDate { get => endDate; set => endDate = value; }
        public int GroupId { get => groupId; set => groupId = value; }
        public string TeacherName { get => teacherName; set => teacherName = value; }
        public double PhoneTeacher { get => phoneTeacher; set => phoneTeacher = value; }
        public string TeacherEmail { get => teacherEmail; set => teacherEmail = value; }
        public string GuideName { get => guideName; set => guideName = value; }
        public int GuideId { get => guideId; set => guideId = value; }
        public double PhoneGuide { get => phoneGuide; set => phoneGuide = value; }
        public string GuideEmail { get => guideEmail; set => guideEmail = value; }

        static List<Journey> JourneyList = new List<Journey>();

        public Journey pullSpecificJourney()
        {
            Journey_DBservices dbs = new Journey_DBservices();
            return dbs.pullSpecificJourney(this);
        }

         public static void Insert(string schoolName) { 
            Journey_DBservices dbs = new Journey_DBservices();
            dbs.Insert(schoolName);
           
        }
        public List<Journey> Read()
        {
            Journey_DBservices dbs = new Journey_DBservices();
            return dbs.Read();
        }

        public int Update()
        {
            Journey_DBservices dbs = new Journey_DBservices();
            return dbs.Update(this);

        }


    }
}

