using Microsoft.EntityFrameworkCore;
using NursingHome.DAL;
using NursingHome.DAL.Models;

namespace NursingHome.BLL
{
    public class WeeklyMenuService
    {
        private readonly NursingHomeDbContext _dbContext;

        public WeeklyMenuService(NursingHomeDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<WeeklyMenu?> GetWeeklyMenu(DateTime startOfWeek, DateTime endOfWeek) 
            => await _dbContext.WeeklyMenus
                .FirstOrDefaultAsync(m => m.StartOfWeek == startOfWeek && m.EndOfWeek == endOfWeek);

        public async Task<List<WeeklyMenu>> GetAll()
            => await _dbContext.WeeklyMenus
                .OrderByDescending(m => m.StartOfWeek)
                .ToListAsync();
    }
}