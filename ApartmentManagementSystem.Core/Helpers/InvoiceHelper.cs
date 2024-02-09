namespace ApartmentManagementSystem.Core.Helpers;

public class InvoiceHelper
{
    public static DateTime CalculateDueDate(int month, int year)
    {
        var daysInMonth = DateTime.DaysInMonth(year, month);
        return new DateTime(year, month, daysInMonth);
    }
}