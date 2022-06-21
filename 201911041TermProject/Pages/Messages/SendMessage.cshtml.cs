using _201911041TermProject.Data;
using _201911041TermProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace _201911041TermProject.Pages.Messages
{
    [Authorize]
    public class SendMessageModel : PageModel
    {

        private readonly ApplicationDbContext _context;

        public SendMessageModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public List<SelectListItem> Users { get; set; }

        [BindProperty]
        public string CurrentUserId { get; set; }

        [BindProperty]
        public string Receiver { get; set; }


        public async Task OnGet()
        {
            CurrentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            Users = await _context.Users.AsNoTracking().Where(u=> u.Id != CurrentUserId).Select(u => new SelectListItem()
            {
                Text = u.FirstName + " " + u.LastName,
                Value = u.Id
            }).ToListAsync();

            
        }
    }
}
