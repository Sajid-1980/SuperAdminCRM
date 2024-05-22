using SuperAdmin.Areas.Identity.Data;
using SuperAdmin.ViewModels.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace SuperAdmin.Controllers.Common
{
    public class ProfileController : Controller
    {
        private readonly UserManager<SuperAdminUser> _userManager;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public ProfileController(UserManager<SuperAdminUser> userManager, IWebHostEnvironment hostingEnvironment)
        {
            _userManager = userManager;
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpGet]
        public async Task<IActionResult> ChangePassword()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            var viewModel = new UserProfileViewModel
            {
                SuperAdminUserModel = user,
                // Initialize other properties of UserProfileViewModel if needed
            };

            return View(viewModel);
        }



        // YourController.cs
        [HttpPost]
        [Authorize] // Ensure only authenticated users can access this action
        public async Task<IActionResult> ChangePassword(UserProfileViewModel viewModel)
        {
            
                var user = await _userManager.GetUserAsync(User);

                if (user == null)
                {
                    return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
                }

                // Match the new password and confirm password
                if (viewModel.ChangePasswordModel.NewPassword != viewModel.ChangePasswordModel.ConfirmPassword)
                {
                    ModelState.AddModelError(string.Empty, "The new password and confirmation password do not match.");
                    return View(viewModel);
                }

                var changePasswordResult = await _userManager.ChangePasswordAsync(user, viewModel.ChangePasswordModel.OldPassword, viewModel.ChangePasswordModel.NewPassword);

                if (changePasswordResult.Succeeded)
                {
                    // Password successfully changed
                    return RedirectToAction("Index", "EmployeeDashboard", new { area = "Employee" });
                }
                else
                {
                    foreach (var error in changePasswordResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            

            // If we got this far, something failed, redisplay the form
            return View(viewModel);
        }


 

        [HttpPost]
        public async Task<IActionResult> EditProfile(UserProfileViewModel model)
        {
            
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return NotFound();
                }

                 user.FirstName = model.SuperAdminUserModel.FirstName;
                user.LastName = model.SuperAdminUserModel.LastName;
                user.Gender = model.SuperAdminUserModel.Gender;
                user.CompanyName = model.SuperAdminUserModel.CompanyName;
                user.JobTitle = model.SuperAdminUserModel.JobTitle;
                user.Address = model.SuperAdminUserModel.Address;
                user.City = model.SuperAdminUserModel.City;
                user.State = model.SuperAdminUserModel.State;
                user.ZipCode = model.SuperAdminUserModel.ZipCode;
                user.Country = model.SuperAdminUserModel.Country;
                user.Notes = model.SuperAdminUserModel.Notes;
                user.Department = model.SuperAdminUserModel.Department;
                user.TeamMemeberType = model.SuperAdminUserModel.TeamMemeberType;

                // Handle profile image upload
                if (model.ProfileImage != null)
                {
                    // Create the profiles folder if it doesn't exist
                    var profilesFolder = Path.Combine(_hostingEnvironment.WebRootPath, "profiles");
                    if (!Directory.Exists(profilesFolder))
                    {
                        Directory.CreateDirectory(profilesFolder);
                    }

                    // Save the uploaded image and update the user's ProfileImage property
                    var imagePath = "profiles/" + Guid.NewGuid() + Path.GetExtension(model.ProfileImage.FileName);
                    var imagePathOnDisk = Path.Combine(_hostingEnvironment.WebRootPath, imagePath);

                    using (var stream = new FileStream(imagePathOnDisk, FileMode.Create))
                    {
                        await model.ProfileImage.CopyToAsync(stream);
                    }

                    user.ProfileImage = imagePath;
                }

                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    // Redirect to a success page or the profile page
                    return RedirectToAction("Index", "EmployeeDashboard", new { area = "Employee" });
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
           

            // If we got this far, something failed, redisplay the form
            return View(model);
        }

    }
}

