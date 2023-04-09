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
using System.Data.Common;

namespace Project_ServerSide.Models.DAL
{
    public class Algorithm_DBS
    {
        //________________________________________________________________________
        const int normScalar = 5;  //used for normalizing tagCount between 0 to 5.
        double maxTagCount = 0;
        double minTagCount = 0;
        List<int> tags = new List<int>();
        List<StudentTagJSON> studentsTags = new List<StudentTagJSON>();
        //________________________________________________________________________


        public SqlConnection connect(String conString)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json").Build();
            string cStr = configuration.GetConnectionString("myProjDB");
            SqlConnection con = new SqlConnection(cStr);
            con.Open();
            return con;
        }

        // ================================ | MAIN FUNCTIONS | ================================== //

        public double[,] GetPreferences()
        {
            //Get the preferences of all students (tagCount for each tag by students)
            SqlConnection con;
            SqlCommand cmd;

            try
            { con = connect("myProjDB"); }
            catch (Exception ex)
            { throw (ex); }

            try
            {
                //Get all tag ids
                cmd = spGetTags(con);
                SqlDataReader dataReader1 = cmd.ExecuteReader();
                tags.Clear();

                while (dataReader1.Read())
                {
                    tags.Add(Convert.ToInt32(dataReader1["tagId"]));
                }
                dataReader1.Close();


                //Get the maximum TagCount and minimum TagCount for normalization.
                cmd = spGetMaxMin(con);
                SqlDataReader dataReader2 = cmd.ExecuteReader();

                while (dataReader2.Read())
                {
                    maxTagCount = Convert.ToDouble(dataReader2["Max"]);
                    minTagCount = Convert.ToDouble(dataReader2["Min"]);
                }
                dataReader2.Close();


                //Get all studentTags and tagCounts: ["studentId"|"TagId"|"TagCount"]
                cmd = spGetStudentsTags(con);
                SqlDataReader dataReader3 = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                studentsTags.Clear();
                double tmpTagCount = 0;

                while (dataReader3.Read())
                {
                    StudentTagJSON studentTag = new StudentTagJSON();
                    studentTag.StudentId = Convert.ToInt32(dataReader3["studentId"]);
                    studentTag.TagId = Convert.ToInt32(dataReader3["tagId"]);
                    tmpTagCount = Convert.ToDouble(dataReader3["tagCount"]);
                    studentTag.TagCount = (maxTagCount == minTagCount) ? 
                       0 : ((tmpTagCount - minTagCount) / maxTagCount - minTagCount) * normScalar;

                    studentsTags.Add(studentTag);//Reformatting the tags, changing each TagCount to a normalized value
                                                 //so that the algorithm will work better
                }
                dataReader3.Close();

            }
            catch (Exception ex)
            { throw (ex); }

            finally
            {
                if (con != null)
                    con.Close();
            }

            /////////////////////////////////////////////////////////////////////////////////////////////////
            ///Now that we have created "studentTags", we have all tags of group 0 connected to each student.
            ///Each tag is built in the following format: {studentId: 1, tagId: 1, tagCount: 2.34}...
            ///If a student didn't encounterd a specific tag yet, tagCount = 0 automatically.
            /////////////////////////////////////////////////////////////////////////////////////////////////

            //Create the matrix - tags and studentTags are already sorted corresponding to each other.
            //Create a list with all student IDs (there are "CoreTagsQuantity" (3 right now) objects for the same student, we need only one)
            var idList = studentsTags.Select(o => o.StudentId).Distinct().ToList();
            int Rows = idList.Count;
            int Cols = tags.Count + 1;

            double[,] preferencesMatrix = new double[Rows, Cols];

            // Fill the matrix with the values from the objects
            for (int i = 0; i < Rows; i++)
            {
                preferencesMatrix[i, 0] = idList[i];
                //Runs over studentsTags, returns all Tag objects (with tagCount) where their ids equals to the row's StudentId.
                var objectsWithId = studentsTags.Where(o => o.StudentId == idList[i]).ToList();

                for (int j = 0; j < objectsWithId.Count; j++)
                    preferencesMatrix[i, j + 1] = objectsWithId[j].TagCount; //Fills the columns with the tags count
            }

            return preferencesMatrix;//studentId | tag1 | tag2 //
                                     //     1    | 4.5  | 1.35 //
        }                            //     2    |  0   | 1.2  //


        public List<int> GetStudentsIds()
        {
            SqlConnection con;
            SqlCommand cmd;

            try
            { con = connect("myProjDB"); }
            catch (Exception ex)
            { throw (ex); }

            cmd = spGetStudentsTags(con);
            List<int> studentsIds = new List<int>();

            try
            {
                SqlDataReader dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    int studentId = Convert.ToInt32(dataReader["studentId"]);
                    studentsIds.Add(studentId);
                }
                dataReader.Close();
                return studentsIds.Select(id => id).Distinct().ToList();
            }

            catch (Exception ex)
            { throw (ex); }

            finally
            {
                if (con != null)
                    con.Close();
            }
        }


        public List<int> GetTagsIds()
        {
            SqlConnection con;
            SqlCommand cmd;

            try
            { con = connect("myProjDB"); }
            catch (Exception ex)
            { throw (ex); }

            cmd = spGetTags(con);
            List<int> tagsIds = new List<int>();

            try
            {
                SqlDataReader dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    tagsIds.Add(Convert.ToInt32(dataReader["tagId"]));
                }
                dataReader.Close();
                return tagsIds;
            }

            catch (Exception ex)
            { throw (ex); }

            finally
            {
                if (con != null)
                    con.Close();
            }
        }


        public void FillPredictionTable(DataTable predictionsTable)
        {
            SqlConnection con;
            SqlCommand cmd;

            try
            { con = connect("myProjDB"); }
            catch (Exception ex)
            { throw (ex); }

            try
            {
                cmd = spClearPredictionTable(con);
                cmd.ExecuteNonQuery();

                cmd = spFillPredictionTable(con, predictionsTable);
                cmd.ExecuteNonQuery();
            }
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



        // ================================ | STORED PROCEDURES | ================================== //

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

        private SqlCommand spGetStudentsTags(SqlConnection con)
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
