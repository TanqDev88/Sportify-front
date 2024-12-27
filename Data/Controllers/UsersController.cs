using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Sportify_Back.Models; 
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Security.Claims;
using Sportify_back.Models;
using Microsoft.EntityFrameworkCore;

namespace Sportify_Back.Controllers
{
    // Restringe el acceso a usuarios autenticados
    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SportifyDbContext _context;

        // Constructor único para la inyección de dependencias
        public UsersController(UserManager<ApplicationUser> userManager, SportifyDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }


        // Acción para mostrar la lista de usuarios
        [Authorize(Policy = "AdministradorOnly")]
        public async Task<IActionResult> Index()
        {
            var users = _userManager.Users
                .Select(user => new ApplicationUser
                {
                    Id = user.Id, 
                    Name = user.Name,
                    LastName = user.LastName,
                    Email = user.Email,
                    DocumentName = user.DocumentName,
                    DocumentContent = user.DocumentContent,
                    DNI = user.DNI
                })
                .ToList();

            return View(users);
        }

        // Acción para ver detalles de un usuario
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // Acción GET para editar un usuario
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // Acción POST para editar un usuario
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Users/Edit/{id}")]
        public async Task<IActionResult> Edit(ApplicationUser user)
        {

            ModelState.Remove(nameof(ApplicationUser.Plans));

            if (ModelState.IsValid)
            {
                
                Console.WriteLine($"Recibido: {user.Name}, {user.LastName}, {user.DNI}, {user.PlansId}");
                // Buscar el usuario existente por su ID
                var existingUser = await _userManager.FindByIdAsync(user.Id);

                if (existingUser != null)
                {
                    // Obtener el PlanId actual del usuario desde la base de datos
                    var planId = await _context.Users
                        .Where(u => u.Id == user.Id)
                        .Select(u => u.PlansId)
                        .FirstOrDefaultAsync();

                    // Opcional: Puedes usar el PlanId en alguna lógica o mostrarlo
                    if (planId != null)
                    {
                        // Asignar el PlanId al usuario si se requiere
                        existingUser.PlansId = planId;
                    }

                    // Actualizar los campos editables
                    existingUser.Name = user.Name;
                    existingUser.LastName = user.LastName;
                    existingUser.DNI = user.DNI;

                    // Procesar el documento médico si se cargó
                    if (user.Document != null && user.Document.Length > 0)
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            await user.Document.CopyToAsync(memoryStream);
                            existingUser.DocumentContent = memoryStream.ToArray(); // Guardar el contenido del archivo
                            existingUser.DocumentName = user.Document.FileName; // Guardar el nombre del archivo
                        }
                    }

                    // Actualizar el usuario con los cambios
                    var result = await _userManager.UpdateAsync(existingUser);

                    if (result.Succeeded)
                    {
                        var userRole = User.FindFirstValue("Profile");

                        if (userRole == "Administrador")
                        {
                            return RedirectToAction(nameof(Index));
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
                else
                {
                    return NotFound();
                }
            }

            return View(user);
        }


    }
}
