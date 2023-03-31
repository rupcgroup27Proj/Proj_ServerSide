using System.Data;
using System.Data.SqlClient;
using Newtonsoft.Json;
using Project_ServerSide.Models.SmartQuestionnaires;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.Extensions.Hosting;

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
        ////--------------------------------------------------------------------------------------------------
        //public List<SocialCloud> ReadByGroupIdAndType(int groupId, string type)
        //{

        //    SqlConnection con;
        //    SqlCommand cmd;

        //    try
        //    {
        //        con = connect("myProjDB"); // create the connection
        //    }
        //    catch (Exception ex)
        //    {
        //        // write to log
        //        throw ex;
        //    }


        //    //get the tags of all post

        //    cmd = CreateCommandSocialCloud(con, groupId, type);// create the command

        //    List<SocialCloud> tempList = new List<SocialCloud>();

        //    try
        //    {
        //        SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);


        //        while (dataReader.Read())
        //        {
        //            SocialCloud tempSocialCloud = new SocialCloud();

        //            tempSocialCloud.PostId = Convert.ToInt32(dataReader["postId"]);
        //            tempSocialCloud.GroupId = Convert.ToInt32(dataReader["groupId"]);
        //            tempSocialCloud.Type = dataReader["type"].ToString();

        //            if (dataReader["type"].ToString() == "Student")
        //            {
        //                tempSocialCloud.StudentId = Convert.ToInt32(dataReader["studentId"]);
        //                tempSocialCloud.TeacherId = 0;
        //                tempSocialCloud.GuideId = 0;
        //            }
        //            else if (dataReader["type"].ToString() == "Teacher")
        //            {
        //                tempSocialCloud.TeacherId = Convert.ToInt32(dataReader["teacherId"]);
        //                tempSocialCloud.StudentId = 0;
        //                tempSocialCloud.GuideId = 0;
        //            }
        //            else if (dataReader["type"].ToString() == "Guide")
        //            {
        //                tempSocialCloud.GuideId = Convert.ToInt32(dataReader["guideId"]);
        //                tempSocialCloud.TeacherId = 0;
        //                tempSocialCloud.StudentId = 0;
        //            }

        //            tempSocialCloud.FileUrl = dataReader["fileUrl"].ToString();
        //            tempSocialCloud.FirstName = dataReader["Firstname"].ToString();
        //            tempSocialCloud.LastName = dataReader["Lastname"].ToString();
        //            tempList.Add(tempSocialCloud);


        //        }
        //        return tempList;

        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }

        //}
        //private SqlCommand CreateCommandSocialCloud(SqlConnection con, int groupId, string type)
        //{

        //    SqlCommand cmd = new SqlCommand(); // create the command object
        //    if (type == "Student")
        //        cmd.CommandText = "spReadSocialCloudByStudent";
        //    else if (type == "Teacher")
        //        cmd.CommandText = "spReadSocialCloudByTeacher";
        //    else
        //        cmd.CommandText = "spReadSocialCloudByGuide";

        //    cmd.Connection = con;
        //    cmd.CommandType = System.Data.CommandType.StoredProcedure;

        //    cmd.Parameters.AddWithValue("@groupId", groupId);


        //    return cmd;
        //}



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

        // NewGet postID+Tags 
        //--------------------------------------------------------------------------------------------------

        public string ReadByGroupIdAndType(int groupId, string type)
        {
            SqlConnection con;
            try
            { con = connect("myProjDB"); }
            catch (Exception ex)
            { throw (ex); }

            //getthe tags of all post
            List<Dictionary<string, string>> tags = getTags(groupId, con, type);

            //get the post
            List<Dictionary<string, string>> Posts = getPost(groupId, con, type);

            //the list we will send back
            List<SocialCloud> data = new List<SocialCloud>();

            foreach (var Post in Posts)
            {
                SocialCloud tempSocialCloud = new SocialCloud();
                tempSocialCloud.PostId = Convert.ToInt32(Post["postId"]);
                tempSocialCloud.GroupId = Convert.ToInt32(Post["groupId"]);
                tempSocialCloud.Type = Post["type"].ToString();

                if (Post["type"].ToString() == "Student")
                {
                    tempSocialCloud.StudentId = Convert.ToInt32(Post["studentId"]);
                    tempSocialCloud.TeacherId = 0;
                    tempSocialCloud.GuideId = 0;
                }
                else if (Post["type"].ToString() == "Teacher")
                {
                    tempSocialCloud.TeacherId = Convert.ToInt32(Post["teacherId"]);
                    tempSocialCloud.StudentId = 0;
                    tempSocialCloud.GuideId = 0;
                }
                else if (Post["type"].ToString() == "Guide")
                {
                    tempSocialCloud.GuideId = Convert.ToInt32(Post["guideId"]);
                    tempSocialCloud.TeacherId = 0;
                    tempSocialCloud.StudentId = 0;
                }

                tempSocialCloud.FileUrl = Post["fileUrl"].ToString();
                tempSocialCloud.FirstName = Post["Firstname"].ToString();
                tempSocialCloud.LastName = Post["Lastname"].ToString();
                tempSocialCloud.CreatedAt = Convert.ToDateTime(Post["createdAt"]);

                tempSocialCloud.Tags = new List<Tag>();

                //fill the post with all its tags
                foreach (var tag in tags)
                {
                    if (tag["postId"] == Post["postId"])
                    {
                        Tag t = new Tag
                        {
                            TagId = Convert.ToInt32(tag["tagId"]),
                            GroupId = Convert.ToInt32(tag["groupId"]),
                            TagName = tag["tagName"]
                        };

                        tempSocialCloud.Tags.Add(t);
                    }
                }
                data.Add(tempSocialCloud);
            }

            con.Close();


            string jsonString = JsonConvert.SerializeObject(data);

            return jsonString;

        }

        private List<Dictionary<string, string>> getTags(int groupId, SqlConnection con, string type)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            if (type == "Student" || type == "Teacher" || type == "Guide")
                cmd.CommandText = "getTagsOfAllGroupPosts";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@groupId", groupId);

            try
            {
                SqlDataReader dataReader = cmd.ExecuteReader();
                List<Dictionary<string, string>> result = new List<Dictionary<string, string>>();

                while (dataReader.Read())
                {
                    Dictionary<string, string> tag = new()
                    {
                        {"postId", dataReader["postId"].ToString()},
                        {"tagId", dataReader["tagId"].ToString()},
                        {"groupId", dataReader["groupId"].ToString()},
                        {"tagName", dataReader["tagName"].ToString()}
                    };

                    result.Add(tag);
                }
                dataReader.Close();//Close the dataReader so that i could open another one on the same connection.
                return result;
            }
            catch (Exception ex)
            { throw; }
        }

        private List<Dictionary<string, string>> getPost(int groupId, SqlConnection con, string type)
        {
            SqlCommand cmd = new SqlCommand();
            if (type == "Student")
                cmd.CommandText = "spReadSocialCloudByStudent";
            else if (type == "Teacher")
                cmd.CommandText = "spReadSocialCloudByTeacher";
            else
                cmd.CommandText = "spReadSocialCloudByGuide";
            cmd.Connection = con;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@groupId", groupId);


            try
            {
                SqlDataReader dataReader = cmd.ExecuteReader();
                List<Dictionary<string, string>> result = new List<Dictionary<string, string>>();

                while (dataReader.Read())
                {

                    Dictionary<string, string> post = new()
        {
            {"postId", dataReader["postId"].ToString()},
            {"groupId", dataReader["groupId"].ToString()},
            {"type", dataReader["type"].ToString()},//type  -שדה בדאטה בייס 
            {"studentId", type == "Student" ? dataReader["studentId"].ToString() : "0"},//type שמכניס המשתמש
            {"teacherId", type == "Teacher" ? dataReader["teacherId"].ToString() : "0"},
            {"guideId", type == "Guide" ? dataReader["guideId"].ToString() : "0"},
            {"fileUrl", dataReader["fileUrl"].ToString()},
            {"Firstname", dataReader["Firstname"].ToString()},
            {"Lastname", dataReader["Lastname"].ToString()},
            {"createdAt", dataReader["createdAt"].ToString()},
        };

                    result.Add(post);
                }

                dataReader.Close();
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}




