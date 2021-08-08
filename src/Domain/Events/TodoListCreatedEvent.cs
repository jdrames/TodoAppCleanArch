using Domain.Common;
using Domain.Entities;

namespace Domain.Events
{
    public class TodoListCreatedEvent : DomainEvent
    {
        /// <summary>
        /// The list that triggered the event.
        /// </summary>
        public TodoList List { get; }

        public TodoListCreatedEvent(TodoList list)
        {
            List = list;
        }
    }
}
