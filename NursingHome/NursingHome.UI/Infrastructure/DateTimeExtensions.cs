namespace NursingHome.UI.Infrastructure
{
    public static class DateTimeExtensions
    {
        public static DateTime GetMondayOfCurrentWeek(this DateTime dateTime)
        {
            var today = DateTime.Now;
            var daysToSubtract = (int)today.DayOfWeek - (int)DayOfWeek.Monday;

            if (daysToSubtract < 0)
                daysToSubtract += 7;

            var startOfWeek = today.Date.AddDays(-daysToSubtract);
            return startOfWeek;
        }

        public static DateTime GetSundayOfCurrentWeek(this DateTime dateTime)
        {
            var dt = new DateTime();
            var endOfWeek = dt.GetMondayOfCurrentWeek().AddDays(6);

            return endOfWeek;
        }
    }
}