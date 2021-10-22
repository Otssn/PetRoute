using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetRoute.API.Helpers
{
    public interface ICombosHelper
    {
        IEnumerable<SelectListItem> GetCombosDocumentTypes();
        IEnumerable<SelectListItem> GetCombosRaces();
        IEnumerable<SelectListItem> GetCombosPetTypes();
    }
}
