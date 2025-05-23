﻿using Microsoft.AspNetCore.Identity;
using NursingHome.DAL;
using NursingHome.DAL.Models;
using System.Security.Claims;
using NursingHome.BLL;
using NursingHome.UI.Models;
using static NursingHome.DAL.Common.ModelConstants;
using NursingHome.UI.Infrastructure;
using Syncfusion.DocIO.DLS;
using Syncfusion.DocIORenderer;
using Syncfusion.ExcelToPdfConverter;
using Syncfusion.XlsIO;

namespace NursingHome.UI.Services
{
    public class FileUiService
    {
        private readonly NursingHomeDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ReportService _reportService;
        private readonly SocialDocumentService _socialDocumentService;
        private readonly WeeklyMenuService _weeklyMenuService;
        private readonly MessageService _messageService;
        private readonly WorkScheduleService _workScheduleService;
        private readonly RoomPlanService _roomPlanService;

        public FileUiService(NursingHomeDbContext context, IWebHostEnvironment env,
            UserManager<ApplicationUser> userManager, ReportService reportService,
            SocialDocumentService socialDocumentService, WeeklyMenuService weeklyMenuService, MessageService messageService, WorkScheduleService workScheduleService, RoomPlanService roomPlanService)
        {
            _context = context;
            _env = env;
            _userManager = userManager;
            _reportService = reportService;
            _socialDocumentService = socialDocumentService;
            _weeklyMenuService = weeklyMenuService;
            _messageService = messageService;
            _workScheduleService = workScheduleService;
            _roomPlanService = roomPlanService;
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
                    var oldPath = Path.Combine(_env.WebRootPath,
                        existing.FilePath.TrimStart('/').Replace("/", Path.DirectorySeparatorChar.ToString()));

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

                var fileName =
                    $"{model.DocumentType}_{model.File.FileName}_{model.ResidentId}{Path.GetExtension(model.File.FileName)}";
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

        public async Task<bool> UploadWeeklyMenu(IFormFile file, ClaimsPrincipal user)
        {
            try
            {
                var uploader = await _userManager.GetUserAsync(user);

                var today = DateTime.Today;
                var startOfWeek = today.GetMondayOfCurrentWeek();
                var endOfWeek = today.GetSundayOfCurrentWeek();

                var folder = Path.Combine(_env.WebRootPath, "uploads", "weeklymenus");
                Directory.CreateDirectory(folder);

                var isWordFile = Path.GetExtension(file.FileName).ToLower() is ".doc" or ".docx";
                var extension = isWordFile ? ".pdf" : Path.GetExtension(file.FileName).ToLower();

                var originalName = Path.GetFileNameWithoutExtension(file.FileName);
                var fileName = $"WeeklyMenu_{startOfWeek:yyyyMMdd}_{originalName}{extension}";
                var filePath = Path.Combine(folder, fileName);

                var existing = await _weeklyMenuService.GetWeeklyMenu(startOfWeek, endOfWeek);

                if (existing != null)
                {
                    File.Delete(Path.Combine(folder, existing.FileName));
                    _context.WeeklyMenus.Remove(existing);
                }

                if (isWordFile)
                {
                    await ConvertWordToPdfAsync(file, filePath);
                }
                else
                {
                    await using var stream = new FileStream(filePath, FileMode.Create);
                    await file.CopyToAsync(stream);
                }

                var menu = new WeeklyMenu
                {
                    FileName = fileName,
                    FilePath = $"/uploads/weeklymenus/{fileName}",
                    ContentType = file.ContentType,
                    UploadedOn = DateTime.UtcNow,
                    StartOfWeek = startOfWeek,
                    EndOfWeek = endOfWeek,
                    UploadedById = uploader!.Id
                };

                _context.WeeklyMenus.Add(menu);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public async Task CreateMessageWithFile(MessageCreateViewModel model, ClaimsPrincipal user)
        {
            try
            {
                var author = await _userManager.GetUserAsync(user);

                string? filePath = null;
                string? fileName = null;

                if (model.File != null)
                {
                    var uploadsDir = Path.Combine(_env.WebRootPath, "uploads", "messages");
                    Directory.CreateDirectory(uploadsDir);

                    fileName = $"{Guid.NewGuid()}_{Path.GetFileName(model.File.FileName)}";
                    filePath = Path.Combine(uploadsDir, fileName);

                    await using var stream = new FileStream(filePath, FileMode.Create);
                    await model.File.CopyToAsync(stream);
                }

                var message = new Message
                {
                    Title = model.Title,
                    Content = model.Content,
                    Audience = model.Audience,
                    AuthorId = author!.Id,
                    AttachmentFileName = fileName,
                    AttachmentFilePath = filePath != null ? $"/uploads/messages/{fileName}" : null
                };

                _context.Messages.Add(message);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public async Task<bool> EditMessageWithFile(MessageCreateViewModel model, string currentUserId)
        {
            var message = await _messageService.GetById(model.Id);

            if (message == null || message.AuthorId != currentUserId)
                return false;

            message.Title = model.Title;
            message.Content = model.Content;

            if (model.File != null && model.File.Length > 0)
            {
                if (!string.IsNullOrEmpty(message.AttachmentFilePath))
                {
                    var oldFilePath = Path.Combine(_env.WebRootPath, message.AttachmentFilePath.TrimStart('/'));
                    if (System.IO.File.Exists(oldFilePath))
                        System.IO.File.Delete(oldFilePath);
                }

                var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads", "messages");
                Directory.CreateDirectory(uploadsFolder);

                var fileName = $"{Guid.NewGuid()}_{model.File.FileName}";
                var filePath = Path.Combine(uploadsFolder, fileName);

                await using (var stream = new FileStream(filePath, FileMode.Create))
                    await model.File.CopyToAsync(stream);

                message.AttachmentFileName = model.File.FileName;
                message.AttachmentFilePath = $"/uploads/messages/{fileName}";
            }

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UploadWorkSchedule(EmployeePosition position, IFormFile file)
        {
            try
            {
                var folder = Path.Combine(_env.WebRootPath, "uploads", "workschedules");
                Directory.CreateDirectory(folder);

                var isWordFile = Path.GetExtension(file.FileName).ToLower() is ".doc" or ".docx";
                var isExcelFile = Path.GetExtension(file.FileName).ToLower() is ".xls" or ".xlsx";
                var extension = isWordFile || isExcelFile ? ".pdf" : Path.GetExtension(file.FileName).ToLower();

                var fileName = $"{position}_WorkSchedule{extension}";
                var filePath = Path.Combine(folder, fileName);
                
                var existing = await _workScheduleService.GetByPosition(position);

                if (existing != null)
                {
                    var oldPath = Path.Combine(folder, existing.FileName);
                    if (System.IO.File.Exists(oldPath))
                        File.Delete(oldPath);

                    existing.FileName = fileName;
                    existing.FilePath = $"/uploads/workschedules/{fileName}";
                    existing.UploadedOn = DateTime.UtcNow;
                }
                else
                {
                    existing = new WorkSchedule
                    {
                        EmployeePosition = position,
                        FileName = fileName,
                        FilePath = $"/uploads/workschedules/{fileName}",
                        UploadedOn = DateTime.UtcNow
                    };
                    _context.WorkSchedules.Add(existing);
                }
                
                if (isWordFile)
                {
                    await ConvertWordToPdfAsync(file, filePath);
                }
                else if (isExcelFile)
                {
                    await ConvertExcelToPdfAsync(file, filePath);
                }
                else
                {
                    await using var stream = new FileStream(filePath, FileMode.Create);
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

        public async Task<bool> UploadRegulatoryDocument(ClaimsPrincipal user, RegulatoryDocumentViewModel model)
        {
            try
            {
                var uploader = await _userManager.GetUserAsync(user);
                var folder = Path.Combine(_env.WebRootPath, "uploads", "regulations");
                Directory.CreateDirectory(folder);

                var fileName = $"{DateTime.UtcNow:yyyyMMddHHmmss}_{model.File.FileName}";
                var filePath = Path.Combine(folder, fileName);

                await using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.File.CopyToAsync(stream);
                }

                var doc = new RegulatoryDocument
                {
                    Title = model.Title,
                    FileName = fileName,
                    FilePath = $"/uploads/regulations/{fileName}",
                    UploadedOn = DateTime.UtcNow,
                    UploadedById = uploader!.Id
                };

                _context.RegulatoryDocuments.Add(doc);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public async Task<bool> UploadRoomPlan(ClaimsPrincipal user, IFormFile file)
        {
            try
            {
                var uploader = await _userManager.GetUserAsync(user);

                var folder = Path.Combine(_env.WebRootPath, "uploads", "roomplans");
                Directory.CreateDirectory(folder);

                var isWordFile = Path.GetExtension(file.FileName).ToLower() is ".doc" or ".docx";
                var extension = isWordFile ? ".pdf" : Path.GetExtension(file.FileName).ToLower();

                var fileName = $"RoomPlan_{DateTime.UtcNow:yyyyMMddHHmmss}.pdf";
                var filePath = Path.Combine(folder, fileName);
                
                var existing = await _roomPlanService.GetLatest();

                if (existing != null)
                {
                    var oldFilePath = Path.Combine(_env.WebRootPath, existing.FilePath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
                    if (File.Exists(oldFilePath))
                    {
                        File.Delete(oldFilePath);
                    }

                    _context.RoomPlans.Remove(existing);
                }
                
                if (isWordFile)
                {
                    await ConvertWordToPdfAsync(file, filePath);
                }
                else
                {
                    fileName = $"RoomPlan_{DateTime.UtcNow:yyyyMMddHHmmss}{extension}";
                    filePath = Path.Combine(folder, fileName);

                    await using var stream = new FileStream(filePath, FileMode.Create);
                    await file.CopyToAsync(stream);
                }

                var newPlan = new RoomPlan
                {
                    FileName = fileName,
                    FilePath = $"/uploads/roomplans/{fileName}",
                    UploadedOn = DateTime.UtcNow,
                    UploadedById = uploader!.Id
                };

                _context.RoomPlans.Add(newPlan);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        private static async Task ConvertWordToPdfAsync(IFormFile file, string filePath)
        {
            await using var stream = file.OpenReadStream();
            using var wordDocument = new WordDocument(stream, Syncfusion.DocIO.FormatType.Automatic);
            using var renderer = new DocIORenderer();
            using var pdfDocument = renderer.ConvertToPDF(wordDocument);

            await using var pdfStream = new FileStream(filePath, FileMode.Create);
            pdfDocument.Save(pdfStream);
            pdfDocument.Close();
        }

        private static async Task ConvertExcelToPdfAsync(IFormFile file, string filePath)
        {
            await using var stream = file.OpenReadStream();

            using var excelEngine = new ExcelEngine();
            var application = excelEngine.Excel;
            application.DefaultVersion = ExcelVersion.Excel2016;
            
            var workbook = application.Workbooks.Open(stream);
            
            using var renderer = new ExcelToPdfConverter(workbook);
            var pdfDoc = renderer.Convert();
            
            await using var pdfStream = new FileStream(filePath, FileMode.Create);
            pdfDoc.Save(pdfStream);

            pdfDoc.Close(true);
            workbook.Close();
        }
    }
}