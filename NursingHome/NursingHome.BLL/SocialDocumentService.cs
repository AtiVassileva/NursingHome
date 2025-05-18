using Microsoft.EntityFrameworkCore;
using NursingHome.DAL;
using NursingHome.DAL.Models;
using static NursingHome.DAL.Common.ModelConstants;

namespace NursingHome.BLL
{
    public class SocialDocumentService
    {
        private readonly NursingHomeDbContext _dbContext;

        public SocialDocumentService(NursingHomeDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<SocialDocument?> GetByTypeAndResident(string residentId, SocialDocumentType type)
         => await _dbContext.SocialDocuments
             .FirstOrDefaultAsync(d => d.ResidentId == residentId && d.DocumentType == type);

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