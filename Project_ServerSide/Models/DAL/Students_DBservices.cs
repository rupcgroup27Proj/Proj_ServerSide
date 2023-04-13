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
        public SqlConnection connect(String conString)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json").Build();
            string cStr = configuration.GetConnectionString("myProjDB");
            SqlConnection con = new SqlConnection(cStr);
            con.Open();
            return con;
        }


        //read all students group 
        //-----------------------------------------------------------------------------------
        public List<Student> GetGroupStudents(int groupId)
        {
            SqlConnection con;
            SqlCommand cmd;

            try
            { con = connect("myProjDB"); }
            catch (Exception ex)
            { throw (ex); }

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
            { throw (ex); }
            finally
            {
                if (con != null)
                    con.Close();
            }

        }

        private SqlCommand CreateReadStudentsCommandSP(String spName, SqlConnection con, int groupId)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = spName;
            cmd.CommandTimeout = 10;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@groupId", groupId);
            return cmd;
        }


        // get Specific Student 
        //-----------------------------------------------------------------------------------
        public Student GetSpecificStudent(Student student)
        {

            SqlConnection con;
            SqlCommand cmd;

            try
            { con = connect("myProjDB"); }
            catch (Exception ex)
            { throw (ex); }

            cmd = CreatePullCommandSP("spPullStudentById", con, student);

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
            { throw; }
            finally
            {
                if (con != null)
                    con.Close();
            }

        }

        private SqlCommand CreatePullCommandSP(String spName, SqlConnection con, Student student)
        {

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = spName;
            cmd.CommandTimeout = 10;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Id", student.StudentId);
            return cmd;
        }


        //insert student 
        //-----------------------------------------------------------------------------------
        public int InsertStudent(Student student)
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

            cmd = CreateInsertStudentCommandSP("spInsertStudent", con, student);

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


        // update student
        //-----------------------------------------------------------------------------------
        public int UpdateStudent(Student student)
        {
            SqlConnection con;
            SqlCommand cmd;

            try
            { con = connect("myProjDB"); }
            catch (Exception ex)
            { throw (ex); }

            cmd = CreateCommandWithStoredProcedure("spUpdateStudent", con, student);

            try
            {
                int numEffected = cmd.ExecuteNonQuery(); // execute the command
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

        private SqlCommand CreateCommandWithStoredProcedure(String spName, SqlConnection con, Student student)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = spName;
            cmd.CommandTimeout = 10;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@id", student.StudentId);
            cmd.Parameters.AddWithValue("@email", student.Email);
            cmd.Parameters.AddWithValue("@phone", student.Phone);
            cmd.Parameters.AddWithValue("@parentPhone", student.ParentPhone);
            return cmd;
        }


        //delete what???????????
        //-----------------------------------------------------------------------------------
        public int DeleteFromGroup(int groupId)
        {
            SqlConnection con;
            SqlCommand cmd;

            try
            { con = connect("myProjDB"); }
            catch (Exception ex)
            { throw (ex); }


            cmd = CreateCommandWithStoredProcedureDelete1("spDeleteStudentFromGroups", con, groupId);

            try
            {
                int numEffected = cmd.ExecuteNonQuery(); // execute the command
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

        private SqlCommand CreateCommandWithStoredProcedureDelete1(String spName, SqlConnection con, int groupId)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = spName;
            cmd.CommandTimeout = 10;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@groupId", groupId);
            return cmd;
        }
    }
}


