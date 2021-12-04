using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PetRoute.API.Data.Entities
{
    public class PhotoPet
    {
        public int Id { get; set; }

        [JsonIgnore]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public Pet pet { get; set; }

        [Display(Name = "Foto")]
        public Guid ImageId { get; set; }

        //TODO: fix

        [Display(Name = "Foto")]
        public String ImageFullPath => ImageId == Guid.Empty
            ? $"https://petrouteapi20211202104619.azurewebsites.net/images/noimage.png"
            : $"https://petroute.blob.core.windows.net/pets/{ImageId}";
    }
}
