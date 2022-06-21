namespace _201911041TermProject.Models
{
    public class UserPost
    {
        // Configure composte key with Fluent API for many-to-many relationship
        public string UserId { get; set; }
        public int PostId { get; set; }

        public Interaction Interaction { get; set; }

        // Navigation Properties
        public User User { get; set; } = new User();
        public Post Post { get; set; } = new Post();
    }
}
