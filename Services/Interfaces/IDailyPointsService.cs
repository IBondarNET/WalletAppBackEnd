namespace WalletApp.Backend.Services.Interfaces;

public interface IDailyPointsService
{
    long GetPointOfThisDate(DateTime dateTime);
    string PointToString(long points);
}