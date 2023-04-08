using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Text;
using System.Xml.Linq;
using Project_ServerSide.Models;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace Project_ServerSide.Models.DAL
{
    public class FavList_DBservices
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

        // GetStudentFavList
        //--------------------------------------------------------------------------------------------------
        public string FavListByStudentId(int studentId)
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

            //getthe tags of all post
            List<Dictionary<string, string>> tags = getTags(studentId, con);

            //get the post
            List<Dictionary<string, string>> Posts = getPost(studentId, con);

            //the list we will send back
            List<FavList> data = new List<FavList>();

            foreach (var Post in Posts)
            {
                FavList temp= new FavList();
                temp.PostId = Convert.ToInt32(Post["postId"]);
                temp.StudentId = Convert.ToInt32(Post["studentId"]);
                temp.FileUrl = Post["fileUrl"].ToString();
             
                temp.Tags = new List<Tag>();

                //fill the post with all its tags
                foreach (var tag in tags)
                {
                    if (tag["postId"] == Post["postId"])
                    {
                        Tag t = new Tag
                        {
                            TagId = Convert.ToInt32(tag["tagId"]),
                            TagName = tag["tagName"]
                        };

                        temp.Tags.Add(t);
                    }
                }
                data.Add(temp);
            }

            con.Close();


            string jsonString = JsonConvert.SerializeObject(data);

            return jsonString;

        }

        private List<Dictionary<string, string>> getTags(int studentId, SqlConnection con)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = "getAllFavTagsByStudentId";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@studentId", studentId);

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

        private List<Dictionary<string, string>> getPost(int studentId, SqlConnection con)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "getFavPostsByStudentId";
            cmd.Connection = con;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@studentId", studentId);


            try
            {
                SqlDataReader dataReader = cmd.ExecuteReader();
                List<Dictionary<string, string>> result = new List<Dictionary<string, string>>();

                while (dataReader.Read())
                {

                    Dictionary<string, string> post = new()
        {
            {"postId", dataReader["postId"].ToString()},
            {"studentId",dataReader["Id"].ToString()},
            {"fileUrl", dataReader["fileUrl"].ToString()},


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
  

        // InsertPostToStudentFavList
        //--------------------------------------------------------------------------------------------------

        public bool InsertFavPost(int studentId, int postId)
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


            cmd = CreateCommandInsertFavPost("spInsertFavPostToStudent", con, studentId, postId);             // create the command

            try
            {
                int numEffected = cmd.ExecuteNonQuery();
                if (numEffected == 1)
                {
                    return true;
                }
                else return false;
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

        private SqlCommand CreateCommandInsertFavPost(String spName, SqlConnection con, int studentId, int postId)
        {

            SqlCommand cmd = new SqlCommand(); // create the command object

            cmd.Connection = con;              // assign the connection to the command object

            cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

            cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

            cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be stored procedure

            cmd.Parameters.AddWithValue("@studentId", studentId);
            cmd.Parameters.AddWithValue("@postId", postId);


            return cmd;
        }


        // DeletePostFromListFav
        //--------------------------------------------------------------------------------------------------
        public int DeletePostFromListFav(int studentId, int postId)
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

            cmd = CreateCommandDeletePostFromListFav("spDeletePostFromListFav", con, studentId, postId);     // create the command

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

        private SqlCommand CreateCommandDeletePostFromListFav(string spName, SqlConnection con, int studentId, int postId)
        {

            SqlCommand cmd = new SqlCommand(); // create the command object

            cmd.Connection = con;              // assign the connection to the command object

            cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

            cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

            cmd.CommandType = CommandType.StoredProcedure; // the type of the command, can also be stored procedure

            cmd.Parameters.AddWithValue("@studentId", studentId);
            cmd.Parameters.AddWithValue("@postId", postId);


            return cmd;
        }


        public void LowerStudentTags(int studentId, List<Tag> tags)
        {
            SqlConnection con;
            SqlCommand cmd;

            try
            { con = connect("myProjDB"); }
            catch (Exception ex)
            { throw (ex); }

            cmd = spLowerStudentTags(studentId, tags, con);
            Console.WriteLine(tags);

            try
            { cmd.ExecuteNonQuery(); }
            catch (Exception ex)
            { throw (ex); }
            finally
            {
                if (con != null)
                    con.Close();
            }
        }

        private SqlCommand spLowerStudentTags(int studentId, List<Tag> tags, SqlConnection con)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = "q_LowerStudentTags";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@studentId", studentId);
            cmd.Parameters.AddWithValue("@tagJson", JsonConvert.SerializeObject(tags));

            return cmd;
        }
    }
}

