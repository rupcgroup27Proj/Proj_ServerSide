namespace Project_ServerSide.Models.Questionnaires
{
    public class JSONQuestionnaire
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
}
