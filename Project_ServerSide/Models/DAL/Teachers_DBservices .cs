using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Text;
using System.Xml.Linq;
using Project_ServerSide.Models;
using System.Numerics;

namespace Project_ServerSide.Models.DAL
{
    public class Teachers_DBservices
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

     
        // inserts a teacher 
        //--------------------------------------------------------------------------------------------------
        public int Insert(Teacher teacher)
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

            cmd = CreateInsertTeacherstCommand("spInsertTeachers", con, teacher);

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

        private SqlCommand CreateInsertTeacherstCommand(String spName, SqlConnection con, Teacher teacher)
        {

            SqlCommand cmd = new SqlCommand(); // create the command object

            cmd.Connection = con;              // assign the connection to the command object

            cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

            cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

            cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be stored procedure

            cmd.Parameters.AddWithValue("@Id", teacher.TeacherId);
            cmd.Parameters.AddWithValue("@password", teacher.Password);
            cmd.Parameters.AddWithValue("@firstName", teacher.FirstName);
            cmd.Parameters.AddWithValue("@lastName", teacher.LastName);
            cmd.Parameters.AddWithValue("@phone", teacher.Phone);
            cmd.Parameters.AddWithValue("@email", teacher.Email);
            cmd.Parameters.AddWithValue("@pictureUrl", teacher.PictureUrl);
            cmd.Parameters.AddWithValue("@groupId", teacher.GroupId);


            return cmd;
        }


        // login teacher
        //---------------------------------------------------------------------------------
        public Teacher Login(Teacher teacher)
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


            cmd = CreateLoginCommand("spLoginTeachers", con, teacher);// create the command
            Teacher u = new Teacher();
            try
            {
                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dataReader.Read())
                {
                    u.TeacherId = Convert.ToInt32(dataReader["Id"]); 
                    u.Password = dataReader["Password"].ToString();
                    u.Email = dataReader["Email"].ToString();
                    u.FirstName = dataReader["Firstname"].ToString();
                    u.LastName = dataReader["Lastname"].ToString();
                    u.Phone = Convert.ToDouble(dataReader["Phone"]);
                    u.PictureUrl = dataReader["PictureUrl"].ToString();
                    u.GroupId = Convert.ToInt32(dataReader["groupId"]);
                    u.Type = "Teacher".ToString();

                }
                return u;

            }
            catch (Exception ex)
            {

                throw;
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

        private SqlCommand CreateLoginCommand(String spName, SqlConnection con, Teacher teacher)
        {

            SqlCommand cmd = new SqlCommand(); // create the command object

            cmd.Connection = con;              // assign the connection to the command object

            cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

            cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

            cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be stored procedure


            cmd.Parameters.AddWithValue("@Id", teacher.TeacherId); 
            cmd.Parameters.AddWithValue("@Password", teacher.Password);

            return cmd;
        }

    }
}
