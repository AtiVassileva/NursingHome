using Microsoft.EntityFrameworkCore;
using NursingHome.DAL;
using static NursingHome.DAL.Common.ModelConstants;
using NursingHome.DAL.Models;

namespace NursingHome.BLL
{
    public class WorkScheduleService
    {
        private readonly NursingHomeDbContext _dbContext;

        public WorkScheduleService(NursingHomeDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<WorkSchedule?> GetByPosition(EmployeePosition position)
            => await _dbContext.WorkSchedules
                .FirstOrDefaultAsync(sch => sch.EmployeePosition == position);
    }
}