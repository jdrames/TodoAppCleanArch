using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Infrastructure.Persistence;
using Application.Common.DTOs;
using Application.Features.TodoLists.Queries.GetAll;
using Microsoft.AspNetCore.Authorization;

namespace WebUI.Pages.Todo
{
    [Authorize]
    public class IndexModel : BasePageModel<IndexModel>
    {        
        public IndexModel(Infrastructure.Persistence.ApplicationDbContext context)
        {
            
        }

        public IList<TodoListDto> TodoList { get;set; }

        public async Task OnGetAsync()
        {
            TodoList = await Mediator.Send(new GetTodoListQuery());
        }
    }
}
