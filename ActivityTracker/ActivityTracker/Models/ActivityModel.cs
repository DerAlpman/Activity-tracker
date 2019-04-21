using System;
using Components.ActivityTracker.Interfaces;

namespace ActivityTracker.Models
{
    internal class ActivityModel : IActivityModel
    {
        #region FIELDS

        private DateTime _TimeStamp;
        private string _Text;

        #endregion

        public ActivityModel(DateTime timeStamp, string text)
        {
            this._TimeStamp = timeStamp;
            this._Text = text;
        }

        #region PROPERTIES

        public string Text { get => _Text; set => _Text = value; }
        public DateTime TimeStamp { get => _TimeStamp; set => _TimeStamp = value; }

        #endregion
    }
}
