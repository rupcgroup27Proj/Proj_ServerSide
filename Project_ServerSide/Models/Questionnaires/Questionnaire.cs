using Microsoft.AspNetCore.Mvc;
using Project_ServerSide.Models.DAL;
using System.Text.Json.Nodes;

namespace Project_ServerSide.Models.Questionnaires
{
    public class Questionnaire
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public List<Tag> Tags { get; set; }
        public List<Question> Questions { get; set; }


        public static string GetAllQuestionnaires(int groupId)
        {
            Questionnaire_DBservices dbs = new Questionnaire_DBservices();
            return dbs.GetAllQuestionnaires(groupId);
        }


        public static void InsertNewQuestionnaire(int groupId, JsonObject questionnaire)
        {
            Questionnaire_DBservices dbs = new Questionnaire_DBservices();
            dbs.InsertNewQuestionnaire(groupId, questionnaire);
        }


        public static void updateStudentTags(int studentId, List<Tag> tags)
        {
            Questionnaire_DBservices dbs = new Questionnaire_DBservices();
            dbs.updateStudentTags(studentId, tags);
        }


        public static void DeleteQuestionnaire(int questionnaireId)
        {
            Questionnaire_DBservices dbs = new Questionnaire_DBservices();
            dbs.DeleteQuestionnaire(questionnaireId);
        }


    }
}
