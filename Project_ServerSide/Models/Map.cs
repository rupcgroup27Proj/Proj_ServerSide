using ExcelDataReader.Log;
using Project_ServerSide.Models.DAL;

namespace Project_ServerSide.Models
{
    public class Map
    {
        int studentId;
        string locationName;
        int locationId;
        double latitude;
        double longitude;
        int fileID;
        string fileUrl;
        string description;

        public int StudentId { get => studentId; set => studentId = value; }
        public string LocationName { get => locationName; set => locationName = value; }
        public int LocationId { get => locationId; set => locationId = value; }
        public double Latitude { get => latitude; set => latitude = value; }
        public double Longitude { get => longitude; set => longitude = value; }
        public int FileID { get => fileID; set => fileID = value; }
        public string FileUrl { get => fileUrl; set => fileUrl = value; }
        public string Description { get => description; set => description = value; }


        static public int newMapComponent(int studentId, double lon, double lat, string locationName)
        {
            Map_DBservices dbs = new Map_DBservices();
            return dbs.newMapComponent(studentId, lon, lat, locationName);
        }

        static public object getStudentMapComponents(int studentId)
        {
            Map_DBservices dbs = new Map_DBservices();
            return dbs.getStudentMapComponents(studentId);
        }

        static public int deleteLocation(int locationId)
        {
            Map_DBservices dbs = new Map_DBservices();
            return dbs.deleteLocation(locationId);
        }

        static public int deleteImage(int fileId)
        {
            Map_DBservices dbs = new Map_DBservices();
            return dbs.deleteImage(fileId);
        }

        static public int addImageToLocation(string uniqueFileName, string description, int studentId, int locationId)
        {
            Map_DBservices dbs = new Map_DBservices();
            return dbs.addImageToLocation(uniqueFileName, description, studentId, locationId);
        }
    }
}
