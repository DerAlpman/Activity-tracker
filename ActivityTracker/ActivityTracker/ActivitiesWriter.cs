using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Components.ActivityTracker.Interfaces;

namespace ActivityTracker
{
    internal static class ActivitiesWriter
    {
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
            string fileName = Regex.Replace(DateTime.Now.ToString(), @"[\.:\s]", "");

            return Path.Combine(filePath, fileName + ".activities");
        }

        #endregion
    }
}
