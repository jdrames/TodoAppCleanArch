using System;

namespace Application.Common.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class RequiresAttribute : Attribute
    {
        public RequiresAttribute() { }

        /// <summary>
        /// A comma seperated list of roles that are allowed to access the resource.
        /// </summary>
        public string Roles { get; set; }

        /// <summary>
        /// The policy name that a user must have to access the resource.
        /// </summary>
        public string Policy { get; set; }
    }
}
