using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Domain.Entities;
using Infrastructure.Persistence;
using Application.Features.TodoLists.Commands.Create;

namespace WebUI.Pages.Todo
{
    public class CreateModel : BasePageModel<CreateModel>
    {
        public CreateModel(Infrastructure.Persistence.ApplicationDbContext context)
        {
            
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public CreateTodoListCommand TodoList { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var result = await Mediator.Send(TodoList);

            return RedirectToPage("./Index");
        }
    }
}
