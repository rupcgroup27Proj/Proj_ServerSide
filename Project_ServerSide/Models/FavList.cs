using Project_ServerSide.Models.DAL;
using System.Net;

namespace Project_ServerSide.Models
{
    public class FavList
    {
        int studentId;
        int postId;
        string fileUrl;
        List<Tag> tags = new List<Tag>();
        public int StudentId { get => studentId; set => studentId = value; }
        public int PostId { get => postId; set => postId = value; }
        public string FileUrl { get => fileUrl; set => fileUrl = value; }

        public List<Tag> Tags { get => tags; set => tags = value; }

        public static string ReadByStudentId(int studentId)
        {
            FavList_DBservices dbs = new FavList_DBservices();
            return dbs.FavListByStudentId(studentId);
        }

        public static bool Insert(int studentId ,int postId)
        {
            FavList_DBservices dbs = new FavList_DBservices();
            return dbs.InsertFavPost(studentId, postId);
        }

        public static int Delete(int studentId, int postId)
        {
            FavList_DBservices dbs = new FavList_DBservices();
            return dbs.DeletePostFromListFav(studentId, postId);
        }
    }
}
