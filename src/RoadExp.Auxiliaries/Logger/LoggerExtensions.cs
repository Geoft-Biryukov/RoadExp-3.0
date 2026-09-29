using Microsoft.Extensions.Logging;

namespace RoadExp.Auxiliaries.Logger
{
    public static class LoggerExtensions
    {
        #region Common
        public static void Information(this ILogger logger, string message, string target)
        {
            using (var scope = logger.BeginScope(new Dictionary<string, object> { [target] = message }))
            {
                logger.LogInformation(message);
            }
        }

        public static void Warning(this ILogger logger, string message, string target)
        {
            using (var scope = logger.BeginScope(new Dictionary<string, object> { [target] = message }))
            {
                logger.LogWarning(message);
            }
        }

        public static void Error(this ILogger logger, string message, string target)
        {
            using (var scope = logger.BeginScope(new Dictionary<string, object> { [target] = message }))
            {
                logger.LogError(message);
            }
        }
        #endregion

        #region User
        public static void UserInformation(this ILogger logger, string message)
        {
            Information(logger, message, "ToUser");            
        }

        public static void UserWarning(this ILogger logger, string message)
        {
            Warning(logger, message, "ToUser");
        }

        public static void UserError(this ILogger logger, string message)
        {
            Error(logger, message, "ToUser");
        }
        #endregion

        #region Gps
        public static void GpsInformation(this ILogger logger, string message)
        {
            Information(logger, message, "ToGpsSystem");
        }

        public static void GpsWarning(this ILogger logger, string message)
        {
            Warning(logger, message, "ToGpsSystem");
        }

        public static void GpsError(this ILogger logger, string message)
        {
            Error(logger, message, "ToGpsSystem");
        }
        #endregion

        #region Geometry
        public static void GeometryInformation(this ILogger logger, string message)
        {
            Information(logger, message, "ToGeometrySystem");
        }

        public static void GeometryWarning(this ILogger logger, string message)
        {
            Warning(logger, message, "ToGeometrySystem");
        }

        public static void GeometryError(this ILogger logger, string message)
        {
            Error(logger, message, "ToGeometrySystem");
        }
        #endregion
    }
}
