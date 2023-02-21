using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Text;
using WebApplication1.Models;
using System.Xml.Linq;


public class DBservices
{
    public SqlDataAdapter da;
    public DataTable dt;

    public DBservices()
    {
    }

    //---------------------------------------------------------------------------------------------------------------------------//
    //General DB Functions//
    //---------------------------------------------------------------------------------------------------------------------------//
    public SqlConnection connect(String conString)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json").Build();
        string cStr = configuration.GetConnectionString("myProjDB");
        SqlConnection con = new SqlConnection(cStr);
        con.Open();
        return con;
    }


    //---------------------------------------------------------------------------------------------------------------------------//
                                                   //USER Class DB Functions & SPs//
    //---------------------------------------------------------------------------------------------------------------------------//

    public int InsertOrUpdate(User user, string spName)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        { con = connect("myProjDB"); }
        catch (Exception ex)
        { throw (ex); }

        // String cStr = BuildInsertCommand(user); 
        cmd = CreateSPCommand(spName, con, user.Email, user.FirstName, user.LastName, user.Password); // create the command

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


    private SqlCommand CreateSPCommand(String spName, SqlConnection con, string email, string firstName, string lastName, string password)
    {
        SqlCommand cmd = new SqlCommand(); // create the command object
        cmd.Connection = con;              // assign the connection to the command object
        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 
        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds
        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be stored procedure
        cmd.Parameters.AddWithValue("@email", email);
        cmd.Parameters.AddWithValue("@firstName", firstName);
        cmd.Parameters.AddWithValue("@lastName", lastName);
        cmd.Parameters.AddWithValue("@password", password);
        return cmd;
    }


    public User Login(string email, string password)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        { con = connect("myProjDB"); }
        catch (Exception ex)
        { throw (ex); }

        cmd = CreateSPCommandRead(con, email);

        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            User user = new User();
            while (dataReader.Read())
            {
                
                user.Email = (dataReader["email"]).ToString();
                user.FirstName = (dataReader["firstName"]).ToString();
                user.LastName = (dataReader["lastName"]).ToString();
                user.Password = (dataReader["password"]).ToString();
            }
            if (password == user.Password)
                return user;
            else
            {
                user.Email = null;
                user.FirstName = null;
                user.LastName = null;
                user.Password = null;
                return user;
            }
        }
        catch (Exception ex)
        { throw (ex); }

        finally
        {
            if (con != null)
                con.Close();
        }
    }


    private SqlCommand CreateSPCommandRead(SqlConnection con, string email)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = con;
        cmd.CommandText = "spLogin";
        cmd.CommandTimeout = 10;
        cmd.CommandType = System.Data.CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@email", email);
        return cmd;
    }

    /////////////////////////////////////////////////////////END OF USER///////////////////////////////////////////////////////////
    //---------------------------------------------------------------------------------------------------------------------------//
                                                //VACATION Class DB Functions & SPs//
    //---------------------------------------------------------------------------------------------------------------------------//
   
    public int Insert(Vacation vac)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        { con = connect("myProjDB"); }
        catch (Exception ex)
        { throw (ex); }

        cmd = CreateSPCommand(con, vac.UserId, vac.FlatId, vac.StartDate, vac.EndDate); 

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


    private SqlCommand CreateSPCommand(SqlConnection con, string UserId, int FlatId, DateTime StartDate, DateTime EndDate)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = con;            
        cmd.CommandText = "spInsertVac";     
        cmd.CommandTimeout = 10;         
        cmd.CommandType = System.Data.CommandType.StoredProcedure; 
        cmd.Parameters.AddWithValue("@userId", UserId);
        cmd.Parameters.AddWithValue("@flatId", FlatId);
        cmd.Parameters.AddWithValue("@startDate", StartDate);
        cmd.Parameters.AddWithValue("@endDate", EndDate);
        return cmd;
    }


    public List<Vacation> GetAllVacations()
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        { con = connect("myProjDB"); }
        catch (Exception ex)
        { throw (ex); }

        cmd = CreateSPCommandReadVacs(con);

        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            List<Vacation> tempList = new List<Vacation>();
            
            while (dataReader.Read())
            {
                Vacation tempVac = new Vacation();
                tempVac.Id = Convert.ToInt32(dataReader["id"]);
                tempVac.UserId = (dataReader["userId"]).ToString();
                tempVac.FlatId = Convert.ToInt32(dataReader["flatId"]);
                tempVac.StartDate = Convert.ToDateTime(dataReader["startDate"]);
                tempVac.EndDate = Convert.ToDateTime(dataReader["endDate"]);

                tempList.Add(tempVac);
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


    private SqlCommand CreateSPCommandReadVacs(SqlConnection con)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = con;
        cmd.CommandText = "spGetAllVacations";
        cmd.CommandTimeout = 10;
        cmd.CommandType = System.Data.CommandType.StoredProcedure;
        return cmd;
    }

    //////////////////////////////////////////////////////END OF VACATION//////////////////////////////////////////////////////////
    //---------------------------------------------------------------------------------------------------------------------------//
                                                  //FLAT Class DB Functions & SPs//
    //---------------------------------------------------------------------------------------------------------------------------//
   
    public int Insert(Flat flat)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        { con = connect("myProjDB"); }
        catch (Exception ex)
        { throw (ex); }

        cmd = CreateSPCommand(con, flat.City, flat.Address, flat.Price, flat.Rooms);

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


    private SqlCommand CreateSPCommand(SqlConnection con, string City, string Address, int Price, int Rooms)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = con;
        cmd.CommandText = "spInsertFlat";
        cmd.CommandTimeout = 10;
        cmd.CommandType = System.Data.CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@city", City);
        cmd.Parameters.AddWithValue("@address", Address);
        cmd.Parameters.AddWithValue("@price", Price);
        cmd.Parameters.AddWithValue("@rooms", Rooms);
        return cmd;
    }


    public List<Flat> GetAllFlats()
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        { con = connect("myProjDB"); }
        catch (Exception ex)
        { throw (ex); }

        cmd = CreateSPCommandReadFlats(con);

        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            List<Flat> tempList = new List<Flat>();
            
            while (dataReader.Read())
            {
                Flat tempFlat = new Flat();
                tempFlat.Id = Convert.ToInt32(dataReader["id"]);
                tempFlat.City = (dataReader["city"]).ToString();
                tempFlat.Address = (dataReader["address"]).ToString();
                tempFlat.Price = Convert.ToInt32(dataReader["price"]);
                tempFlat.Rooms = Convert.ToInt32(dataReader["rooms"]);

                tempList.Add(tempFlat);
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


    private SqlCommand CreateSPCommandReadFlats(SqlConnection con)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = con;
        cmd.CommandText = "spGetAllFlats";
        cmd.CommandTimeout = 10;
        cmd.CommandType = System.Data.CommandType.StoredProcedure;
        return cmd;
    }

    /////////////////////////////////////////////////////////END OF FLAT///////////////////////////////////////////////////////////
}
