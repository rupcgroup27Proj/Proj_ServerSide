using System.Data.SqlClient;
using System.Data;

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
        public int SubmitTaskByStudent(Submission Submission)
        {

            SqlConnection con;
            SqlCommand cmd;

            try
            { con = connect("myProjDB"); }
            catch (Exception ex)
            { throw ex; }

            cmd = CreateSubmitTasksCommand("spSubmitByStudent", con, Submission);

            try
            {
                int numEffected = cmd.ExecuteNonQuery();
                return numEffected;
            }
            catch (Exception ex)
            { throw ex; }

            finally
            {
                if (con != null)
                    con.Close();
            }
        }
        private SqlCommand CreateSubmitTasksCommand(String spName, SqlConnection con, Submission Submission)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = spName;
            cmd.CommandTimeout = 10;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@id", Submission.Id);
            cmd.Parameters.AddWithValue("@taskId", Submission.TaskId);
            cmd.Parameters.AddWithValue("@description", Submission.Description);
            cmd.Parameters.AddWithValue("@submittedAt", Submission.SubmittedAt);
            cmd.Parameters.AddWithValue("@fileUrl", Submission.FileURL);

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
    }
}
