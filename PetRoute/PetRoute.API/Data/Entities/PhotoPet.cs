using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PetRoute.API.Data.Entities
{
    public class PhotoPet
    {
        public int Id { get; set; }

        [Display(Name = "Foto")]
        public Guid ImageId { get; set; }

        //TODO: fix

        [Display(Name = "Foto")]
        public String ImageFullPath => ImageId == Guid.Empty
            ? $"https://localhost:44355/images/noimage.png"
            : $"https://vehiclesotssn.blob.core.windows.net/vehicles/{ImageId}";
    }
}
