using Domain.Common;
using Domain.Events;
using System.Collections.Generic;

namespace Domain.Entities
{

    public class TodoListItem : AuditableEntity, IHasDomainEvent
    {
        /// <summary>
        /// Identifier for the list item.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The list the item belongs to.
        /// </summary>
        public TodoList List { get; set; }

        /// <summary>
        /// The identifier for the list the item belongs to.
        /// </summary>
        public string ListId { get; set; }

        /// <summary>
        /// Unique title for the todo item.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Any additional notes for the item.
        /// </summary>
        public string Notes { get; set; }

        private bool _done { get; set; }

        /// <summary>
        /// Indicates if the item has been completed.
        /// </summary>
        public bool Done
        {
            get => _done;
            set
            {
                if (value == true && !_done)
                    DomainEvents.Add(new TodoListItemCompletedEvent(this));

                _done = value;
            }
        }

        public List<DomainEvent> DomainEvents { get; set; } = new List<DomainEvent>();
    }
}
