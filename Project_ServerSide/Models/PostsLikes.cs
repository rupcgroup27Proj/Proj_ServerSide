using Project_ServerSide.Models.DAL;

namespace Project_ServerSide.Models
{
    public class PostsLikes
    {

        int studentId;
        int postId;

        public int StudentId { get => studentId; set => studentId = value; }
        public int PostId { get => postId; set => postId = value; }


        public static List<PostsLikes> ReadByStudentId(int studentId)
        {
            PostsLikes_DBservices dbs = new PostsLikes_DBservices();
            return dbs.PostsLikesByStudentId(studentId);
        }

        public static bool Insert(int studentId, int postId)
        {
            PostsLikes_DBservices dbs = new PostsLikes_DBservices();
            return dbs.InsertPostsLikes(studentId, postId);
        }

        public int Delete(int studentId, int postId)
        {
            PostsLikes_DBservices dbs = new PostsLikes_DBservices();
            return dbs.DeleteFromPostsLikes(studentId, postId);
        }
    }
}
