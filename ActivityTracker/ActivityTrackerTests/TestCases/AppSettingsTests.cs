using ActivityTracker;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ActivityTrackerTests.TestCases
{
    /// <summary>
    /// Summary description for AppSettings
    /// </summary>
    [TestClass]
    public class AppSettingsTests
    {
        [TestMethod]
        [DataRow(AppSettings.OUTPUT_PATH_KEY)]
        public void TryGetSetting_ExistingKey_ReturnsTrue(string key)
        {
            #region ARRANGE

            #endregion

            #region ACT

            bool result = AppSettings.TryGetSetting(key, out string value);

            #endregion

            #region ASSERT

            Assert.IsTrue(result);

            #endregion
        }
    }
}
