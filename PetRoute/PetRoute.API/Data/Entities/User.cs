using Microsoft.AspNetCore.Identity;
using PetRoute.commons.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PetRoute.API.Data.Entities
{
    public class User : IdentityUser
    {
        [Display(Name = "Nombres")]
        [MaxLength(50, ErrorMessage = "El campo {0} no puede tener mas de {1} carácteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatiorio.")]
        public string FirstName { get; set; }

        [Display(Name = "Apellidos")]
        [MaxLength(50, ErrorMessage = "El campo {0} no puede tener mas de {1} carácteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatiorio.")]
        public string LastName { get; set; }

        [Display(Name = "Tipo de documento")]
        [Required(ErrorMessage = "El campo {0} es obligatiorio.")]
        public DocumentType DocumentType { get; set; }

        [Display(Name = "Documento")]
        [MaxLength(50, ErrorMessage = "El campo {0} no puede tener mas de {1} carácteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatiorio.")]
        public string Document { get; set; }

        [Display(Name = "Dirección")]
        [MaxLength(50, ErrorMessage = "El campo {0} no puede tener mas de {1} carácteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatiorio.")]

        public string Address { get; set; }

        [Display(Name = "Foto")]
        public Guid ImageId { get; set; }

        //TODO: Fix the images path
        [Display(Name = "Foto")]
        public string ImageFullPath =>logerType == LogerType.Email ? ImageId == Guid.Empty
            ? $"https://petrouteapi20211202104619.azurewebsites.net/images/noimage.png"
            : $"https://petroute.blob.core.windows.net/users/{ImageId}"
            : string.IsNullOrEmpty(SocialImageURL)
            ? $"https://petrouteapi20211202104619.azurewebsites.net/images/noimage.png"
            : SocialImageURL;

        [Display(Name = "Tipo de usuario")]
        public UserType userType { get; set; }

        [Display(Name = "Tipo de login")]
        public LogerType logerType { get; set; }

        [Display(Name = "Foto")]
        public String SocialImageURL { get; set; }

        [Display(Name = "Usuario")]
        public string FullName => $"{FirstName} {LastName}";
        public ICollection<Pet> pet { get; set; }
        [Display(Name = "# mascotas")]
        public int PetCount => pet == null ? 0 : pet.Count;
    }
}
