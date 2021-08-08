using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Infrastructure.Persistence;
using Application.Features.TodoLists.Queries.GetById;
using Application.Common.DTOs;
using Application.Features.TodoLists.Commands.Delete;

namespace WebUI.Pages.Todo
{
    public class DeleteModel : BasePageModel<DeleteModel>
    {        

        public DeleteModel(Infrastructure.Persistence.ApplicationDbContext context)
        {
            
        }

        [BindProperty]
        public TodoListDto TodoList { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            TodoList = await Mediator.Send(new GetTodoListByIdQuery() { Id = id});

            if (TodoList == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            TodoList = await Mediator.Send(new GetTodoListByIdQuery() { Id = id });

            if (TodoList != null)
            {
                await Mediator.Send(new DeleteTodoListCommand() { Id = id});
            }

            return RedirectToPage("./Index");
        }
    }
}
