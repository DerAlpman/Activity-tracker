using System;
using Components.ActivityTracker.Interfaces;

namespace ActivityTracker.Models
{
    internal class ActivityModel : IActivityModel
    {
        #region FIELDS

        private readonly int _OrderNumber;
        private readonly DateTime _TimeStamp;
        private readonly string _Text;
        private readonly TimeSpan _TimeSpan;

        #endregion

        public ActivityModel(int orderNumber, DateTime timeStamp, string text, TimeSpan timeSpan)
        {
            this._OrderNumber = orderNumber;
            this._TimeStamp = timeStamp;
            this._Text = text;
            this._TimeSpan = timeSpan;
        }

        #region PROPERTIES

        public int OrderNumber => _OrderNumber;
        public string Text => _Text;
        public DateTime TimeStamp => _TimeStamp;
        public TimeSpan TimeSpan => _TimeSpan;

        #endregion
    }
}
