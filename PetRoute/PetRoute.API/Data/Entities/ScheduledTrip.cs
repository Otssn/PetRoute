using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PetRoute.API.Data.Entities
{
    public class ScheduledTrip
    {
        public int Id { get; set; }

        [Display(Name = "Mascota")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public Pet pet { get; set; }

        [Display(Name = "Fecha de la cita")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public DateTime DateScheduled { get; set; }
    }

}
