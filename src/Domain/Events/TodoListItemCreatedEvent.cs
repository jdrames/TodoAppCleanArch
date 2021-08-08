using Domain.Common;
using Domain.Entities;

namespace Domain.Events
{
    public class TodoListItemCreatedEvent : DomainEvent
    {
        /// <summary>
        /// The list item that triggered the event.
        /// </summary>
        public TodoListItem Item { get; }

        public TodoListItemCreatedEvent(TodoListItem item)
        {
            Item = item;
        }
    }
}
