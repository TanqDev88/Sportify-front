using Sportify_back.Models;
using System.Collections.Generic;

namespace Sportify_Back.Services
{
    public interface IActivityService
    {
        Task<List<Activities>> GetAllAsync();
        Task<Activities?> GetByIdAsync(int id);
        Task CreateAsync(Activities activity);
        Task UpdateAsync(Activities activity);
        Task DeleteAsync(int id);
        bool ActivityExists(int id);
    }
}
