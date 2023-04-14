using Microsoft.VisualBasic;
using System.Buffers.Text;
using System.Security.Cryptography.Xml;
using System.Xml.Linq;
using System;
using System.Data;
using Project_ServerSide.Models.DAL;
using System.Reflection;


namespace Project_ServerSide.Models.Algorithm
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

    public class Algorithm
    {
        static public void RunAlgorithm()
        {
            //Creates a table in which we will calc the predicted value of each tag 
            DataTable predictionsTable = new DataTable();                        //
            predictionsTable.Columns.Add("currentStudentId", typeof(int));       //
            predictionsTable.Columns.Add("tagId", typeof(int));                  //
            predictionsTable.Columns.Add("predictedValue", typeof(double));      //
            //-------------------------------------------------------------------//

            //Connect to the DB and create a table of all students tagCount for each tag:
            //studentId | tag1 | tag2 // (this row not exists, it's just for our intuition)
            //     1    | 4.5  | 1.35 //
            //     2    |  0   | 1.2  //
            double[,] preferences = GetPreferences();

            // Calculate similarity matrix
            double[,] similarity = CalculateSimilarity(preferences);

            // Make predictions for all students
            int studentCount = preferences.GetLength(0);
            int tagCount = (preferences.GetLength(1));

            Algorithm_DBservices a_dbs = new Algorithm_DBservices();
            List<int> studentsIds = a_dbs.GetStudentsIds();
            List<int> tagsIds = a_dbs.GetTagsIds();

            //Create a prediction table and fill it with values
            for (int i = 0; i < studentCount; i++)
            {
                double[] predictions = Predict(preferences, similarity, studentsIds[i]);
                for (int j = 1; j < tagCount; j++)
                {
                    //for each tag that the current student has not yet rated:
                    if (preferences[i, j] == 0)
                    {
                        double predictionValue = predictions[j];
                        predictionsTable.Rows.Add(studentsIds[i], tagsIds[j - 1], predictionValue);
                    }
                    else
                    {
                        double predictionValue = preferences[i, j];
                        predictionsTable.Rows.Add(studentsIds[i], tagsIds[j - 1], predictionValue);
                    }
                }
            }
            //Stores all prediction in the DB for future usage (when recommanding data from Wikipedia)
            a_dbs.FillPredictionTable(predictionsTable);
        }


        static public double[,] GetPreferences()
        {
            Algorithm_DBservices a_dbs = new Algorithm_DBservices();
            return a_dbs.GetPreferences();
        }


        static double[,] CalculateSimilarity(double[,] preferences)
        {
            int studentCount = preferences.GetLength(0);
            int tagCount = preferences.GetLength(1);

            //Similarity calculation between EACH student to EACH other student.Returns the inner cells:
            //          |    student1   |    student2   |
            // student1 |       0       |(1,2)similarity|
            // student2 |(2,1)similarity|       0       |
            //
            double[,] similarity = new double[studentCount, studentCount];

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

                    //Using Cosine Similarity formule. -1 indicates a perfect negative similarity,
                    //0 indicates no similarity, and +1 indicates a perfect positive similarity between the two students' preferences.
                    //The higher the number, the more wieght we'll give to the matching student tagCount.
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
            Algorithm_DBservices dbs = new Algorithm_DBservices();
            List<int> studentsIds = dbs.GetStudentsIds();

            int studentCount = preferences.GetLength(0);
            int tagCount = preferences.GetLength(1);
            int studentIndex = studentsIds.IndexOf(student);

            double[] predictions = new double[tagCount];

            //Calculate predictions based on similarity and preferences
            for (int i = 0; i < tagCount; i++)
            {
                //If the current student has already "rated" the tag,
                //no need to predict it for him.
                if (preferences[studentIndex, i] != 0)
                    continue;

                //Else, try to predict what his "rating" of the current
                //tag would be:
                double numerator = 0;
                double denominator = 0;

                //Run over the tagCount of all other students
                for (int j = 0; j < studentCount; j++)
                {
                    if (preferences[j, i] == 0 || j == studentIndex)
                        continue;

                    numerator += similarity[studentIndex, j] * preferences[j, i];
                    denominator += Math.Abs(similarity[studentIndex, j]);
                }

                if (denominator == 0)
                    predictions[i] = 0;
                else
                    predictions[i] = numerator / denominator;

                if (predictions[i] == 0)
                    predictions[i] = calcColAvg(preferences, i);//Default value in case that there is no prediction (when no one is similar to the student).                      
            }

            return predictions;
        }


        static double calcColAvg(double[,] preferences, int column)
        {
            //Calc the average of Preferences
            int deno = 0;
            double sum = 0;
            for (int row = 0; row < preferences.GetLength(0); row++)
            {
                for (int col = 0; col < preferences.GetLength(1); col++)
                {
                    if (col == 0 || col != column || preferences[row, col] == 0)
                        continue;
                    sum += preferences[row, col];
                    deno += 1;
                }
            }

            return deno == 0 ? 0 : sum / deno;
        }


        static public List<string> GetStudentRecommandations(int studentId)
        {
            Algorithm_DBservices a_dbs = new Algorithm_DBservices();
            return a_dbs.GetStudentRecommandations(studentId);
        }
    }
}





