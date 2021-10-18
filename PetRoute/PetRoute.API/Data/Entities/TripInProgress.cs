using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PetRoute.API.Data.Entities
{
    public class TripInProgress
    {
        public int Id { get; set; }

        [Display(Name = "Ubicación")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [MaxLength(200, ErrorMessage = "El campo {0} no puede tener mas de {1} carácteres.")]
        public String ubication { get; set; }

        public ICollection<Pet> pet { get; set; }
    }
}
