using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using ActivityTracker.Models;
using ActivityTracker.Validation;
using Components.ActivityTracker.Interfaces;
using Prism.Commands;
using Prism.Mvvm;

namespace ActivityTracker.ViewModels
{
    public class ActivitiesViewModel : BindableBase, IDataErrorInfo
    {
        #region FIELDS

        private readonly ObservableCollection<IActivityModel> _Activities;
        private string _Text;
        private ActivityModel _SelectedRecord;

        private Dictionary<string, string> _Errors { get; } = new Dictionary<string, string>();
        private static List<PropertyInfo> _PropertyInfos;
        private DateTime _TimeStamp;

        #endregion

        #region COMMANDS

        public DelegateCommand AddActivity { get; set; }
        public DelegateCommand DeleteActivity { get; set; }
        public DelegateCommand SaveActivities { get; set; }

        #endregion

        #region CONSTRUCTOR

        public ActivitiesViewModel()
        {
            _Activities = new ObservableCollection<IActivityModel>();

            AddActivity = new DelegateCommand(ExecuteAddActivity, CanExecuteAddActivity);
            DeleteActivity = new DelegateCommand(ExecuteDeleteActivity, CanExecuteDeleteActivity);
            SaveActivities = new DelegateCommand(ExecuteSaveActivities, CanExecuteSaveActivities);

            _TimeStamp = DateTime.Now;

#if DEBUG
            LoadActivities();
#endif
        }

        private void LoadActivities()
        {
            _Activities.Add(new ActivityModel(new DateTime(2019, 4, 21, 8, 0, 0), "541", new TimeSpan(0, 20, 0)));
            _Activities.Add(new ActivityModel(new DateTime(2019, 4, 21, 8, 20, 0), "541", new TimeSpan(0, 20, 0)));
            _Activities.Add(new ActivityModel(new DateTime(2019, 4, 21, 8, 40, 0), "541", new TimeSpan(0, 20, 0)));
            _Activities.Add(new ActivityModel(new DateTime(2019, 4, 21, 9, 0, 0), "541", new TimeSpan(0, 20, 0)));
            _Activities.Add(new ActivityModel(new DateTime(2019, 4, 21, 9, 20, 0), "111", new TimeSpan(0, 20, 0)));
            _Activities.Add(new ActivityModel(new DateTime(2019, 4, 21, 9, 40, 0), "111", new TimeSpan(0, 20, 0)));
            _Activities.Add(new ActivityModel(new DateTime(2019, 4, 21, 10, 0, 0), "541", new TimeSpan(0, 20, 0)));
            _Activities.Add(new ActivityModel(new DateTime(2019, 4, 21, 10, 20, 0), "134", new TimeSpan(0, 20, 0)));
            _Activities.Add(new ActivityModel(new DateTime(2019, 4, 21, 10, 40, 0), "134", new TimeSpan(0, 20, 0)));
            _Activities.Add(new ActivityModel(new DateTime(2019, 4, 21, 11, 20, 0), "111", new TimeSpan(0, 40, 0)));
            _Activities.Add(new ActivityModel(new DateTime(2019, 4, 21, 12, 0, 0), "134", new TimeSpan(0, 40, 0)));
            _Activities.Add(new ActivityModel(new DateTime(2019, 4, 21, 12, 20, 0), "134", new TimeSpan(0, 20, 0)));
            _Activities.Add(new ActivityModel(new DateTime(2019, 4, 21, 12, 40, 0), "134", new TimeSpan(0, 20, 0)));
        }

        #endregion

        #region COMMANDS

        private bool CanExecuteDeleteActivity()
        {
            return SelectedRecord != null;
        }

        private void ExecuteDeleteActivity()
        {
            Activities.Remove(SelectedRecord);
        }

        private void ExecuteSaveActivities()
        {
            if (ActivitiesWriter.TryWriteActivitiesToFile(Activities))
            {
                Activities.Clear();
                UpdateSaveActivitiesCommand();
            }
        }

        private bool CanExecuteSaveActivities()
        {
            return !_Errors.Any(e => e.Key.Equals(nameof(Activities)));
        }

        private void ExecuteAddActivity()
        {
            TimeSpan duration = GetDurationAndUpdateTimeStamp();

            Activities.Add(new ActivityModel(_TimeStamp, this.Text, duration));

            UpdateSaveActivitiesCommand();
        }

        #endregion

        private TimeSpan GetDurationAndUpdateTimeStamp()
        {
            DateTime now = DateTime.Now;

            TimeSpan duration = now - _TimeStamp;

            _TimeStamp = now;

            return duration;
        }

        private void UpdateSaveActivitiesCommand()
        {
            CollectErrors();
            SaveActivities.RaiseCanExecuteChanged();
        }

        private bool CanExecuteAddActivity()
        {
            return !_Errors.Any(e => e.Key.Equals(nameof(Text)));
        }

        #region PROPERTIES

        [EnsureMinimumElements(1, ErrorMessage = "Es muss mindestens eine Aktivität in der Liste vorhanden sein.")]
        public ObservableCollection<IActivityModel> Activities { get => _Activities; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Es ist keine Aktivität eingetragen.")]
        [MaxLength(40, ErrorMessage = "Es sind maximal 40 Zeichen möglich.")]
        public string Text
        {
            get => _Text;
            set
            {
                _Text = value;
                AddActivity.RaiseCanExecuteChanged();
            }
        }

        public ActivityModel SelectedRecord
        {
            get => _SelectedRecord;
            set
            {
                _SelectedRecord = value;
                DeleteActivity.RaiseCanExecuteChanged();
            }
        }

        protected List<PropertyInfo> PropertyInfos
        {
            get
            {
                if (_PropertyInfos == null)
                {
                    _PropertyInfos = GetType()
                        .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                        .Where(prop => prop.IsDefined(typeof(RequiredAttribute), true)
                        || prop.IsDefined(typeof(MaxLengthAttribute), true)
                        || prop.IsDefined(typeof(EnsureMinimumElementsAttribute), true))
                        .ToList();
                }
                return _PropertyInfos;
            }
        }

        #endregion

        #region IDataErrorInfo

        public string Error => string.Empty;

        public string this[string columnName]
        {
            get
            {
                CollectErrors();
                return _Errors.ContainsKey(columnName) ? _Errors[columnName] : string.Empty;
            }
        }

        private void CollectErrors()
        {
            _Errors.Clear();

            PropertyInfos.ForEach(
                prop =>
                {
                    var currentValue = prop.GetValue(this);
                    var reqAttr = prop.GetCustomAttribute<RequiredAttribute>();
                    var maxLenAttr = prop.GetCustomAttribute<MaxLengthAttribute>();
                    var ensureMinEleAttr = prop.GetCustomAttribute<EnsureMinimumElementsAttribute>();

                    if (reqAttr != null)
                    {
                        if (string.IsNullOrWhiteSpace(currentValue?.ToString() ?? string.Empty))
                        {
                            _Errors.Add(prop.Name, reqAttr.ErrorMessage);
                        }
                    }

                    if (maxLenAttr != null)
                    {
                        if ((currentValue?.ToString() ?? string.Empty).Length > maxLenAttr.Length)
                        {
                            _Errors.Add(prop.Name, maxLenAttr.ErrorMessage);
                        }
                    }

                    if (ensureMinEleAttr != null && currentValue != null)
                    {
                        var list = currentValue as IList;

                        if (list != null)
                        {
                            if (list.Count < ensureMinEleAttr.MinElements)
                            {
                                _Errors.Add(prop.Name, ensureMinEleAttr.ErrorMessage);
                            }
                        }
                    }
                });
        }

        #endregion

    }
}
