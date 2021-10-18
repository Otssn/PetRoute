using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PetRoute.API.Data.Entities
{
    public class Trip
    {
        public int Id { get; set; }
        [Display(Name = "Mascota")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public Pet pet { get; set; }

        [Display(Name = "Paseador")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public User user { get; set; }

        public ICollection<Description> description { get; set; }
    }
}
