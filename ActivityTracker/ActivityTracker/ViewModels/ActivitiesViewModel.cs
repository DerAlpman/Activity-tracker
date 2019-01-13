﻿using System;
using System.Collections.ObjectModel;
using ActivityTracker.Models;
using Components.ActivityTracker.Interfaces;

namespace ActivityTracker.ViewModels
{
    internal class ActivitiesViewModel
    {
        #region FIELDS

        private ObservableCollection<IActivityModel> _Activities;

        #endregion

        public ActivitiesViewModel()
        {
            _Activities = new ObservableCollection<IActivityModel>();
            LoadActivities();
        }

        #region PROPERTIES

        public ObservableCollection<IActivityModel> Activities { get => _Activities; set => _Activities = value; }

        #endregion

        #region METHODS

        private void LoadActivities()
        {
            _Activities.Add(new ActivityModel(new DateTime(2019, 1, 22, 8, 0, 0), "Issue 5461"));
            _Activities.Add(new ActivityModel(new DateTime(2019, 1, 22, 8, 20, 0), "Issue 5461"));
            _Activities.Add(new ActivityModel(new DateTime(2019, 1, 22, 8, 40, 0), "Issue 5461"));
            _Activities.Add(new ActivityModel(new DateTime(2019, 1, 22, 9, 00, 0), "Sprintretrospektive"));
        }

        #endregion
    }
}
