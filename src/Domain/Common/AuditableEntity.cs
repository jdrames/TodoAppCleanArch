using System;

namespace Domain.Common
{
    public abstract class AuditableEntity
    {
        /// <summary>
        /// The date and time the entity was created.
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        /// The User Id that created the entity.
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// The date and time the entity was last modified.
        /// </summary>
        public DateTime? Modified { get; set; }

        /// <summary>
        /// The User Id that last modified the entity.
        /// </summary>
        public string ModifiedBy { get; set; }

        /// <summary>
        /// Indicates of the entity has been modified.
        /// </summary>
        public bool IsModified => Modified != null ? true : false;
    }
}
