using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PetRoute.API.Data.Entities
{
    public class Race
    {
        public int Id { get; set; }

        [Display(Name = "Raza")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [MaxLength(50, ErrorMessage = "El campo {0} no puede tener mas de {1} carácteres.")]
        public String Description { get; set; }
    }
}
