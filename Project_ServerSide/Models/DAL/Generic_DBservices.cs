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
    public class Generic_DBservices
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

        public GenericUser Login(int id, string password, string type)
        {
            SqlConnection con;
            SqlCommand cmd;

            try
            { con = connect("myProjDB"); }
            catch (Exception ex) { throw (ex); }

            cmd = LoginCommand(con, id, password, type);
            GenericUser user = new GenericUser();
            try
            {
                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dataReader.Read())
                {
                    user.Id = Convert.ToInt32(dataReader["Id"]);
                    user.PersonalId = Convert.ToInt32(dataReader["PersonalId"]);
                    user.Password = dataReader["Password"].ToString();
                    user.Email = dataReader["Email"].ToString();
                    user.FirstName = dataReader["Firstname"].ToString();
                    user.LastName = dataReader["Lastname"].ToString();
                    user.Phone = dataReader["Phone"].ToString();
                    user.PictureUrl = dataReader["PictureUrl"].ToString();
                    user.GroupId = Convert.ToInt32(dataReader["groupId"]);
                    user.ParentPhone = (type == "Student") ? dataReader["ParentPhone"].ToString() : null;
                    user.IsAdmin = (type == "Guide") ? Convert.ToBoolean(dataReader["isAdmin"]) : false;
                    user.Type = type;
                    user.StartDate = Convert.ToDateTime(dataReader["StartDate"]);
                    user.EndDate = Convert.ToDateTime(dataReader["EndDate"]);
                }
                return user;
            }
            catch (Exception ex) { throw; }
        }

        private SqlCommand LoginCommand(SqlConnection con, int id, string password, string type)
        {

            SqlCommand cmd = new SqlCommand();
            if (type == "Student")
                cmd.CommandText = "spLoginStudent";
            else if (type == "Teacher")
                cmd.CommandText = "spLoginTeachers";
            else
                cmd.CommandText = "spLoginGuides"; 
            cmd.Connection = con;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Id", id);
            cmd.Parameters.AddWithValue("@Password", password);
            return cmd;
        }

    }
}
