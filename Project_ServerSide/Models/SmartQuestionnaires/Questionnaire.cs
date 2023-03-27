using Project_ServerSide.Models.DAL;

namespace Project_ServerSide.Models.SmartQuestionnaires
{
    public class Questionnaire
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public List<Tag> Tags { get; set; }
        public List<Question> Questions { get; set; }


        static public void updateStudentTags(int studentId, List<Tag> tags)
        {
            Questionnaire_DBservices dbs = new Questionnaire_DBservices();
            dbs.updateStudentTags(studentId, tags);
        }
    }
}
