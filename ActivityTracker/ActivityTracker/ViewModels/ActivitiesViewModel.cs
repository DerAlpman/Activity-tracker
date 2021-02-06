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
using Components.ActivityTracker;
using Prism.Commands;
using Prism.Mvvm;

namespace ActivityTracker.ViewModels
{
    internal class ActivitiesViewModel : BindableBase, IDataErrorInfo
    {
        #region FIELDS

        private readonly ObservableCollection<IActivityModel> _Activities;
        private string _Text;
        private ActivityModel _SelectedRecord;
        private string _DocumentedHours;

        private Dictionary<string, string> Errors { get; } = new Dictionary<string, string>();
        private static List<PropertyInfo> _PropertyInfos;
        private DateTime _TimeStamp;

        #endregion

        #region COMMANDS

        /// <summary>
        /// <para>A <see cref="DelegateCommand"/> to add an activity.</para>
        /// </summary>
        public DelegateCommand AddActivity { get; set; }

        /// <summary>
        /// <para>A <see cref="DelegateCommand"/> to add a break.</para>
        /// </summary>
        public DelegateCommand AddBreak { get; set; }

        /// <summary>
        /// <para>A <see cref="DelegateCommand"/> to delete an activity.</para>
        /// </summary>
        public DelegateCommand DeleteActivity { get; set; }

        /// <summary>
        /// <para>A <see cref="DelegateCommand"/> to save all activities.</para>
        /// </summary>
        public DelegateCommand SaveActivities { get; set; }

        #endregion

        #region CONSTRUCTOR

        /// <summary>
        /// <para>Initializes a new instance of the <see cref="ActivitiesViewModel"/> class.</para>
        /// </summary>
        public ActivitiesViewModel()
        {
            _Activities = new ObservableCollection<IActivityModel>();

            AddActivity = new DelegateCommand(ExecuteAddActivity, CanExecuteAddActivity);
            AddBreak = new DelegateCommand(ExecuteAddBreak, CanExecuteAddBreak);
            DeleteActivity = new DelegateCommand(ExecuteDeleteActivity, CanExecuteDeleteActivity);
            SaveActivities = new DelegateCommand(ExecuteSaveActivities, CanExecuteSaveActivities);

            DocumentedHours = "0";

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
            _Activities.Add(new ActivityModel(new DateTime(2019, 4, 21, 9, 0, 0), "PAUSE", new TimeSpan(0, 20, 0), ActivityType.BREAK));
            _Activities.Add(new ActivityModel(new DateTime(2019, 4, 21, 9, 20, 0), "111", new TimeSpan(0, 20, 0)));
            _Activities.Add(new ActivityModel(new DateTime(2019, 4, 21, 9, 40, 0), "111", new TimeSpan(0, 20, 0)));
            _Activities.Add(new ActivityModel(new DateTime(2019, 4, 21, 10, 0, 0), "541", new TimeSpan(0, 20, 0)));
            _Activities.Add(new ActivityModel(new DateTime(2019, 4, 21, 10, 20, 0), "134", new TimeSpan(0, 20, 0)));
            _Activities.Add(new ActivityModel(new DateTime(2019, 4, 21, 10, 40, 0), "134", new TimeSpan(0, 20, 0)));
            _Activities.Add(new ActivityModel(new DateTime(2019, 4, 21, 11, 20, 0), "111", new TimeSpan(0, 40, 0)));
            _Activities.Add(new ActivityModel(new DateTime(2019, 4, 21, 12, 0, 0), "PAUSE", new TimeSpan(0, 40, 0), ActivityType.BREAK));
            _Activities.Add(new ActivityModel(new DateTime(2019, 4, 21, 12, 20, 0), "134", new TimeSpan(0, 20, 0)));
            _Activities.Add(new ActivityModel(new DateTime(2019, 4, 21, 12, 40, 0), "134", new TimeSpan(0, 20, 0)));

            DocumentedHours = Activities
                .Where(a => a.Type != ActivityType.BREAK)
                .Sum(a => a.Duration.TotalHours)
                .ToString("F1");
        }

        #endregion

        #region COMMANDS

        #region DELETE

        private bool CanExecuteDeleteActivity()
        {
            return SelectedRecord != null;
        }

        private void ExecuteDeleteActivity()
        {
            Activities.Remove(SelectedRecord);
        }

        #endregion

        #region SAVE

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
            return !Errors.Any(e => e.Key.Equals(nameof(Activities)));
        }

        #endregion

        #region ADD

        private void ExecuteAddActivity()
        {
            TimeSpan duration = GetDurationAndUpdateTimeStamp();

            Activities.Add(new ActivityModel(_TimeStamp, this.Text, duration));
            DocumentedHours = Activities
                .Where(a => a.Type != ActivityType.BREAK)
                .Sum(a => a.Duration.TotalHours)
                .ToString("F1");

            UpdateSaveActivitiesCommand();
        }

        private bool CanExecuteAddActivity()
        {
            return !Errors.Any(e => e.Key.Equals(nameof(Text)));
        }

        private void ExecuteAddBreak()
        {
            TimeSpan duration = GetDurationAndUpdateTimeStamp();

            Activities.Add(new ActivityModel(_TimeStamp, "PAUSE", duration, ActivityType.BREAK));

            UpdateSaveActivitiesCommand();
        }

        private bool CanExecuteAddBreak()
        {
            return true;
        }

        #endregion

        #endregion

        #region METHODS

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

        #endregion

        #region PROPERTIES

        /// <summary>
        /// <para>This property contains all activities added by the user.</para>
        /// </summary>
        [EnsureMinimumElements(1, ErrorMessage = "Es muss mindestens eine Aktivität in der Liste vorhanden sein.")]
        public ObservableCollection<IActivityModel> Activities { get => _Activities; }

        /// <summary>
        /// <see cref="IActivityModel.Text"/>
        /// </summary>
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

        /// <summary>
        /// <para>This property contains the sum of the duration of all captured activities.</para>
        /// </summary>
        public string DocumentedHours
        {
            get => _DocumentedHours;

            set
            {
                if (_DocumentedHours != value)
                {
                    _DocumentedHours = value;
                    RaisePropertyChanged(nameof(DocumentedHours));
                }
            }
        }

        /// <summary>
        /// <para>This property holds the currently in the data grid selected activity.</para>
        /// </summary>
        public ActivityModel SelectedRecord
        {
            get => _SelectedRecord;
            set
            {
                _SelectedRecord = value;
                DeleteActivity.RaiseCanExecuteChanged();
            }
        }

        /// <summary>
        /// <para>The property determine all <see cref="PropertyInfo"/> of properties with certain <see cref="ValidationAttribute"/>.</para>
        /// </summary>
        internal List<PropertyInfo> PropertyInfos
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

        /// <summary>
        /// <see cref="IDataErrorInfo.Error"/>
        /// </summary>
        public string Error => string.Empty;

        /// <summary>
        /// <see cref="IDataErrorInfo"/>
        /// <para>Whenever a property of an instance of <see cref="ActivitiesViewModel"/> is changed, possible errors are collected.</para>
        /// </summary>
        /// <param name="propertyName">The name of a property of this class.</param>
        /// <returns>Message stored for property <paramref name="propertyName"/>. <see cref="string.Empty"/> if no error is found.</returns>
        public string this[string propertyName]
        {
            get
            {
                CollectErrors();
                return Errors.ContainsKey(propertyName) ? Errors[propertyName] : string.Empty;
            }
        }

        /// <summary>
        /// <para>Every property is asked for their <see cref="ValidationAttribute"/>.</para>
        /// <para>If the requirement is met, the attributes <see cref="ValidationAttribute.ErrorMessage"/> is stored.</para>
        /// </summary>
        private void CollectErrors()
        {
            Errors.Clear();

            PropertyInfos.ForEach(
                prop =>
                {
                    var currentValue = prop.GetValue(this);
                    var reqAttr = prop.GetCustomAttribute<RequiredAttribute>();
                    var maxLenAttr = prop.GetCustomAttribute<MaxLengthAttribute>();
                    var ensureMinEleAttr = prop.GetCustomAttribute<EnsureMinimumElementsAttribute>();

                    if (reqAttr != null
                    && string.IsNullOrWhiteSpace(currentValue?.ToString() ?? string.Empty))
                    {
                        Errors.Add(prop.Name, reqAttr.ErrorMessage);
                    }

                    if (maxLenAttr != null
                    && (currentValue?.ToString() ?? string.Empty).Length > maxLenAttr.Length)
                    {
                        Errors.Add(prop.Name, maxLenAttr.ErrorMessage);
                    }

                    if (ensureMinEleAttr != null
                    && currentValue != null
                    && currentValue is IList list
                    && list.Count < ensureMinEleAttr.MinElements)
                    {
                        Errors.Add(prop.Name, ensureMinEleAttr.ErrorMessage);
                    }
                });
        }

        #endregion

    }
}
