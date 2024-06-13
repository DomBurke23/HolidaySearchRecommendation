using System.Data.Common;

namespace TFP.HolidaySearchRecommendation.Infrastructure.Context
{
    public class DbContext
    {
        public DbConnection Connection { get; set; }
        public DbTransaction Transaction { get; set; }
    }
}
