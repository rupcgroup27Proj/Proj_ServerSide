using Project_ServerSide.Models.DAL;

namespace Project_ServerSide.Models
{
    public class PostsComments
    {
        int commentId;
        int postId;
        int studentId;
        string commentText;
        DateTime createdAt;
        string stuFirstName;
        string stuLastName;

        public int CommentId { get => commentId; set => commentId = value; }
        public int PostId { get => postId; set => postId = value; }
        public int StudentId { get => studentId; set => studentId = value; }
        public string CommentText { get => commentText; set => commentText = value; }
        public DateTime CreatedAt { get => createdAt; set => createdAt = value; }
        public string StuFirstName { get => stuFirstName; set => stuFirstName = value; }
        public string StuLastName { get => stuLastName; set => stuLastName = value; }

        public static List<PostsComments> ReadByPostId(int postId)
        {
            PostsComments_DBservices dbs = new PostsComments_DBservices();
            return dbs.GetCommentsByPostId(postId);
        }

        public bool Insert()
        {
            PostsComments_DBservices dbs = new PostsComments_DBservices();
            if (dbs.InsertPostsComments(this) == 1)
                return true;
            return false;
        }

        public static int Delete(int commentId)
        {
            PostsComments_DBservices dbs = new PostsComments_DBservices();
            return dbs.DeleteFromPostsComments(commentId);
        }
    }
}
