﻿

namespace ForumAdminPanel.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public int Value { get; set; }

        // Navigation props

        public int UserId { get; set; }

        public User User { get; set; }
        public int ThemeId { get; set; }
        public Theme Theme { get; set; }
        public int AnswerId { get; set; }
        public List<Answer> Answers { get; set; }

        public DateTime TimePostCreated { get; set; }
    }
}
