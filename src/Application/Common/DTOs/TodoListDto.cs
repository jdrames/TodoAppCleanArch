using Application.Common.Mappings;
using Domain.Entities;
using Domain.Enums;

namespace Application.Common.DTOs
{
    public class TodoListDto : IMapFrom<TodoList>
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public PriorityLevel Priority { get; set; }
    }
}
