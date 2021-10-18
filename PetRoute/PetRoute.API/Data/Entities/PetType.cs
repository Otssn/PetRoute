using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PetRoute.API.Data.Entities
{
    public class PetType
    {
        public int Id { get; set; }

        [Display(Name = "Tipo de mascota")]
        [MaxLength(50, ErrorMessage = "El campo {0} no puede tener mas de {1} carácteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatiorio.")]
        public string Description { get; set; }
    }
}
