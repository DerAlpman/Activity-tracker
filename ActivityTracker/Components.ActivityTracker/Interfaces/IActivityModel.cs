using System;

namespace Components.ActivityTracker.Interfaces
{
    public interface IActivityModel
    {
        string Text { get; set; }
        DateTime TimeStamp { get; set; }
    }
}