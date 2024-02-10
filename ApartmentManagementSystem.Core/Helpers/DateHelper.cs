namespace ApartmentManagementSystem.Core.Helpers;

public class DateHelper
{
    public static bool IsValidMonth(int month)
    {
        return month >= 1 && month <= 12;
    }

    public static bool IsValidYear(int year)
    {
        return year >= DateTime.MinValue.Year && year <= DateTime.MaxValue.Year;
    }

    public static DateTime CalculateDueDate(int year, int month)
    {
        var daysInMonth = DateTime.DaysInMonth(year, month);
        return new DateTime(year, month, daysInMonth, 23, 59, 59);
    }
}