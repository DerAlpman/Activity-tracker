using System;
using Components.ActivityTracker.Interfaces;

namespace ActivityTracker.Models
{
    /// <summary>
    /// <see cref="IActivityModel"/>
    /// </summary>
    internal class ActivityModel : IActivityModel
    {
        /// <summary>
        /// <para>Initializes a new instance of the <see cref="ActivityModel"/> class.</para>
        /// </summary>
        internal ActivityModel(DateTime timeStamp, string text, TimeSpan duration)
        {
            this.ID = Guid.NewGuid();
            this.Start = timeStamp;
            this.Text = text;
            this.Duration = duration;
        }

        #region PROPERTIES
        /// <summary>
        /// <see cref="IActivityModel.ID"/>
        /// </summary>
        public Guid ID { get; }

        /// <summary>
        /// <see cref="IActivityModel.Text"/>
        /// </summary>
        public string Text { get; }

        /// <summary>
        /// <see cref="IActivityModel.Start"/>
        /// </summary>
        public DateTime Start { get; }

        /// <summary>
        /// <see cref="IActivityModel.Duration"/>
        /// </summary>
        public TimeSpan Duration { get; }

        #endregion
    }
}
