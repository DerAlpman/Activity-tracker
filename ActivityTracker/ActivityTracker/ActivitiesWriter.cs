using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Components.ActivityTracker;

namespace ActivityTracker
{
    /// <summary>
    /// <para>This class writes activies to a file. The absolute path to the file is configured in App.config under the key <see cref="AppSettings.OUTPUT_PATH_KEY"/>.</para>
    /// </summary>
    internal static class ActivitiesWriter
    {
        #region CONSTS

        /// <summary>
        /// <para>This pattern finds all occurences of '.', ':' or any whitespace character.</para>
        /// </summary>
        private const string RegexPattern = @"[\.:\s]";

        #endregion

        #region METHODS
        internal static bool TryWriteActivitiesToFile(IList<IActivityModel> activities)
        {
            if (!AppSettings.TryGetSetting(AppSettings.OUTPUT_PATH_KEY, out string filePath))
            {
                return false;
            }

            if (!Directory.Exists(filePath))
            {
                return false;
            }

            string fileName = GetAbsoluteFileName(filePath);

            WriteActivities(activities, fileName);

            return true;
        }

        private static void WriteActivities(IList<IActivityModel> activities, string output)
        {
            using (StreamWriter sw = new StreamWriter(output))
            {
                sw.WriteLine("Chronologie:");
                sw.WriteLine("------------");
                for (int i = 0; i < activities.Count; i++)
                {
                    sw.WriteLine(String.Format("{0}: {1} ({2} Minuten)",
                        activities[i].Start.ToString(CultureInfo.CurrentCulture),
                        activities[i].Text,
                        activities[i].Duration.TotalMinutes.ToString("F0")));
                }

                sw.WriteLine();
                sw.WriteLine();

                sw.WriteLine("Gruppiert:");
                sw.WriteLine("----------");
                foreach (var group in activities.GroupBy(a => a.Text))
                {
                    var groupDuration = group.Sum(am => am.Duration.TotalHours);
                    sw.WriteLine(String.Format("{0}: {1} Stunden", group.Key, groupDuration.ToString("F1")));
                }

                sw.WriteLine();
                sw.WriteLine();

                sw.WriteLine(String.Format("Gesamtzeit: {0}", activities.Sum(a => a.Duration.TotalHours).ToString("F1")));
            }
        }

        private static string GetAbsoluteFileName(string filePath)
        {
            string fileName = Regex.Replace(DateTime.Now.ToString(), RegexPattern, "");

            return Path.Combine(filePath, fileName + ".activities");
        }
        #endregion
    }
}
