// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using _201911041TermProject.Data;
using _201911041TermProject.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;

namespace _201911041TermProject.Areas.Identity.Pages.Account.Manage
{
    public class ImageModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ApplicationDbContext _context;

        public ImageModel(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }


        [BindProperty]
        public IFormFile? FileUpload { get; set; }

        public byte[] Image { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [EmailAddress]
            [Display(Name = "New email")]
            public string NewEmail { get; set; }
        }

        private async Task LoadAsync(User user)
        {
            // 


            // daha sonra resim ismini tutabilecek bir variable ekle.
            var userImg = _context.Images.FirstOrDefault(i => i.Id == user.ImageId);
            string imgPath = $@"wwwroot\images\user\{userImg.Name}";

            //string path = "wwwroot/images/pexels-alina-vilchenko-7015865.jpg";
            var memoryStream = new MemoryStream();
            using (var stream = System.IO.File.OpenRead(imgPath))
            {
                await new FormFile(stream, 0, stream.Length, null, Path.GetFileName(stream.Name)).CopyToAsync(memoryStream);
                Image = memoryStream.ToArray();
            };


        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            var userImg = _context.Images.FirstOrDefault(i => i.Id == user.ImageId);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            if (userImg.File != Image)
            {
                var memoryStream = new MemoryStream();
                await FileUpload.CopyToAsync(memoryStream);
                userImg.File = memoryStream.ToArray();

                _context.Users.Update(user);
                await _context.SaveChangesAsync();
                StatusMessage = "Your profile photo is changed.";

                return RedirectToPage();
            }

            return RedirectToPage();
        }


    }
}
