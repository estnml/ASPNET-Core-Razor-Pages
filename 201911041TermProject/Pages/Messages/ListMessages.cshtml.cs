using _201911041TermProject.Data;
using _201911041TermProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace _201911041TermProject.Pages.Messages
{
    public class ListMessagesModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public ListMessagesModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Message> MessagesReceived { get; set; }
        public List<User> Users { get; set; }


        public async Task OnGet()
        {

            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var currentUser = await _context.Users.FindAsync(currentUserId);


            MessagesReceived = await _context.Messages.Where(m => m.ReceiverId == currentUserId).ToListAsync();
            Users = await _context.Users.AsNoTracking().ToListAsync();
        }

        public async Task<IActionResult> OnPostDeleteMessage(int messageId)
        {
            var msgToDelete = await _context.Messages.FindAsync(messageId);

            if (msgToDelete == null)
            {
                return NotFound();
            }


            _context.Messages.Remove(msgToDelete);
            await _context.SaveChangesAsync();

            return RedirectToPage();
        }

    }
}
