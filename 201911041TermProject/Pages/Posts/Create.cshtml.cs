using _201911041TermProject.Data;
using _201911041TermProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace _201911041TermProject.Pages.Posts
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public CreateModel(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        [BindProperty]
        public InputModel Input { get; set; }

        public List<SelectListItem> CategorySelectList { get; set; }

        public class InputModel
        {
            [BindProperty]
            [Required]
            [Display(Name = "Title")]
            public string Title { get; set; }

            [BindProperty]
            [Required]
            [Display(Name = "Last Name")]
            public string Content { get; set; }


            [BindProperty]
            public IFormFile FileUpload { get; set; }


        }



        public async Task OnGet()
        {
            
        }



        public async Task<IActionResult> OnPostAsync()
        {

            Post newPost = new Post();
            Image newPostImg = new Image();



            using (var memoryStream = new MemoryStream())
            {
                // images klasörü
                string imgPath = @"wwwroot\images\post\";

                // image extension
                var imgExtension = Path.GetExtension(Input.FileUpload.FileName);

                // resim için guid oluştur
                string imgName = Guid.NewGuid().ToString();

                // resmi klasöre kaydet
                using (var fileStream = new FileStream(Path.Combine(imgPath, imgName + imgExtension), FileMode.Create))
                {
                    Input.FileUpload.CopyTo(fileStream);
                }


                await Input.FileUpload.CopyToAsync(memoryStream);
                newPostImg.File = memoryStream.ToArray();
                newPostImg.Name = imgName + imgExtension;

                await _context.Images.AddAsync(newPostImg);

            }


            newPost.Title = Input.Title;
            newPost.Content = Input.Content;
            newPost.Image = newPostImg;
            newPost.Date = DateTime.Now;
            newPost.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);


            await _context.Posts.AddAsync(newPost);
            await _context.SaveChangesAsync();
            return RedirectToPage("/Posts/Index");
        }
    }
}
