using Microsoft.EntityFrameworkCore;
using NursingHome.DAL;
using NursingHome.DAL.Common;
using NursingHome.DAL.Models;

namespace NursingHome.BLL
{
    using static ModelConstants;

    public class UserService
    {
        private readonly NursingHomeDbContext _dbContext;

        public UserService(NursingHomeDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ApplicationUser> GetById(string id)
        {
            var user = await _dbContext.Users
                .Include(u => u.EmployeeInfo)
                .Include(u => u.ResidentInfo)
                .FirstOrDefaultAsync(u => u.Id == id && u.UserStatus == UserStatus.Active);

            if (user == null)
            {
                throw new NullReferenceException("User not found!");
            }

            return user;
        }

        public async Task<List<ApplicationUser>> GetAllActiveUsers()
        {
            return await _dbContext.Users
                .Where(u => u.UserStatus == UserStatus.Active)
                .ToListAsync();
        }

        public async Task<List<ApplicationUser>> GetAllInactiveUsers()
        {
            return await _dbContext.Users
                .Where(u => u.UserStatus == UserStatus.Inactive)
                .ToListAsync();
        }

        public async Task<List<ApplicationUser>> GetUsersWithEmployeeInfo(IEnumerable<ApplicationUser> employees)
        {
            var employeeIds = employees.Select(e => e.Id).ToList();

            var usersWithInfo = await _dbContext.Users
                .Where(u => employeeIds.Contains(u.Id))
                .Include(u => u.EmployeeInfo)
                .ToListAsync();

            return usersWithInfo;
        }

        public async Task<List<ApplicationUser>> GetUsersWithResidentInfo(IEnumerable<ApplicationUser> residents)
        {
            var residentIds = residents.Select(e => e.Id).ToList();

            var usersWithInfo = await _dbContext.Users
                .Where(u => residentIds.Contains(u.Id))
                .Include(u => u.ResidentInfo)
                .ToListAsync();

            return usersWithInfo;
        }

        public async Task<decimal> GetResidentTotalIncome(string userId)
        {
            var user = await GetById(userId);

            if (user == null || user.ResidentInfo == null)
                return 0.0m;

            var totalIncome = 0.0m;

            if (user.ResidentInfo?.Pension.HasValue == true)
            {
                totalIncome += (decimal)user.ResidentInfo?.Pension.Value!;
            }

            if (user.ResidentInfo?.Rent.HasValue == true)
            {
                totalIncome += (decimal)user.ResidentInfo?.Rent.Value!;
            }

            if (user.ResidentInfo?.Salary.HasValue == true)
            {
                totalIncome += (decimal)user.ResidentInfo?.Salary.Value!;
            }

            if (user.ResidentInfo?.OtherIncome.HasValue == true)
            {
                totalIncome += (decimal)user.ResidentInfo?.OtherIncome.Value!;
            }

            return totalIncome;
        }

        public async Task<bool> HasResidentFeeExceptions(string userId)
        {
            var user = await GetById(userId);

            if (user == null || user.ResidentInfo == null)
                return false;

            var hasExceptions = user.ResidentInfo?.HasDonation == true 
                                || user.ResidentInfo?.HasInheritance == true 
                                || user.ResidentInfo?.HasRealEstateSale == true 
                                || user.ResidentInfo?.HasSupportContract == true;

            return hasExceptions;
        }

        public async Task RecordEmployeeInfo(string userId, EmployeeInfo employeeInfo)
        {
            employeeInfo.ApplicationUserId = userId;
            await _dbContext.EmployeeInfos.AddAsync(employeeInfo);
            await _dbContext.SaveChangesAsync();
        }

        public async Task RecordResidentInfo(string userId, ResidentInfo residentInfo)
        {
            residentInfo.ApplicationUserId = userId;
            await _dbContext.ResidentInfos.AddAsync(residentInfo);
            await _dbContext.SaveChangesAsync();
        }
    }
}