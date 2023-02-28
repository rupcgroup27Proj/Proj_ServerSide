﻿using System;
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
    public class Journey_DBservices
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
        // This method inserts a Journey to the Journey table 
        //--------------------------------------------------------------------------------------------------
        public int Insert(Journey journey)
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

            cmd = CreateInsertJourneyCommandSP("spInsertJourney", con, journey);

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
        private SqlCommand CreateInsertJourneyCommandSP(String spName, SqlConnection con, Journey journey)
        {

            SqlCommand cmd = new SqlCommand(); // create the command object

            cmd.Connection = con;              // assign the connection to the command object

            cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

            cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

            cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be stored procedure

            //cmd.Parameters.AddWithValue("@groupId", journey.GroupId);
            //cmd.Parameters.AddWithValue("@startDate", journey.StartDate);
            //cmd.Parameters.AddWithValue("@endDate", journey.EndDate);
            //cmd.Parameters.AddWithValue("@schoolName", journey.SchoolName);
            //cmd.Parameters.AddWithValue("@teacherName", journey.TeacherName);
            //cmd.Parameters.AddWithValue("@phoneTeacher", journey.PhoneTeacher);
            //cmd.Parameters.AddWithValue("@teacherEmail", journey.TeacherEmail);
            //cmd.Parameters.AddWithValue("@guideName", journey.GuideName);
            //cmd.Parameters.AddWithValue("@guideId", journey.GuideId);
            //cmd.Parameters.AddWithValue("@phoneGuide", journey.PhoneGuide);
            //cmd.Parameters.AddWithValue("@guideEmail", journey.GuideEmail);


            return cmd;
        }
        //--------------------------------------------------------------------------------------------------
        // This method read Journey 
        //--------------------------------------------------------------------------------------------------

        public List<Journey> Read()

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

            //String cStr = BuildUpdateCommand(Journey);      // helper method to build the insert string

            //cmd = CreateCommand(cStr, con);             // create the command

            cmd = CreateReadJourneysCommandSP("spReadJourney", con);

            List<Journey> JourneyList = new List<Journey>();

            try
            {
                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);


                while (dataReader.Read())
                {

                    Journey u = new Journey();
                    u.GroupId = Convert.ToInt32(dataReader["groupId"]);
                    u.PhoneGuide = Convert.ToDouble(dataReader["phone"]);
                    u.GuideEmail = dataReader["Email"].ToString();
                    u.GuideName = dataReader["firstName"].ToString();
                    u.GuideId = Convert.ToInt32(dataReader["guideId"]);
                    u.PhoneTeacher = Convert.ToDouble(dataReader["phone"]);
                    u.TeacherEmail = dataReader["Email"].ToString();
                    u.SchoolName = dataReader["schoolName"].ToString();
                    u.TeacherName = dataReader["firstName"].ToString();
                    u.StartDate = Convert.ToDateTime(dataReader["StartDate"]);
                    u.EndDate = Convert.ToDateTime(dataReader["EndDate"]);
                    JourneyList.Add(u);

                }
                return JourneyList;
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
        // Create the ReadJourney SqlCommand
        //---------------------------------------------------------------------------------
        private SqlCommand CreateReadJourneysCommandSP(String spName, SqlConnection con)
        {

            SqlCommand cmd = new SqlCommand(); // create the command object

            cmd.Connection = con;              // assign the connection to the command object

            cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

            cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

            cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be stored procedure


            return cmd;
        }

        // This method - pull Specific Journey by GroupId
        //---------------------------------------------------------------------------------
        public Journey pullSpecificJourney(Journey journey)
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


            cmd = CreatePullCommandSP("spPullJourney", con, journey);// create the command
            Journey u = new Journey();
            try
            {
                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dataReader.Read())
                {
                    u.GroupId = Convert.ToInt32(dataReader["groupId"]);
                    u.PhoneGuide = Convert.ToDouble(dataReader["phone"]);
                    u.GuideEmail = dataReader["Email"].ToString();
                    u.GuideName = dataReader["firstName"].ToString();
                    u.GuideId = Convert.ToInt32(dataReader["guideId"]);
                    u.PhoneTeacher = Convert.ToDouble(dataReader["phone"]);
                    u.TeacherEmail = dataReader["Email"].ToString();
                    u.SchoolName = dataReader["schoolName"].ToString();
                    u.TeacherName = dataReader["firstName"].ToString();
                    u.StartDate = Convert.ToDateTime(dataReader["StartDate"]);
                    u.EndDate = Convert.ToDateTime(dataReader["EndDate"]);
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

        // Create the pull Specific Journey SqlCommand
        //---------------------------------------------------------------------------------
        private SqlCommand CreatePullCommandSP(String spName, SqlConnection con, Journey journey)
        {

            SqlCommand cmd = new SqlCommand(); // create the command object

            cmd.Connection = con;              // assign the connection to the command object

            cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

            cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

            cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be stored procedure


            cmd.Parameters.AddWithValue("@GroupId", journey.GroupId);
            cmd.Parameters.AddWithValue("@SchoolName", journey.SchoolName);


            return cmd;
        }

    }
}
