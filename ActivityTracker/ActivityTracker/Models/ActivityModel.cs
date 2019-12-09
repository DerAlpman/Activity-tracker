using System;
using Components.ActivityTracker.Interfaces;

namespace ActivityTracker.Models
{
    public class ActivityModel : IActivityModel
    {
        #region FIELDS

        private readonly Guid _ID;
        private readonly DateTime _TimeStamp;
        private readonly string _Text;
        private TimeSpan _TimeSpan;

        #endregion

        public ActivityModel(DateTime timeStamp, string text, TimeSpan timeSpan)
        {
            this._ID = Guid.NewGuid();
            this._TimeStamp = timeStamp;
            this._Text = text;
            this._TimeSpan = timeSpan;
        }

        #region PROPERTIES

        public Guid ID { get => _ID; }
        public string Text { get => _Text; }
        public DateTime TimeStamp { get => _TimeStamp; }
        public TimeSpan TimeSpan { get => _TimeSpan; }

        #endregion
    }
}
