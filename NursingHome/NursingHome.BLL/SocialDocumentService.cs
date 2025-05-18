using Microsoft.EntityFrameworkCore;
using NursingHome.DAL;
using NursingHome.DAL.Models;

namespace NursingHome.BLL
{
    public class SocialDocumentService
    {
        private readonly NursingHomeDbContext _dbContext;

        public SocialDocumentService(NursingHomeDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<SocialDocument>> GetAll() 
            => await _dbContext.SocialDocuments
                .Include(d => d.Resident)
                .Include(d => d.UploadedBy)
                .OrderBy(d => d.Resident.FirstName)
                .ThenBy(d => d.Resident.MiddleName)
                .ThenBy(d => d.Resident.LastName)
                .ToListAsync();
    }
}