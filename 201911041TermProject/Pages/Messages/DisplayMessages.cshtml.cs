using _201911041TermProject.Data;
using _201911041TermProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace _201911041TermProject.Pages.Messages
{
    public class DisplayMessagesModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DisplayMessagesModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public Message Message { get; set; }
        public List<User> Users { get; set; }

        public async Task OnGet(int messageId)
        {

            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Users = await _context.Users.AsNoTracking().ToListAsync();
            Message = await _context.Messages.AsNoTracking().FirstOrDefaultAsync(m => m.Id == messageId);

        }

    }
}
