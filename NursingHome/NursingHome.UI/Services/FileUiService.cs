using Microsoft.EntityFrameworkCore;
using NursingHome.DAL;
using NursingHome.DAL.Models;
using System.Security.Claims;
using static NursingHome.DAL.Common.ModelConstants;

namespace NursingHome.UI.Services
{
    public class FileUiService
    {
        private readonly NursingHomeDbContext _context;
        private readonly IWebHostEnvironment _env;

        public FileUiService(NursingHomeDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<bool> UploadReport(IFormFile file, ReportType type, ClaimsPrincipal user)
        {
            try
            {
                var uploadsDir = Path.Combine(_env.WebRootPath, "uploads", "reports");
                Directory.CreateDirectory(uploadsDir);

                var now = DateTime.UtcNow;
                var extension = Path.GetExtension(file.FileName);

                var uniqueName = $"report_{type}_{now:yyyyMM}_{file.Name}{extension}";
                var fullPath = Path.Combine(uploadsDir, uniqueName);
                var virtualPath = $"/uploads/reports/{uniqueName}";

                var existing = await _context.Reports
                    .FirstOrDefaultAsync(r =>
                        r.Type == type &&
                        r.UploadedOn.Month == now.Month &&
                        r.UploadedOn.Year == now.Year);

                if (existing != null)
                {
                    var oldPath = Path.Combine(_env.WebRootPath, existing.FilePath.TrimStart('/').Replace("/", Path.DirectorySeparatorChar.ToString()));
                    if (File.Exists(oldPath))
                    {
                        File.Delete(oldPath);
                    }

                    existing.FileName = uniqueName;
                    existing.FilePath = virtualPath;
                    existing.ContentType = file.ContentType;
                    existing.UploadedOn = now;
                    existing.UploadedById = user.FindFirstValue(ClaimTypes.NameIdentifier)!;
                }
                else
                {
                    var report = new Report
                    {
                        FileName = uniqueName,
                        FilePath = virtualPath,
                        ContentType = file.ContentType,
                        Type = type,
                        UploadedOn = now,
                        UploadedById = user.FindFirstValue(ClaimTypes.NameIdentifier)!
                    };

                    _context.Reports.Add(report);
                }

                await using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public async Task<List<Report>> GetOccupationalTherapistsReports() 
            => await _context.Reports
                .Include(r => r.UploadedBy)
                .Where(r => r.UploadedBy!.EmployeeInfo!.EmployeePosition == EmployeePosition.OccupationalTherapist)
                .OrderByDescending(r => r.UploadedOn)
                .ToListAsync();

        public async Task<List<Report>> GetPsychologistsReports()
            => await _context.Reports
                .Include(r => r.UploadedBy)
                .Where(r => r.UploadedBy!.EmployeeInfo!.EmployeePosition == EmployeePosition.Psychologist)
                .OrderByDescending(r => r.UploadedOn)
                .ToListAsync();
    }
}