﻿namespace Quiz.WebUI.Entities
{
    public class Questions
    {
        public int Id { get; set; }

        public string Question { get; set; }

        public string? Option1 { get; set; }
        public string? Option2 { get; set; }
        public string? Option3 { get; set; }
        public string? Option4 { get; set; }
        public string CorrectOption { get; set; }
        public int Answer { get; set; }
        public string ImageUrl { get; set; }
        public int Second { get; set; }
        public int Puan { get; set; }
        public EnumQuestionType QuestionType { get; set; }


    }
}

public enum EnumQuestionType
{

    choice = 0,
    photo = 1,
    video = 2,
    sound = 3,
    guess = 4,

}