using _201911041TermProject.Data;
using _201911041TermProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace _201911041TermProject.Pages.Users
{
    public class FriendshipRequestModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public FriendshipRequestModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Friendship> Friendships { get; set; }
        public List<User> Users { get; set; }

        public async Task OnGet(string currentUserId)
        {
            Friendships = await _context.Friendships.AsNoTracking().Where(f => (f.ReceiverUserId == currentUserId) && (f.IsApproved == false)).ToListAsync();
            Users = await _context.Users.AsNoTracking().ToListAsync();
        }

        public async Task<IActionResult> OnPostHandleFriendRequest(string senderId, string receiverId, bool isAccepted)
        {
            var friendship = await _context.Friendships.FindAsync(senderId, receiverId);

            if (friendship == null)
            {
                return NotFound();
            }

            if (isAccepted)
            {
                friendship.IsApproved = true;
            }

            else
            {
                _context.Friendships.Remove(friendship);
            }

            await _context.SaveChangesAsync();
            return RedirectToPage("/Users/FriendshipRequest");

        }
    }
}
