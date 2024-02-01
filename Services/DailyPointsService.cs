using WalletApp.Backend.Services.Interfaces;

namespace WalletApp.Backend.Services;

public class DailyPointsService : IDailyPointsService
{
    private int _winter;
    private const int Spring = 92;
    private const int Summer = 92;
    private const int Autumn = 91;
    private const int December = 31;

    public long GetPointOfThisDate(DateTime dateTime)
    {
        _winter = dateTime.Year / 4 == 0 ? 60 : 59;
        var dateOfYear = dateTime.DayOfYear;
        if (dateOfYear <= _winter) return CalculatePoint(dateOfYear + December);

        dateOfYear -= _winter;
        if (dateOfYear <= Spring) return CalculatePoint(dateOfYear);

        dateOfYear -= Spring;
        if (dateOfYear <= Summer) return CalculatePoint(dateOfYear);

        dateOfYear -= Summer;
        if (dateOfYear <= Autumn) return CalculatePoint(dateOfYear);

        dateOfYear -= Autumn;
        return CalculatePoint(dateOfYear);
    }

    public string PointToString(long points)
    {
        var pointsToDouble = (double)points;
        switch (pointsToDouble)
        {
            case < 1000:
                return points.ToString();
            case < 1000000:
                var roundedPointsK = Math.Round(pointsToDouble / 1000) * 1000;
                return (roundedPointsK / 1000.0).ToString("0") + "K";
            case < 1000000000:
                var roundedPointsM = Math.Round(pointsToDouble / 1000000) * 1000000;
                return (roundedPointsM / 1000000.0).ToString("0") + "M";
                
            default:
                var roundedPointsB = Math.Round(pointsToDouble / 1000000000) * 1000000000;
                return (points / 1000000000.0).ToString("0") + "B";
        }
    }

    private static long CalculatePoint(int daysOfMonth)
    {
        switch (daysOfMonth)
        {
            case 1:
                return 2;
            case 2:
                return 3;
            default:
                long first = 2;
                long second = 3;
                long buffer = 0;
                for (var i = 0; i < daysOfMonth-2; i++)
                {
                    buffer = first + second * 60 / 100;
                    first = second;
                    second = buffer;
                }

                return second;
        }
    }
    
}