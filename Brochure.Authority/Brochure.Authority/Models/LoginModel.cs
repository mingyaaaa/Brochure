using System;
using System.ComponentModel.DataAnnotations;

namespace Brochure.Authority.Models
{
    public class LoginModel
    {
        [Required]
        public string UseName { get; set; }

        [Required]
        public string Passward { get; set; }
    }
}