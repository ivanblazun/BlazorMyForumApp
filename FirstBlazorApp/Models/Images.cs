using ForumAdminPanel.Models;

namespace FirstBlazorApp.Models
{
    public class Images
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public byte[] Content { get; set; }

        public DateTime TimePostCreated { get; set; }
        // Navigation props
        public int UserId { get; set; }

        public User User { get; set; }

        public int ThemeId { get; set; }

        public Theme Theme { get; set; }


    }
}
