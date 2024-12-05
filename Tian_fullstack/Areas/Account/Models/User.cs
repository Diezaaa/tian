using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Tian_fullstack.Areas.Account.Models
{
    public class User : IdentityUser
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string Surname { get; set; }
        [Required]
        public DateOnly BirthDay { get; set; }
    }
}
