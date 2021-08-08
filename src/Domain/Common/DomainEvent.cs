using System;
using System.Collections.Generic;

namespace Domain.Common
{
    public interface IHasDomainEvent
    {
        List<DomainEvent> DomainEvents { get; set; }
    }

    public abstract class DomainEvent
    {
        /// <summary>
        /// The date and time the event was triggered.
        /// </summary>
        public DateTime OccurredAt { get; protected set; } = DateTime.UtcNow;

        /// <summary>
        /// Indicates if the event has been handled/published.
        /// </summary>
        public bool IsPublished { get; set; }

        public DomainEvent()
        {
            OccurredAt = DateTime.UtcNow;
        }
    }
}
