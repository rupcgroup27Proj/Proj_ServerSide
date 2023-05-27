using System.Data.SqlClient;
using System.Data;


namespace Project_ServerSide.Models.DAL
{
    public class feedback_DBservices

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


        //get all feedback
        //-----------------------------------------------------------------------------------
        public List<Feedback> GetFeedbackList()
        {
            SqlConnection con;
            SqlCommand cmd;

            try
            { con = connect("myProjDB"); }
            catch (Exception ex)
            { throw (ex); }

            cmd = CreateReadCommand("spReadFeedback", con);

            List<Feedback> feedback = new List<Feedback>();

            try
            {
                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dataReader.Read())
                {
                    Feedback u = new Feedback();
                    u.FeedbackId = Convert.ToInt32(dataReader["feedbackId"]);
                    u.ReplierId = Convert.ToInt32(dataReader["replierId"]);
                    u.GroupId = Convert.ToInt32(dataReader["groupId"]);
                    u.FeedbackText = dataReader["feedbackText"].ToString();
                    u.GuideId = Convert.ToInt32(dataReader["guideId"]);

                    feedback.Add(u);
                }

                return feedback;
            }
            catch (Exception ex)
            { throw (ex); }
            finally
            {
                if (con != null)
                    con.Close();
            }
        }
        private SqlCommand CreateReadCommand(String spName, SqlConnection con)
        {

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = spName;
            cmd.CommandTimeout = 10;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            return cmd;
        }

        // insert Feedback by guideId  
        //-----------------------------------------------------------------------------------
        public int Insert(Feedback feedback)
        {
            SqlConnection con;
            SqlCommand cmd;

            try
            { con = connect("myProjDB"); }
            catch (Exception ex)
            { throw ex; }

            cmd = CreateInsertFeedbackCommand("spInsertFeedback", con, feedback);

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
                {
                    con.Close();
                }
            }
        }
        private SqlCommand CreateInsertFeedbackCommand(String spName, SqlConnection con, Feedback feedback)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = spName;
            cmd.CommandTimeout = 10;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@groupId", feedback.GroupId);
            cmd.Parameters.AddWithValue("@feedbackText", feedback.FeedbackText);
            cmd.Parameters.AddWithValue("@replierId", feedback.ReplierId);

            return cmd;
        }


        
    }
}
