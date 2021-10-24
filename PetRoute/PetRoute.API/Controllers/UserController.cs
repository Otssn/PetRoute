using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetRoute.API.Data;
using PetRoute.API.Data.Entities;
using PetRoute.API.Helpers;
using PetRoute.API.Models;
using PetRoute.commons.Enums;
using PetRoute.commons.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetRoute.API.Controllers
{
    public class UserController : Controller
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;
        private readonly ICombosHelper _combosHelper;
        private readonly IConverterHelper _converterHelper;
        private readonly IBlobHelper _blobHelper;
        private readonly IMailHelper _mailHelper;

        public UserController(DataContext context, IUserHelper userHelper, ICombosHelper combosHelper, IConverterHelper converterHelper, IBlobHelper blobHelper, IMailHelper mailHelper)
        {
            _context = context;
            _userHelper = userHelper;
            _combosHelper = combosHelper;
            _converterHelper = converterHelper;
            _blobHelper = blobHelper;
            _mailHelper = mailHelper;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _context.Users
                .Include(x => x.DocumentType)
                .Include(x => x.pet)
                .Where(x => x.userType == commons.Enums.UserType.User)
                .ToListAsync());
        }
        public IActionResult Create()
        {
            UserViewModel model = new UserViewModel
            {
                DocumentTypes = _combosHelper.GetCombosDocumentTypes()
            };

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                Guid imageId = Guid.Empty;

                if (model.ImageFile != null)
                {
                    imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "users");
                }

                User user = await _converterHelper.ToUserAsync(model, imageId, true);
                user.userType = UserType.User;
                await _userHelper.AddUserAsync(user, "123456");
                await _userHelper.AddUserToRoleAsync(user, user.userType.ToString());

                string myToken = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
                string tokenLink = Url.Action("ConfirmEmail", "Account", new
                {
                    userid = user.Id,
                    token = myToken
                }, protocol: HttpContext.Request.Scheme);

                Responses response = _mailHelper.SendMail(model.Email, "Vehicles - Confirmación de cuenta", $"<h1>Vehicles - Confirmación de cuenta</h1>" +
                    $"Para habilitar el usuario, " +
                    $"por favor hacer clic en el siguiente enlace: </br></br><a href = \"{tokenLink}\">Confirmar Email</a>");

                return RedirectToAction(nameof(Index));
            }
            model.DocumentTypes = _combosHelper.GetCombosDocumentTypes();
            return View(model);
        }
        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            User user = await _userHelper.GetUserAsync(Guid.Parse(id));
            if (user == null)
            {
                return NotFound();
            }
            UserViewModel model = _converterHelper.ToUserViewModel(user);
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                Guid imageId = Guid.Empty;

                if (model.ImageFile != null)
                {
                    imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "users");
                }

                User user = await _converterHelper.ToUserAsync(model, imageId, true);
                await _userHelper.UpdateUserAsync(user);
                return RedirectToAction(nameof(Index));
            }
            model.DocumentTypes = _combosHelper.GetCombosDocumentTypes();
            return View(model);
        }
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            User user = await _userHelper.GetUserAsync(Guid.Parse(id));
            if (user == null)
            {
                return NotFound();
            }

            await _blobHelper.DeleteBlobAsync(user.ImageId, "users");
            await _userHelper.DeleteUserAsync(user);
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            User user = await _context.Users
                .Include(x => x.DocumentType)
                .Include(x => x.pet)
                .ThenInclude(x => x.race)
                .Include(x => x.pet)
                .ThenInclude(x => x.photoPets)
                .Include(x => x.pet)
                .ThenInclude(x => x.photoPets)
                .Include(x => x.pet)
                .ThenInclude(x => x.historyTrips)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }
        public async Task<IActionResult> AddPet(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            User user = await _context.Users
                .Include(x => x.pet)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            PetViewModel model = new PetViewModel
            {
                races = _combosHelper.GetCombosRaces(),
                UserId = user.Id,
                PetTypes = _combosHelper.GetCombosPetTypes()
            };

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPet(PetViewModel petViewModel)
        {
            User user = await _context.Users
                .Include(x => x.pet)
                .FirstOrDefaultAsync(x => x.Id == petViewModel.UserId);
            if (user == null)
            {
                return NotFound();
            }

            Guid imageId = Guid.Empty;
            if (petViewModel.ImageFile != null)
            {
                imageId = await _blobHelper.UploadBlobAsync(petViewModel.ImageFile, "vehicles");
            }

            Pet pet = await _converterHelper.ToPetAsync(petViewModel, true);
            if (pet.photoPets == null)
            {
                pet.photoPets = new List<PhotoPet>();
            }

            pet.photoPets.Add(new PhotoPet
            {
                ImageId = imageId
            });

            try
            {
                user.pet.Add(pet);
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Details), new { id = user.Id });
            }
            catch (DbUpdateException dbUpdateException)
            {
                if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                {
                    ModelState.AddModelError(string.Empty, "Ya existe una mascota con esa id.");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
                }
            }
            catch (Exception exception)
            {
                ModelState.AddModelError(string.Empty, exception.Message);
            }

            petViewModel.races = _combosHelper.GetCombosRaces();
            petViewModel.PetTypes = _combosHelper.GetCombosPetTypes();
            return View(petViewModel);
        }
        public async Task<IActionResult> EditPet(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Pet pet = await _context.pet
                .Include(x => x.User)
                .Include(x => x.race)
                .Include(x => x.petType)
                .Include(x => x.photoPets)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (pet == null)
            {
                return NotFound();
            }

            PetViewModel model = _converterHelper.ToPetViewModel(pet);
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPet(int id, PetViewModel petViewModel)
        {
            if (id != petViewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Pet vehicle = await _converterHelper.ToPetAsync(petViewModel, false);
                    _context.pet.Update(vehicle);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Details), new { id = petViewModel.UserId });
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, "Ya existe una mascota con esa Id.");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
                    }
                }
                catch (Exception exception)
                {
                    ModelState.AddModelError(string.Empty, exception.Message);
                }
            }

            petViewModel.races = _combosHelper.GetCombosRaces();
            petViewModel.PetTypes = _combosHelper.GetCombosPetTypes();
            return View(petViewModel);
        }

        public async Task<IActionResult> DeletePet(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Pet pet = await _context.pet
                .Include(x => x.User)
                .Include(x => x.photoPets)
                .Include(x => x.historyTrips)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (pet == null)
            {
                return NotFound();
            }

            _context.pet.Remove(pet);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Details), new { id = pet.User.Id });
        }
        public async Task<IActionResult> DeleteImagePet(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            PhotoPet petPhoto = await _context.photoPets
                .Include(x => x.pet)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (petPhoto == null)
            {
                return NotFound();
            }

            try
            {
                await _blobHelper.DeleteBlobAsync(petPhoto.ImageId, "petphotos");
            }
            catch { }

            _context.photoPets.Remove(petPhoto);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(EditPet), new { id = petPhoto.pet.Id });
        }
        public async Task<IActionResult> AddPetImage(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Pet pet = await _context.pet
                .FirstOrDefaultAsync(x => x.Id == id);
            if (pet == null)
            {
                return NotFound();
            }

            PetPhotoViewModel model = new()
            {
                PetId = pet.Id
            };

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPetImage(PetPhotoViewModel model)
        {
            if (ModelState.IsValid)
            {
                Guid imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "vehicles");
                Pet pet = await _context.pet
                    .Include(x => x.photoPets)
                    .FirstOrDefaultAsync(x => x.Id == model.PetId);
                if (pet.photoPets == null)
                {
                    pet.photoPets = new List<PhotoPet>();
                }

                pet.photoPets.Add(new PhotoPet
                {
                    ImageId = imageId
                });

                _context.pet.Update(pet);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(EditPet), new { id = pet.Id });
            }

            return View(model);
        }
    }
}
