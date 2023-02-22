using System.Data.SqlClient;
using System.Data;
using System.Text;

namespace Project_ServerSide.Models.DAL
{
    public class Students_DBservices
    {
        //public SqlDataAdapter da;
        //public DataTable dt;

        ////public Students_DBservices()
        ////{
        //    //
        //    // TODO: Add constructor logic here
        //    //
        //}

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

            String cStr = BuildInsertCommand(student);      // helper method to build the insert string

            cmd = CreateCommand(cStr, con);             // create the command

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



        //--------------------------------------------------------------------------------------------------
        // This method update a student to the student table 
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

            //String cStr = BuildUpdateCommand(student);      // helper method to build the insert string

            cmd = CreateCommandWithStoredProcedure("spUpdateStudent1", con, student);             // create the command

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



        //--------------------------------------------------------------------
        // Build the Insert command String
        //--------------------------------------------------------------------
        private String BuildInsertCommand(Student student)//להוסיף את כל הפרטים בשורה -הוספת רשומה של הפרטי תלמיד 138
        {
            String command;

            StringBuilder sb = new StringBuilder();
            // use a string builder to create the dynamic string
            sb.AppendFormat("Values('{0}', '{1}',)", student.StudentId, student.Password);
            String prefix = "INSERT INTO Students " + "(ID, Password) ";
            command = prefix + sb.ToString();

            return command;
        }

        //--------------------------------------------------------------------
        // Build the Insert command String
        //--------------------------------------------------------------------
        private String BuildUpdateCommand(Student student)
        {

            StringBuilder sb = new StringBuilder();
            // use a string builder to create the dynamic string
            //sb.AppendFormat("Values('{0}', '{1}')", student.Name, student.Age,student.Id);
            //String prefix = "INSERT INTO Students_2022 " + "(name, age) ";
            //command = prefix + sb.ToString();

            //update Students_2022 set name = 'messi', age = 35 where id = 3
            string command = sb.AppendFormat("update Students set name = '{0}', age = {1} where id = {2}", student.Name, student.Age, student.Id).ToString();

            return command;
        }

        //---------------------------------------------------------------------------------
        // Create the SqlCommand
        //---------------------------------------------------------------------------------
        private SqlCommand CreateCommand(String CommandSTR, SqlConnection con)
        {

            SqlCommand cmd = new SqlCommand(); // create the command object

            cmd.Connection = con;              // assign the connection to the command object

            cmd.CommandText = CommandSTR;      // can be Select, Insert, Update, Delete 

            cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

            cmd.CommandType = System.Data.CommandType.Text; // the type of the command, can also be stored procedure

            return cmd;
        }


        //---------------------------------------------------------------------------------
        // Create the SqlCommand using a stored procedure
        //---------------------------------------------------------------------------------
        private SqlCommand CreateCommandWithStoredProcedure(String spName, SqlConnection con, Student student)
        {

            SqlCommand cmd = new SqlCommand(); // create the command object

            cmd.Connection = con;              // assign the connection to the command object

            cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

            cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

            cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be stored procedure

            cmd.Parameters.AddWithValue("@id", student.Id);

            cmd.Parameters.AddWithValue("@name", student.Name);

            cmd.Parameters.AddWithValue("@age", student.Age);


            return cmd;
        }

    }
}
