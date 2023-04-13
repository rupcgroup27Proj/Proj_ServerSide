using Microsoft.AspNetCore.Mvc.RazorPages;
using Project_ServerSide.Models.DAL;
using System.Numerics;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace Project_ServerSide.Models
{
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
        bool isAdmin;
        DateTime startDate;
        DateTime endDate;


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


        public bool InsertGuide()
        {
            Guides_DBservices dbs = new Guides_DBservices();
            return (dbs.InsertGuide(this) == 2) ? true : false;
        }
    }
}

