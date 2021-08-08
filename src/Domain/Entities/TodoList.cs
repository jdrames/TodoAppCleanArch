using Domain.Common;
using Domain.Enums;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class TodoList : AuditableEntity, IHasDomainEvent
    {
        /// <summary>
        /// Identifier for the list.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Unique title for the list.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The priority level of the list.
        /// </summary>
        public PriorityLevel Priority { get; set; } = default(PriorityLevel);

        /// <summary>
        /// Items associated with the list.
        /// </summary>
        public IList<TodoListItem> Items { get; private set; } = new List<TodoListItem>();

        /// <summary>
        /// Events that have been triggered by this list.
        /// </summary>
        public List<DomainEvent> DomainEvents { get; set; } = new List<DomainEvent>();
    }
}
