﻿using System;
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
                sw.Write("Chronologie:");
                sw.Write("------------");
                for (int i = 0; i < activities.Count; i++)
                {
                    sw.WriteLine(String.Format("{0}: {1} ({2} Minuten)",
                        activities[i].TimeStamp.ToString(CultureInfo.CurrentCulture),
                        activities[i].Text,
                        activities[i].TimeSpan.TotalMinutes.ToString("F0")));
                }

                sw.WriteLine();
                sw.WriteLine();

                sw.WriteLine("Gruppiert:");
                sw.WriteLine("----------");
                foreach (var group in activities.GroupBy(a => a.Text))
                {
                    var groupDuration = group.Sum(am => am.TimeSpan.TotalHours);
                    sw.WriteLine(String.Format("{0}: {1} Stunden", group.Key, groupDuration.ToString("F2")));
                }
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
