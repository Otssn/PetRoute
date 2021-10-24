using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PetRoute.API.Data.Entities
{
    public class Pet
    {
        public int Id { get; set; }

        [Display(Name = "Nombre de la mascota")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [MaxLength(50, ErrorMessage = "El campo {0} no puede tener mas de {1} carácteres.")]
        public String name { get; set; }

        [Display(Name = "Raza de la mascota")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [MaxLength(50, ErrorMessage = "El campo {0} no puede tener mas de {1} carácteres.")]
        public Race race { get; set; }

        [Display(Name = "Tipo de mascota")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [MaxLength(50, ErrorMessage = "El campo {0} no puede tener mas de {1} carácteres.")]
        public PetType petType { get; set; }

        [Display(Name = "Propietario")]
        [JsonIgnore]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public User User { get; set; }

        public ICollection<ScheduledTrip> scheduledTrips { get; set; }

        public ICollection<HistoryTrip> historyTrips { get; set; }

        public ICollection<PhotoPet> photoPets { get; set; }

        [Display(Name = "# Fotos")]
        public int PetPhotosCount => photoPets == null ? 0 : photoPets.Count;

        [Display(Name = "Foto")]
        public string ImageFullPath => photoPets == null || photoPets.Count == 0
            ? $"https://localhost:44355/images/noimage.png"
            : photoPets.FirstOrDefault().ImageFullPath;

        [Display(Name = "Descripción de la mascota")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [MaxLength(50, ErrorMessage = "El campo {0} no puede tener mas de {1} carácteres.")]
        public string description { get; set; }
    }

}
