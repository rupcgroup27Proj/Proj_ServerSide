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

        public int CommentId { get => commentId; set => commentId = value; }
        public int PostId { get => postId; set => postId = value; }
        public int StudentId { get => studentId; set => studentId = value; }
        public string CommentText { get => commentText; set => commentText = value; }
        public DateTime CreatedAt { get => createdAt; set => createdAt = value; }

        public static List<PostsComments> ReadByPostId(int PostId)
        {
            PostsComments_DBservices dbs = new PostsComments_DBservices();
            return dbs.GetCommentsByPostId(PostId);
        }

        public int Insert()
        {
            PostsComments_DBservices dbs = new PostsComments_DBservices();
            return dbs.InsertPostsComments(this);
        }

        public static bool DeleteByStudent(int studentId, int commentId)
        {
            PostsComments_DBservices dbs = new PostsComments_DBservices();
            return dbs.DeleteFromPostsCommentsByStudent(studentId, commentId);
        }

        public static int DeleteByTeacher(int teacherId, int commentId)
        {
            PostsComments_DBservices dbs = new PostsComments_DBservices();
            return dbs.DeleteFromPostsCommentsByTeacher(teacherId, commentId);
        }
    }
}
