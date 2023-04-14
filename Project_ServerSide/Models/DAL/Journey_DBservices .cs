using System.Data.SqlClient;
using System.Data;


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


        //get all journeys
        //-----------------------------------------------------------------------------------
        public List<Journey> GetJourneyList()
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
        private SqlCommand CreateReadJourneysCommand(String spName, SqlConnection con)
        {

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = spName;
            cmd.CommandTimeout = 10;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            return cmd;
        }


        //get the journeys of the current user (teacher or guide)
        //-----------------------------------------------------------------------------------
        public List<Journey> GetUserJourneyList(int userId, string userType)
        {
            SqlConnection con;
            SqlCommand cmd;

            try
            { con = connect("myProjDB"); }
            catch (Exception ex)
            { throw (ex); }

            cmd = CreateReadUserJourneysCommand("spReadUserJourney", con, userId, userType);

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
        private SqlCommand CreateReadUserJourneysCommand(String spName, SqlConnection con, int userId, string userType)
        {

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = spName;
            cmd.CommandTimeout = 10;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@userId", userId);
            cmd.Parameters.AddWithValue("@userType", userType);
            return cmd;
        }


        //get specific journey's Dates and schoolName  
        //-----------------------------------------------------------------------------------
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


        // insert Journey by schoolname 
        //-----------------------------------------------------------------------------------
        public int InsertSchoolName(string schoolName)
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


        //update journey dates
        //-----------------------------------------------------------------------------------
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
