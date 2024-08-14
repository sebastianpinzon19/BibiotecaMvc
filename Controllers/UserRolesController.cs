using BibliotecaWebApplicationMvc.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;


namespace BibliotecaWebApplicationMvc.Controllers
{
    public class UserRolesController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        // O referencias
        public UserRolesController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            this._userManager = userManager;
            this._roleManager = roleManager;
        }

        public async Task<IActionResult> index()
        {
            var users = _userManager.Users.ToList();
            var userRolesViewModel = new List<UserRolesModels>();
            foreach (var user in users)
            {
                var thisViewModel = new UserRolesModels();
                thisViewModel.UserId = user.Id;
                thisViewModel.Email = user.Email;
                thisViewModel.Roles = await GetUserRoles(user);
                userRolesViewModel.Add(thisViewModel);

            }
            return View(userRolesViewModel);
        }


        private async Task<List<string>> GetUserRoles(IdentityUser user)
        {
            return new List<string>(await _userManager.GetRolesAsync(user));
        }

        public async Task<IActionResult> Manage(string userId)
        {
            ViewBag.UserId = userId;
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"User witch Id = {userId} cannot be found";
                return View("NotFound");
            }
            ViewBag.UserName = user.UserName;

            var model = new List<ManageUseRolesViewModel>();

            foreach (var role in _roleManager.Roles.ToList())
            {
                var userRolesviewModel = new ManageUseRolesViewModel
                {
                    RoleId = role.Id,
                    RoleName = role.Name
                };
                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    userRolesviewModel.IsSelected = true;
                }
                else
                {
                    userRolesviewModel.IsSelected = false;
                }
                model.Add(userRolesviewModel);
            }

            return View(model);
        }
        // Post: userrolescontroller;Manager
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Manage(List<ManageUseRolesViewModel> model, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return View("NotFound");
            }

            var currentRoles = await _userManager.GetRolesAsync(user);

            var rolesToAdd = model.Where(x => x.IsSelected && !currentRoles.Contains(x.RoleName)).Select(x => x.RoleName);
            var rolesToRemove = currentRoles.Where(role => model.Any(x => x.RoleName == role && !x.IsSelected));

            var removeResult = await _userManager.RemoveFromRolesAsync(user, rolesToRemove);
            if (!removeResult.Succeeded)
            {
                ModelState.AddModelError("", "Cannot remove user existing roles");
                return View(model);
            }

            var addResult = await _userManager.AddToRolesAsync(user, rolesToAdd);
            if (!addResult.Succeeded)
            {
                ModelState.AddModelError("", "Cannot add selected roles to user");
                return View(model);
            }

            return RedirectToAction("Index");
        }


    }


}