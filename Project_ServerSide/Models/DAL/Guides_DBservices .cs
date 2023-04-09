using System.Data.SqlClient;
using System.Data;

namespace Project_ServerSide.Models.DAL
{
    public class Guides_DBservices
    {
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


        public int Insert(Guide guide)
        {
            SqlConnection con;
            SqlCommand cmd;

            try
            { con = connect("myProjDB"); }
            catch (Exception ex)
            { throw ex; }

            cmd = CreateInsertGuidestCommand("spInsertguides", con, guide);

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


        public Guide Login(Guide guide)
        {

            SqlConnection con;
            SqlCommand cmd;

            try
            { con = connect("myProjDB"); }
            catch (Exception ex)
            { throw (ex); }

            cmd = CreateLoginCommand("spLoginGuides", con, guide);

            Guide u = new Guide();

            try
            {
                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dataReader.Read())
                {
                    u.GuideId = Convert.ToInt32(dataReader["Id"]);
                    u.Password = dataReader["Password"].ToString();
                    u.Email = dataReader["Email"].ToString();
                    u.FirstName = dataReader["Firstname"].ToString();
                    u.LastName = dataReader["Lastname"].ToString();
                    u.Phone = Convert.ToDouble(dataReader["Phone"]);
                    u.PictureUrl = dataReader["PictureUrl"].ToString();
                    u.GroupId = Convert.ToInt32(dataReader["groupId"]);
                    u.IsAdmin = Convert.ToBoolean(dataReader["isAdmin"]);
                    u.Type = "Guide".ToString();

                }

                return u;
            }
            catch (Exception ex)
            { throw; }

            finally
            {
                if (con != null)
                    con.Close();
            }
        }


        private SqlCommand CreateInsertGuidestCommand(String spName, SqlConnection con, Guide guide)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = spName;
            cmd.CommandTimeout = 10;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Id", guide.GuideId);
            cmd.Parameters.AddWithValue("@password", guide.Password);
            cmd.Parameters.AddWithValue("@firstName", guide.FirstName);
            cmd.Parameters.AddWithValue("@lastName", guide.LastName);
            cmd.Parameters.AddWithValue("@phone", guide.Phone);
            cmd.Parameters.AddWithValue("@email", guide.Email);
            cmd.Parameters.AddWithValue("@isAdmin", guide.IsAdmin);
            cmd.Parameters.AddWithValue("@pictureUrl", guide.PictureUrl);
            cmd.Parameters.AddWithValue("@groupId", guide.GroupId);
            return cmd;
        }

        private SqlCommand CreateLoginCommand(String spName, SqlConnection con, Guide guide)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = spName;
            cmd.CommandTimeout = 10;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Id", guide.GuideId);
            cmd.Parameters.AddWithValue("@Password", guide.Password);
            return cmd;
        }
    }
}
