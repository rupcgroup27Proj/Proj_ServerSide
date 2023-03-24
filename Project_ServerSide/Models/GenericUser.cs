using Microsoft.AspNetCore.Mvc;
using Project_ServerSide.Models.DAL;
using System.Data.SqlClient;
using System.Data;

namespace Project_ServerSide.Models
{
    public class GenericUser
    {
        string type;
        int groupId;
        int id;
        int personalId;
        string password;
        string firstName;
        string lastName;
        string phone;
        string email;
        string pictureUrl;
        string parentPhone;
        bool isAdmin;
        DateTime startDate;
        DateTime endDate;

        public string Type { get => type; set => type = value; }
        public int GroupId { get => groupId; set => groupId = value; }
        public int Id { get => id; set => id = value; }
        public int PersonalId { get => personalId; set => personalId = value; }
        public string Password { get => password; set => password = value; }
        public string FirstName { get => firstName; set => firstName = value; }
        public string LastName { get => lastName; set => lastName = value; }
        public string Phone { get => phone; set => phone = value; }
        public string Email { get => email; set => email = value; }
        public string PictureUrl { get => pictureUrl; set => pictureUrl = value; }
        public string ParentPhone { get => parentPhone; set => parentPhone = value; }
        public bool IsAdmin { get => isAdmin; set => isAdmin = value; }
        public DateTime StartDate { get => startDate; set => startDate = value; }
        public DateTime EndDate { get => endDate; set => endDate = value; }


        static public GenericUser Login(int id, string password, string type)
        {
            Generic_DBservices dbs = new Generic_DBservices();
            return dbs.Login(id, password, type);
        }
    }
}
