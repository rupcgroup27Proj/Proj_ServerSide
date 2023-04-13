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
        string firstName;
        string lastName;


        public int CommentId { get => commentId; set => commentId = value; }
        public int PostId { get => postId; set => postId = value; }
        public int StudentId { get => studentId; set => studentId = value; }
        public string CommentText { get => commentText; set => commentText = value; }
        public DateTime CreatedAt { get => createdAt; set => createdAt = value; }
        public string FirstName { get => firstName; set => firstName = value; }
        public string LastName { get => lastName; set => lastName = value; }


        public static List<PostsComments> ReadpostsComments(int postId)
        {
            PostsComments_DBservices dbs = new PostsComments_DBservices();
            return dbs.ReadpostsComments(postId);
        }

        public bool InsertPostsComments()
        {
            PostsComments_DBservices dbs = new PostsComments_DBservices();
            return (dbs.InsertPostsComments(this) == 1) ? true : false;
        }

        public static int DeletePostsComments(int commentId)
        {
            PostsComments_DBservices dbs = new PostsComments_DBservices();
            return dbs.DeletePostsComments(commentId);
        }
    }
}
