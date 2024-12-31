using Microsoft.EntityFrameworkCore;
using Sportify_back.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sportify_Back.Services
{
    public class TeacherService : ITeacherService
    {
        private readonly SportifyDbContext _context;

        public TeacherService(SportifyDbContext context)
        {
            _context = context;
        }

        public async Task<List<Teachers>> GetAllAsync()
        {
            return await _context.Teachers
                .Include(t => t.Activities)
                .ToListAsync();
        }

        public async Task<Teachers?> GetByIdAsync(int id)
        {
            return await _context.Teachers
                .Include(t => t.Activities)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task CreateAsync(Teachers teacher)
        {
            _context.Teachers.Add(teacher);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Teachers teacher)
        {
            _context.Teachers.Update(teacher);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var teacher = await GetByIdAsync(id);
            if (teacher != null)
            {
                _context.Teachers.Remove(teacher);
                await _context.SaveChangesAsync();
            }
        }

        public bool TeacherExists(int id)
        {
            return _context.Teachers.Any(t => t.Id == id);
        }
        public async Task<List<Activities>> GetAllActivitiesAsync()
{
    return await _context.Activities.ToListAsync();
}
        
    }
}
