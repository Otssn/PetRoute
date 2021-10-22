using PetRoute.API.Data.Entities;
using PetRoute.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetRoute.API.Helpers
{
    public interface IConverterHelper
    {
        Task<User> ToUserAsync(UserViewModel model, Guid imageId, bool isNew);

        UserViewModel ToUserViewModel(User user);
        Task<Pet> ToPetAsync(PetViewModel model, bool isNew);

        PetViewModel ToPetViewModel(Pet user);
    }
}
