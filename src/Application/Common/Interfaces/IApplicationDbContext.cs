using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<TodoList> TodoLists { get; set; }

        DbSet<TodoListItem> TodoListItems { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
