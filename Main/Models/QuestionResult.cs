﻿namespace ClientQzHalley.Models
{
    public class QuestionResult
    {
        public int UserId { get; set; } // Foreign key to User.Id
        public int QuestionId { get; set; } // Foreign key to Question.Id
        public string? SelectedOption { get; set; } // The option the user chose
    }
}
