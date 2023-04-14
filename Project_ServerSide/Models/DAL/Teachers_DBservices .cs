using System.Data.SqlClient;


namespace Project_ServerSide.Models.DAL
{
    public class Teachers_DBservices
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


        //insert teacher
        //-----------------------------------------------------------------------------------
        public int InsertTeacher(Teacher teacher)
        {
            SqlConnection con;
            SqlCommand cmd;

            try
            { con = connect("myProjDB"); }
            catch (Exception ex)
            { throw ex; }

            cmd = CreateInsertTeacherstCommand("spInsertTeachers", con, teacher);

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

        private SqlCommand CreateInsertTeacherstCommand(String spName, SqlConnection con, Teacher teacher)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = spName;
            cmd.CommandTimeout = 10;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Id", teacher.TeacherId);
            cmd.Parameters.AddWithValue("@password", teacher.Password);
            cmd.Parameters.AddWithValue("@firstName", teacher.FirstName);
            cmd.Parameters.AddWithValue("@lastName", teacher.LastName);
            cmd.Parameters.AddWithValue("@phone", teacher.Phone);
            cmd.Parameters.AddWithValue("@email", teacher.Email);
            cmd.Parameters.AddWithValue("@pictureUrl", teacher.PictureUrl);
            cmd.Parameters.AddWithValue("@groupId", teacher.GroupId);
            return cmd;
        }
    }
}
