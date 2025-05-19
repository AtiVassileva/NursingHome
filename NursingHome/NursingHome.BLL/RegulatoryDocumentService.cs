using Microsoft.EntityFrameworkCore;
using NursingHome.DAL;
using NursingHome.DAL.Models;

namespace NursingHome.BLL
{
    public class RegulatoryDocumentService
    {
        private readonly NursingHomeDbContext _dbContext;

        public RegulatoryDocumentService(NursingHomeDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<RegulatoryDocument?> GetById(Guid id)
            => await _dbContext.RegulatoryDocuments
                .FirstOrDefaultAsync(d => d.Id == id);

        public async Task<List<RegulatoryDocument>> GetAll()
        => await _dbContext.RegulatoryDocuments
            .Include(d => d.UploadedBy)
            .OrderByDescending(d => d.UploadedOn)
            .ToListAsync();

        public async Task<bool> Delete(Guid id)
        {
            var document = await GetById(id);

            if (document == null)
            {
                return false;
            }

            _dbContext.RegulatoryDocuments.Remove(document);
            await _dbContext.SaveChangesAsync();

            return true;
        }
    }
}