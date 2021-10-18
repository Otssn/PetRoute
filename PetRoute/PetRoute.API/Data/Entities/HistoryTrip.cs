using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PetRoute.API.Data.Entities
{
    public class HistoryTrip
    {
        public int Id { get; set; }

        [Display(Name = "Mascota")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [MaxLength(200, ErrorMessage = "El campo {0} no puede tener mas de {1} carácteres.")]
        public Pet pet { get; set; }

        public ICollection<Description> description { get; set; }
    }
}
