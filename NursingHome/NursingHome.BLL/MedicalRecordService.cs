using Microsoft.EntityFrameworkCore;
using NursingHome.DAL;
using NursingHome.DAL.Models;

namespace NursingHome.BLL
{
    public class MedicalRecordService
    {
        private readonly NursingHomeDbContext _dbContext;

        public MedicalRecordService(NursingHomeDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<MedicalRecord>> GetAll() =>
            await _dbContext.MedicalRecords
                .Include(m => m.User)
                .ToListAsync();

        public async Task<MedicalRecord?> GetMedicalRecordForUser(string userId) =>
            await _dbContext.MedicalRecords
                .Include(m => m.User)
                .FirstOrDefaultAsync(m => m.UserId == userId);

        public async Task CreateMedicalRecordForUser(string userId)
        {
            var medicalRecord = new MedicalRecord
            {
                UserId = userId
            };

            await _dbContext.MedicalRecords.AddAsync(medicalRecord);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateUserMedicalRecord(MedicalRecord model)
        {
            var record = await GetMedicalRecordForUser(model.UserId);

            if (record == null) return;

            record.DisabilityPercent = model.DisabilityPercent;
            record.Allergies = model.Allergies;
            record.Therapy = model.Therapy;

            await _dbContext.SaveChangesAsync();
        }
    }
}