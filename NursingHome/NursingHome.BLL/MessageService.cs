using Microsoft.EntityFrameworkCore;
using NursingHome.DAL;
using static NursingHome.DAL.Common.ModelConstants;
using NursingHome.DAL.Models;

namespace NursingHome.BLL
{
    public class MessageService
    {
        private readonly NursingHomeDbContext _dbContext;

        public MessageService(NursingHomeDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Message?> GetById(Guid id)
            => await _dbContext.Messages
                .FirstOrDefaultAsync(m => m.Id == id);

        public async Task<List<Message>> GetAll()
        =>  await _dbContext.Messages
            .Include(m => m.Sender)
            .OrderByDescending(m => m.CreatedOn)
            .ToListAsync();

        public async Task<List<Message>> GetMessagesForResidents()
            => await _dbContext.Messages
                .Include(m => m.Sender)
                .Where(m => m.Audience == MessageAudience.Users || m.Audience == MessageAudience.Both)
                .OrderByDescending(m => m.CreatedOn)
                .ToListAsync();

        public async Task<bool> Delete(Guid id, string userId)
        {
            var message = await GetById(id);

            if (message == null || message.AuthorId != userId)
            {
                return false;
            }

            _dbContext.Messages.Remove(message);
            await _dbContext.SaveChangesAsync();

            return true;
        }
    }
}