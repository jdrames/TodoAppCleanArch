using Application.Common.Mappings;
using Domain.Entities;

namespace Application.Common.DTOs
{
    public class TodoListItemDto : IMapFrom<TodoListItem>
    {
        public string Id { get; set; }
        public string ListId { get; set; }
        public string Title { get; set; }
        public string Notes { get; set; }
        public bool Done { get; set; }
    }
}
