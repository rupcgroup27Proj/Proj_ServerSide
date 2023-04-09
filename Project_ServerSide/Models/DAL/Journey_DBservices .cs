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
using System.Text.Json;

namespace Project_ServerSide.Models.DAL
{
    public class Journey_DBservices
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


        // ================================ | MAIN FUNCTIONS | ================================== //

        // insert Journey by schoolname 
        public int Insert(string schoolName)
        {
            SqlConnection con;
            SqlCommand cmd;

            try
            { con = connect("myProjDB"); }
            catch (Exception ex)
            { throw ex; }

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
            { throw ex; }
            finally
            {
                if (con != null)
                    con.Close();
            }
        }


        public List<Journey> Read()
        {
            SqlConnection con;
            SqlCommand cmd;

            try
            { con = connect("myProjDB"); }
            catch (Exception ex)
            { throw (ex); }

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
            { throw (ex); }
            finally
            {
                if (con != null)
                    con.Close();
            }
        }


        // Get Specific Journey by GroupId
        public Journey pullSpecificJourney(Journey journey)
        {
            SqlConnection con;
            SqlCommand cmd;

            try
            { con = connect("myProjDB"); }
            catch (Exception ex)
            { throw (ex); }

            cmd = CreatePullCommand("spPullSpecificJourney", con, journey);

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
            { throw; }

            finally
            {
                if (con != null)
                    con.Close();
            }
        }


        //get specific journey's Dates and schoolName
        public object GetJourneyDatesAndSchoolName(int groupId)
        {
            SqlConnection con;
            SqlCommand cmd;

            try
            { con = connect("myProjDB"); }
            catch (Exception ex)
            { throw (ex); }

            cmd = spGetJourneyDatesAndSchoolName(con, groupId);


            try
            {
                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                object journey = new object();
                while (dataReader.Read())
                {
                    object tmpJ = new
                    {
                        groupId = Convert.ToInt32(dataReader["groupId"]),
                        schoolName = dataReader["schoolName"].ToString(),
                        startDate = Convert.ToDateTime(dataReader["StartDate"]),
                        endDate = Convert.ToDateTime(dataReader["EndDate"])
                    };
                    journey = tmpJ;
                }
                return journey;
            }

            catch (Exception ex)
            { throw; }

            finally
            {
                if (con != null)
                    con.Close();
            }

        }


        // update dates to the groups table 
        public int Update(Journey journey)
        {
            SqlConnection con;
            SqlCommand cmd;

            try
            { con = connect("myProjDB"); }
            catch (Exception ex)
            {
                throw (ex);
            }

            cmd = CreateCommandInsertNewDates("spInsertNewDates", con, journey);

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


        public int UpdateJourneyDates(int groupId, DateTime startDate, DateTime endDate)
        {
            SqlConnection con;
            SqlCommand cmd;

            try
            { con = connect("myProjDB"); }
            catch (Exception ex)
            { throw (ex); }

            cmd = spUpdateJourneyDates(con, groupId, startDate, endDate);

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



        // ================================ | STORED PROCEDURES | ================================== //

        private SqlCommand CreateInsertJourneyCommand(String spName, SqlConnection con, string schoolName)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = spName;
            cmd.CommandTimeout = 10;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@schoolName", schoolName);
            return cmd;
        }

        private SqlCommand CreateReadJourneysCommand(String spName, SqlConnection con)
        {

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = spName;
            cmd.CommandTimeout = 10;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            return cmd;
        }

        private SqlCommand CreatePullCommand(String spName, SqlConnection con, Journey journey)
        {

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = spName;
            cmd.CommandTimeout = 10;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@GroupId", journey.GroupId);
            cmd.Parameters.AddWithValue("@SchoolName", journey.SchoolName);
            return cmd;
        }

        private SqlCommand spGetJourneyDatesAndSchoolName(SqlConnection con, int groupId)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = "spGetJourneyDatesAndSchoolName";
            cmd.CommandTimeout = 10;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@groupId", groupId);
            return cmd;
        }

        private SqlCommand CreateCommandInsertNewDates(String spName, SqlConnection con, Journey journey)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = spName;
            cmd.CommandTimeout = 10;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@groupId", journey.GroupId);
            cmd.Parameters.AddWithValue("@startDate", journey.StartDate);
            cmd.Parameters.AddWithValue("@endDate", journey.EndDate);
            return cmd;
        }

        private SqlCommand spUpdateJourneyDates(SqlConnection con, int groupId, DateTime startDate, DateTime endDate)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = "spUpdateJourneyDates";
            cmd.CommandTimeout = 10;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@groupId", groupId);
            cmd.Parameters.AddWithValue("@startDate", startDate);
            cmd.Parameters.AddWithValue("@endDate", endDate);
            return cmd;
        }
    }
}
