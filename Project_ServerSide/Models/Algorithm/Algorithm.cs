using Microsoft.VisualBasic;
using System.Buffers.Text;
using System.Security.Cryptography.Xml;
using System.Xml.Linq;
using System;
using System.Data;
using Project_ServerSide.Models.DAL;

namespace Project_ServerSide.Models.Algorithm
{
    public class Algorithm
    {
        //1.Define a two-dimensional array `preferences` to represent the ratings of different items by different users.
        //2. Calculate a similarity matrix `similarity` based on the `preferences` array, where each element `similarity[i, j]` represents the similarity between user `i` and user `j`.
        //3. Make predictions for a specific student `student` using the `preferences` and `similarity` matrices.
        //4.For each item `i` that `student` has not rated:
        //    a.Initialize `numerator` and `denominator` to 0.
        //    b.For each other student `j` who has rated the item:
        //        i.If `j` is equal to `student`, skip this iteration.
        //    ii.If the rating of `j` for item `i` is 0, skip this iteration.
        //        iii.Add the product of `similarity[student, j]` and `preferences[j, i]` to `numerator`.
        //        iv.Add the absolute value of `similarity[student, j]` to `denominator`.
        //    c.If `denominator` is 0, set the prediction for item `i` to the average rating of all items.
        //    d.Otherwise, set the prediction for item `i` to `numerator / denominator`.
        //5. Print out the predictions for student `student`.

        static public List<string> RunAlgorithm(bool isGettingTags, int studentId)
        {
            Algorithm_DBS a_dbs = new Algorithm_DBS();

            DataTable predictionsTable = new DataTable();
            predictionsTable.Columns.Add("currentStudentId", typeof(int));
            predictionsTable.Columns.Add("tagId", typeof(int));
            predictionsTable.Columns.Add("predictedValue", typeof(double));

            double[,] preferences = GetPreferences();

            // Calculate similarity matrix
            double[,] similarity = CalculateSimilarity(preferences);

            // Make predictions for all students
            int studentCount = preferences.GetLength(0);
            int tagCount = preferences.GetLength(1);

            for (int i = 0; i < studentCount; i++)
            {
                for (int j = 1; j < tagCount; j++)
                {
                    if (preferences[i, j] == 0)
                    {
                        double[] predictions = Predict(preferences, similarity, i);
                        double predictionValue = predictions[j];

                        predictionsTable.Rows.Add(i + 1, j, predictionValue);
                    }
                    else
                    {
                        double predictionValue = preferences[i, j];
                        predictionsTable.Rows.Add(i + 1, j, predictionValue);
                    }

                }
            }
            
            if (isGettingTags)
                return a_dbs.GetStudentRecommandations(studentId);
            else
            {
                //Stores all prediction in the DB for future usage (while recommanding data from wikipedia)
                a_dbs.FillPredictionTable(predictionsTable);
                return new List<string>();
            }
        }


        static public double[,] GetPreferences()
        {
            Algorithm_DBS a_dbs = new Algorithm_DBS();
            return a_dbs.GetPreferences();
        }


        static double[,] CalculateSimilarity(double[,] preferences)
        {
            int studentCount = preferences.GetLength(0);
            int tagCount = preferences.GetLength(1);

            double[,] similarity = new double[studentCount, studentCount]; //Similarity calculation between students. 

            for (int i = 0; i < studentCount; i++)
            {
                for (int j = 0; j < studentCount; j++)
                {
                    if (i == j)
                    {
                        similarity[i, j] = 0;
                        continue;
                    }

                    double numerator = 0;
                    double denominator1 = 0;
                    double denominator2 = 0;

                    for (int k = 1; k < tagCount; k++)
                    {
                        numerator += preferences[i, k] * preferences[j, k];
                        denominator1 += preferences[i, k] * preferences[i, k];
                        denominator2 += preferences[j, k] * preferences[j, k];
                    }

                    if (denominator1 == 0 || denominator2 == 0)
                        similarity[i, j] = 0;
                    else
                        similarity[i, j] = (numerator / (Math.Sqrt(denominator1) * Math.Sqrt(denominator2)));
                }
            }

            return similarity;
        }


        static double[] Predict(double[,] preferences, double[,] similarity, int student)
        {
            int studentCount = preferences.GetLength(0);
            int tagCount = preferences.GetLength(1);

            double[] predictions = new double[tagCount];

            for (int i = 0; i < tagCount; i++)
            {
                if (preferences[student, i] != 0)
                    continue;

                double numerator = 0;
                double denominator = 0;

                for (int j = 0; j < studentCount; j++)
                {
                    if (preferences[j, i] == 0 || j == student)
                        continue;

                    numerator += similarity[student, j] * preferences[j, i];
                    denominator += Math.Abs(similarity[student, j]);
                }

                if (denominator == 0)
                    predictions[i] = 0;
                else
                    predictions[i] = numerator / denominator;

                if (predictions[i] == 0)
                    predictions[i] = (preferences.Cast<double>().Average());//Default value in case that there is no prediction (when no one os similar to the student).                      
            }

            return predictions;
        }
    }
}





