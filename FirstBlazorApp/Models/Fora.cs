

using System.ComponentModel.DataAnnotations.Schema;

namespace ForumAdminPanel.Models
{
    public class Fora
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ThemesCounter { get; set; }
        public int UserCounter { get; set; }

        //[ForeignKey("MainForum")]
        public int MainForumId { get; set; }
        //public MainForum MainForums { get; set; }

        // Navigation props

        public List<Theme> Themes { get; set; }
    }
}
