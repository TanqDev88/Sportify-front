using Sportify_back.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sportify_Back.Services
{
    public interface IClassService
    {
        Task<List<Classes>> GetAllAsync();
        Task<Classes?> GetByIdAsync(int id);
        Task CreateAsync(Classes classes);
        Task UpdateAsync(Classes classes);
        Task DeleteAsync(int id);
        bool ClassExists(int id);
    }
}

