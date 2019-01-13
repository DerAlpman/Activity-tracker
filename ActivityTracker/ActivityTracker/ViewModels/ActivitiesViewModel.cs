using System.Collections.ObjectModel;
using Components.ActivityTracker.Interfaces;

namespace ActivityTracker.ViewModels
{
    internal class ActivitiesViewModel
    {
        #region FIELDS

        private ObservableCollection<IActivityModel> _Activities;

        #endregion

        #region PROPERTIES

        public ObservableCollection<IActivityModel> Activities { get => _Activities; set => _Activities = value; }

        #endregion
    }
}
