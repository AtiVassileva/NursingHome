namespace NursingHome.UI.Models
{
    public class FeeBookRowViewModel
    {
        public string FullName { get; set; } = null!;
        public int PresentDays { get; set; }
        public decimal RealCost { get; set; }
        public decimal Pension { get; set; }
        public decimal Rent { get; set; }
        public decimal Salary { get; set; }
        public decimal OtherIncome { get; set; }
        public decimal TotalIncome { get; set; }
        public string PercentageType { get; set; } = "";
        public decimal FeeCalculated { get; set; }
        public string HasFeeException { get; set; } = "НЕ";
    }
}