using System.Data.SqlClient;
using System.Data;
using Microsoft.AspNetCore.Http;
using System.Globalization;
using System;

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
        //public int InsertTasksbyTeacher(PdfModel pdf)
        //{

        //    SqlConnection con;
        //    SqlCommand cmd;

        //    try
        //    { con = connect("myProjDB"); }
        //    catch (Exception ex)
        //    { throw ex; }

        //    cmd = CreateInsertTasksCommand("spInsertTasksByTeacher", con, pdf);

        //    try
        //    {
        //        int numEffected = cmd.ExecuteNonQuery();
        //        return numEffected;
        //    }
        //    catch (Exception ex)
        //    { throw ex; }

        //    finally
        //    {
        //        if (con != null)
        //            con.Close();
        //    }
        //}



        //get Task details by taskId
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





        public int addPdf(string uniqueFileName, string description, string date, string name, int groupId)
        {
            SqlConnection con;
            SqlCommand cmd;

            try
            { con = connect("myProjDB"); }
            catch (Exception ex)
            { throw (ex); }

            try
            {
                cmd = CreateInsertTasksCommand("spInsertTasksByTeacher", con, uniqueFileName, description, date, name, groupId);
                return cmd.ExecuteNonQuery();
            }
            finally
            {
                if (con != null)
                    con.Close();
            }
        }

        private SqlCommand CreateInsertTasksCommand(String spName, SqlConnection con, string uniqueFileName, string description, string date, string name, int groupId)
        {
            DateTime dateTime;
            DateTime.TryParseExact(date, "MM/dd/yy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = spName;
            cmd.CommandTimeout = 10;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@groupId", groupId);
            cmd.Parameters.AddWithValue("@name", name);
            cmd.Parameters.AddWithValue("@description", description);
            cmd.Parameters.AddWithValue("@createdAt", DateTime.Now);
            cmd.Parameters.AddWithValue("@due", dateTime);
            cmd.Parameters.AddWithValue("@fileUrl", uniqueFileName);

            return cmd;
        }
    }
}
