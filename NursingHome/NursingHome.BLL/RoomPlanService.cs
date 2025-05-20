using Microsoft.EntityFrameworkCore;
using NursingHome.DAL;

namespace NursingHome.BLL
{
    public class RoomPlanService
    {
        private readonly NursingHomeDbContext _dbContext;

        public RoomPlanService(NursingHomeDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<RoomPlan?> GetLatest()
        => await _dbContext.RoomPlans
            .OrderByDescending(p => p.UploadedOn)
            .FirstOrDefaultAsync();
    }
}