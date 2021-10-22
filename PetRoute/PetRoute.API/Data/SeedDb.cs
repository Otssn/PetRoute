using PetRoute.API.Data.Entities;
using PetRoute.API.Helpers;
using PetRoute.commons.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetRoute.API.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;

        public SeedDb(DataContext context, IUserHelper userHelper)
        {
            _context = context;
            _userHelper = userHelper;
        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();
            await CheckPetTypeAsync();
            await CheckRacesAsync();
            await CheckDocumentTypesAsync();
            await CheckRikesAsync();
            await CheckUserAsync("1010", "Camilo", "Torres", "camilo@yopmail.com", "3113123123", "calle 123", UserType.Admin);
            await CheckUserAsync("2020", "Esteban", "Pabon", "esteban@yopmail.com", "3113123123", "calle 123", UserType.User);
            await CheckUserAsync("3030", "Sara", "Davila", "saris@yopmail.com", "3113123123", "calle 123", UserType.walker);
        }

        private async Task CheckUserAsync(string document, string fisrtName, string lastName, string email, string phoneNumber, string addres, UserType userType)
        {
            User user = await _userHelper.GetUserAsync(email);
            if (user == null)
            {
                user = new User
                {
                    Address = addres,
                    Document = document,
                    DocumentType = _context.documentTypes.FirstOrDefault(x => x.Description == "Cédula"),
                    Email = email,
                    FirstName = fisrtName,
                    LastName = lastName,
                    PhoneNumber = phoneNumber,
                    UserName = email,
                    userType = userType
                };

                await _userHelper.AddUserAsync(user, "123456");
                await _userHelper.AddUserToRoleAsync(user, userType.ToString());

                string token = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
                await _userHelper.ConfirmEmailAsync(user, token);
            }
        }
        private async Task CheckRikesAsync()
        {
            await _userHelper.CheckRoleAsync(UserType.Admin.ToString());
            await _userHelper.CheckRoleAsync(UserType.User.ToString());
            await _userHelper.CheckRoleAsync(UserType.walker.ToString());
        }
        private async Task CheckDocumentTypesAsync()
        {
            if (!_context.documentTypes.Any())
            {
                _context.documentTypes.Add(new DocumentType { Description = "Cédula" });
                _context.documentTypes.Add(new DocumentType { Description = "Tarjeta de Identidad" });
                _context.documentTypes.Add(new DocumentType { Description = "NIT" });
                _context.documentTypes.Add(new DocumentType { Description = "Pasaporte" });
                await _context.SaveChangesAsync();
            }
        }
        private async Task CheckPetTypeAsync()
        {
            if (!_context.petTypes.Any())
            {
                _context.petTypes.Add(new PetType { Description = "Pequeño" });
                _context.petTypes.Add(new PetType { Description = "Mediano" });
                _context.petTypes.Add(new PetType { Description = "Grande" });
                await _context.SaveChangesAsync();
            }
        }
        private async Task CheckRacesAsync()
        {
            if (!_context.races.Any())
            {
                _context.races.Add(new Race { Description = "Alaskan malamute" });
                _context.races.Add(new Race { Description = "Perro affenpinscher" });
                _context.races.Add(new Race { Description = "Setter inglés" });
                _context.races.Add(new Race { Description = "Akita inu" });
                _context.races.Add(new Race { Description = "Dachshund" });
                _context.races.Add(new Race { Description = "Setter irlandés rojo" });
                _context.races.Add(new Race { Description = "Lebrel escocés" });
                _context.races.Add(new Race { Description = "Pastor del sur de Rusia" });
                _context.races.Add(new Race { Description = "Lakeland terrier" });
                _context.races.Add(new Race { Description = "Caniche" });
                _context.races.Add(new Race { Description = "Mastín inglés" });
                _context.races.Add(new Race { Description = "Labrador Retriever" });
                _context.races.Add(new Race { Description = "Pastor de Brie" });
                _context.races.Add(new Race { Description = "Mastín español" });
                _context.races.Add(new Race { Description = "Perro de Groenlandia" });
                _context.races.Add(new Race { Description = "Landseer" });
                _context.races.Add(new Race { Description = "Border collie" });
                _context.races.Add(new Race { Description = "Akita Americano" });
                _context.races.Add(new Race { Description = "Shih tzu" });
                _context.races.Add(new Race { Description = "Bichón Maltés" });
                _context.races.Add(new Race { Description = "Pit bull terrier americano" });
                _context.races.Add(new Race { Description = "Spaniel bretón" });
                _context.races.Add(new Race { Description = "Terrier americano sin pelo" });
                await _context.SaveChangesAsync();
            }
        }
    }
}
