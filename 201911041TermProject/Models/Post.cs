using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _201911041TermProject.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime Date { get; set; }

        public int LikeCount { get; set; }
        public int DislikeCount { get; set; }

        public List<UserPost> UsersLD { get; set; } = new List<UserPost>();
        
        public string? UserId { get; set; }
        public User User { get; set; }

        public int? ImageId { get; set; }
        public Image Image { get; set; }

    }
}
