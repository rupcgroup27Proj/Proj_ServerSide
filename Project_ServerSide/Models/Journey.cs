﻿using Microsoft.AspNetCore.Mvc.RazorPages;
using Project_ServerSide.Models.DAL;
using System.Numerics;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace Project_ServerSide.Models
{
    public class Journey
    {
        int groupId;
        string schoolName;
        string teacherFirstName;
        string teacherLastName;
        int teacherId;
        string teacherEmail;
        double phoneTeacher;
        string guideFirstName;
        string guideLastName;
        int guideId;
        string guideEmail;
        double phoneGuide;
        DateTime startDate;
        DateTime endDate;


        public int GroupId { get => groupId; set => groupId = value; }
        public string SchoolName { get => schoolName; set => schoolName = value; }
        public string TeacherFirstName { get => teacherFirstName; set => teacherFirstName = value; }
        public string TeacherLastName { get => teacherLastName; set => teacherLastName = value; }
        public int TeacherId { get => teacherId; set => teacherId = value; }
        public string TeacherEmail { get => teacherEmail; set => teacherEmail = value; }
        public double PhoneTeacher { get => phoneTeacher; set => phoneTeacher = value; }
        public string GuideFirstName { get => guideFirstName; set => guideFirstName = value; }
        public string GuideLastName { get => guideLastName; set => guideLastName = value; }
        public int GuideId { get => guideId; set => guideId = value; }
        public string GuideEmail { get => guideEmail; set => guideEmail = value; }
        public double PhoneGuide { get => phoneGuide; set => phoneGuide = value; }
        public DateTime StartDate { get => startDate; set => startDate = value; }
        public DateTime EndDate { get => endDate; set => endDate = value; }


        public Journey pullSpecificJourney()
        {
            Journey_DBservices dbs = new Journey_DBservices();
            return dbs.pullSpecificJourney(this);
        }

        public static int Insert(string schoolName)
        {
            Journey_DBservices dbs = new Journey_DBservices();
            return dbs.Insert(schoolName);

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

