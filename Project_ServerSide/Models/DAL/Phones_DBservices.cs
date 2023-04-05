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
    public class Phones_DBservices
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


        // inserts Phone
        //--------------------------------------------------------------------------------------------------
        public int Insert(Phones phones)
        {

            SqlConnection con;
            SqlCommand cmd;

            try
            { con = connect("myProjDB"); }
            catch (Exception ex)
            { throw ex; }

            cmd = CreateInsertPhonesCommand("spInsertPhones", con, phones);

            try
            {
                int numEffected = cmd.ExecuteNonQuery(); // execute the command
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

        private SqlCommand CreateInsertPhonesCommand(String spName, SqlConnection con, Phones phones)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = spName;
            cmd.CommandTimeout = 10;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@phone", phones.Phone);
            cmd.Parameters.AddWithValue("@title", phones.Title);
            cmd.Parameters.AddWithValue("@notes", phones.Notes);
            cmd.Parameters.AddWithValue("@groupId", phones.GroupId);

            return cmd;
        }


        //read all phones 
        //--------------------------------------------------------------------------------------------------

        public List<Phones> Read(int groupId)
        {
            SqlConnection con;
            SqlCommand cmd;

            try
            { con = connect("myProjDB"); }
            catch (Exception ex)
            { throw (ex); }

            cmd = CreateReadPhonesCommand("spReadPhones", con, groupId);

            List<Phones> PhoneList = new List<Phones>();

            try
            {
                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dataReader.Read())
                {
                    Phones u = new Phones();
                    u.Title = dataReader["title"].ToString();
                    u.Phone = dataReader["phone"].ToString();
                    u.Id = Convert.ToInt32(dataReader["id"]);
                    u.GroupId = Convert.ToInt32(dataReader["groupId"]);
                    u.Notes = dataReader["notes"].ToString();
                    PhoneList.Add(u);

                }
                return PhoneList;
            }
            catch (Exception ex)
            { throw (ex); }

            finally
            {
                if (con != null)
                    con.Close();
            }
        }

        private SqlCommand CreateReadPhonesCommand(String spName, SqlConnection con, int groupId)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = spName;
            cmd.CommandTimeout = 10;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@groupId", groupId);

            return cmd;
        }



        //update a phones 
        //--------------------------------------------------------------------------------------------------
        public int Update(Phones phones)
        {
            SqlConnection con;
            SqlCommand cmd;

            try
            { con = connect("myProjDB"); }

            catch (Exception ex)
            { throw (ex); }

            cmd = CreateCommandUpdatePhone("spUpdatePhone", con, phones);

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


        private SqlCommand CreateCommandUpdatePhone(String spName, SqlConnection con, Phones phones)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = spName;
            cmd.CommandTimeout = 10;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@phone", phones.Phone);
            cmd.Parameters.AddWithValue("@title", phones.Title);
            cmd.Parameters.AddWithValue("@notes", phones.Notes);
            cmd.Parameters.AddWithValue("@id", phones.Id);
            cmd.Parameters.AddWithValue("@groupId", phones.GroupId);

            return cmd;
        }

        //Delete a phoneNumber
        public int Delete(int id)
        {
            SqlConnection con;
            SqlCommand cmd;

            try
            { con = connect("myProjDB"); }

            catch (Exception ex)
            { throw (ex); }

            cmd = CreateCommandDeletePhone("spDeletePhone", con, id);

            try
            {
                int numEffected = cmd.ExecuteNonQuery();
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

        private SqlCommand CreateCommandDeletePhone(String spName, SqlConnection con, int id)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = spName;
            cmd.CommandTimeout = 10;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@id", id);
           
            return cmd;
        }

        // pull embassy of Israel
        //---------------------------------------------------------------------------------
        public Phones pullEmbassy(Phones phones)
        {
            SqlConnection con;
            SqlCommand cmd;

            try
            { con = connect("myProjDB"); }

            catch (Exception ex)
            { throw (ex); }

            cmd = CreateCommandPullEmbassy("sppullEmbassy", con, phones);

            Phones X = new Phones();

            try
            {
                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dataReader.Read())
                {
                    X.Title = dataReader["title"].ToString();
                    X.Phone = dataReader["phone"].ToString();
                    X.Notes = dataReader["notes"].ToString();
                }
                return X;

            }
            catch (Exception ex)
            { throw (ex); }

            finally
            {
                if (con != null)
                    con.Close();
            }
        }

        private SqlCommand CreateCommandPullEmbassy(String spName, SqlConnection con, Phones phones)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = spName;
            cmd.CommandTimeout = 10;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@title", phones.Title);

            return cmd;
        }
    }
}


