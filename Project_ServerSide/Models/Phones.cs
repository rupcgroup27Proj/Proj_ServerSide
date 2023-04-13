using Microsoft.AspNetCore.Mvc.RazorPages;
using Project_ServerSide.Models.DAL;
using System.Numerics;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace Project_ServerSide.Models
{
    public class Phones
    {
        int id;
        string title;
        string phone;
        string notes;
        int groupId;

        public string Phone { get => phone; set => phone = value; }
        public string Title { get => title; set => title = value; }
        public string Notes { get => notes; set => notes = value; }
        public int Id { get => id; set => id = value; }
        public int GroupId { get => groupId; set => groupId = value; }
     
        public List<Phones> ReadPhoneList(int groupId)
        {
            Phones_DBservices dbs = new Phones_DBservices();
            return dbs.ReadPhoneList(groupId);
        }

        public Phones pullEmbassy()
        {
            Phones_DBservices dbs = new Phones_DBservices();
            return dbs.pullEmbassy(this);
        }

        public bool InsertPhone()
        {
            Phones_DBservices dbs = new Phones_DBservices();
            return (dbs.InsertPhone(this) == 1) ? true : false;
        }

        public int UpdatePhone()
        {
            Phones_DBservices dbs = new Phones_DBservices();
            return dbs.UpdatePhone(this);
        }

        public int DeletePhone(int id)
        {
            Phones_DBservices dbs = new Phones_DBservices();
            return dbs.DeletePhone(id);
        }      
    }
}

