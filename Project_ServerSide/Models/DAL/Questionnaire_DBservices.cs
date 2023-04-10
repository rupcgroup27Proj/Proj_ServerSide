using System.Data;
using System.Data.SqlClient;
using System.Text.Json;
using System.Text.Json.Nodes;
using Newtonsoft.Json;
using Project_ServerSide.Models.SmartQuestionnaires;


namespace Project_ServerSide.Models.DAL
{
    public class Questionnaire_DBservices
    {
        private class JSONQuestionnaire
        {
            string id;
            string title;
            string description;
            List<Tag> tags;
            List<Question> questions;

            public string Id { get => id; set => id = value; }
            public string Title { get => title; set => title = value; }
            public string Description { get => description; set => description = value; }
            public List<Tag> Tags { get => tags; set => tags = value; }
            public List<Question> Questions { get => questions; set => questions = value; }
        }

        public SqlConnection connect(String conString)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json").Build();
            string cStr = configuration.GetConnectionString("myProjDB");
            SqlConnection con = new SqlConnection(cStr);
            con.Open();
            return con;
        }



        public void InsertNewQuestionnaire(int groupId, JsonObject sq)
        {
            SqlConnection con;
            SqlCommand cmd;

            try
            { con = connect("myProjDB"); }
            catch (Exception ex)
            { throw (ex); }

            cmd = spPostQuestionnare(con, groupId, sq);

            try
            {cmd.ExecuteNonQuery();}

            catch (Exception ex)
            { throw (ex); }

            finally
            {
                if (con != null)
                    con.Close();
            }
        }

        private SqlCommand spPostQuestionnare(SqlConnection con, int groupId, JsonObject json) 
        {
            string jsonString = System.Text.Json.JsonSerializer.Serialize(json);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = "InsertQuestionnaire";
            cmd.CommandTimeout = 10;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@groupId", groupId);
            cmd.Parameters.AddWithValue("@jsonData", jsonString);

            return cmd;
        }



        public string GetAllQuestionnaires(int groupId)
        {
            SqlConnection con;
            try
            { con = connect("myProjDB"); }
            catch (Exception ex)
            { throw (ex); }

            //get all questionnaires with their id, title and description:
            List<Dictionary<string, string>> questionnaires = getQuestionnaires(groupId, con);

            //get the tags of all questionnares
            List<Dictionary<string, string>> tags = getQuestionnaireTags(groupId, con);

            //get all questionnaire questions' ids
            List<Dictionary<string, string>> questions = getQuestions(groupId, con);

            //get all questions options
            List<Dictionary<string, string>> options = getOptions(groupId, con);

            //the list we will send back
            List<JSONQuestionnaire> data = new List<JSONQuestionnaire>();

            foreach (var questionnaire in questionnaires)
            {
                JSONQuestionnaire tmpQuestionnaire = new JSONQuestionnaire();
                tmpQuestionnaire.Id = questionnaire["questionnaireId"];
                tmpQuestionnaire.Title = questionnaire["title"];
                tmpQuestionnaire.Description = questionnaire["description"];
                tmpQuestionnaire.Tags = new List<Tag>();
                tmpQuestionnaire.Questions = new List<Question>();

                //fill the questionnaire with all its tags
                foreach (var tag in tags)
                {
                    if (tag["questionnaireId"] == questionnaire["questionnaireId"])
                    {
                        Tag t = new Tag
                        {
                            TagId = Convert.ToInt32(tag["tagId"]),
                            GroupId = Convert.ToInt32(tag["groupId"]),
                            TagName = tag["tagName"]
                        };

                        tmpQuestionnaire.Tags.Add(t);
                    }
                }


                //fill the questionnaire with all its questions
                foreach (var question in questions)
                {
                    if (question["questionnaireId"] == questionnaire["questionnaireId"])
                    {
                        Question q = new Question();
                        q.Text = question["questionText"];
                        q.Type = question["type"];

                        List<object> opt = new List<object>();
                        string correctOption = "";

                        foreach (var option in options)
                        {
                            if (option["questionId"] == question["questionId"])
                            {
                                if (Convert.ToBoolean(option["true"]) == true)
                                    correctOption = option["text"];
                                var opti = new
                                {
                                    choiceId = option["choiceId"],
                                    text = option["text"]
                                };
                                object o = opti;
                                opt.Add(o);
                            };
                        }

                        q.Options = opt;
                        q.CorrectOption = correctOption;

                        tmpQuestionnaire.Questions.Add(q);
                    }
                }
                data.Add(tmpQuestionnaire);
            }

            con.Close();

            string jsonString = JsonConvert.SerializeObject(data);

            return jsonString;

        }

        private List<Dictionary<string, string>> getQuestionnaires(int groupId, SqlConnection con)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = "q_getQuestionnairesIdsAndTitles";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@groupId", groupId);

            try
            {
                SqlDataReader dataReader = cmd.ExecuteReader();
                List<Dictionary<string, string>> result = new List<Dictionary<string, string>>();

                while (dataReader.Read())
                {
                    Dictionary<string, string> questionnaire = new()
                    {
                        {"questionnaireId", dataReader["questionnaireId"].ToString()},
                        {"title", dataReader["title"].ToString()},
                        {"description", dataReader["description"].ToString() }
                    };
                    result.Add(questionnaire);
                }
                dataReader.Close();//Close the dataReader so that i could open another one on the same connection.
                return result;
            }
            catch (Exception ex)
            { throw; }
        }

        private List<Dictionary<string, string>> getQuestionnaireTags(int groupId, SqlConnection con)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = "q_getQuestionnaireTags";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@groupId", groupId);

            try
            {
                SqlDataReader dataReader = cmd.ExecuteReader();
                List<Dictionary<string, string>> result = new List<Dictionary<string, string>>();

                while (dataReader.Read())
                {
                    Dictionary<string, string> tag = new()
                    {
                        {"questionnaireId", dataReader["questionnaireId"].ToString()},
                        {"tagId", dataReader["tagId"].ToString()},
                        {"groupId", dataReader["groupId"].ToString()},
                        {"tagName", dataReader["tagName"].ToString()}
                    };

                    result.Add(tag);
                }
                dataReader.Close();//Close the dataReader so that i could open another one on the same connection.
                return result;
            }
            catch (Exception ex)
            { throw; }
        }

        private List<Dictionary<string, string>> getQuestions(int groupId, SqlConnection con)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = "q_getQuestions";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@groupId", groupId);

            try
            {
                SqlDataReader dataReader = cmd.ExecuteReader();
                List<Dictionary<string, string>> result = new List<Dictionary<string, string>>();

                while (dataReader.Read())
                {
                    Dictionary<string, string> question = new()
                    {
                        {"questionnaireId", dataReader["questionnaireId"].ToString()},
                        {"questionId", dataReader["questionId"].ToString()},
                        {"questionText", dataReader["question"].ToString()},
                        {"type", dataReader["type"].ToString()}
                    };
                    result.Add(question);
                }
                dataReader.Close();//Close the dataReader so that i could open another one on the same connection.
                return result;
            }
            catch (Exception ex)
            { throw; }
        }

        private List<Dictionary<string, string>> getOptions(int groupId, SqlConnection con)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = "q_getOptions";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@groupId", groupId);

            try
            {
                SqlDataReader dataReader = cmd.ExecuteReader();
                List<Dictionary<string, string>> result = new List<Dictionary<string, string>>();

                while (dataReader.Read())
                {
                    Dictionary<string, string> question = new()
                    {
                        {"questionnaireId", dataReader["questionnaireId"].ToString()},
                        {"questionId", dataReader["questionId"].ToString()},
                        {"choiceId", dataReader["choiceId"].ToString()},
                        {"text", dataReader["text"].ToString()},
                        {"true", dataReader["true"].ToString()}
                    };
                    result.Add(question);
                }
                dataReader.Close();//Close the dataReader so that i could open another one on the same connection.
                return result;
            }
            catch (Exception ex)
            { throw; }
        }



        public void updateStudentTags(int studentId, List<Tag> tags)
        {
            SqlConnection con;
            SqlCommand cmd;

            try
            { con = connect("myProjDB"); }
            catch (Exception ex)
            { throw (ex); }

            cmd = spUpdateStudentTags(studentId, tags, con);
            Console.WriteLine(tags);

            try
            { cmd.ExecuteNonQuery(); }
            catch (Exception ex)
            { throw (ex); }
            finally
            {
                if (con != null)
                    con.Close();
            }
        }

        private SqlCommand spUpdateStudentTags(int studentId, List<Tag> tags, SqlConnection con)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = "q_updateStudentTags";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@studentId", studentId);
            cmd.Parameters.AddWithValue("@tagJson", JsonConvert.SerializeObject(tags));

            return cmd;
        }



        public void DeleteQuestionnaire(int questionnaireId)
        {
            SqlConnection con;
            SqlCommand cmd;

            try
            { con = connect("myProjDB"); }
            catch (Exception ex)
            { throw (ex); }


            cmd = spDeleteQuestionnaire(con, questionnaireId);

            try
            { cmd.ExecuteNonQuery(); }
            catch (Exception ex)
            { throw (ex); }

            finally
            {
                if (con != null)
                    con.Close();
            }
        }

        private SqlCommand spDeleteQuestionnaire(SqlConnection con, int questionnaireId)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = "q_deleteQuestionnaire";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@questionnaireId", questionnaireId);

            return cmd;
        }
    }
}

