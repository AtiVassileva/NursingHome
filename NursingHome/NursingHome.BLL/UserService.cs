using Microsoft.EntityFrameworkCore;
using NursingHome.DAL;
using NursingHome.DAL.Common;
using NursingHome.DAL.Models;

namespace NursingHome.BLL
{
    using static ModelConstants;

    public class UserService
    {
        private readonly ApplicationDbContext _dbContext;

        public UserService(ApplicationDbContext dbContext)
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

        public async Task<List<ApplicationUser>> GetAllUsers()
        {
            return await _dbContext.Users.ToListAsync();
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