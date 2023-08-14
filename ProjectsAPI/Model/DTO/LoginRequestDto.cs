using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ProjectsAPI.Model.DTO
{
    public class LoginRequestDto
    {
        [JsonPropertyName("UserName")]
        [Required]
        [DataType(DataType.EmailAddress)]
        public string UserName { get; set; }

        [JsonPropertyName("Password")]
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
