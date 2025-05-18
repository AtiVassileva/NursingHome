using Microsoft.AspNetCore.Identity;
using NursingHome.DAL;
using NursingHome.DAL.Models;
using System.Security.Claims;
using NursingHome.BLL;
using NursingHome.UI.Models;
using static NursingHome.DAL.Common.ModelConstants;

namespace NursingHome.UI.Services
{
    public class FileUiService
    {
        private readonly NursingHomeDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ReportService _reportService;
        private readonly SocialDocumentService _socialDocumentService;

        public FileUiService(NursingHomeDbContext context, IWebHostEnvironment env, UserManager<ApplicationUser> userManager, ReportService reportService, SocialDocumentService socialDocumentService)
        {
            _context = context;
            _env = env;
            _userManager = userManager;
            _reportService = reportService;
            _socialDocumentService = socialDocumentService;
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

                var existing = await _reportService.GetByMonthAndType(type, DateTime.Now.Month, DateTime.Now.Year);

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

        public async Task<bool> UploadSocialDocument(ClaimsPrincipal user, UploadSocialDocumentViewModel model)
        {
            try
            {
                var existing = await _socialDocumentService.GetByTypeAndResident(model.ResidentId, model.DocumentType);

                var fileName = $"{model.DocumentType}_{model.File.FileName}_{model.ResidentId}{Path.GetExtension(model.File.FileName)}";
                var folder = Path.Combine(_env.WebRootPath, "uploads", "social");
                Directory.CreateDirectory(folder);
                var path = Path.Combine(folder, fileName);

                await using (var stream = new FileStream(path, FileMode.Create))
                {
                    await model.File.CopyToAsync(stream);
                }

                var uploader = await _userManager.GetUserAsync(user);

                if (existing != null)
                {
                    File.Delete(Path.Combine(folder, existing.FileName));

                    existing.FileName = fileName;
                    existing.FilePath = $"/uploads/social/{fileName}";
                    existing.ContentType = model.File.ContentType;
                    existing.UploadedOn = DateTime.UtcNow;
                    existing.UploadedById = uploader!.Id;
                }
                else
                {
                    var doc = new SocialDocument
                    {
                        FileName = fileName,
                        FilePath = $"/uploads/social/{fileName}",
                        ContentType = model.File.ContentType,
                        DocumentType = model.DocumentType,
                        ResidentId = model.ResidentId,
                        UploadedById = uploader!.Id
                    };
                    _context.SocialDocuments.Add(doc);
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
    }
}