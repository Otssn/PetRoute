using Microsoft.AspNetCore.Mvc.Rendering;
using PetRoute.API.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetRoute.API.Helpers
{
    public class CombosHelper : ICombosHelper
    {
        private readonly DataContext _dataContext;

        public CombosHelper(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public IEnumerable<SelectListItem> GetCombosDocumentTypes()
        {
            List<SelectListItem> list = _dataContext.documentTypes.Select(x => new SelectListItem
            {
                Text = x.Description,
                Value = $"{x.Id}"
            })
               .OrderBy(x => x.Text)
               .ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "[Seleccione un tipo de documento...]",
                Value = "0"
            });

            return list;
        }

        public IEnumerable<SelectListItem> GetCombosPetTypes()
        {
            List<SelectListItem> list = _dataContext.petTypes.Select(x => new SelectListItem
            {
                Text = x.Description,
                Value = $"{x.Id}"
            })
               .OrderBy(x => x.Text)
               .ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "[Seleccione un tipo de mascota...]",
                Value = "0"
            });

            return list;
        }

        public IEnumerable<SelectListItem> GetCombosRaces()
        {
            List<SelectListItem> list = _dataContext.races.Select(x => new SelectListItem
            {
                Text = x.Description,
                Value = $"{x.Id}"
            })
               .OrderBy(x => x.Text)
               .ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "[Seleccione una raza...]",
                Value = "0"
            });

            return list;
        }
    }
}
