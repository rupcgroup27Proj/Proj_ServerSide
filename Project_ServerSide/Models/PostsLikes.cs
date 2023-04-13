using Project_ServerSide.Models.DAL;

namespace Project_ServerSide.Models
{
    public class PostsLikes
    {
        int studentId;
        int postId;


        public int StudentId { get => studentId; set => studentId = value; }
        public int PostId { get => postId; set => postId = value; }


        public static List<PostsLikes> ReadPostsLikes(int studentId)
        {
            PostsLikes_DBservices dbs = new PostsLikes_DBservices();
            return dbs.ReadPostsLikes(studentId);
        }

        public static bool InsertPostsLikes(int studentId, int postId)
        {
            PostsLikes_DBservices dbs = new PostsLikes_DBservices();

            return dbs.InsertPostsLikes(studentId, postId);
        }

        public static int DeletePostsLikes(int studentId, int postId)
        {
            PostsLikes_DBservices dbs = new PostsLikes_DBservices();
            return dbs.DeletePostsLikes(studentId, postId);
        }
    }
}
