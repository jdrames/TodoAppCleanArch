using Domain.Common;
using Domain.Entities;

namespace Domain.Events
{
    public class TodoListItemCompletedEvent : DomainEvent
    {
        /// <summary>
        /// The todo list item that triggered the event.
        /// </summary>
        public TodoListItem Item { get; }

        public TodoListItemCompletedEvent(TodoListItem item)
        {
            Item = item;
        }
    }
}
