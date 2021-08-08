using Microsoft.AspNetCore.Identity;

namespace Application.Common.Models
{
    public class ApplicationUser : IdentityUser
    {
        [PersonalData]
        public string FirstName { get; set; }

        [PersonalData]
        public string LastName { get; set; }

        [PersonalData]
        public string TimeZone { get; set; }
    }
}
