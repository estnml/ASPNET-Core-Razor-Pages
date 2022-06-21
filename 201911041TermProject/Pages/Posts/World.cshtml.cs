using _201911041TermProject.Data;
using _201911041TermProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;


namespace _201911041TermProject.Pages.Posts
{
    [Authorize]
    public class WorldModel : PageModel
    {

        private readonly ApplicationDbContext _context;

        public WorldModel(ApplicationDbContext context)
        {
            _context = context;
        }


        public List<Post> AllPosts { get; set; } = new List<Post>();
        public List<Friendship> Friendships { get; set; }
        public List<User> Users { get; set; }


        [BindProperty(SupportsGet = true)]
        public bool? SortByRating { get; set; }

        public async Task OnGet(bool? init)
        {

            if (init != null)
            {
                SortByRating = init;
            }


            Friendships = await _context.Friendships.AsNoTracking().ToListAsync();
            Users = await _context.Users.AsNoTracking().ToListAsync();




            if (SortByRating == null || SortByRating == false)
            {
                AllPosts = await _context.Posts
                .Include(p => p.User)
                .Include(p => p.Image)
                .AsNoTracking()
                .ToListAsync();
            }

            else
            {
                AllPosts = await _context.Posts
                .Include(p => p.User)
                .Include(p => p.Image)
                .AsNoTracking()
                .OrderByDescending(p => p.LikeCount - p.DislikeCount)
                .ToListAsync();
            }

        }


        public async Task<IActionResult> OnPostToggleSorting(bool currentMode)
        {
            if (currentMode == false)
            {
                SortByRating = true;
            }

            else
            {
                SortByRating = false;
            }

            return RedirectToPage(new { init = SortByRating });
        }
    }
}
