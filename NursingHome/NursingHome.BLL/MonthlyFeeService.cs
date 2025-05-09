using Microsoft.EntityFrameworkCore;
using NursingHome.DAL;
using NursingHome.DAL.Models;

namespace NursingHome.BLL
{
    public class MonthlyFeeService
    {
        private readonly UserService _userService;
        private readonly NursingHomeDbContext _dbContext;

        public MonthlyFeeService(UserService userService, NursingHomeDbContext dbContext)
        {
            _userService = userService;
            _dbContext = dbContext;
        }

        public async Task<MonthlyFee?> GetMonthlyFeeForUserByMonth(string userId, int month, int year)
        {
            var monthlyFee =  await _dbContext.MonthlyFees
                .FirstOrDefaultAsync(m => m.UserId == userId && m.Month == month && m.Year == year);

            return monthlyFee;
        }

        public async Task<Tuple<decimal, decimal>> CalculateMonthlyFee(ApplicationUser user, MonthlyParameter monthParams, int presentDaysCount)
        {
            if (presentDaysCount <= 0) return new Tuple<decimal, decimal>(0.0m, 0.0m);

            var dietRate = monthParams.DietRates.FirstOrDefault(d => d.DietNumber == user.ResidentInfo?.DietNumber);
            var stayRate = monthParams.StayRates.FirstOrDefault(s => s.RoomType == user.ResidentInfo?.RoomType);
            var daysInMonth = monthParams.DaysInMonth;

            var foodPerDay = Math.Round(dietRate?.MonthlyRate / daysInMonth ?? 0, 2);
            var stayPerDay = Math.Round(stayRate?.MonthlyRate / daysInMonth ?? 0, 2);

            var realCost = Math.Round((foodPerDay + stayPerDay) * presentDaysCount, 2);

            var userIncome = await _userService.GetResidentTotalIncome(user.Id);
            var seventyPercent = Math.Round(userIncome * 0.7M, 2);

            var hasExceptions = await _userService.HasResidentFeeExceptions(user.Id);

            var actualFeeAmount = hasExceptions
                ? realCost
                : Math.Min(realCost, seventyPercent);

            var feesTuple = new Tuple<decimal, decimal>(realCost, actualFeeAmount);
            return feesTuple;
        }

        public async Task<bool> SaveMonthlyFee(MonthlyFee model)
        {
            var existingMonthlyFee = await GetMonthlyFeeForUserByMonth(model.UserId, model.Month, model.Year);

            if (existingMonthlyFee != null)
            {
                existingMonthlyFee.PresentDays = model.PresentDays;
                existingMonthlyFee.RealCost = model.RealCost;
                existingMonthlyFee.FeeAmount = model.FeeAmount;

                _dbContext.MonthlyFees.Update(existingMonthlyFee);
            }
            else
            {
                await _dbContext.MonthlyFees.AddAsync(model);
            }

            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}