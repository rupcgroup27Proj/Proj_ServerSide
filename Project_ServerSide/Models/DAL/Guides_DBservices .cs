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
    public class Guides_DBservices
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

      
        // inserts guide
        //---------------------------------------------------------------------------------
        public int Insert(Guide guide)
        {
            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("myProjDB"); 
            }
            catch (Exception ex)
            {
                // write to log
                throw ex;
            }

            cmd = CreateInsertGuidestCommand("spInsertguides", con, guide);

            try
            {
                int numEffected = cmd.ExecuteNonQuery();
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
     
        private SqlCommand CreateInsertGuidestCommand(String spName, SqlConnection con, Guide guide)
        {

            SqlCommand cmd = new SqlCommand(); // create the command object

            cmd.Connection = con;              // assign the connection to the command object

            cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

            cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

            cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be stored procedure

            cmd.Parameters.AddWithValue("@Id", guide.GuideId);
            cmd.Parameters.AddWithValue("@password", guide.Password);
            cmd.Parameters.AddWithValue("@firstName", guide.FirstName);
            cmd.Parameters.AddWithValue("@lastName", guide.LastName);
            cmd.Parameters.AddWithValue("@phone", guide.Phone);
            cmd.Parameters.AddWithValue("@email", guide.Email);
            cmd.Parameters.AddWithValue("@isAdmin", guide.IsAdmin);
            cmd.Parameters.AddWithValue("@pictureUrl", guide.PictureUrl);
            cmd.Parameters.AddWithValue("@groupId", guide.GroupId);

            return cmd;
        }



        // login guide
        //---------------------------------------------------------------------------------
        public Guide Login(Guide guide)
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


            cmd = CreateLoginCommand("spLoginGuides", con, guide);// create the command
            Guide u = new Guide();
            try
            {
                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dataReader.Read())
                {
                    u.GuideId = Convert.ToInt32(dataReader["Id"]); 
                    u.Password = dataReader["Password"].ToString();
                    u.Email = dataReader["Email"].ToString();
                    u.FirstName = dataReader["Firstname"].ToString();
                    u.LastName = dataReader["Lastname"].ToString();
                    u.Phone = Convert.ToDouble(dataReader["Phone"]);
                    u.PictureUrl = dataReader["PictureUrl"].ToString();
                    u.GroupId = Convert.ToInt32(dataReader["groupId"]);
                    u.IsAdmin = Convert.ToBoolean(dataReader["isAdmin"]);
                    u.Type = "Guide".ToString();

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
  
        private SqlCommand CreateLoginCommand(String spName, SqlConnection con, Guide guide)
        {

            SqlCommand cmd = new SqlCommand(); // create the command object

            cmd.Connection = con;              // assign the connection to the command object

            cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

            cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

            cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be stored procedure

            cmd.Parameters.AddWithValue("@Id", guide.GuideId); 
            cmd.Parameters.AddWithValue("@Password", guide.Password);

            return cmd;
        }

    }
}
