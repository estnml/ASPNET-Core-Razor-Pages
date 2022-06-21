using _201911041TermProject.Data;
using _201911041TermProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace _201911041TermProject.Pages.Posts
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public IndexModel(ApplicationDbContext context, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }


        public List<Post> FriendsPosts { get; set; }


        public async Task OnGet()
        {
            if (_signInManager.IsSignedIn(User))
            {

                var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var currentUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == currentUserId);

                FriendsPosts = await _context.Posts
                    .AsNoTracking()
                    .Include(p => p.User)
                    .Include(p => p.Image)
                    .Where(post => _context.Friendships.FirstOrDefault(f => f.IsApproved == true && (f.SenderUserId == currentUserId && f.ReceiverUserId == post.UserId)
                    || (f.SenderUserId == post.UserId && f.ReceiverUserId == currentUserId)) != null)
                    .ToListAsync();

            }

        }
    }
}
