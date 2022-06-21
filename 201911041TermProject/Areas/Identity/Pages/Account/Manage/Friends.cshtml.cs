using _201911041TermProject.Data;
using _201911041TermProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace _201911041TermProject.Areas.Identity.Pages.Account.Manage
{
    public class FriendsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public FriendsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<User> FriendList { get; set; } = new List<User>();

        public async Task OnGet()
        {

            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            foreach (var user in _context.Users)
            {
                if (user.Id == currentUserId)
                {
                    continue;
                }

                if (_context.Friendships.FirstOrDefault(f => (((f.SenderUserId == user.Id && f.ReceiverUserId == currentUserId) || (f.SenderUserId == currentUserId && f.ReceiverUserId == user.Id)) && f.IsApproved == true)) != null)
                {
                    FriendList.Add(user);
                }
            }
        }




        public async Task<IActionResult> OnPostDeleteFriend(string deleteUserId)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var friendship = _context.Friendships.FirstOrDefault(f => (((f.SenderUserId == deleteUserId && f.ReceiverUserId == currentUserId) || (f.SenderUserId == currentUserId && f.ReceiverUserId == deleteUserId))));

            if (friendship == null)
            {
                return NotFound();
            }

            _context.Friendships.Remove(friendship);
            await _context.SaveChangesAsync();


            return RedirectToPage();

        }
    }
}
