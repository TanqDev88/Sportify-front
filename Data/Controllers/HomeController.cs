using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sportify_back.Models;
using Sportify_Back.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Sportify_Back.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    private readonly SportifyDbContext _context;

    public HomeController(SportifyDbContext context, ILogger<HomeController> logger)
    {
        _context = context;
        _logger = logger;
    }

    private bool IsUserAuthenticated()
    {
        return User.Identity?.IsAuthenticated == true;
    }

    private string GetUserId()
    {
        return User.FindFirstValue(ClaimTypes.NameIdentifier);
    }

    public IActionResult Welcome()
    {
        if (IsUserAuthenticated()) 
        {
            return RedirectToAction("Index", "Home"); 
        }

        return View(); 
    }
    
    public IActionResult Index()
    {
        if (!IsUserAuthenticated()) 
        {
            return RedirectToAction("Welcome");
        }
        return View();
    }

    public IActionResult MisProximosTurnos()
    {
    if (!IsUserAuthenticated()) 
    {
        return RedirectToAction("Welcome");
    }
    return View(); 
    }

    public IActionResult HistorialTurnos()
    {
    if (!IsUserAuthenticated()) 
    {
        return RedirectToAction("Welcome");
    }
    return View(); 
    }

    public IActionResult TerminosCondiciones()
    {
        return View();
    }
    public IActionResult SobreNosotros()
    {
        return View();
    }

    public IActionResult Contacto()
    {
        return View();
    }


    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    [HttpGet]
    public JsonResult GetActivities()
    {

        var activities = _context.Classes
            .Where(a => a.Active)
            .Select(a => new {
                ClassId = a.Id,
                Title = a.Activities.NameActivity,
                Date = a.Sched.ToString("dd/MM/yyyy"),
                Day = a.Sched.DayOfWeek.ToString(),
                Time = a.Sched.ToString("HH:mm"),
                Teacher = a.Teachers.Name,
                Cupo = a.Quota
            })
            .ToList();

        return Json(activities);
    }
    [HttpGet]
    public JsonResult GetUserApto(){
        var userId = GetUserId();
        if (userId == null) return Json(new { success = false, message = "Usuario no autenticado." });

        var user = _context.Users
        .Where(p => p.Id == userId)
        .FirstOrDefault();

        bool tieneDocumento = user.DocumentContent != null && user.DocumentContent.Length > 0;

        if (tieneDocumento)
        {
            return Json(new { success = true, message = "Documento cargado correctamente." });
        }
        else
        {
            return Json(new { success = false, message = "No se ha cargado ningún documento." });
        }
        }

    [HttpGet]
    public JsonResult GetUserPayment(){
        var userId = GetUserId();
        if (userId == null) return Json(new { success = false, message = "Usuario no autenticado." });

        var ultimoPago = _context.Payments
        .Where(p => p.UsersId == userId)
        .OrderByDescending(p => p.Fecha)
        .FirstOrDefault();

        if (ultimoPago.Fecha.HasValue)
        {
            var fechaUltimoPago = ultimoPago.Fecha.Value;
            
            var fechaActual = DateTime.Now;
            bool pagoEnMesActual = fechaUltimoPago.Month == fechaActual.Month && fechaUltimoPago.Year == fechaActual.Year;

            if (pagoEnMesActual)
            {
                return Json(new { success = true, message = "Pago realizado este mes.", fechaPago = fechaUltimoPago });
            }
            else
            {
                int mesUltimoPago = fechaUltimoPago.Month;
                int anioUltimoPago = fechaUltimoPago.Year;

                var nombreMes = new DateTime(anioUltimoPago, mesUltimoPago, 1).ToString("MMMM yyyy");

                return Json(new { 
                    success = false, 
                    message = $"El último pago fue realizado en {nombreMes}. El pago no está al día." 
                });
            }
        }
        else
        {
            return Json(new { success = false, message = "No se encontró información sobre el pago." });
        }
    }

    [HttpGet]
    public JsonResult GetUserClasses()
    {
        try
        {
            var userId = GetUserId();
            if (userId == null) return Json(new { success = false, message = "Usuario no autenticado." });
        
            var classIds = _context.ProgrammingUsers
            .Where(pu => pu.UserId == userId)
            .Select(pu => pu.ClassId)
            .ToList();

            if (!classIds.Any())
            return Json(new { success = true, data = new List<object>() });

            var userClasses = _context.Classes
            .Where(c => classIds.Contains(c.Id) && c.Sched > DateTime.Now) 
            .Select(c => new
            {
                ClassId = c.Id,
                Title = c.Name,
                Date = c.Sched.ToString("dd/MM/yyyy"),
                Time = c.Sched.ToString("HH:mm"),
                Teacher = c.Teachers.Name, 
                QRCode = $"https://api.qrserver.com/v1/create-qr-code/?data=AccesoClase_{c.Id}&size=200x200"
            })
            .ToList();

            return Json(new { success = true, data = userClasses });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = "Ocurrió un error al obtener las clases.", error = ex.Message });
        }
    }

    [HttpGet]
    public JsonResult GetUserClassesHistory()
    {
        try
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Json(new { success = false, message = "Usuario no autenticado." });

            var classIds = _context.ProgrammingUsers
                .Where(pu => pu.UserId == userId)
                .Select(pu => pu.ClassId)
                .ToList();

            if (!classIds.Any())
                return Json(new { success = true, data = new List<object>() });

            var userClasses = _context.Classes
                .Where(c => classIds.Contains(c.Id) && c.Sched <= DateTime.Now) 
                .Select(c => new
                {
                    ClassId = c.Id,
                    Title = c.Name, 
                    Date = c.Sched.ToString("dd/MM/yyyy"),
                    Time = c.Sched.ToString("HH:mm"),
                    Teacher = c.Teachers.Name,
                })
                .ToList();

            return Json(new { success = true, data = userClasses });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = "Ocurrió un error al obtener el historial de clases.", error = ex.Message });
        }
    }

    [HttpPost]
    public async Task<IActionResult> Inscribirse(int classId)
    {
        try{
        var userRole = User.FindFirstValue("Profile");
        if (userRole == "Administrador")
        {
            return BadRequest(new { message = "Los administradores no pueden inscribirse en clases" });
        }
        Console.WriteLine("Enroll action reached with classId: " + classId);
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();

        if (classId <= 0) return BadRequest(new { message = "ID de clase inválido." });

        var selectedClass = await _context.Classes
            .Include(c => c.Activities) 
            .Include(c => c.Teachers)
            .FirstOrDefaultAsync(c => c.Id == classId && c.Active);

        if (selectedClass == null || selectedClass.Quota <= 0)
        {
            return BadRequest(new { message = "La clase no está disponible o no tiene cupos." });
        }

        var alreadyEnrolled = await _context.ProgrammingUsers
            .AnyAsync(pu => pu.UserId == userId && pu.ClassId == classId);

        if (alreadyEnrolled)
        {
            return BadRequest(new { message = "Ya estás inscrito en esta clase." });
        }

        var user = await _context.Users
                        .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
        {
            return BadRequest(new { message = "Usuario no encontrado." });
        }

        var userPlanType = user.PlansId;

          // Validar el límite de clases mensuales si el plan es de tipo 1
        if (userPlanType == 1){
        var targetMonth = selectedClass.Sched.Month;
        var targetYear = selectedClass.Sched.Year;

            var enrolledClassesCount = await _context.ProgrammingUsers
                .Include(pu => pu.Class)
                .Where(pu => pu.UserId == userId &&
                            pu.Class.Sched.Year == targetYear &&
                            pu.Class.Sched.Month == targetMonth)
                .CountAsync();

            if (enrolledClassesCount >= 5)
            {
                return BadRequest(new { message = "Has alcanzado el límite de 5 clases para este mes con tu plan actual." });
            }
        }

        // Inscribir al usuario
        _context.ProgrammingUsers.Add(new ProgrammingUsers
        {
            UserId = userId,
            ClassId = classId,
            InscriptionDate = DateTime.Now
        });
        

        selectedClass.Quota -= 1;
        await _context.SaveChangesAsync();

        return Json(new { success = true, message = "Inscripción realizada con éxito." });
        }
        catch (Exception ex)
    {
        return Json(new { 
        success = false, 
        message = "Ocurrió un error interno.", 
        error = ex.Message, 
        stackTrace = ex.StackTrace 
    });
    }
    }


    
    [HttpPost]
    public async Task<IActionResult> CancelarReserva(int classId)
    {
        try
        {
            // Verificar si el usuario está autenticado
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            // Buscar la inscripción en la clase para el usuario
            var programmingUser = await _context.ProgrammingUsers
                .FirstOrDefaultAsync(pu => pu.UserId == userId && pu.ClassId == classId);

            if (programmingUser == null)
            {
                return BadRequest(new { message = "No estás inscrito en esta clase." });
            }

            // Eliminar la inscripción del usuario en la clase
            _context.ProgrammingUsers.Remove(programmingUser);

            // Buscar la clase para actualizar el cupo
            var selectedClass = await _context.Classes
                .FirstOrDefaultAsync(c => c.Id == classId && c.Active);

            if (selectedClass == null)
            {
                return BadRequest(new { message = "Clase no encontrada o no disponible." });
            }

            // Aumentar el cupo de la clase
            selectedClass.Quota += 1;

            // Guardar los cambios en la base de datos
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Reserva cancelada y cupo actualizado con éxito." });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = "Ocurrió un error al cancelar la reserva.", error = ex.Message });
        }
    }


    [HttpGet]
    public JsonResult SearchClasses(string searchQuery, DateTime? fromDate, DateTime? toDate)
    {
        try
        {
            var query = _context.Classes
                .Where(c => c.Active);

            if (!string.IsNullOrEmpty(searchQuery))
            {
                query = query.Where(c => c.Activities.NameActivity.ToLower().Contains(searchQuery.ToLower()));
            }

            

            if (fromDate.HasValue)
            {
                query = query.Where(c => c.Sched.Date >= fromDate.Value.Date);  // Usando .Date para comparar solo la fecha
            }

            if (toDate.HasValue)
            {
                query = query.Where(c => c.Sched.Date <= toDate.Value.Date);  // Usando .Date para comparar solo la fecha
            }

            var activities = query
                .Select(c => new
                {
                    ClassId = c.Id,
                    Title = c.Activities.NameActivity,
                    Date = c.Sched.ToString("dd/MM/yyyy"),
                    Day = c.Sched.DayOfWeek.ToString(),
                    Time = c.Sched.ToString("HH:mm"),
                    Teacher = c.Teachers.Name,
                    Cupo = c.Quota
                })
                .ToList();

            return Json(activities);
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = "Ocurrió un error al realizar la búsqueda.", error = ex.Message });
        }
    }
}
