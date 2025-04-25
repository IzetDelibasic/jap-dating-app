namespace DatingApp.Extensions;

public static class DateTimeExtensions
{
    public static int CalculateAge(this DateOnly date)
    {
        var today = DateOnly.FromDateTime(DateTime.Now);

        var age = today.Year - date.Year;

        if (date > today.AddYears(-age)) age--;

        return age;
    }
}
