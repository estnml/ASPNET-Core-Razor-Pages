// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using _201911041TermProject.Data;
using _201911041TermProject.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace _201911041TermProject.Areas.Identity.Pages.Account.Manage
{
    public class HideProfileModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public HideProfileModel(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task OnGetAsync()
        {
            var CurrentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            CurrentUser = await _context.Users.FindAsync(CurrentUserId);
        }


        public User CurrentUser { get; set; }

        public async Task<IActionResult> OnPostToggleProfileVisibility()
        {

            var CurrentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            CurrentUser = await _context.Users.FindAsync(CurrentUserId);

            if (CurrentUser == null)
            {
                return NotFound();
            }

            CurrentUser.IsProfileHidden = CurrentUser.IsProfileHidden == true ? false : true;
            await _context.SaveChangesAsync();

            return RedirectToPage();
        }
    }
}
