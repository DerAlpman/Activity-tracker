using System;
using Components.ActivityTracker.Interfaces;

namespace ActivityTracker.Models
{
    /// <summary>
    /// <see cref="IActivityModel"/>
    /// </summary>
    internal class ActivityModel : IActivityModel
    {
        #region FIELDS

        private readonly Guid _ID;
        private readonly DateTime _TimeStamp;
        private readonly string _Text;
        private TimeSpan _TimeSpan;

        #endregion

        /// <summary>
        /// <para>Initializes a new instance of the <see cref="ActivityModel"/> class.</para>
        /// </summary>
        internal ActivityModel(DateTime timeStamp, string text, TimeSpan timeSpan)
        {
            this._ID = Guid.NewGuid();
            this._TimeStamp = timeStamp;
            this._Text = text;
            this._TimeSpan = timeSpan;
        }

        #region PROPERTIES
        /// <summary>
        /// <see cref="IActivityModel.ID"/>
        /// </summary>
        public Guid ID { get => _ID; }

        /// <summary>
        /// <see cref="IActivityModel.Text"/>
        /// </summary>
        public string Text { get => _Text; }

        /// <summary>
        /// <see cref="IActivityModel.Start"/>
        /// </summary>
        public DateTime Start { get => _TimeStamp; }

        /// <summary>
        /// <see cref="IActivityModel.Duration"/>
        /// </summary>
        public TimeSpan Duration { get => _TimeSpan; }

        #endregion
    }
}
