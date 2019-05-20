using System;
using Components.ActivityTracker.Interfaces;

namespace ActivityTracker.Models
{
    internal class ActivityModel : IActivityModel
    {
        #region FIELDS

        private readonly DateTime _TimeStamp;
        private readonly string _Text;
        private TimeSpan _TimeSpan;

        #endregion

        public ActivityModel(DateTime timeStamp, string text, TimeSpan timeSpan)
        {
            this._TimeStamp = timeStamp;
            this._Text = text;
            this._TimeSpan = timeSpan;
        }

        #region PROPERTIES

        public string Text { get => _Text; }
        public DateTime TimeStamp { get => _TimeStamp; }
        public TimeSpan TimeSpan { get => _TimeSpan; }

        #endregion
    }
}
