using _201911041TermProject.Data;
using _201911041TermProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace _201911041TermProject.Pages.Posts
{
    [Authorize]
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DetailsModel(ApplicationDbContext context)
        {
            _context = context;
        }


        public Post Post { get; set; }

        public int IsLike { get; set; }

        public async Task OnGet(int postId)
        {
            Post = await _context.Posts
                .Include(p => p.User)
                .Include(p => p.Image)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == postId);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var interaction = await _context.UsersPosts.FindAsync(userId, postId);

            IsLike = -1;

            if (interaction != null)
            {
                if (interaction.Interaction == Interaction.Like)
                {
                    IsLike = 1;
                }

                else if (interaction.Interaction == Interaction.Dislike)
                {
                    IsLike = 0;
                }
            }


            


        }


        public async Task<IActionResult> OnPostUpdateLike(int postId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var post = await _context.Posts.FindAsync(postId);
            var user = await _context.Users.FindAsync(userId);

            if (post == null || user == null)
            {
                return NotFound();
            }

            // Kullanıcı daha önce post'u beğendiyse, like'ı azalt.
            var userInteraction = await _context.UsersPosts.FindAsync(userId, postId);

            if (userInteraction != null)
            {

                if (userInteraction.Interaction == Interaction.Dislike)
                {
                    post.DislikeCount--;
                    post.LikeCount++;
                    userInteraction.Interaction = Interaction.Like;
                }

                else
                {
                    post.LikeCount--;
                    _context.UsersPosts.Remove(userInteraction);
                }
                
            }

            else
            {
                var newInteraction = new UserPost()
                {
                    PostId = postId,
                    Post = post,
                    UserId = userId,
                    User = user,
                };

                post.LikeCount++;
                newInteraction.Interaction = Interaction.Like;
                await _context.UsersPosts.AddAsync(newInteraction);
            }

            await _context.SaveChangesAsync();
            return RedirectToPage("/Posts/Details", new { postId = post.Id });
        }


        public async Task<IActionResult> OnPostUpdateDislike(int postId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var post = await _context.Posts.FindAsync(postId);
            var user = await _context.Users.FindAsync(userId);

            if (post == null || user == null)
            {
                return NotFound();
            }

            // Kullanıcı daha önce post'u beğendiyse, like'ı azalt.
            var userInteraction = await _context.UsersPosts.FindAsync(userId, postId);

            if (userInteraction != null)
            {
                if (userInteraction.Interaction == Interaction.Like)
                {
                    post.LikeCount--;
                    post.DislikeCount++;
                    userInteraction.Interaction = Interaction.Dislike;
                }

                else
                {
                    post.DislikeCount--;
                    _context.UsersPosts.Remove(userInteraction);
                }

                
                
            }

            else
            {
                var newInteraction = new UserPost()
                {
                    PostId = postId,
                    Post = post,
                    UserId = userId,
                    User = user,
                };

                post.DislikeCount++;
                newInteraction.Interaction = Interaction.Dislike;
                await _context.UsersPosts.AddAsync(newInteraction);
            }

            await _context.SaveChangesAsync();
            return RedirectToPage("/Posts/Details", new { postId = post.Id });
        }
    }
}
