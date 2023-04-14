using System.Data.SqlClient;
using System.Data;


namespace Project_ServerSide.Models.DAL
{
    public class PostsComments_DBservices
    {
        public SqlConnection connect(String conString)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json").Build();
            string cStr = configuration.GetConnectionString("myProjDB");
            SqlConnection con = new SqlConnection(cStr);
            con.Open();
            return con;
        }


        // get post comments by postId
        //-----------------------------------------------------------------------------------
        public List<PostsComments> ReadpostsComments(int postId)
        {
            SqlConnection con;
            SqlCommand cmd;

            try
            { con = connect("myProjDB"); }
            catch (Exception ex)
            { throw ex; }

            cmd = CreateCommandPostsCommentsByPostId("spReadPostsCommentsByPostId", con, postId);

            List<PostsComments> tempList = new List<PostsComments>();

            try
            {
                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dataReader.Read())
                {
                    PostsComments tempPostsComments = new PostsComments();

                    tempPostsComments.CommentId = Convert.ToInt32(dataReader["commentId"]);
                    tempPostsComments.StudentId = Convert.ToInt32(dataReader["studentId"]);
                    tempPostsComments.CommentText = dataReader["commentText"].ToString();
                    tempPostsComments.CreatedAt = Convert.ToDateTime(dataReader["createdAt"]);
                    tempPostsComments.FirstName = dataReader["firstName"].ToString();
                    tempPostsComments.LastName = dataReader["lastName"].ToString();

                    tempList.Add(tempPostsComments);

                }
                return tempList;

            }
            catch (Exception ex)
            { throw ex; }
            finally
            {
                if (con != null)
                    con.Close();
            }
        }

        private SqlCommand CreateCommandPostsCommentsByPostId(string spName, SqlConnection con, int postId)
        {

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = spName;
            cmd.CommandTimeout = 10;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@postId", postId);
            return cmd;
        }


        // insert comment to post
        //-----------------------------------------------------------------------------------
        public int InsertPostsComments(PostsComments postsComments)
        {
            SqlConnection con;
            SqlCommand cmd;

            try
            { con = connect("myProjDB"); }
            catch (Exception ex)
            { throw (ex); }

            cmd = CreateCommandInsertPostsComments("spInsertPostsComments", con, postsComments);

            try
            {
                int numEffected = cmd.ExecuteNonQuery();
                return numEffected;
            }
            catch (Exception ex)
            { throw (ex); }
            finally
            {
                if (con != null)
                    con.Close();
            }
        }

        private SqlCommand CreateCommandInsertPostsComments(String spName, SqlConnection con, PostsComments postsComments)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = spName;
            cmd.CommandTimeout = 10;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@studentId", postsComments.StudentId);
            cmd.Parameters.AddWithValue("@postId", postsComments.PostId);
            cmd.Parameters.AddWithValue("@commentText", postsComments.CommentText);
            return cmd;
        }


        // delete from comment from post
        //-----------------------------------------------------------------------------------
        public int DeletePostsComments(int commentId)
        {
            SqlConnection con;
            SqlCommand cmd;

            try
            { con = connect("myProjDB"); }
            catch (Exception ex)
            { throw ex; }

            cmd = CreateCommandDeleteFromPostsComments("spDeleteFromPostsComments", con, commentId);

            try
            {
                int numEffected = cmd.ExecuteNonQuery();
                return numEffected;
            }
            catch (Exception ex)
            { throw ex; }
            finally
            {
                if (con != null)
                    con.Close();
            }
        }

        private SqlCommand CreateCommandDeleteFromPostsComments(string spName, SqlConnection con, int commentId)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = spName;
            cmd.CommandTimeout = 10;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@commentId", commentId);
            return cmd;
        }
    }
}
