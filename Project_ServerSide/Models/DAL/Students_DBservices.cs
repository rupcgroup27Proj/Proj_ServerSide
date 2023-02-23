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

namespace Project_ServerSide.Models.DAL
{
    public class Students_DBservices
    {
        public SqlDataAdapter da;
        public DataTable dt;

        public Students_DBservices()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        //--------------------------------------------------------------------------------------------------
        // This method creates a connection to the database according to the connectionString name in the web.config 
        //--------------------------------------------------------------------------------------------------
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

        //--------------------------------------------------------------------------------------------------
        // This method inserts a student to the student table 
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
            throw (ex);
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


    //---------------------------------------------------------------------------------
    // Create the SqlCommand InsertCommand
    //---------------------------------------------------------------------------------
    private SqlCommand CreateInsertStudentCommandSP(String spName, SqlConnection con, Student student)
{

    SqlCommand cmd = new SqlCommand(); // create the command object

    cmd.Connection = con;              // assign the connection to the command object

    cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

    cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

    cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be stored procedure

    cmd.Parameters.AddWithValue("@studentId", student.StudentId);
    cmd.Parameters.AddWithValue("@password", student.Password);
    cmd.Parameters.AddWithValue("@firstName", student.FirstName);
    cmd.Parameters.AddWithValue("@lastName", student.LastName);
    cmd.Parameters.AddWithValue("@phone", student.Phone);
    cmd.Parameters.AddWithValue("@email", student.Email);
    cmd.Parameters.AddWithValue("@parentPhone", student.ParentPhone);
    cmd.Parameters.AddWithValue("@pictureUrl", student.PictureUrl);


    return cmd;
}
        //--------------------------------------------------------------------------------------------------
        // This method read users 
        //--------------------------------------------------------------------------------------------------
        public List<Student> ReadUsers()
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

            //String cStr = BuildUpdateCommand(student);      // helper method to build the insert string

            //cmd = CreateCommand(cStr, con);             // create the command

            cmd = CreateReadStudentsCommandSP("spReadStudent", con);

            try
            {
                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                List<Student> StudentList = new List<Student>();

                while (dataReader.Read())
                {
                    Student u = new Student();
                    u.StudentId = Convert.ToInt32(dataReader["studentId"]);
                    u.Password = Convert.ToInt32(dataReader["password"]);
                    u.Email = dataReader["email"].ToString();
                    u.FirstName = dataReader["firstname"].ToString();
                    u.LastName = dataReader["lastname"].ToString();
                    u.Phone = Convert.ToInt32(dataReader["phone"]);
                    u.ParentPhone = Convert.ToInt32(dataReader["parentPhone"]);
                    u.PictureUrl= dataReader["pictureUrl"].ToString();

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

        //---------------------------------------------------------------------------------
        // Create the ReadStudents SqlCommand
        //---------------------------------------------------------------------------------
        private SqlCommand CreateReadStudentsCommandSP(String spName, SqlConnection con)
        {

            SqlCommand cmd = new SqlCommand(); // create the command object

            cmd.Connection = con;              // assign the connection to the command object

            cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

            cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

            cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be stored procedure


            return cmd;
        }

    }
}
