using Project_ServerSide.Models.DAL;
using System.Net;

namespace Project_ServerSide.Models
{
    public class FavList
    {
        int studentId;
        int postId;
        string fileUrl;
        string firstName;
        string lastName;
        string type;
        List<Tag> tags = new List<Tag>();
        string description;

        public int StudentId { get => studentId; set => studentId = value; }
        public int PostId { get => postId; set => postId = value; }
        public string FileUrl { get => fileUrl; set => fileUrl = value; }
        public string FirstName { get => firstName; set => firstName = value; }
        public string LastName { get => lastName; set => lastName = value; }
        public string Type { get => type; set => type = value; }
        public List<Tag> Tags { get => tags; set => tags = value; }
        public string Description { get => description; set => description = value; }

        public static string ReadFavListByStudentId(int studentId)
        {
            FavList_DBservices dbs = new FavList_DBservices();
            return dbs.ReadFavListByStudentId(studentId);
        }

        public static bool InsertFav(int studentId, int postId)
        {
            FavList_DBservices dbs = new FavList_DBservices();
            return dbs.InsertFav(studentId, postId);
        }

        public static int DeleteFav(int studentId, int postId)
        {
            FavList_DBservices dbs = new FavList_DBservices();
            return dbs.DeleteFav(studentId, postId);

        }
        public static void LowerStudentTags(int studentId, List<Tag> tags)
        {
            FavList_DBservices dbs = new FavList_DBservices();
            dbs.LowerStudentTags(studentId, tags);
        }

    }
}
