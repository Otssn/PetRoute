using PetRoute.API.Data;
using PetRoute.API.Data.Entities;
using PetRoute.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetRoute.API.Helpers
{
    public class ConverterHelper : IConverterHelper
    {
        private readonly DataContext _context;
        private readonly ICombosHelper _combosHelper;

        public ConverterHelper(DataContext context, ICombosHelper combosHelper)
        {
            _context = context;
            _combosHelper = combosHelper;
        }
        public async Task<Pet> ToPetAsync(PetViewModel model, bool isNew)
        {
            return new Pet
            {
                race = await _context.races.FindAsync(model.raceId),
                Id = isNew ? 0 : model.Id,
                name = model.name,
                petType = await _context.petTypes.FindAsync(model.petTypeId)
            };
        }

        public PetViewModel ToPetViewModel(Pet pet)
        {
            return new PetViewModel
            {
                raceId = pet.race.Id,
                races = _combosHelper.GetCombosRaces(),
                Id = pet.Id,
                UserId = pet.User.Id,
                photoPets = pet.photoPets,
                petTypeId = pet.petType.Id,
                PetTypes = _combosHelper.GetCombosPetTypes()
            };  
        }

        public async Task<User> ToUserAsync(UserViewModel model, Guid imageId, bool isNew)
        {
            return new User
            {
                Address = model.Address,
                Document = model.Document,
                DocumentType = await _context.documentTypes.FindAsync(model.DocumentTypeId),
                Email = model.Email,
                FirstName = model.FirstName,
                Id = isNew ? Guid.NewGuid().ToString() : model.Id,
                ImageId = imageId,
                LastName = model.LastName,
                PhoneNumber = model.PhoneNumber,
                UserName = model.Email,
                userType = model.UserType,
            };
        }

        public UserViewModel ToUserViewModel(User user)
        {
            return new UserViewModel
            {
                Address = user.Address,
                Document = user.Document,
                DocumentTypeId = user.DocumentType.Id,
                DocumentTypes = _combosHelper.GetCombosDocumentTypes(),
                Email = user.Email,
                FirstName = user.FirstName,
                Id = user.Id,
                ImageId = user.ImageId,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                UserType = user.userType,
            };
        }
    }
}
