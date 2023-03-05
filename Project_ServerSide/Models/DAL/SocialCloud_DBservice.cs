using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Text;
using System.Xml.Linq;
using Project_ServerSide.Models;
using System.Text.RegularExpressions;

namespace Project_ServerSide.Models.DAL
{
    public class SocialCloud_DBservice
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



        // GetSocialCloud
        //--------------------------------------------------------------------------------------------------
        public List<SocialCloud> PostsLikesByGroupId (int groupId)
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


            cmd = CreateCommandSocialCloud("spReadSocialCloud", con, groupId);// create the command

            List<SocialCloud> tempList = new List<SocialCloud>();

            try
            {
                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);


                while (dataReader.Read())
                {
                    SocialCloud tempSocialCloud = new SocialCloud();

                    tempSocialCloud.PostId = Convert.ToInt32(dataReader["postId"]);
                    tempSocialCloud.StudentId = Convert.ToInt32(dataReader["studentId"]);
                    tempSocialCloud.TeacherId = Convert.ToInt32(dataReader["teacherId"]);
                    tempSocialCloud.FileUrl = dataReader["fileUrl"].ToString();

                    tempList.Add(tempSocialCloud);

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

        private SqlCommand CreateCommandSocialCloud(string spName, SqlConnection con, int groupId)
        {

            SqlCommand cmd = new SqlCommand(); // create the command object

            cmd.Connection = con;              // assign the connection to the command object

            cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

            cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

            cmd.CommandType = CommandType.StoredProcedure; // the type of the command, can also be stored procedure


            cmd.Parameters.AddWithValue("@groupId", groupId);


            return cmd;
        }



        // InsertToSocialCloud
        //--------------------------------------------------------------------------------------------------

        public int InsertSocialCloud(SocialCloud socialCloud)
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


            cmd = CreateCommandInsertSocialCloud("spInsertSocialCloud", con, socialCloud);             // create the command

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

        private SqlCommand CreateCommandInsertSocialCloud(String spName, SqlConnection con, SocialCloud socialCloud)
        {

            SqlCommand cmd = new SqlCommand(); // create the command object

            cmd.Connection = con;              // assign the connection to the command object

            cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

            cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

            cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be stored procedure

            cmd.Parameters.AddWithValue("@studentId", socialCloud.StudentId);
            cmd.Parameters.AddWithValue("@teacherId", socialCloud.TeacherId);

            return cmd;
        }




        // InsertTagsToPost
        //--------------------------------------------------------------------------------- 
        public int InsertTagsToPost(int tagId, int postId)
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


            cmd = CreatCommandInsertTagsToPost("spInsertTagsToPost", con, tagId, postId);             // create the command

            try
            {
                int numEffected = cmd.ExecuteNonQuery(); // execute the command
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

        private SqlCommand CreatCommandInsertTagsToPost(String spName, SqlConnection con, int tagId, int postId)
        {

            SqlCommand cmd = new SqlCommand(); // create the command object

            cmd.Connection = con;              // assign the connection to the command object

            cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

            cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

            cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be stored procedure

            cmd.Parameters.AddWithValue("@tagId", tagId);
            cmd.Parameters.AddWithValue("@postId", postId);

            return cmd;
        }



        // DeleteFromSocialCloudByStudent
        //--------------------------------------------------------------------------------------------------
        public bool DeleteFromSocialCloudByStudent(int studentId, int postId)
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

            cmd = CreateCommandDeleteFromSocialCloudByStudent("spDeleteFromSocialCloudByStudent", con, studentId, postId);     // create the command

            try
            {
                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                SocialCloud tempSocialCloud = new SocialCloud();

                while (dataReader.Read())
                {
                    tempSocialCloud.StudentId = Convert.ToInt32(dataReader["studentId"]);
                    tempSocialCloud.PostId = Convert.ToInt32(dataReader["postId"]);
                }

                if ((tempSocialCloud.StudentId == studentId) && (tempSocialCloud.PostId == postId))
                    return true;

                else
                {
                    return false;
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

        private SqlCommand CreateCommandDeleteFromSocialCloudByStudent(string spName, SqlConnection con, int studentId, int postId)
        {

            SqlCommand cmd = new SqlCommand(); // create the command object

            cmd.Connection = con;              // assign the connection to the command object

            cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

            cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

            cmd.CommandType = CommandType.StoredProcedure; // the type of the command, can also be stored procedure

            cmd.Parameters.AddWithValue("@studentId", studentId);
            cmd.Parameters.AddWithValue("@postId", postId);


            return cmd;
        }



        // DeleteFromSocialCloudByTeacher
        //--------------------------------------------------------------------------------------------------
        public int DeleteFromSocialCloudByTeacher(int teacherId, int postId)
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

            cmd = CreateCommandDeleteFromSocialCloudByTeacher("spDeleteFromSocialCloudByTeacher", con, teacherId, postId);     // create the command


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

        private SqlCommand CreateCommandDeleteFromSocialCloudByTeacher(string spName, SqlConnection con, int teacherId, int postId)
        {

            SqlCommand cmd = new SqlCommand(); // create the command object

            cmd.Connection = con;              // assign the connection to the command object

            cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

            cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

            cmd.CommandType = CommandType.StoredProcedure; // the type of the command, can also be stored procedure

            cmd.Parameters.AddWithValue("@teacherId", teacherId);
            cmd.Parameters.AddWithValue("@postId", postId);


            return cmd;
        }


    }
}
