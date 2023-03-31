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


        static List<Phones> PhoneList = new List<Phones>();

      
        public int Update()
        {
            Phones_DBservices dbs = new Phones_DBservices();
            return dbs.Update(this);

        }

        public bool Insert()
        {
            Phones_DBservices dbs = new Phones_DBservices();

            if (dbs.Insert(this) == 1)
            {
                PhoneList.Add(this);
                return true;
            }
            return false;
        }

        public List<Phones> Read()
        {
            Phones_DBservices dbs = new Phones_DBservices();
            return dbs.Read();
        }

        public Phones pullEmbassy()
        {
            Phones_DBservices dbs = new Phones_DBservices();
            return dbs.pullEmbassy(this);
        }

    }
}

