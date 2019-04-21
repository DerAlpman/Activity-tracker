using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using Components.ActivityTracker.Interfaces;

namespace ActivityTracker
{
    internal static class ActivitiesWriter
    {
        #region METHODS

        internal static bool TryWriteActivitiesToFile(IEnumerable<IActivityModel> activities)
        {
            if (!AppSettings.TryGetSetting(AppSettings.OUTPUT_PATH_KEY, out string filePath))
            {
                return false;
            }

            if (!Directory.Exists(filePath))
            {
                return false;
            }

            string fileName = GetFileName();

            using (StreamWriter sw = new StreamWriter(Path.Combine(filePath, fileName + ".activities")))
            {
                foreach (var item in activities)
                {
                    sw.WriteLine(String.Format("{0}: {1}", item.TimeStamp.ToString(CultureInfo.CurrentCulture), item.Text));
                }
            }

            return true;
        }

        private static string GetFileName()
        {
            return Regex.Replace(DateTime.Now.ToString(), @"[\.:\s]", "");
        }

        #endregion
    }
}
