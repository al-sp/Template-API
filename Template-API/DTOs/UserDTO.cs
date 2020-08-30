using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Template_API.DTOs
{
    public class UserDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [StringLength(30, ErrorMessage = "Your password is limited to {2} to {1} characters",  MinimumLength = 6)]
        public string Password { get; set; }
    }
}
