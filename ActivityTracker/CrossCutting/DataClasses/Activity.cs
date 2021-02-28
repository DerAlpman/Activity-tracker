using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StefanAlpers.ActivityTracker.CrossCutting.DataClasses
{
    /// <summary>
    /// <para>Provides properties for an activity that can be bound to a view model.</para>
    /// </summary>
    public class Activity
    {
        #region CONSTRUCTORS
        /// <summary>
        /// <para>Initializes a new instance of the <see cref="Activity"/> class.</para>
        /// </summary>
        public Activity(DateTime timeStamp, string text, TimeSpan duration, ActivityType type)
        {
            this.ID = Guid.NewGuid();
            this.Start = timeStamp;
            this.Text = text;
            this.Duration = duration;
            this.Type = type;
        }


        /// <summary>
        /// <para>Initializes a new instance of the <see cref="Activity"/> class.</para>
        /// </summary>
        public Activity(DateTime timeStamp, string text, TimeSpan duration)
        {
            this.ID = Guid.NewGuid();
            this.Start = timeStamp;
            this.Text = text;
            this.Duration = duration;
            this.Type = ActivityType.WORK;
        }

        #endregion

        #region PROPERTIES
        /// <summary>
        /// <para>The unique identifier of an activity</para>
        /// </summary>
        public Guid ID { get; }

        /// <summary>
        /// <para>The content of the activity, for example what was done.</para>
        /// </summary>
        public string Text { get; }

        /// <summary>
        /// <para>The start of the activity.</para>
        /// </summary>
        public DateTime Start { get; }

        /// <summary>
        /// <para>The duration of the activity.</para>
        /// </summary>
        public TimeSpan Duration { get; }

        /// <summary>
        /// <para>The <see cref="ActivityType"/> of this activity.</para>
        /// </summary>
        public ActivityType Type { get; }
        #endregion
    }
}
