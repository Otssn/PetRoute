using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PetRoute.API.Data.Entities
{
    public class Description
    {
        public int Id { get; set; }

        [Display(Name = "Descripción")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [MaxLength(200, ErrorMessage = "El campo {0} no puede tener mas de {1} carácteres.")]
        public String Descriptions { get; set; }

        [Display(Name = "Viaje")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [MaxLength(200, ErrorMessage = "El campo {0} no puede tener mas de {1} carácteres.")]
        public Trip trip { get; set; }

        [Display(Name = "Historia")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [MaxLength(200, ErrorMessage = "El campo {0} no puede tener mas de {1} carácteres.")]
        public HistoryTrip historyTrip { get; set; }

    }
}
