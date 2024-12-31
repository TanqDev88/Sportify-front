using System.Collections.Generic;
using System.Threading.Tasks;
using Sportify_back.Models;

namespace Sportify_Back.Services
{
    public interface ITeacherService
    {
        Task<List<Teachers>> GetAllAsync();
        Task<Teachers?> GetByIdAsync(int id);
        Task CreateAsync(Teachers teacher);
        Task UpdateAsync(Teachers teacher);
        Task DeleteAsync(int id);
        bool TeacherExists(int id);

        // Nuevo método para obtener actividades
        Task<List<Activities>> GetAllActivitiesAsync();
    }
}
