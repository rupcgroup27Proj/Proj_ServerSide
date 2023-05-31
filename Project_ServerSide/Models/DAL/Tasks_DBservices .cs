using System.Data.SqlClient;
using System.Data;

namespace Project_ServerSide.Models.DAL
{
    public class Tasks_DBservices
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

        //get Tasks list by groupId
        //-----------------------------------------------------------------------------------
        public List<Tasks> ReadTaskList(int groupId)
        {
            SqlConnection con;
            SqlCommand cmd;

            try
            { con = connect("myProjDB"); }
            catch (Exception ex)
            { throw (ex); }

            cmd = CreateReadPhonesCommand("spReadTasks", con, groupId);

            List<Tasks> TaskList = new List<Tasks>();

            try
            {
                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dataReader.Read())
                {
                    Tasks u = new Tasks();
                    u.TaskId = Convert.ToInt32(dataReader["taskID"]);
                    u.Name = dataReader["name"].ToString();
                    u.Description = dataReader["description"].ToString();
                    u.FileURL = dataReader["fileUrl"].ToString();
                    u.GroupId = Convert.ToInt32(dataReader["groupId"]);
                    u.CreatedAt = Convert.ToDateTime(dataReader["createdAt"]);
                    u.Due = Convert.ToDateTime(dataReader["due"]);

                    TaskList.Add(u);


                }
                return TaskList;
            }
            catch (Exception ex)
            { throw (ex); }

            finally
            {
                if (con != null)
                    con.Close();
            }
        }
        private SqlCommand CreateReadPhonesCommand(String spName, SqlConnection con, int groupId)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = spName;
            cmd.CommandTimeout = 10;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@groupId", groupId);
            return cmd;
        }


        //insert Tasks by Teacher
        //-----------------------------------------------------------------------------------
        public int InsertTasksbyTeacher(Tasks tasks)
        {

            SqlConnection con;
            SqlCommand cmd;

            try
            { con = connect("myProjDB"); }
            catch (Exception ex)
            { throw ex; }

            cmd = CreateInsertTasksCommand("spInsertTasksByTeacher", con, tasks);

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
        private SqlCommand CreateInsertTasksCommand(String spName, SqlConnection con, Tasks tasks)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = spName;
            cmd.CommandTimeout = 10;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@groupId", tasks.GroupId);
            cmd.Parameters.AddWithValue("@name", tasks.Name);
            cmd.Parameters.AddWithValue("@description", tasks.Description);
            cmd.Parameters.AddWithValue("@createdAt", tasks.CreatedAt);
            cmd.Parameters.AddWithValue("@due", tasks.Due);
            cmd.Parameters.AddWithValue("@fileUrl", tasks.FileURL);

            return cmd;
        }

        //get Tasks list by TASK id
        //-----------------------------------------------------------------------------------
        public List<Tasks> GetTaskByID(int taskId)
        {
            SqlConnection con;
            SqlCommand cmd;

            try
            { con = connect("myProjDB"); }
            catch (Exception ex)
            { throw (ex); }

            cmd = CreateReadCommand("spGetTaskByID", con, taskId);

            List<Tasks> TaskList = new List<Tasks>();

            try
            {
                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dataReader.Read())
                {
                    Tasks u = new Tasks();
                    u.TaskId = Convert.ToInt32(dataReader["taskID"]);
                    u.Name = dataReader["name"].ToString();
                    u.Description = dataReader["description"].ToString();
                    u.FileURL = dataReader["fileUrl"].ToString();
                    u.GroupId = Convert.ToInt32(dataReader["groupId"]);
                    u.CreatedAt = Convert.ToDateTime(dataReader["createdAt"]);
                    u.Due = Convert.ToDateTime(dataReader["due"]);

                    TaskList.Add(u);


                }
                return TaskList;
            }
            catch (Exception ex)
            { throw (ex); }

            finally
            {
                if (con != null)
                    con.Close();
            }
        }
        private SqlCommand CreateReadCommand(String spName, SqlConnection con, int taskId)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = spName;
            cmd.CommandTimeout = 10;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@taskId", taskId);
            return cmd;
        }


    }
}
