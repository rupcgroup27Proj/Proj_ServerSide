using System.Data.SqlClient;
using System.Data;
using System.Globalization;
using System.Threading.Tasks;

namespace Project_ServerSide.Models.DAL
{
    public class Submissions_DBservice
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


        //get which student submit task 
        //-----------------------------------------------------------------------------------
        public List<Submission> ReadSubList(int taskId)
        {
            SqlConnection con;
            SqlCommand cmd;

            try
            { con = connect("myProjDB"); }
            catch (Exception ex)
            { throw (ex); }

            cmd = CreateReadsubCommand("spReadWhoDidTask", con, taskId);

            List<Submission> SubList = new List<Submission>();

            try
            {
                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dataReader.Read())
                {
                    Submission u = new Submission();
                    u.FirstName = dataReader["firstName"].ToString();
                    u.LastName = dataReader["lastName"].ToString();
                    u.Id = Convert.ToInt32(dataReader["studentId"]);
                    u.Description = dataReader["description"].ToString();
                    u.FileURL = dataReader["fileUrl"].ToString();
                    u.TaskId = Convert.ToInt32(dataReader["taskId"]);
                    u.SubmittedAt = Convert.ToDateTime(dataReader["submittedAt"]);
                    u.SubmissionId = Convert.ToInt32(dataReader["submissionId"]);
                    u.Grade = Convert.ToInt32(dataReader["grade"]);

                    SubList.Add(u);

                }
                return SubList;
            }
            catch (Exception ex)
            { throw (ex); }

            finally
            {
                if (con != null)
                    con.Close();
            }
        }
        private SqlCommand CreateReadsubCommand(String spName, SqlConnection con, int taskId)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = spName;
            cmd.CommandTimeout = 10;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@taskId", taskId);
            return cmd;
        }



        //submit Tasks by Student
        //-----------------------------------------------------------------------------------

        public int addSubmission(string uniqueFileName, string description, int id, int taskId, string submittedAt)
        {
            SqlConnection con;
            SqlCommand cmd;

            try
            { con = connect("myProjDB"); }
            catch (Exception ex)
            { throw (ex); }

            try
            {
                cmd = CreateSubmitTasksCommand("spSubmitByStudent", con, uniqueFileName, description, id, taskId, submittedAt);
                return cmd.ExecuteNonQuery();
            }
            finally
            {
                if (con != null)
                    con.Close();
            }
        }

        private SqlCommand CreateSubmitTasksCommand(String spName, SqlConnection con, string uniqueFileName, string description, int id, int taskId, string submittedAt)
        {
            DateTime dateTime;
            DateTime.TryParseExact(submittedAt, "MM/dd/yy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime);
            Console.WriteLine(dateTime);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = spName;
            cmd.CommandTimeout = 10;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@taskId", taskId);
            cmd.Parameters.AddWithValue("@description", description);
            cmd.Parameters.AddWithValue("@submittedAt", dateTime);
            cmd.Parameters.AddWithValue("@fileUrl", uniqueFileName);

            return cmd;
        }

        //delete Submittion 
        //-----------------------------------------------------------------------------------
        public int DeleteSubmittion(int submissionId)
        {
            SqlConnection con;
            SqlCommand cmd;

            try
            { con = connect("myProjDB"); }
            catch (Exception ex)
            { throw (ex); }

            cmd = CreateCommandDelete("spDeletesubmission", con, submissionId);

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
        private SqlCommand CreateCommandDelete(String spName, SqlConnection con, int submissionId)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = spName;
            cmd.CommandTimeout = 10;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@submissionId", submissionId);

            return cmd;
        }


        // update Submissiom
        //-----------------------------------------------------------------------------------
        public int UpdateSubmittion(Submission Submission)
        {
            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("myProjDB");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            cmd = CreateCommandWithStoredProcedure("spUpdateSubmission", con, Submission);

            try
            {
                int numEffected = cmd.ExecuteNonQuery();
                return numEffected;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (con != null)
                    con.Close();
            }
        }
        private SqlCommand CreateCommandWithStoredProcedure(string spName, SqlConnection con, Submission Submission)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = spName;
            cmd.CommandTimeout = 10;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@submissionId", Submission.SubmissionId);
            cmd.Parameters.AddWithValue("@grade", Submission.Grade);

            return cmd;
        }

        //Get all submission of a student
        public List<int> GetAllStudentSubmissions(int studentId)
        {
            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("myProjDB");
            }
            catch (Exception ex)
            { throw (ex); }

            cmd = spGetAllStudentSubmissions("spGetAllStudentSubmissions", con, studentId);

            List<int> taskIds = new List<int>();

            try
            {
                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dataReader.Read())
                {
                    int a = Convert.ToInt32(dataReader["taskId"]);
                    taskIds.Add(a);
                }
                return taskIds;
            }
            catch (Exception ex)
            { throw (ex); }

            finally
            {
                if (con != null)
                    con.Close();
            }
        }

        private SqlCommand spGetAllStudentSubmissions(String spName, SqlConnection con, int studentId)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = spName;
            cmd.CommandTimeout = 10;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@studentId", studentId);
            return cmd;
        }

        //Get student's specific submission
        public string GetSubmission(int studentId, int taskId)
        {
            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("myProjDB");
            }
            catch (Exception ex)
            { throw (ex); }

            cmd = spGetSpecificSubmission("spGetSpecificSubmission", con, studentId, taskId);

            try
            {
                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                string s = "";
                while (dataReader.Read())
                {
                    s = (dataReader["fileUrl"]).ToString();
                }
                return s;
            }
            catch (Exception ex)
            { throw (ex); }

            finally
            {
                if (con != null)
                    con.Close();
            }
        }

        private SqlCommand spGetSpecificSubmission(String spName, SqlConnection con, int studentId, int taskId)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = spName;
            cmd.CommandTimeout = 10;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@studentId", studentId);
            cmd.Parameters.AddWithValue("@taskId", taskId);
            return cmd;
        }


        
        //Get student's specific submission grade
        public int GetSubmissionData(int studentId, int taskId)
        {
            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("myProjDB");
            }
            catch (Exception ex)
            { throw (ex); }

            cmd = spGetSpecificSubmissionData("spGetSpecificSubmissionData", con, studentId, taskId);

            try
            {
                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                int a = 0;
                while (dataReader.Read())
                {
                    a = Convert.ToInt32(dataReader["grade"]);
                }
                return a;
            }
            catch (Exception ex)
            { throw (ex); }

            finally
            {
                if (con != null)
                    con.Close();
            }
        }

        private SqlCommand spGetSpecificSubmissionData(String spName, SqlConnection con, int studentId, int taskId)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = spName;
            cmd.CommandTimeout = 10;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@studentId", studentId);
            cmd.Parameters.AddWithValue("@taskId", taskId);
            return cmd;
        }


    }
}
