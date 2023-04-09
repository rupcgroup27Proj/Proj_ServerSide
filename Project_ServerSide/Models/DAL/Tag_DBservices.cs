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

namespace Project_ServerSide.Models.DAL
{
    public class Tag_DBservices
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


        //Get all built-in tags - Barel 
        public List<Tag> GetBuiltInTags()
        {
            SqlConnection con;
            SqlCommand cmd;

            try
            { con = connect("myProjDB"); }
            catch (Exception ex)
            { throw (ex); }

            cmd = spGetBuiltInTags(con);

            List<Tag> tags = new List<Tag>();
            try
            {
                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dataReader.Read())
                {
                    Tag tag = new Tag();
                    tag.TagId = Convert.ToInt32(dataReader["tagId"]);
                    tag.GroupId = 0;
                    tag.TagName = dataReader["tagName"].ToString();
                    tags.Add(tag);
                }
                return tags;
            }

            catch (Exception ex)
            { throw (ex); }
            finally
            {
                if (con != null)
                    con.Close();
            }
        }

        private SqlCommand spGetBuiltInTags(SqlConnection con)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = "GetBuiltInTags";
            cmd.CommandTimeout = 10;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            return cmd;
        }



        //ReadTagList                                               
        public List<Tag> GetTags(int groupId)
        {

            SqlConnection con;
            SqlCommand cmd;

            try
            { con = connect("myProjDB"); }
            catch (Exception ex)
            { throw (ex); }

            cmd = CreateCommandGetTags("spGetTags", con, groupId);

            List<Tag> tempList = new List<Tag>();

            try
            {
                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dataReader.Read())
                {
                    Tag tempTagList = new Tag();

                    tempTagList.TagId = Convert.ToInt32(dataReader["tagId"]);
                    tempTagList.TagName = dataReader["tagName"].ToString();

                    tempList.Add(tempTagList);
                }
                return tempList;

            }
            catch (Exception ex)
            { throw (ex); }
            finally
            {
                if (con != null)
                    con.Close();
            }
        }

        private SqlCommand CreateCommandGetTags(string spName, SqlConnection con, int groupId)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = spName;
            cmd.CommandTimeout = 10;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@groupId", groupId);
            return cmd;
        }



        //ReadTagInPost                                                
        public List<Tag> ReadTagInPost(int postId)
        {
            SqlConnection con;
            SqlCommand cmd;

            try
            { con = connect("myProjDB"); }
            catch (Exception ex)
            { throw ex; }


            cmd = CreateCommandGetTagsByPostId("spGetTagsByPostId", con, postId);// create the command

            List<Tag> tempList = new List<Tag>();

            try
            {
                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dataReader.Read())
                {
                    Tag tempTagList = new Tag();

                    tempTagList.TagId = Convert.ToInt32(dataReader["tagId"]);
                    tempTagList.TagName = dataReader["tagName"].ToString();

                    tempList.Add(tempTagList);

                }
                return tempList;

            }
            catch (Exception ex)
            { throw ex; }
            finally
            {
                if (con != null)
                    con.Close();
            }

        }

        private SqlCommand CreateCommandGetTagsByPostId(string spName, SqlConnection con, int postId)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = spName;
            cmd.CommandTimeout = 10;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@postId", postId);
            return cmd;
        }
    }
}
