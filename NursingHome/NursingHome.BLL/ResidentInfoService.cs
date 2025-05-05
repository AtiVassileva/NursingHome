using Microsoft.EntityFrameworkCore;
using NursingHome.DAL;
using NursingHome.DAL.Models;

namespace NursingHome.BLL
{
    public class ResidentInfoService
    {
        private readonly NursingHomeDbContext _dbContext;

        public ResidentInfoService(NursingHomeDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ResidentInfo?> GetResidentInfoByUserId(string userId)
        {
            return await _dbContext.ResidentInfos
                .FirstOrDefaultAsync(r => r.ApplicationUserId == userId);
        }

        public async Task DeleteResidentInfo(Guid id)
        {
            var residentInfo = await _dbContext.ResidentInfos
                .FirstOrDefaultAsync(r => r.Id == id);

            if (residentInfo == null)
                return;

            _dbContext.ResidentInfos.Remove(residentInfo);
            await _dbContext.SaveChangesAsync();
        }
    }
}