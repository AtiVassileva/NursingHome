using Microsoft.EntityFrameworkCore;
using NursingHome.DAL;
using NursingHome.DAL.Models;
using static NursingHome.DAL.Common.ModelConstants;

namespace NursingHome.BLL
{
    public class ReportService
    {
        private readonly NursingHomeDbContext _dbContext;

        public ReportService(NursingHomeDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Report?> GetByMonthAndType(ReportType type, int month, int year)
        => await _dbContext.Reports
            .FirstOrDefaultAsync(r =>
                r.Type == type &&
                r.UploadedOn.Month == month &&
                r.UploadedOn.Year == year);

        public async Task<List<Report>> GetAll()
            => await _dbContext.Reports
                .Include(r => r.UploadedBy)
                .OrderByDescending(r => r.UploadedOn)
                .ToListAsync();

        public async Task<List<Report>> GetOccupationalTherapistsReports()
            => await _dbContext.Reports
                .Include(r => r.UploadedBy)
                .Where(r => r.UploadedBy!.EmployeeInfo!.EmployeePosition == EmployeePosition.OccupationalTherapist)
                .OrderByDescending(r => r.UploadedOn)
                .ToListAsync();

        public async Task<List<Report>> GetPsychologistsReports()
            => await _dbContext.Reports
                .Include(r => r.UploadedBy)
                .Where(r => r.UploadedBy!.EmployeeInfo!.EmployeePosition == EmployeePosition.Psychologist)
                .OrderByDescending(r => r.UploadedOn)
                .ToListAsync();
    }
}