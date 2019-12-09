using System;

namespace Components.ActivityTracker.Interfaces
{
    /// <summary>
    /// <para>Provides properties for an activity that can be bound to a view model.</para>
    /// </summary>
    public interface IActivityModel
    {
        /// <summary>
        /// <para>The unique identifier of an activity</para>
        /// </summary>
        Guid ID { get; }

        /// <summary>
        /// <para>The content of the activity, for example what was done.</para>
        /// </summary>
        string Text { get; }

        /// <summary>
        /// <para>The start of the activity.</para>
        /// </summary>
        DateTime Start { get; }

        /// <summary>
        /// <para>The duration of the activity.</para>
        /// </summary>
        TimeSpan Duration { get; }
    }
}