using _201911041TermProject.Data;
using _201911041TermProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace _201911041TermProject.Pages.Users
{
    public class ProfileModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public ProfileModel(ApplicationDbContext context)
        {
            _context = context;
        }


        public User? User { get; set; }

        public async Task OnGet(string userId)
        {
            User = await _context.Users
                .Include(u => u.Posts)
                .Include(u => u.Posts).ThenInclude(p => p.Image)
                .Include(u => u.Image)
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == userId);
        }
    }
}
