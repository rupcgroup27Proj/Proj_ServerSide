using Project_ServerSide.Models.DAL;
using System.Net;

namespace Project_ServerSide.Models
{
    public class FavList
    {
        int studentId;
        int postId;

        public int StudentId { get => studentId; set => studentId = value; }
        public int PostId { get => postId; set => postId = value; }


        public static List<FavList> ReadByStudentId(int studentId)
        {
            FavList_DBservices dbs = new FavList_DBservices();
            return dbs.FavListByStudentId(studentId);
        }

        public static bool Insert(int studentId ,int postId)
        {
            FavList_DBservices dbs = new FavList_DBservices();
            return dbs.InsertFavPost(studentId, postId);

        }

        public int Delete(int studentId, int postId)
        {
            FavList_DBservices dbs = new FavList_DBservices();
            return dbs.DeletePostFromListFav(studentId, postId);
        }
    }
}
