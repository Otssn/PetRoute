using PetRoute.commons.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PetRoute.API.Models.Request
{
    public class SocialLoginRequest
    {
        [EmailAddress]
        [Required]
        public string Email { get; set; }

        [Required]
        public string Id { get; set; }

        [Required]
        public LogerType LogerType { get; set; }

        [Required]
        public string FullName { get; set; }

        public string PhotoURL { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
