using Sportify_Back.Models;
namespace Sportify_Back.Services;

public interface IEntityReportService
{
    IEnumerable<MonthlyClassReport> GenerateClassReport();

    IEnumerable<ClassConcurrenceReport> GetClassesWithMostUsers();
}