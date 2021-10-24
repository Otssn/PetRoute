using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using PetRoute.API.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PetRoute.API.Models
{
    public class PetViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Nombre de la mascota")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [MaxLength(100, ErrorMessage = "El campo {0} no puede tener mas de {1} carácteres.")]
        public String name { get; set; }

        [Display(Name = "Raza de la mascota")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public int raceId { get; set; }
        public IEnumerable<SelectListItem> races { get; set; }

        [Display(Name = "Tipo de mascota")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public int petTypeId { get; set; }
        public IEnumerable<SelectListItem> PetTypes { get; set; }

        public ICollection<ScheduledTrip> scheduledTrips { get; set; }

        public ICollection<HistoryTrip> historyTrips { get; set; }

        public ICollection<PhotoPet> photoPets { get; set; }

        [Display(Name = "Foto")]
        public IFormFile ImageFile { get; set; }

        [Display(Name = "Descripción de la mascota")]
        [MaxLength(100, ErrorMessage = "El campo {0} no puede tener mas de {1} carácteres.")]
        public string description { get; set; }

        public String UserId { get; set; }
    }
}
