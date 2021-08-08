using Domain.Common;
using Domain.Entities;

namespace Domain.Events
{
    public class TodoListCompletedEvent : DomainEvent
    {
        /// <summary>
        /// The todo list that triggered the event.
        /// </summary>
        public TodoList List { get; }

        public TodoListCompletedEvent(TodoList list)
        {
            List = list;
        }
    }
}
