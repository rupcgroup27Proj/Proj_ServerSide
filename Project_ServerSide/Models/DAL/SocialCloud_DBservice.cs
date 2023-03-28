using System.Data;
using System.Data.SqlClient;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Data.SqlTypes;
using Project_ServerSide.Models;
using Microsoft.AspNetCore.Mvc.Formatters;
namespace Project_ServerSide.Models.DAL
{
    public class SocialCloud_DBservice
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



        // GetSocialCloud --  שמעלים תמונה להחזיר שם סטודנט או מורה+ תגים(בראל)
        //--------------------------------------------------------------------------------------------------
        public List<SocialCloud> ReadByGroupId(int groupId)
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


            cmd = CreateCommandSocialCloud("spReadSocialCloud", con, groupId);// create the command

            List<SocialCloud> tempList = new List<SocialCloud>();

            try
            {
                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);


                while (dataReader.Read())
                {
                    SocialCloud tempSocialCloud = new SocialCloud();

                    tempSocialCloud.PostId = Convert.ToInt32(dataReader["postId"]);
                    tempSocialCloud.GroupId = Convert.ToInt32(dataReader["groupId"]);
                    tempSocialCloud.StudentId = Convert.ToInt32(dataReader["studentId"]);
                    tempSocialCloud.TeacherId = Convert.ToInt32(dataReader["teacherId"]);
                    tempSocialCloud.GuideId = Convert.ToInt32(dataReader["guideId"]);
                    tempSocialCloud.FileUrl = dataReader["fileUrl"].ToString();
                    tempSocialCloud.Type = dataReader["type"].ToString();

                    tempList.Add(tempSocialCloud);

                }
                return tempList;

            }
            catch (Exception ex)
            {

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

        private SqlCommand CreateCommandSocialCloud(string spName, SqlConnection con, int groupId)
        {

            SqlCommand cmd = new SqlCommand(); // create the command object

            cmd.Connection = con;              // assign the connection to the command object

            cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

            cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

            cmd.CommandType = CommandType.StoredProcedure; // the type of the command, can also be stored procedure


            cmd.Parameters.AddWithValue("@groupId", groupId);


            return cmd;
        }



        // InsertToSocialCloud  
        //--------------------------------------------------------------------------------------------------

        public int InsertSocialCloud(SocialCloud socialCloud, string tags)
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

            dynamic parsedJson = JsonConvert.DeserializeObject<dynamic>(tags);

            // Access the array of tag objects using the "Tags" property
            dynamic tagsJson = parsedJson.Tags;

            // Convert the tag objects to a list of Tag objects
            List<Tag> tagsList = tagsJson.ToObject<List<Tag>>();

            // Serialize the list of Tag objects back into JSON format
            string tagsJsonString = JsonConvert.SerializeObject(tagsList);

            cmd = CreateCommandInsertSocialCloud("spInsertSocialCloud", con, socialCloud, tagsJsonString);             // create the command

            try
            {
                int numEffected = cmd.ExecuteNonQuery();
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

        private SqlCommand CreateCommandInsertSocialCloud(String spName, SqlConnection con, SocialCloud socialCloud, string tagsJson)
        {

            SqlCommand cmd = new SqlCommand(); // create the command object

            cmd.Connection = con;              // assign the connection to the command object

            cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

            cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

            cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be stored procedure
            
            cmd.Parameters.AddWithValue("@groupId", socialCloud.GroupId);
            cmd.Parameters.AddWithValue("@studentId", socialCloud.StudentId);
            cmd.Parameters.AddWithValue("@teacherId", socialCloud.TeacherId);
            cmd.Parameters.AddWithValue("@guideId", socialCloud.GuideId);
            cmd.Parameters.AddWithValue("@fileUrl", socialCloud.FileUrl);
            cmd.Parameters.AddWithValue("@type", socialCloud.Type);
            cmd.Parameters.AddWithValue("@tagsJson", tagsJson);

            return cmd;
        }



        // DeleteFromSocialCloud 
        //--------------------------------------------------------------------------------------------------
        public int DeleteFromSocialCloud(int postId)
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

            cmd = CreateCommandDeleteFromSocialCloud("spDeleteFromSocialCloud", con, postId);     // create the command


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

        private SqlCommand CreateCommandDeleteFromSocialCloud(string spName, SqlConnection con, int postId)
        {

            SqlCommand cmd = new SqlCommand(); // create the command object

            cmd.Connection = con;              // assign the connection to the command object

            cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

            cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

            cmd.CommandType = CommandType.StoredProcedure; // the type of the command, can also be stored procedure

            cmd.Parameters.AddWithValue("@postId", postId);


            return cmd;
        }
    }
}
