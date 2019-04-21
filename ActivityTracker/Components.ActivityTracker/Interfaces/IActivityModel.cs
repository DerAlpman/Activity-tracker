using System;

namespace Components.ActivityTracker.Interfaces
{
    public interface IActivityModel
    {
        string Text { get; }
        DateTime TimeStamp { get; }
        TimeSpan TimeSpan { get; }
    }
}