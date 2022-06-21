using _201911041TermProject.Data;
using _201911041TermProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace _201911041TermProject.Pages.Users
{
    public class DiscoverModel : PageModel
    {

        private readonly ApplicationDbContext _context;

        public DiscoverModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty(SupportsGet = true)]
        public string SearchString { get; set; }


        public List<User> Users { get; set; }

        public async Task OnGet()
        {

            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!string.IsNullOrEmpty(SearchString))
            {
                Users = await _context.Users
                    .Include(u => u.Image)
                    .Include(u => u.Posts)
                    .AsNoTracking()
                    .Where(u => u.FirstName.Contains(SearchString) && (u.Id != currentUserId))
                    .ToListAsync();
            }

            else
            {

                Users = await _context.Users
                    .Include(u => u.Image)
                    .Include(u => u.Posts)
                    .AsNoTracking()
                    .AsNoTracking().Where(u => u.Id != currentUserId).ToListAsync();

            }



        }



        public async Task<IActionResult> OnPostSendFriendRequestAsync(string receiverId)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (await _context.Friendships.FindAsync(currentUserId, receiverId) == null && await _context.Friendships.FindAsync(receiverId, currentUserId) == null)
            {
                var friendRequest = new Friendship()
                {
                    SenderUserId = currentUserId,
                    ReceiverUserId = receiverId,
                    IsApproved = false
                };

                await _context.Friendships.AddAsync(friendRequest);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("/Users/Discover");
        }
    }
}
