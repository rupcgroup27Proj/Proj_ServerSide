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
    public class Phones_DBservices
    {
        public SqlDataAdapter da;
        public DataTable dt;


        //--------------------------------------------------------------------------------------------------
        // This method creates a connection to the database according to the connectionString name in the web.config 
        //--------------------------------------------------------------------------------------------------
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

        //--------------------------------------------------------------------------------------------------
        // This method inserts a Phone to the phones table 
        //--------------------------------------------------------------------------------------------------
        public int Insert(Phones phones)
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

            cmd = CreateInsertPhonesCommandSP("spInsertPhones", con, phones);

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


        //---------------------------------------------------------------------------------
        // Create the SqlCommand InsertCommand
        //---------------------------------------------------------------------------------
        private SqlCommand CreateInsertPhonesCommandSP(String spName, SqlConnection con, Phones phones)
        {

            SqlCommand cmd = new SqlCommand(); // create the command object

            cmd.Connection = con;              // assign the connection to the command object

            cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

            cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

            cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be stored procedure

            cmd.Parameters.AddWithValue("@phone", phones.Phone);
            cmd.Parameters.AddWithValue("@title", phones.Title);
            cmd.Parameters.AddWithValue("@notes", phones.Notes);
            cmd.Parameters.AddWithValue("@groupId", phones.GroupId);


            return cmd;
        }
        //--------------------------------------------------------------------------------------------------
        // This method read phones 
        //--------------------------------------------------------------------------------------------------

        public List<Phones> Read()

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


            //cmd = CreateCommand(cStr, con);             // create the command

            cmd = CreateReadPhonesCommandSP("spReadPhones", con);

            List<Phones> PhoneList = new List<Phones>();

            try
            {
                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);


                while (dataReader.Read())
                {
                    Phones u = new Phones();
                    u.Title = dataReader["title"].ToString();
                    u.Phone = dataReader["phone"].ToString();
                    u.Id = Convert.ToInt32(dataReader["id"]);
                    u.GroupId = Convert.ToInt32(dataReader["groupId"]);
                    u.Notes = dataReader["notes"].ToString();
                    PhoneList.Add(u);

                }
                return PhoneList;
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

        //---------------------------------------------------------------------------------
        // Create the ReadPhones SqlCommand
        //---------------------------------------------------------------------------------
        private SqlCommand CreateReadPhonesCommandSP(String spName, SqlConnection con)
        {

            SqlCommand cmd = new SqlCommand(); // create the command object

            cmd.Connection = con;              // assign the connection to the command object

            cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

            cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

            cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be stored procedure


            return cmd;
        }

        
        //--------------------------------------------------------------------------------------------------
        // This method update a phones 
        //--------------------------------------------------------------------------------------------------
        public int Update(Phones phones)
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


            cmd = CreateCommandWithStoredProcedure("spUpdatePhone", con, phones);           

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

        private String BuildUpdateCommand(Phones phones)
        {

            StringBuilder sb = new StringBuilder();
            
            string command = sb.AppendFormat("update ImportantNumbers set phone = '{0}', title = {1}, notes = {2}, groupId = {3} where id = {4}", phones.Phone, phones.Title, phones.Notes,phones.GroupId, phones.Id).ToString();

            return command;
        }

        //---------------------------------------------------------------------------------
        // Create the SqlCommand using a stored procedure
        //---------------------------------------------------------------------------------
        private SqlCommand CreateCommandWithStoredProcedure(String spName, SqlConnection con, Phones phones)
        {

            SqlCommand cmd = new SqlCommand(); // create the command object

            cmd.Connection = con;              // assign the connection to the command object

            cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

            cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

            cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be stored procedure

           
            cmd.Parameters.AddWithValue("@phone", phones.Phone);
            cmd.Parameters.AddWithValue("@title", phones.Title);
            cmd.Parameters.AddWithValue("@notes", phones.Notes);
            cmd.Parameters.AddWithValue("@id", phones.Id);
            cmd.Parameters.AddWithValue("@groupId", phones.GroupId);


            return cmd;
        }

        // This method - pull Specific embassy of Israel
        //---------------------------------------------------------------------------------
        public Phones pullEmbassy(Phones phones)
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


            cmd = CreatePullCommandSP("sppullEmbassy", con, phones);// create the command
            Phones X = new Phones();
            try
            {
                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dataReader.Read())
                {
                    X.Title = dataReader["title"].ToString();
                    X.Phone = dataReader["phone"].ToString();
                    X.Notes = dataReader["notes"].ToString();
                    
                }
                return X;

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

        // Create the pull Specific  SqlCommand
        //---------------------------------------------------------------------------------
        private SqlCommand CreatePullCommandSP(String spName, SqlConnection con, Phones phones)
        {

            SqlCommand cmd = new SqlCommand(); // create the command object

            cmd.Connection = con;              // assign the connection to the command object

            cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

            cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

            cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be stored procedure


            cmd.Parameters.AddWithValue("@title", phones.Title);


            return cmd;
        }

    }
}


