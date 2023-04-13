using System.Data.SqlClient;
using System.Data;

namespace Project_ServerSide.Models.DAL
{
    public class Guides_DBservices
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


        //insert guide
        //-----------------------------------------------------------------------------------
        public int InsertGuide(Guide guide)
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

    }
}
