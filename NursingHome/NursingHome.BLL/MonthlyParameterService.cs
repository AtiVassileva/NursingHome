using Microsoft.EntityFrameworkCore;
using NursingHome.DAL;
using NursingHome.DAL.Models;

namespace NursingHome.BLL
{
    public class MonthlyParameterService
    {
        private readonly NursingHomeDbContext _dbContext;

        public MonthlyParameterService(NursingHomeDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<MonthlyParameter?> GetMonthlyParametersByMonth(int month, int year)
        {
            var monthlyParameters = await _dbContext.MonthlyParameters
                .Include(m => m.DietRates)
                .Include(m => m.StayRates)
                .FirstOrDefaultAsync(mp => mp.Month == month && mp.Year == year);

            return monthlyParameters;
        }

        public async Task<bool> SaveMonthlyParameters(MonthlyParameter model)
        {
            try
            {
                var existingMonthlyParameters =  await GetMonthlyParametersByMonth(model.Month, model.Year);

                if (existingMonthlyParameters != null)
                {
                    foreach (var rate in model.DietRates)
                    {
                        var existingRate = existingMonthlyParameters.DietRates.FirstOrDefault(r => r.DietNumber == rate.DietNumber);
                        if (existingRate != null)
                            existingRate.MonthlyRate = rate.MonthlyRate;
                    }
                    
                    foreach (var rate in model.StayRates)
                    {
                        var existingRate = existingMonthlyParameters.StayRates.FirstOrDefault(r => r.RoomType == rate.RoomType);
                        if (existingRate != null)
                            existingRate.MonthlyRate = rate.MonthlyRate;
                    }

                    await _dbContext.SaveChangesAsync();
                    return true;
                }

                model.Id = Guid.NewGuid();

                foreach (var dietRate in model.DietRates)
                {
                    dietRate.Id = Guid.NewGuid();
                    dietRate.MonthId = model.Id;
                }

                foreach (var stayRate in model.StayRates)
                {
                    stayRate.Id = Guid.NewGuid();
                    stayRate.MonthId = model.Id;
                }

                await _dbContext.MonthlyParameters.AddAsync(model);
                await _dbContext.SaveChangesAsync();

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