using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Text;
using System.Xml.Linq;
using Project_ServerSide.Models;

namespace Project_ServerSide.Models.DAL
{
    public class PostsComments_DBservices
    {

        public SqlDataAdapter da;
        public DataTable dt;
        public SqlConnection connect(String conString)
        {
            // read the connection string from the configuration file
            IConfigurationRoot configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json").Build();
            string cStr = configuration.GetConnectionString("myProjDB");
            SqlConnection con = new SqlConnection(cStr);
            con.Open();
            return con;
        }

        // GetPostsComments
        //--------------------------------------------------------------------------------------------------
        public List<PostsComments> GetCommentsByPostId(int postId)
        {

            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("myProjDB"); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw ex;
            }


            cmd = CreateCommandPostsCommentsByPostId("spReadPostsCommentsByPostId", con, postId);// create the command

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


                    tempList.Add(tempPostsComments);

                }
                return tempList;

            }
            catch (Exception ex)
            {

                throw ex;
            }

            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
            }

        }

        private SqlCommand CreateCommandPostsCommentsByPostId(string spName, SqlConnection con, int postId)
        {

            SqlCommand cmd = new SqlCommand(); // create the command object

            cmd.Connection = con;              // assign the connection to the command object

            cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

            cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

            cmd.CommandType = CommandType.StoredProcedure; // the type of the command, can also be stored procedure


            cmd.Parameters.AddWithValue("@postId", postId);


            return cmd;
        }


        // InsertCommentToPost
        //--------------------------------------------------------------------------------------------------

        public int InsertPostsComments(PostsComments postsComments)
        {

            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("myProjDB"); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }


            cmd = CreateCommandInsertPostsComments("spInsertPostsComments", con, postsComments);             // create the command

            try
            {
                int numEffected = cmd.ExecuteNonQuery();
                return numEffected;
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
            }

        }

        private SqlCommand CreateCommandInsertPostsComments(String spName, SqlConnection con, PostsComments postsComments)
        {

            SqlCommand cmd = new SqlCommand(); // create the command object

            cmd.Connection = con;              // assign the connection to the command object

            cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

            cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

            cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be stored procedure

            cmd.Parameters.AddWithValue("@studentId", postsComments.StudentId);
            cmd.Parameters.AddWithValue("@postId", postsComments.PostId);


            return cmd;
        }



        // DeleteFromPostsCommentsByStudent
        //--------------------------------------------------------------------------------------------------
        public bool DeleteFromPostsCommentsByStudent(int studentId, int commentId)
        {

            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("myProjDB"); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw ex;
            }

            cmd = CreateCommandDeleteFromPostsCommentsByStudent("spDeleteFromPostsCommentsByStudent", con,  studentId,  commentId);     // create the command

            try
            {
                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                PostsComments tempPostsComments = new PostsComments();

                while (dataReader.Read())
                {
                    tempPostsComments.StudentId = Convert.ToInt32(dataReader["studentId"]);
                    tempPostsComments.CommentId = Convert.ToInt32(dataReader["commentId"]);
                }

                if ((tempPostsComments.StudentId == studentId) && (tempPostsComments.CommentId == commentId))
                    return true ;

                else
                {
                    return false ;
                }
            }
            catch (Exception ex)
            {
                // write to log
                throw ex;
            }

            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
            }

        }

        private SqlCommand CreateCommandDeleteFromPostsCommentsByStudent(string spName, SqlConnection con, int studentId,int commentId)
        {

            SqlCommand cmd = new SqlCommand(); // create the command object

            cmd.Connection = con;              // assign the connection to the command object

            cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

            cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

            cmd.CommandType = CommandType.StoredProcedure; // the type of the command, can also be stored procedure

            cmd.Parameters.AddWithValue("@studentId", studentId);
            cmd.Parameters.AddWithValue("@commentId", commentId);


            return cmd;
        }



        // DeleteFromPostsCommentsByTeacher
        //--------------------------------------------------------------------------------------------------
        public int DeleteFromPostsCommentsByTeacher(int teacherId, int commentId)
        {

            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("myProjDB"); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw ex;
            }

            cmd = CreateCommandDeleteFromPostsCommentsByTeacher("spDeleteFromPostsCommentsByTeacher", con, teacherId, commentId);     // create the command


            try
            {
                int numEffected = cmd.ExecuteNonQuery(); // execute the command
                return numEffected;
            }
            catch (Exception ex)
            {
                // write to log
                throw ex;
            }

            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
            }

        }

        private SqlCommand CreateCommandDeleteFromPostsCommentsByTeacher(string spName, SqlConnection con, int teacherId, int commentId)
        {

            SqlCommand cmd = new SqlCommand(); // create the command object

            cmd.Connection = con;              // assign the connection to the command object

            cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

            cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

            cmd.CommandType = CommandType.StoredProcedure; // the type of the command, can also be stored procedure

            cmd.Parameters.AddWithValue("@teacherId", teacherId);
            cmd.Parameters.AddWithValue("@commentId", commentId);


            return cmd;
        }


    }
}
