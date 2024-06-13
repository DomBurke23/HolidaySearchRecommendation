namespace TFP.HolidaySearchRecommendation.Infrastructure.Data.Migrations
{
    public interface ISchemaMigration
    {
        Task ApplyAsync();
    }
}
