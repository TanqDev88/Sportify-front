using Microsoft.EntityFrameworkCore;
using Sportify_back.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sportify_Back.Services
{
    public class ActivityService : IActivityService
    {
        private readonly SportifyDbContext _context;

        public ActivityService(SportifyDbContext context)
        {
            _context = context;
        }

        public async Task<List<Activities>> GetAllAsync()
        {
            return await _context.Activities.ToListAsync();
        }

        public async Task<Activities?> GetByIdAsync(int id)
        {
            return await _context.Activities.FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task CreateAsync(Activities activity)
        {
            _context.Activities.Add(activity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Activities activity)
        {
            _context.Activities.Update(activity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var activity = await GetByIdAsync(id);
            if (activity != null)
            {
                _context.Activities.Remove(activity);
                await _context.SaveChangesAsync();
            }
        }

        public bool ActivityExists(int id)
        {
            return _context.Activities.Any(a => a.Id == id);
        }
    }
}
