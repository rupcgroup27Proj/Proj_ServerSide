using ExcelDataReader.Log;
using System.Data;
using System.Data.SqlClient;

namespace Project_ServerSide.Models.DAL
{
    public class Map_DBservices
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


        public int newMapComponent(int studentId, double lon, double lat, string locationName)
        {
            SqlConnection con;
            SqlCommand cmd;

            try
            { con = connect("myProjDB"); }
            catch (Exception ex)
            { throw (ex); }

            try
            {
                cmd = spPostNewMapComponent(con, studentId, lon, lat, locationName);
                return cmd.ExecuteNonQuery();
            }
            finally
            {
                if (con != null)
                    con.Close();
            }
        }

        public object getStudentMapComponents(int studentId)
        {
            SqlConnection con;
            SqlCommand cmd;

            try
            { con = connect("myProjDB"); }
            catch (Exception ex)
            { throw (ex); }

            try
            {
                List<Map> mapComponents = new List<Map>();

                cmd = spGetMapComponents(con, studentId);
                SqlDataReader dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    Map mapComponent = new Map();
                    mapComponent.StudentId = Convert.ToInt32(dataReader["studentId"]);
                    mapComponent.LocationName = dataReader["locationName"].ToString();
                    mapComponent.LocationId = Convert.ToInt32(dataReader["locationId"]);
                    mapComponent.Latitude = Convert.ToDouble(dataReader["latitude"]);
                    mapComponent.Longitude = Convert.ToDouble(dataReader["longitude"]);
                    if (dataReader.IsDBNull(dataReader.GetOrdinal("fileUrl")))
                    {
                        mapComponents.Add(mapComponent);
                        continue;
                    }
                    mapComponent.FileID = Convert.ToInt32(dataReader["fileID"]);
                    mapComponent.FileUrl = dataReader["fileUrl"].ToString();
                    mapComponent.Description = dataReader["description"].ToString();
                    mapComponents.Add(mapComponent);
                }

                var map = mapComponents
                    .GroupBy(m => new
                    {
                        m.LocationId,
                        m.LocationName,
                        m.Latitude,
                        m.Longitude
                    })
                    .Select(g => new
                    {
                        locationId = g.Key.LocationId,
                        locationName = g.Key.LocationName,
                        latitude = g.Key.Latitude,
                        longitude = g.Key.Longitude,
                        files = g.Select(f => new
                        {
                            fileId = f.FileID,
                            fileUrl = f.FileUrl,
                            description = f.Description
                        }).ToArray()
                    })
                    .ToArray();

                return map;

            }
            finally
            {
                if (con != null)
                    con.Close();
            }
        }

        public int deleteLocation(int locationId)
        {
            SqlConnection con;
            SqlCommand cmd;

            try
            { con = connect("myProjDB"); }
            catch (Exception ex)
            { throw (ex); }

            try
            {
                cmd = spDeleteLocation(con, locationId);
                return cmd.ExecuteNonQuery();
            }
            finally
            {
                if (con != null)
                    con.Close();
            }
        }

        public int deleteImage(int fileId)
        {
            SqlConnection con;
            SqlCommand cmd;

            try
            { con = connect("myProjDB"); }
            catch (Exception ex)
            { throw (ex); }

            try
            {
                cmd = spDeleteImage(con, fileId);
                return cmd.ExecuteNonQuery();
            }
            finally
            {
                if (con != null)
                    con.Close();
            }
        }

        public int addImageToLocation(string uniqueFileName, string description, int studentId, int locationId)
        {
            SqlConnection con;
            SqlCommand cmd;

            try
            { con = connect("myProjDB"); }
            catch (Exception ex)
            { throw (ex); }

            try
            {
                cmd = spAddImageToLocation(con, uniqueFileName, description, studentId, locationId);
                return cmd.ExecuteNonQuery();
            }
            finally
            {
                if (con != null)
                    con.Close();
            }
        }


        
        private SqlCommand spPostNewMapComponent(SqlConnection con, int studentId, double lon, double lat, string locationName)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = "spPostNewMapComponent";
            cmd.CommandTimeout = 10;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@longitude", lon);
            cmd.Parameters.AddWithValue("@latitude", lat);
            cmd.Parameters.AddWithValue("@name", locationName);
            cmd.Parameters.AddWithValue("@studentId", studentId);
            return cmd;
        }

        private SqlCommand spGetMapComponents(SqlConnection con, int studentId)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = "spGetMapComponents";
            cmd.CommandTimeout = 10;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@studentId", studentId);
            return cmd;
        }

        private SqlCommand spDeleteLocation(SqlConnection con, int locationId)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = "spDeleteLocation";
            cmd.CommandTimeout = 10;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@locationId", locationId);
            return cmd;
        }

        private SqlCommand spDeleteImage(SqlConnection con, int fileId)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = "spDeleteImage";
            cmd.CommandTimeout = 10;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@fileId", fileId);
            return cmd;
        }

        private SqlCommand spAddImageToLocation(SqlConnection con, string uniqueFileName, string description, int studentId, int locationId)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = "spAddImageToLocation";
            cmd.CommandTimeout = 10;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@fileUrl", uniqueFileName);
            cmd.Parameters.AddWithValue("@description", description);
            cmd.Parameters.AddWithValue("@studentId", studentId);
            cmd.Parameters.AddWithValue("@locationId", locationId);
            return cmd;
        }
    }
}
