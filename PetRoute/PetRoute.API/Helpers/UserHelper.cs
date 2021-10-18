using Microsoft.AspNetCore.Identity;
using PetRoute.API.Data;
using PetRoute.API.Data.Entities;
using PetRoute.API.Models;
using PetRoute.commons.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetRoute.API.Helpers
{
    public class UserHelper : IUserHelper
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly DataContext _dataContext;
        private readonly SignInManager<User> _signInManager;

        public UserHelper(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, DataContext dataContext, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _dataContext = dataContext;
            _signInManager = signInManager;
        }
        public async Task<IdentityResult> AddUserAsync(User user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }

        public async Task<User> AddUserAsync(AddUserViewModel model, Guid imageId, UserType userType)
        {
            User user = new()
            {
                Addres = model.Address,
                Document = model.Document,
                Email = model.Username,
                FirstName = model.FirstName,
                LastName = model.LastName,
                ImageId = imageId,
                PhoneNumber = model.PhoneNumber,
                DocumentType = await _dataContext.documentTypes.FindAsync(model.DocumentTypeId),
                UserName = model.Username,
                userType = userType
            };

            IdentityResult result = await _userManager.CreateAsync(user, model.Password);
            if (result != IdentityResult.Success)
            {
                return null;
            }

            User newUser = await GetUserAsync(model.Username);
            await AddUserToRoleAsync(newUser, user.userType.ToString());
            return newUser;
        }

        public async Task AddUserToRoleAsync(User user, string roleName)
        {
            await _userManager.AddToRoleAsync(user, roleName);
        }

        public async Task<IdentityResult> ChangePasswordAsync(User user, string oldPassword, string newPassword)
        {
            return await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
        }

        public async Task CheckRoleAsync(string roleName)
        {
            bool roleExists = await _roleManager.RoleExistsAsync(roleName);
            if (!roleExists)
            {
                await _roleManager.CreateAsync(new IdentityRole { Name = roleName });
            }
        }

        public async Task<IdentityResult> ConfirmEmailAsync(User user, string token)
        {
            return await _userManager.ConfirmEmailAsync(user, token);
        }

        public async Task<IdentityResult> DeleteUserAsync(User user)
        {
            return await _userManager.DeleteAsync(user);
        }

        public async Task<string> GenerateEmailConfirmationTokenAsync(User user)
        {
            return await _userManager.GenerateEmailConfirmationTokenAsync(user);
        }

        public async Task<User> GetUserAsync(string email)
        {
            return await _dataContext.Users
                .Include(x => x.DocumentType)
                .FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task<User> GetUserAsync(Guid id)
        {
            return await _dataContext.Users
                .Include(x => x.DocumentType)
                .Include(x => x.Vehicles)
                .ThenInclude(x => x.VehiclePhotos)
                .Include(x => x.Vehicles)
                .ThenInclude(x => x.Histories)
                .ThenInclude(x => x.Details)
                .FirstOrDefaultAsync(x => x.Id == id.ToString());
        }

        public async Task<bool> IsUserInRoleAsync(User user, string roleName)
        {
            return await _userManager.IsInRoleAsync(user, roleName);
        }

        public async Task<SignInResult> LoginAsync(LoginViewModel model)
        {
            return await _signInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberMe, false);
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<IdentityResult> UpdateUserAsync(User user)
        {
            User currentUser = await GetUserAsync(user.Email);
            currentUser.LastName = user.LastName;
            currentUser.FirstName = user.FirstName;
            currentUser.DocumentType = user.DocumentType;
            currentUser.Document = user.Document;
            currentUser.Addres = user.Addres;
            currentUser.ImageId = user.ImageId;
            currentUser.PhoneNumber = user.PhoneNumber;
            return await _userManager.UpdateAsync(currentUser);
        }
        public async Task<string> GeneratePasswordResetTokenAsync(User user)
        {
            return await _userManager.GeneratePasswordResetTokenAsync(user);
        }
        public async Task<IdentityResult> ResetPasswordAsync(User user, string token, string password)
        {
            return await _userManager.ResetPasswordAsync(user, token, password);
        }

        public async Task<SignInResult> ValidatePasswordAsync(User user, string password)
        {
            return await _signInManager.CheckPasswordSignInAsync(user, password, false);
        }

        Task<User> IUserHelper.GetUserAsync(string email)
        {
            throw new NotImplementedException();
        }

        Task<User> IUserHelper.GetUserAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityResult> AddUserAsync(User user, string password)
        {
            throw new NotImplementedException();
        }

        public Task<User> AddUserAsync(AddUserViewModel model, Guid imageId, UserType userType)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityResult> UpdateUserAsync(User user)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityResult> DeleteUserAsync(User user)
        {
            throw new NotImplementedException();
        }

        public Task AddUserToRoleAsync(User user, string roleName)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsUserInRoleAsync(User user, string roleName)
        {
            throw new NotImplementedException();
        }

        public Task<SignInResult> LoginAsync(LoginViewModel model)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityResult> ChangePasswordAsync(User user, string oldPassword, string newPassword)
        {
            throw new NotImplementedException();
        }

        public Task<string> GenerateEmailConfirmationTokenAsync(User user)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityResult> ConfirmEmailAsync(User user, string token)
        {
            throw new NotImplementedException();
        }

        public Task<string> GeneratePasswordResetTokenAsync(User user)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityResult> ResetPasswordAsync(User user, string token, string password)
        {
            throw new NotImplementedException();
        }

        public Task<SignInResult> ValidatePasswordAsync(User user, string password)
        {
            throw new NotImplementedException();
        }
    }
}
