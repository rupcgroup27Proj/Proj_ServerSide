namespace Project_ServerSide.Models.SmartQuestionnaires
{
    public class Questionnaire
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public List<Tag> Tags { get; set; }
        public List<Question> Questions { get; set; }
    }
}
