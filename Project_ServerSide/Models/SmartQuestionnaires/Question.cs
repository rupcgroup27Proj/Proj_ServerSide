﻿namespace Project_ServerSide.Models.SmartQuestionnaires
{
    public class Question
    {
        public string Type { get; set; }
        public string Text { get; set; }
        public List<object> Options { get; set; }
        public object CorrectOption { get; set; }
    }
}
