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
using System.Threading.Tasks;

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
                    u.Name = dataReader["name"].ToString();
                    u.Description = dataReader["description"].ToString();
                    u.FileURL = dataReader["fileUrl"].ToString();
                    u.StartingAt = Convert.ToDateTime(dataReader["startingAt"]);
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
            cmd.Parameters.AddWithValue("@startingAt", tasks.StartingAt);
            cmd.Parameters.AddWithValue("@createdAt", tasks.CreatedAt);
            cmd.Parameters.AddWithValue("@due", tasks.Due);
            cmd.Parameters.AddWithValue("@fileUrl", tasks.FileURL);




            return cmd;
        }

        //get which student submit task 
        //-----------------------------------------------------------------------------------
        public List<Submittion> ReadSubList(int taskId)
        {
            SqlConnection con;
            SqlCommand cmd;

            try
            { con = connect("myProjDB"); }
            catch (Exception ex)
            { throw (ex); }

            cmd = CreateReadsubCommand("spReadWhoDidTask", con, taskId);

            List<Submittion> SubList = new List<Submittion>();

            try
            {
                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dataReader.Read())
                {
                    Submittion u = new Submittion();
                    u.FirstName = dataReader["firstName"].ToString();
                    u.LastName = dataReader["lastName"].ToString();
                    u.Id = Convert.ToInt32(dataReader["Id"]);
                    u.DescriptionStu = dataReader["description"].ToString();
                    u.FileURL = dataReader["fileUrl"].ToString();
                    u.TaskId = Convert.ToInt32(dataReader["taskId"]);
                    u.SubmittedAt = Convert.ToDateTime(dataReader["submittedAt"]);


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
        public int SubmitTaskByStudent(Submittion submittion)
        {

            SqlConnection con;
            SqlCommand cmd;

            try
            { con = connect("myProjDB"); }
            catch (Exception ex)
            { throw ex; }

            cmd = CreateSubmitTasksCommand("spSubmitByStudent", con, submittion);

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
        private SqlCommand CreateSubmitTasksCommand(String spName, SqlConnection con, Submittion submittion)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = spName;
            cmd.CommandTimeout = 10;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Id", submittion.Id);
            cmd.Parameters.AddWithValue("@taskId", submittion.TaskId);
            cmd.Parameters.AddWithValue("@description", submittion.DescriptionStu);
            cmd.Parameters.AddWithValue("@submittedAt", submittion.SubmittedAt);
            cmd.Parameters.AddWithValue("@fileUrl", submittion.FileURL);



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
        public int UpdateSubmittion(Submittion submittion)
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

            cmd = CreateCommandWithStoredProcedure("spUpdateSubmission", con, submittion);

            try
            {
                int numEffected = cmd.ExecuteNonQuery(); // execute the command
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

        private SqlCommand CreateCommandWithStoredProcedure(string spName, SqlConnection con, Submittion submittion)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = spName;
            cmd.CommandTimeout = 10;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@submissionId", submittion.SubmissionId);
            cmd.Parameters.AddWithValue("@grade", submittion.Grade);

            return cmd;
        }
    }
}
