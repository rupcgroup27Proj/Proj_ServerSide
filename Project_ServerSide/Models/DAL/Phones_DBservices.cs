﻿using System;
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
            IConfigurationRoot configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json").Build();
            string cStr = configuration.GetConnectionString("myProjDB");
            SqlConnection con = new SqlConnection(cStr);
            con.Open();
            return con;
        }

        //get phone list by groupId
        //-----------------------------------------------------------------------------------
        public List<Phones> ReadPhoneList(int groupId)
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


        //insert phone
        //-----------------------------------------------------------------------------------
        public int InsertPhone(Phones phones)
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


        //delete phone 
        //-----------------------------------------------------------------------------------
        public int DeletePhone(int id)
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
    }
}


