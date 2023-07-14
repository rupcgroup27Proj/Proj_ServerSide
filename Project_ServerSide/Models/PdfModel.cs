using Project_ServerSide.Models.DAL;

namespace Project_ServerSide.Models
{
    public class PdfModel
    {
        public int GroupId { get; set; }
        public IFormFile Document { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Date { get; set; }

        static public int addPdf(string uniqueFileName, string description, string date, string name, int groupId)
        {
            Tasks_DBservices dbs = new Tasks_DBservices();
            return dbs.addPdf(uniqueFileName, description, date, name, groupId);
        }

    }
}
