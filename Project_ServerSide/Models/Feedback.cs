using Project_ServerSide.Models.DAL;
using System;
using System.Net;
using System.Security.Cryptography.Xml;

namespace Project_ServerSide.Models
{
    public class Feedback
    {
        int guideId;
        string feedbackText;
        int replierId;
        int groupId;

        List<Feedback> feedback = new List<Feedback>();
  

        public int GuideId { get => guideId; set => guideId = value; }
        public string FeedbackText { get => feedbackText; set => feedbackText = value; }
        public int ReplierId { get => replierId; set => replierId = value; }
        public int GroupId { get => groupId; set => groupId = value; }






        public List<Feedback> GetFeedbackList()
        {
            feedback_DBservices dbs = new feedback_DBservices();
            return dbs.GetFeedbackList();
        }

        public static bool Insert()
        {
            feedback_DBservices dbs = new feedback_DBservices();
            return (dbs.Insert(this) == 1) ? true : false;
        }

       

    }
}
