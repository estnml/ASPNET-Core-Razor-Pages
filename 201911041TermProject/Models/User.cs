using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _201911041TermProject.Models
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

        public bool IsProfileHidden { get; set; } = false;

        public string? Description { get; set; } = string.Empty;

        public DateTime? DateOfBirth { get; set; }

        public List<Post> Posts { get; set; } = new List<Post>();

        public List<UserPost> PostsLD { get; set; } = new List<UserPost>();

        public string? City { get; set; }

        public int? ImageId { get; set; }
        public Image Image { get; set; } = new Image();
    }
}
