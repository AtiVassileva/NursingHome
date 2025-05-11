using Microsoft.EntityFrameworkCore;
using NursingHome.DAL;
using NursingHome.DAL.Common;
using NursingHome.DAL.Models;
using static NursingHome.DAL.Common.ModelConstants;

namespace NursingHome.BLL
{
    public class PaymentService
    {
        private readonly NursingHomeDbContext _dbContext;

        public PaymentService(NursingHomeDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Payment?> GetById(Guid id) => await _dbContext.Payments.FirstOrDefaultAsync(p => p.Id == id);

        public async Task MarkAsPaid(Guid id)
        {
            var payment = await GetById(id);

            if (payment == null)
            {
                return;
            }

            payment.Status = PaymentStatus.Paid;
            await _dbContext.SaveChangesAsync();
        }

        public async Task MarkAsUnpaid(Guid id)
        {
            var payment = await GetById(id);

            if (payment == null)
            {
                return;
            }

            payment.Status = PaymentStatus.Unpaid;
            await _dbContext.SaveChangesAsync();
        }
    }
}
