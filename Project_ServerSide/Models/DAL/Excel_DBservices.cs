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
    public class Excel_DBservices
    {
        public SqlDataAdapter da;
        public DataTable dt;


        //--------------------------------------------------------------------------------------------------
        // This method creates a connection to the database according to the connectionString name in the web.config 
        //--------------------------------------------------------------------------------------------------
        //public SqlConnection connect(String conString)
        //{

        //    // read the connection string from the configuration file
        //    IConfigurationRoot configuration = new ConfigurationBuilder()
        //    .AddJsonFile("appsettings.json").Build();
        //    string cStr = configuration.GetConnectionString("myProjDB");
        //    SqlConnection con = new SqlConnection(cStr);
        //    con.Open();
        //    return con;
        //}


        private void InsertDataIntoDatabase(DataTable dataTable)
        {
            // Connect to the database
            var connectionString = "Data Source=Media.ruppin.ac.il;Initial Catalog=igroup127_prod; User ID=igroup127; Password=igroup127_29833";
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (var command = new SqlCommand("spInsertStudent", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    foreach (DataRow row in dataTable.Rows)
                    {
                        command.Parameters.Clear();

                        command.Parameters.Add("@Id", SqlDbType.Int).Value = row["StudentId"];
                        command.Parameters.Add("@password", SqlDbType.NVarChar).Value = row["Password"];
                        command.Parameters.Add("@firstName", SqlDbType.NVarChar).Value = row["FirstName"];
                        command.Parameters.Add("@lastName", SqlDbType.NVarChar).Value = row["LastName"];
                        command.Parameters.Add("@phone", SqlDbType.NVarChar).Value = row["Phone"];
                        command.Parameters.Add("@email", SqlDbType.VarChar).Value = row["Email"];
                        command.Parameters.Add("@parentPhone", SqlDbType.VarChar).Value = row["ParentPhone"];
                        command.Parameters.Add("@pictureUrl", SqlDbType.VarChar).Value = row["PictureUrl"];
                        command.Parameters.Add("@groupId", SqlDbType.Int).Value = row["GroupId"];



                        command.ExecuteNonQuery();
                    }
                }
            }
        }
    }
}

            