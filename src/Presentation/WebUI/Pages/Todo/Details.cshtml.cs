using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Infrastructure.Persistence;

namespace WebUI.Pages.Todo
{
    public class DetailsModel : PageModel
    {
        private readonly Infrastructure.Persistence.ApplicationDbContext _context;

        public DetailsModel(Infrastructure.Persistence.ApplicationDbContext context)
        {
            _context = context;
        }

        public TodoList TodoList { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            TodoList = await _context.TodoLists.FirstOrDefaultAsync(m => m.Id == id);

            if (TodoList == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
