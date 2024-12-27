using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sportify_back.Models;
using Sportify_Back.Services;

namespace Sportify_Back.Controllers
{
    public class TeachersController : Controller
    {
        private readonly ITeacherService _teacherService;

        public TeachersController(ITeacherService teacherService)
        {
            _teacherService = teacherService;
        }

        [Authorize(Policy = "AdministradorOnly")]
        public async Task<IActionResult> Index()
        {
            var teachers = await _teacherService.GetAllAsync();
            return View(teachers);
        }

        [Authorize(Policy = "AdministradorOnly")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var teacher = await _teacherService.GetByIdAsync(id.Value);
            if (teacher == null) return NotFound();

            return View(teacher);
        }

        [Authorize(Policy = "AdministradorOnly")]
        public async Task<IActionResult> Create()
        {
            // Asegúrate de obtener las actividades para poblar el SelectList
            var activities = await _teacherService.GetAllActivitiesAsync();
            ViewData["ActivitiesId"] = new SelectList(activities, "Id", "NameActivity");
            return View();
        }

        [Authorize(Policy = "AdministradorOnly")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Dni,Mail,Phone,Address,ActivitiesId,Active")] Teachers teacher)
        {
            if (ModelState.IsValid)
            {
                await _teacherService.CreateAsync(teacher);
                return RedirectToAction(nameof(Index));
            }

            // Reobtener las actividades si ocurre un error de validación
            var activities = await _teacherService.GetAllActivitiesAsync();
            ViewData["ActivitiesId"] = new SelectList(activities, "Id", "NameActivity", teacher.ActivitiesId);
            return View(teacher);
        }

        [Authorize(Policy = "AdministradorOnly")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var teacher = await _teacherService.GetByIdAsync(id.Value);
            if (teacher == null) return NotFound();

            var activities = await _teacherService.GetAllActivitiesAsync();
            ViewData["ActivitiesId"] = new SelectList(activities, "Id", "NameActivity", teacher.ActivitiesId);
            return View(teacher);
        }

        [Authorize(Policy = "AdministradorOnly")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Dni,Mail,Phone,Address,ActivitiesId,Active")] Teachers teacher)
        {
            if (id != teacher.Id) return NotFound();

            if (ModelState.IsValid)
            {
                await _teacherService.UpdateAsync(teacher);
                return RedirectToAction(nameof(Index));
            }

            var activities = await _teacherService.GetAllActivitiesAsync();
            ViewData["ActivitiesId"] = new SelectList(activities, "Id", "NameActivity", teacher.ActivitiesId);
            return View(teacher);
        }

        [Authorize(Policy = "AdministradorOnly")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var teacher = await _teacherService.GetByIdAsync(id.Value);
            if (teacher == null) return NotFound();

            return View(teacher);
        }

        [Authorize(Policy = "AdministradorOnly")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _teacherService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
