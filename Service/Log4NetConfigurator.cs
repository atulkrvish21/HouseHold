namespace HHSurvey.Service
{
    using log4net;
    using log4net.Appender;
    using System.Linq;

    public static class Log4NetConfigurator
    {
        public static void SetConnectionString(string connectionString)
        {
            var appender = LogManager.GetRepository()
                .GetAppenders()
                .OfType<AdoNetAppender>()
                .FirstOrDefault();

            if (appender != null)
            {
                appender.ConnectionString = connectionString;
                appender.ActivateOptions(); // Apply changes
            }
        }
    }

}
