using Sportify_back.Models;
using Sportify_Back.Models;
using Sportify_Back.Services;
using System.Linq;

public class EntityReportService : IEntityReportService
{
    private readonly SportifyDbContext _context;

    public EntityReportService(SportifyDbContext context)
    {
        _context = context;
    }

    public IEnumerable<MonthlyClassReport> GenerateClassesReport()
    {
        throw new NotImplementedException();
    }

    public IEnumerable<MonthlyClassReport> GenerateClassReport()
    {
        var report = _context.Classes
            .Where(c => c.Active) // Solo considera clases activas (si aplica)
            .GroupBy(c => c.Sched.Month) // Agrupa por mes
            .Select(group => new
            {
                Month = group.Key,
                TotalClasses = group.Count(), // Total de clases en el mes
                TotalQuota = group.Sum(c => c.Quota), // Suma de cupos
                AverageQuota = group.Average(c => c.Quota), // Promedio de cupos
                TopTeacherId = group.GroupBy(c => c.TeachersId) // Agrupa por profesor
                                    .OrderByDescending(g => g.Count()) // Ordena por clases impartidas
                                    .Select(g => g.Key) // Obtén el ID del profesor
                                    .FirstOrDefault() // Profesor con más clases
            })
            .ToList();

        // Enlazar con los nombres de los profesores
        var teacherNames = _context.Teachers
            .Where(t => report.Select(r => r.TopTeacherId).Contains(t.Id)) // Filtra los profesores relevantes
            .ToDictionary(t => t.Id, t => t.Name); // Crea un diccionario de Id -> Nombre

        // Convertir el reporte en el modelo final
        var finalReport = report.Select(r => new MonthlyClassReport
        {
            Month = r.Month,
            TotalClasses = r.TotalClasses,
            TotalQuota = r.TotalQuota,
            AverageQuota = r.AverageQuota,
            TopTeacherId = r.TopTeacherId,
            TopTeacherName = teacherNames.ContainsKey(r.TopTeacherId) ? teacherNames[r.TopTeacherId] : "Desconocido" // Asigna el nombre del profesor o un valor por defecto
        }).OrderBy(r => r.Month).ToList();

        return finalReport;
    }

    public IEnumerable<ClassConcurrenceReport> GetClassesWithMostUsers()
    {
        // Agrupar por clases y contar la concurrencia
        var report = _context.ProgrammingUsers
            .GroupBy(pu => pu.ClassId) 
            .Select(group => new
            {
                ClassId = group.Key,
                UserCount = group.Count() 
            })
            .OrderByDescending(r => r.UserCount) // Ordenar por mayor concurrencia
            .Take(10) //Tomo 10 nomas
            .ToList();

            var classData = _context.Classes
                .Where(c => report.Select(r => r.ClassId).Contains(c.Id))
                .Select(c => new
                {
                    c.Id,
                    ClassName = c.Name,
                    ActivityName = c.Activities.NameActivity,
                    ProfesorName = c.Teachers.Name
                })
                .ToList();

            var classDictionary = classData.ToDictionary(c => c.Id, c => new { c.ClassName, c.ActivityName, c.ProfesorName });


            var finalReport = report.Select(r => new ClassConcurrenceReport
            {
                ClassId = r.ClassId,
                ClassName = classDictionary.ContainsKey(r.ClassId) ? classDictionary[r.ClassId].ClassName : "Clase desconocida",
                ActivityName = classDictionary.ContainsKey(r.ClassId) ? classDictionary[r.ClassId].ActivityName : "Actividad desconocida",
                ProfesorName = classDictionary.ContainsKey(r.ClassId) ? classDictionary[r.ClassId].ProfesorName : "Profesor desconocido",
                UserCount = r.UserCount
            }).ToList();

            return finalReport;
            }

}
