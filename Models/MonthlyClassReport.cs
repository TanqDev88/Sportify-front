namespace Sportify_Back.Models;
public class MonthlyClassReport
{
    public int Month { get; set; } // Mes del reporte
    public int TotalClasses { get; set; } // Total de clases
    public int TotalQuota { get; set; } // Suma de cupos
    public double AverageQuota { get; set; } // Promedio de cupos
    public int TopTeacherId { get; set; } // ID del profesor con más clases
    public string TopTeacherName { get; set; }
}

