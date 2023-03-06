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
using System.Collections;
using Project_ServerSide.Models.Algorithm;

namespace Project_ServerSide.Models.DAL
{
    public class Algorithm_DBS
    {
        //////////////////////////////////////////
        ///might need to adapt the code for normalized "TagCount" (to 5).
        const int CoreTagsQuantity = 3;
        const int normScalar = 5;
        List<int> tags = new List<int>();
        List<StudentTagJSON> studentsTags = new List<StudentTagJSON>();
        double maxTagCount = 0;
        double minTagCount = 0;
        //////////////////////////////////////////

        public SqlConnection connect(String conString)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json").Build();
            string cStr = configuration.GetConnectionString("myProjDB");
            SqlConnection con = new SqlConnection(cStr);
            con.Open();
            return con;
        }

        //Functions
        public double[,] GetPreferences()
        {
            SqlConnection con;
            SqlCommand cmd;

            //Get all tags
            try
            { con = connect("myProjDB"); }
            catch (Exception ex)
            { throw (ex); }

            cmd = spGetTags(con);
            try
            {
                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                tags.Clear();

                while (dataReader.Read())
                    tags.Add(Convert.ToInt32(dataReader["tagId"]));
            }

            catch (Exception ex)
            { throw (ex); }

            finally
            {
                if (con != null)
                    con.Close();
            }


            //Get the maximum TagCount and minimum TagCount for normalization.
            try
            { con = connect("myProjDB"); }
            catch (Exception ex)
            { throw (ex); }

            cmd = spGetMaxMin(con);
            try
            {
                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);


                while (dataReader.Read())
                {
                    maxTagCount = Convert.ToDouble(dataReader["Max"]);
                    minTagCount = Convert.ToDouble(dataReader["Min"]);
                }
            }

            catch (Exception ex)
            { throw (ex); }

            finally
            {
                if (con != null)
                    con.Close();
            }


            //Get all studentTags and tagCounts
            try
            { con = connect("myProjDB"); }
            catch (Exception ex)
            { throw (ex); }

            cmd = spGetStudentdTags(con);
            try
            {
                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                studentsTags.Clear();
                double tmpTagCount = 0;
                while (dataReader.Read())
                {
                    StudentTagJSON studentTag = new StudentTagJSON();
                    studentTag.StudentId = Convert.ToInt32(dataReader["studentId"]);
                    studentTag.TagId = Convert.ToInt32(dataReader["TagId"]);
                    tmpTagCount = Convert.ToDouble(dataReader["TagCount"]);
                    studentTag.TagCount = ((tmpTagCount - minTagCount) / maxTagCount - minTagCount) * normScalar;

                    studentsTags.Add(studentTag);
                }
            }

            catch (Exception ex)
            { throw (ex); }

            finally
            {
                if (con != null)
                    con.Close();
            }

            //Create the matrix - tags and studentTags are already sorted corresponding to each other.
            var idList = studentsTags.Select(o => o.StudentId).Distinct().ToList(); //Create a list with all ids (there are 3 objects for the same student, we need only one)
            int Rows = idList.Count;
            int Cols = tags.Count + 1;

            double[,] preferencesMatrix = new double[Rows, Cols];

            // Fill the matrix with the values from the objects
            for (int i = 0; i < Rows; i++)
            {
                preferencesMatrix[i, 0] = idList[i];
                var objectsWithId = studentsTags.Where(o => o.StudentId == idList[i]).ToList(); //Runs over studentsTags, returns all objects where their ids
                                                                                                //equals to the row's StudentId.
                for (int j = 0; j < objectsWithId.Count; j++)
                    preferencesMatrix[i, j + 1] = objectsWithId[j].TagCount; //Fills the columns with the tags count
            }
            return preferencesMatrix;
        }


        public void FillPredictionTable(DataTable predictionsTable)
        {
            SqlConnection con;
            SqlCommand cmd;

            try
            { con = connect("myProjDB"); }
            catch (Exception ex)
            { throw (ex); }

            cmd = spClearPredictionTable(con);

            try
            {
                cmd.ExecuteNonQuery(); // execute the command
            }

            catch (Exception ex)
            { throw (ex); }

            finally
            {
                if (con != null)
                    con.Close();
            }

            try
            { con = connect("myProjDB"); }
            catch (Exception ex)
            { throw (ex); }

            cmd = spFillPredictionTable(con, predictionsTable);

            try
            { cmd.ExecuteNonQuery(); } // execute the command 
            catch (Exception ex)
            { throw (ex); }

            finally
            {
                if (con != null)
                    con.Close();
            }
        }


        public List<string> GetStudentRecommandations(int studentId)
        {
            SqlConnection con;
            SqlCommand cmd;

            try
            { con = connect("myProjDB"); }
            catch (Exception ex)
            { throw (ex); }

            cmd = spGetStudentRecommandations(con, studentId);

            try
            {
                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                List<string> studentRecommandations = new List<string>();
                while (dataReader.Read())
                    studentRecommandations.Add((dataReader["tagName"]).ToString());

                return studentRecommandations;
            }

            catch (Exception ex)
            { throw (ex); }

            finally
            {
                if (con != null)
                    con.Close();
            }
        }


        //Stored Procedure
        private SqlCommand spGetTags(SqlConnection con)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = "AlgorithmGetAllTags";
            cmd.CommandTimeout = 10;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            return cmd;
        }

        private SqlCommand spGetMaxMin(SqlConnection con)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = "AlgorithmGetMaxMin";
            cmd.CommandTimeout = 10;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            return cmd;
        }

        private SqlCommand spGetStudentdTags(SqlConnection con)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = "AlgorithmGetStudentsTags";
            cmd.CommandTimeout = 10;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            return cmd;
        }

        private SqlCommand spFillPredictionTable(SqlConnection con, DataTable predictionsTable)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = "AlgorithmPredictionResults";
            cmd.CommandTimeout = 10;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@data", predictionsTable);
            return cmd;
        }

        private SqlCommand spClearPredictionTable(SqlConnection con)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = "AlgorithmClearPredictionTable";
            cmd.CommandTimeout = 10;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            return cmd;
        }

        private SqlCommand spGetStudentRecommandations(SqlConnection con, int studentId)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = "AlgorithmGetStudentRecommandations";
            cmd.CommandTimeout = 10;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@studentId", studentId);
            return cmd;
        }
    }
}
