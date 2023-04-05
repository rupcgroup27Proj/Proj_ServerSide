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


        // insert Journey by schoolname 
        //--------------------------------------------------------------------------------------------------
        public int Insert(string schoolName)
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

            cmd = CreateInsertJourneyCommand("spInsertJourney", con, schoolName);

            try
            {
                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                int groupId = 0;
                while (dataReader.Read())
                {
                    groupId = Convert.ToInt32(dataReader["groupId"]);
                }
                Console.WriteLine(groupId);
                return groupId;
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

        private SqlCommand CreateInsertJourneyCommand(String spName, SqlConnection con,  string schoolName)
        {

            SqlCommand cmd = new SqlCommand(); 

            cmd.Connection = con;       

            cmd.CommandText = spName;     

            cmd.CommandTimeout = 10;        

            cmd.CommandType = System.Data.CommandType.StoredProcedure; 

            //cmd.Parameters.AddWithValue("@groupId", journey.GroupId);// מקבל רק אחרי השליחה 
            cmd.Parameters.AddWithValue("@schoolName", schoolName);
            //cmd.Parameters.AddWithValue("@teacherFirstName", journey.TeacherFirstName);
            //cmd.Parameters.AddWithValue("@teacherLastName", journey.TeacherLastName);
            //cmd.Parameters.AddWithValue("@teacherId", journey.TeacherId);
            //cmd.Parameters.AddWithValue("@teacherEmail", journey.TeacherEmail);
            //cmd.Parameters.AddWithValue("@phoneTeacher", journey.PhoneTeacher);
            //cmd.Parameters.AddWithValue("@guideFirstName", journey.GuideFirstName);
            //cmd.Parameters.AddWithValue("@guideLastName", journey.GuideLastName);
            //cmd.Parameters.AddWithValue("@guideId", journey.GuideId);
            //cmd.Parameters.AddWithValue("@guideEmail", journey.GuideEmail);
            //cmd.Parameters.AddWithValue("@phoneGuide", journey.PhoneGuide);

            return cmd;
        }
       

        // read Journey 
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

            cmd = CreateReadJourneysCommand("spReadJourney", con);

            List<Journey> JourneyList = new List<Journey>();

            try
            {
                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dataReader.Read())
                {
                    Journey u = new Journey();
                    u.GroupId = Convert.ToInt32(dataReader["groupId"]);
                    u.SchoolName = dataReader["schoolName"].ToString();
                    u.TeacherFirstName = dataReader["teacherFirstName"].ToString();
                    u.TeacherLastName = dataReader["teacherLastName"].ToString();
                    u.TeacherId = Convert.ToInt32(dataReader["teacherId"]);
                    u.TeacherEmail = dataReader["teacherEmail"].ToString();
                    u.PhoneTeacher = Convert.ToDouble(dataReader["teacherPhone"]);
                    u.GuideFirstName = dataReader["guideFirstName"].ToString();
                    u.GuideLastName = dataReader["guideLastName"].ToString();
                    u.GuideId = Convert.ToInt32(dataReader["guideId"]);
                    u.GuideEmail = dataReader["guideEmail"].ToString();
                    u.PhoneGuide = Convert.ToDouble(dataReader["guidePhone"]);
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

        private SqlCommand CreateReadJourneysCommand(String spName, SqlConnection con)
        {

            SqlCommand cmd = new SqlCommand(); // create the command object

            cmd.Connection = con;              // assign the connection to the command object

            cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

            cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

            cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be stored procedure

            return cmd;
        }



        // Get Specific Journey by GroupId
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


            cmd = CreatePullCommand("spPullSpecificJourney", con, journey);// create the command
            Journey u = new Journey();
            try
            {
                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dataReader.Read())
                {
                    u.GroupId = Convert.ToInt32(dataReader["groupId"]);
                    u.SchoolName = dataReader["schoolName"].ToString();
                    u.TeacherFirstName = dataReader["teacherFirstName"].ToString();
                    u.TeacherLastName = dataReader["teacherLastName"].ToString();
                    u.TeacherId = Convert.ToInt32(dataReader["teacherId"]);
                    u.TeacherEmail = dataReader["teacherEmail"].ToString();
                    u.PhoneTeacher = Convert.ToDouble(dataReader["teacherPhone"]);
                    u.GuideFirstName = dataReader["guideFirstName"].ToString();
                    u.GuideLastName = dataReader["guideLastName"].ToString();
                    u.GuideId = Convert.ToInt32(dataReader["guideId"]);
                    u.GuideEmail = dataReader["guideEmail"].ToString();
                    u.PhoneGuide = Convert.ToDouble(dataReader["guidePhone"]);
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

        private SqlCommand CreatePullCommand(String spName, SqlConnection con, Journey journey)
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



       
        // update dates to the groups table 
        //--------------------------------------------------------------------------------------------------
        public int Update(Journey journey)
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


            cmd = CreateCommandInsertNewDates("spInsertNewDates", con, journey);

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

        private String BuildUpdateCommand(Journey journey)
        {

            StringBuilder sb = new StringBuilder();

            string command = sb.AppendFormat("update Groups set startDate = '{0}', endDate = {1} where groupId = {2}", journey.StartDate, journey.EndDate,journey.GroupId).ToString();

            return command;
        }

        private SqlCommand CreateCommandInsertNewDates(String spName, SqlConnection con, Journey journey)
        {

            SqlCommand cmd = new SqlCommand(); // create the command object

            cmd.Connection = con;              // assign the connection to the command object

            cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

            cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

            cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be stored procedure

            cmd.Parameters.AddWithValue("@groupId", journey.GroupId);

            cmd.Parameters.AddWithValue("@startDate", journey.StartDate);

            cmd.Parameters.AddWithValue("@endDate", journey.EndDate);

            return cmd;
        }



        // This method - pull Specific GroupId for jourey - screen2
        //---------------------------------------------------------------------------------
        public JourneyId readGroupId(JourneyId journeyId)
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


            cmd = CreatePullCommandSP1("spReadID_journey", con, journeyId);// create the command
            JourneyId u = new JourneyId();
            try
            {
                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dataReader.Read())
                {
                    u.GroupId = Convert.ToInt32(dataReader["groupId"]);
                    u.SchoolName = dataReader["schoolName"].ToString();

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

        private SqlCommand CreatePullCommandSP1(String spName, SqlConnection con, JourneyId journeyId)
        {

            SqlCommand cmd = new SqlCommand(); // create the command object

            cmd.Connection = con;              // assign the connection to the command object

            cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

            cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

            cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be stored procedure


            cmd.Parameters.AddWithValue("@SchoolName", journeyId.SchoolName);


            return cmd;
        }
    }
}
