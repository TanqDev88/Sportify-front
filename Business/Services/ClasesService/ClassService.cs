using Microsoft.EntityFrameworkCore;
using Sportify_back.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sportify_Back.Services
{
    public class ClassService : IClassService
    {
        private readonly SportifyDbContext _context;

        public ClassService(SportifyDbContext context)
        {
            _context = context;
        }

        public async Task<List<Classes>> GetAllAsync()
        {
            return await _context.Classes
                .Include(c => c.Activities)
                .Include(c => c.Teachers)
                .ToListAsync();
        }

        public async Task<Classes?> GetByIdAsync(int id)
        {
            return await _context.Classes
                .Include(c => c.Activities)
                .Include(c => c.Teachers)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task CreateAsync(Classes classes)
        {
            _context.Classes.Add(classes);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Classes classes)
        {
            _context.Classes.Update(classes);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var classes = await GetByIdAsync(id);
            if (classes != null)
            {
                _context.Classes.Remove(classes);
                await _context.SaveChangesAsync();
            }
        }

        public bool ClassExists(int id)
        {
            return _context.Classes.Any(c => c.Id == id);
        }
    }
}

