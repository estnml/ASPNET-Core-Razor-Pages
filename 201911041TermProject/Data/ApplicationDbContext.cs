using _201911041TermProject.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace _201911041TermProject.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Properties<DateTime>().HaveColumnType("date");
        }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>().Property(u => u.FirstName).HasMaxLength(50).IsRequired();
            builder.Entity<User>().Property(u => u.LastName).HasMaxLength(50).IsRequired();


            builder.Entity<Post>().Property(p => p.Title).HasMaxLength(50).IsRequired();
            builder.Entity<Post>().Property(p => p.Content).IsRequired();

            builder.Entity<Message>().Property(m => m.Content).HasMaxLength(500).IsRequired(false);


            // Many-to-many configuration of UsersPosts table
            builder.Entity<UserPost>().HasKey(up => new
            {
                up.UserId,
                up.PostId
            });
            builder.Entity<UserPost>().Property(up => up.Interaction).HasDefaultValue(Interaction.None);

            builder.Entity<Friendship>().HasKey(f => new
            {
                f.SenderUserId,
                f.ReceiverUserId
            });
            
            
        }

        public DbSet<Message> Messages { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Friendship> Friendships { get; set; }
        public DbSet<UserPost> UsersPosts { get; set; }
    }
}