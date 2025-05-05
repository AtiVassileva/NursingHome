using Microsoft.EntityFrameworkCore;
using NursingHome.DAL.Models;
using NursingHome.DAL;

namespace NursingHome.BLL
{
    public class EmployeeInfoService
    {
        private readonly NursingHomeDbContext _dbContext;

        public EmployeeInfoService(NursingHomeDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<EmployeeInfo?> GetEmployeeInfoByUserId(string userId)
        {
            return await _dbContext.EmployeeInfos
                .FirstOrDefaultAsync(r => r.ApplicationUserId == userId);
        }

        public async Task DeleteEmployeeInfo(Guid id)
        {
            var employeeInfo = await _dbContext.EmployeeInfos
                .FirstOrDefaultAsync(r => r.Id == id);

            if (employeeInfo == null)
                return;

            _dbContext.EmployeeInfos.Remove(employeeInfo);
            await _dbContext.SaveChangesAsync();
        }
    }
}