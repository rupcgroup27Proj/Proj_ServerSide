using System.Data.SqlClient;
using System.Data;


namespace Project_ServerSide.Models.DAL
{
    public class PostsLikes_DBservices
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


        // get student posts likes  
        //-----------------------------------------------------------------------------------
        public List<PostsLikes> ReadPostsLikes(int groupId)
        {
            SqlConnection con;
            SqlCommand cmd;

            try
            { con = connect("myProjDB"); }
            catch (Exception ex)
            { throw ex; }

            cmd = CreateCommandPostsLikesByStudentId("spReadPostsLikesByGroupId", con, groupId);// create the command

            List<PostsLikes> tempList = new List<PostsLikes>();

            try
            {
                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);


                while (dataReader.Read())
                {
                    PostsLikes tempPostsLikes = new PostsLikes();

                    tempPostsLikes.PostId = Convert.ToInt32(dataReader["postId"]);
                    tempPostsLikes.StudentId = Convert.ToInt32(dataReader["studentId"]);

                    tempList.Add(tempPostsLikes);

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
        private SqlCommand CreateCommandPostsLikesByStudentId(string spName, SqlConnection con, int groupId)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = spName;
            cmd.CommandTimeout = 10;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@groupId", groupId);
            return cmd;
        }


        // insert like to post   
        //-----------------------------------------------------------------------------------
        public bool InsertPostsLikes(int studentId, int postId)
        {

            SqlConnection con;
            SqlCommand cmd;

            try
            { con = connect("myProjDB"); }
            catch (Exception ex)
            { throw (ex); }

            cmd = CreateCommandInsertPostsLikes("spInsertPostsLikesToStudent", con, studentId, postId);             // create the command

            try
            {
                int numEffected = cmd.ExecuteNonQuery();
                return (numEffected == 1) ? true : false;
            }
            catch (Exception ex)
            { throw (ex); }
            finally
            {
                if (con != null)
                    con.Close();
            }

        }
        private SqlCommand CreateCommandInsertPostsLikes(String spName, SqlConnection con, int studentId, int postId)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = spName;
            cmd.CommandTimeout = 10;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@studentId", studentId);
            cmd.Parameters.AddWithValue("@postId", postId);
            return cmd;
        }


        // delete like from post 
        //-----------------------------------------------------------------------------------
        public int DeletePostsLikes(int studentId, int postId)
        {
            SqlConnection con;
            SqlCommand cmd;

            try
            { con = connect("myProjDB"); }
            catch (Exception ex)
            { throw ex; }

            cmd = CreateCommandDeleteFromPostsLikes("spDeleteFromPostsLikes", con, studentId, postId);

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
        private SqlCommand CreateCommandDeleteFromPostsLikes(string spName, SqlConnection con, int studentId, int postId)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = spName;
            cmd.CommandTimeout = 10;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@studentId", studentId);
            cmd.Parameters.AddWithValue("@postId", postId);
            return cmd;
        }
    }
}

