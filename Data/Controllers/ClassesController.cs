using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sportify_back.Models;
using Sportify_Back.Services;

namespace Sportify_Back.Controllers
{
    public class ClassesController : Controller
    {
        private readonly IClassService _classService;

        public ClassesController(IClassService classService)
        {
            _classService = classService;
        }

        // GET: Classes
        public async Task<IActionResult> Index()
        {
            var classes = await _classService.GetAllAsync();
            return View(classes);
        }

        // GET: Classes/Details/5
        [Authorize(Policy = "AdministradorOnly")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var classes = await _classService.GetByIdAsync(id.Value);
            if (classes == null) return NotFound();

            return View(classes);
        }

        // GET: Classes/Create
        [Authorize(Policy = "AdministradorOnly")]
        public IActionResult Create()
        {
            // Populate dropdown lists
            ViewData["ActivityId"] = new SelectList(_classService.GetAllAsync().Result, "Id", "NameActivity");
            ViewData["TeachersId"] = new SelectList(_classService.GetAllAsync().Result, "Id", "Name");
            return View();
        }

        // POST: Classes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,ActivityId,Sched,TeachersId,Quota,Active")] Classes classes)
        {
            if (ModelState.IsValid)
            {
                await _classService.CreateAsync(classes);
                return RedirectToAction(nameof(Index));
            }

            ViewData["ActivityId"] = new SelectList(_classService.GetAllAsync().Result, "Id", "NameActivity", classes.ActivityId);
            ViewData["TeachersId"] = new SelectList(_classService.GetAllAsync().Result, "Id", "Name", classes.TeachersId);
            return View(classes);
        }

        // GET: Classes/Edit/5
        [Authorize(Policy = "AdministradorOnly")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var classes = await _classService.GetByIdAsync(id.Value);
            if (classes == null) return NotFound();

            ViewData["ActivityId"] = new SelectList(_classService.GetAllAsync().Result, "Id", "NameActivity", classes.ActivityId);
            ViewData["TeachersId"] = new SelectList(_classService.GetAllAsync().Result, "Id", "Name", classes.TeachersId);
            return View(classes);
        }

        // POST: Classes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,ActivityId,Sched,TeachersId,Quota,Active")] Classes classes)
        {
            if (id != classes.Id) return NotFound();

            if (ModelState.IsValid)
            {
                await _classService.UpdateAsync(classes);
                return RedirectToAction(nameof(Index));
            }

            ViewData["ActivityId"] = new SelectList(_classService.GetAllAsync().Result, "Id", "NameActivity", classes.ActivityId);
            ViewData["TeachersId"] = new SelectList(_classService.GetAllAsync().Result, "Id", "Name", classes.TeachersId);
            return View(classes);
        }

        // GET: Classes/Delete/5
        [Authorize(Policy = "AdministradorOnly")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var classes = await _classService.GetByIdAsync(id.Value);
            if (classes == null) return NotFound();

            return View(classes);
        }

        // POST: Classes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _classService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
