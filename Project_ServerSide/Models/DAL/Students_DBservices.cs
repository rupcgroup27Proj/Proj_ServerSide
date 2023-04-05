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
using System.Text.RegularExpressions;

namespace Project_ServerSide.Models.DAL
{
    public class Students_DBservices
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


        // inserts student
        //--------------------------------------------------------------------------------------------------
        public int Insert(Student student)
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

            cmd = CreateInsertStudentCommandSP("spInsertStudent", con, student);

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

        private SqlCommand CreateInsertStudentCommandSP(String spName, SqlConnection con, Student student)
        {

            SqlCommand cmd = new SqlCommand(); // create the command object

            cmd.Connection = con;              // assign the connection to the command object

            cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

            cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

            cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be stored procedure

            cmd.Parameters.AddWithValue("@Id", student.StudentId);
            cmd.Parameters.AddWithValue("@password", student.Password);
            cmd.Parameters.AddWithValue("@firstName", student.FirstName);
            cmd.Parameters.AddWithValue("@lastName", student.LastName);
            cmd.Parameters.AddWithValue("@phone", Convert.ToInt32(student.Phone));
            cmd.Parameters.AddWithValue("@email", student.Email);
            cmd.Parameters.AddWithValue("@parentPhone", Convert.ToInt32(student.ParentPhone));
            cmd.Parameters.AddWithValue("@pictureUrl", student.PictureUrl);
            cmd.Parameters.AddWithValue("@groupId", student.GroupId);


            return cmd;
        }


        //read all students group students
        //--------------------------------------------------------------------------------------------------

        public List<Student> Read(int groupId)
        {
            SqlConnection con;
            SqlCommand cmd;

            try
            { con = connect("myProjDB"); }


            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            cmd = CreateReadStudentsCommandSP("spReadStudent", con, groupId);

            List<Student> StudentList = new List<Student>();

            try
            {
                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);


                while (dataReader.Read())
                {
                    Student u = new Student();
                    u.StudentId = Convert.ToInt32(dataReader["id"]);
                    u.Password = dataReader["Password"].ToString();
                    u.Email = dataReader["Email"].ToString();
                    u.FirstName = dataReader["Firstname"].ToString();
                    u.LastName = dataReader["Lastname"].ToString();
                    u.Phone = Convert.ToDouble(dataReader["Phone"]);
                    u.ParentPhone = Convert.ToDouble(dataReader["ParentPhone"]);
                    u.PictureUrl = dataReader["PictureUrl"].ToString();
                    u.GroupId = Convert.ToInt32(dataReader["groupId"]);
                    u.StartDate = Convert.ToDateTime(dataReader["StartDate"]);
                    u.EndDate = Convert.ToDateTime(dataReader["EndDate"]);
                    StudentList.Add(u);

                }
                return StudentList;
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

        private SqlCommand CreateReadStudentsCommandSP(String spName, SqlConnection con, int groupId)
        {

            SqlCommand cmd = new SqlCommand(); // create the command object

            cmd.Connection = con;              // assign the connection to the command object

            cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

            cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

            cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be stored procedure
            cmd.Parameters.AddWithValue("@groupId", groupId);

            return cmd;
        }


        //login student
        //---------------------------------------------------------------------------------
        public Student Login(Student student)
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


            cmd = CreateLoginCommandSP("spLoginStudent", con, student);// create the command
            Student u = new Student();
            try
            {
                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dataReader.Read())
                {
                    u.StudentId = Convert.ToInt32(dataReader["Id"]);
                    u.Password = dataReader["Password"].ToString();
                    u.Email = dataReader["Email"].ToString();
                    u.FirstName = dataReader["Firstname"].ToString();
                    u.LastName = dataReader["Lastname"].ToString();
                    u.Phone = Convert.ToDouble(dataReader["Phone"]);
                    u.ParentPhone = Convert.ToDouble(dataReader["ParentPhone"]);
                    u.PictureUrl = dataReader["PictureUrl"].ToString();
                    u.GroupId = Convert.ToInt32(dataReader["groupId"]);
                    u.Type = "Student".ToString();
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

        private SqlCommand CreateLoginCommandSP(String spName, SqlConnection con, Student student)
        {

            SqlCommand cmd = new SqlCommand(); // create the command object

            cmd.Connection = con;              // assign the connection to the command object

            cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

            cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

            cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be stored procedure


            cmd.Parameters.AddWithValue("@Id", student.StudentId);
            cmd.Parameters.AddWithValue("@Password", student.Password);


            return cmd;
        }



        // Get Specific Student by studentId
        //---------------------------------------------------------------------------------
        public Student pullSpecificStudent(Student student)
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


            cmd = CreatePullCommandSP("spPullStudentById", con, student);// create the command
            Student Y = new Student();
            try
            {
                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dataReader.Read())
                {

                    Y.StudentId = Convert.ToInt32(dataReader["Id"]);
                    Y.Password = dataReader["Password"].ToString();
                    Y.Email = dataReader["Email"].ToString();
                    Y.FirstName = dataReader["Firstname"].ToString();
                    Y.LastName = dataReader["Lastname"].ToString();
                    Y.Phone = Convert.ToDouble(dataReader["Phone"]);
                    Y.ParentPhone = Convert.ToDouble(dataReader["ParentPhone"]);
                    Y.PictureUrl = dataReader["PictureUrl"].ToString();
                    Y.GroupId = Convert.ToInt32(dataReader["groupId"]);
                    Y.Type = "Student".ToString();
                }
                return Y;

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

        private SqlCommand CreatePullCommandSP(String spName, SqlConnection con, Student student)
        {

            SqlCommand cmd = new SqlCommand(); // create the command object

            cmd.Connection = con;              // assign the connection to the command object

            cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

            cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

            cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be stored procedure

            cmd.Parameters.AddWithValue("@Id", student.StudentId);


            return cmd;
        }


        // update a student
        //--------------------------------------------------------------------------------------------------
        public int Update(Student student)
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


            cmd = CreateCommandWithStoredProcedure("spUpdateStudent", con, student);

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

        private String BuildUpdateCommand(Student student)
        {

            StringBuilder sb = new StringBuilder();

            string command = sb.AppendFormat("update Students set phone = '{0}', email = {1}, parentPhone = {2} where id = {3}", student.Phone, student.Email, student.ParentPhone, student.StudentId).ToString();

            return command;
        }

        private SqlCommand CreateCommandWithStoredProcedure(String spName, SqlConnection con, Student student)
        {

            SqlCommand cmd = new SqlCommand(); // create the command object

            cmd.Connection = con;              // assign the connection to the command object

            cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

            cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

            cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be stored procedure

            cmd.Parameters.AddWithValue("@id", student.StudentId);

            cmd.Parameters.AddWithValue("@email", student.Email);

            cmd.Parameters.AddWithValue("@phone", student.Phone);

            cmd.Parameters.AddWithValue("@parentPhone", student.ParentPhone);



            return cmd;
        }



        //delete a student
        //--------------------------------------------------------------------------------------------------
        public int DeleteFromGroupe(int groupId)
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


            cmd = CreateCommandWithStoredProcedureDelete1("spDeleteStudentFromGroups", con, groupId);

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

        private SqlCommand CreateCommandWithStoredProcedureDelete1(String spName, SqlConnection con, int groupId)
        {

            SqlCommand cmd = new SqlCommand(); // create the command object

            cmd.Connection = con;              // assign the connection to the command object

            cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

            cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

            cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be stored procedure

            cmd.Parameters.AddWithValue("@groupId", groupId);



            return cmd;
        }



    }
}


