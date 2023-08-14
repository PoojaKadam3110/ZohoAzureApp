using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectsAPI.Model
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(256)]
        [EmailAddress]
        public string UserName { get; set; }

        [Required]
        [MaxLength(64)] // Adjust the length based on your password hashing algorithm
        public string PasswordHash { get; set; }

        [Required]
        [MaxLength(128)] // Length of the salt used for hashing
        public string PasswordSalt { get; set; }

        // Add other user properties if needed
    }
}





