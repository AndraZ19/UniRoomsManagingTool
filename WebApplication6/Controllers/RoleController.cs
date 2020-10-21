using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using WebApplication6.Models;

namespace WebApplication6.Controllers
{
    public class RoleController : Controller
    {
        private ApplicationRoleManager _roleManager;

        public RoleController()
        {
        }

        public RoleController(ApplicationRoleManager roleManager)
        {
            RoleManager = roleManager;

        }

        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }
        // GET: Role
        [Authorize(Roles = "Administrator")]

        public ActionResult Index()
        {
            List<RoleViewModel> list = new List<RoleViewModel>();
            foreach (var role in RoleManager.Roles)
            {
                list.Add(new RoleViewModel(role));
            }
            return View(list);
        }

        public ActionResult Create()
        {
            return View();
        }
        [Authorize(Roles = "Administrator")]

        [HttpPost]
        public async Task<ActionResult> Create(RoleViewModel model)
        {
            var role = new ApplicationRole() { Name = model.Name };
            await RoleManager.CreateAsync(role);
            return RedirectToAction("Index");
        }
        [Authorize(Roles = "Administrator")]

        public async Task<ActionResult> Edit(string Id)
        {
            var role = await RoleManager.FindByIdAsync(Id);
            return View(new RoleViewModel(role));
        }
        [HttpPost]
        public async Task<ActionResult>Edit(RoleViewModel model)
        {
            var role = new ApplicationRole() { Id = model.Id,Name = model.Name };
            await RoleManager.UpdateAsync(role);
            return RedirectToAction("Index");
        }
        [Authorize(Roles = "Administrator")]

        public async Task<ActionResult> Details(string Id)
        {
            var role = await RoleManager.FindByIdAsync(Id);
            return View(new RoleViewModel(role));
        }
        [Authorize(Roles = "Administrator")]

        [HttpDelete]
        public async Task<ActionResult> Delete(string Id)
        {
            var role = await RoleManager.FindByIdAsync(Id);
            return View(new RoleViewModel(role));
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string Id)
        {
            var role = await RoleManager.FindByIdAsync(Id);
            await RoleManager.DeleteAsync(role);
            return RedirectToAction("Index");
        }

    }
}