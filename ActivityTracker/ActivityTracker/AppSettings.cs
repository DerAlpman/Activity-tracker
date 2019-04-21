using System.Configuration;

namespace ActivityTracker
{
    internal static class AppSettings
    {
        #region CONSTS

        internal const string OUTPUT_PATH_KEY = "outputPath";

        #endregion

        internal static bool TryGetSetting(string key, out string value)
        {
            value = ConfigurationManager.AppSettings.Get(key);

            return !string.IsNullOrWhiteSpace(value);
        }
    }
}
