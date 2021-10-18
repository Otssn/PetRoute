using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PetRoute.API.Data.Entities
{
    public class DocumentType
    {
        public int Id { get; set; }

        [Display(Name = "Tipo de documento")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [MaxLength(20, ErrorMessage = "El campo {0} no puede tener mas de {1} carácteres.")]
        public String Description { get; set; }
    }
}
