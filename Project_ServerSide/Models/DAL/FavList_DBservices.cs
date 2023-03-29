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
    public class FavList_DBservices
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

        // GetStudentFavList
        //--------------------------------------------------------------------------------------------------
        public List<FavList> FavListByStudentId(int studentId)
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


            cmd = CreateCommandFavListByStudentId("spReadFavListByStudentId", con, studentId);// create the command

            List<FavList> tempList = new List<FavList>();

            try
            {
                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);


                while (dataReader.Read())
                {
                    FavList tempFavList = new FavList();

                    tempFavList.StudentId = Convert.ToInt32(dataReader["studentId"]);
                    tempFavList.PostId = Convert.ToInt32(dataReader["postId"]);
                    tempFavList.FileUrl = Convert.ToString(dataReader["fileUrl"]);


                    tempList.Add(tempFavList);

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

        private SqlCommand CreateCommandFavListByStudentId(string spName, SqlConnection con, int studentId)
        {

            SqlCommand cmd = new SqlCommand(); // create the command object

            cmd.Connection = con;              // assign the connection to the command object

            cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

            cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

            cmd.CommandType = CommandType.StoredProcedure; // the type of the command, can also be stored procedure


            cmd.Parameters.AddWithValue("@studentId", studentId);


            return cmd;
        }


        // InsertPostToStudentFavList
        //--------------------------------------------------------------------------------------------------

        public bool InsertFavPost(int studentId, int postId)
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


            cmd = CreateCommandInsertFavPost("spInsertFavPostToStudent", con, studentId, postId);             // create the command

            try
            {
                int numEffected = cmd.ExecuteNonQuery();
                if (numEffected == 1)
                {
                    return true;
                }
                else return false;
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

        private SqlCommand CreateCommandInsertFavPost(String spName, SqlConnection con, int studentId, int postId)
        {

            SqlCommand cmd = new SqlCommand(); // create the command object

            cmd.Connection = con;              // assign the connection to the command object

            cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

            cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

            cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be stored procedure

            cmd.Parameters.AddWithValue("@studentId", studentId);
            cmd.Parameters.AddWithValue("@postId", postId);


            return cmd;
        }


        // DeletePostFromListFav
        //--------------------------------------------------------------------------------------------------
        public int DeletePostFromListFav(int studentId, int postId)
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

            cmd = CreateCommandDeletePostFromListFav("spDeletePostFromListFav", con, studentId, postId);     // create the command

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

        private SqlCommand CreateCommandDeletePostFromListFav(string spName, SqlConnection con, int studentId, int postId)
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
    }
}

