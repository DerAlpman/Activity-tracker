using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using Components.ActivityTracker.Interfaces;
using Library.ActivityTracker.Models;
using Library.ActivityTracker.Validation;
using Prism.Commands;
using Prism.Mvvm;

namespace Library.ActivityTracker.ViewModels
{
    public class ActivitiesViewModel : BindableBase, IDataErrorInfo
    {
        #region FIELDS

        private ObservableCollection<IActivityModel> _Activities; private string _Text;
        private Dictionary<string, string> _Errors { get; } = new Dictionary<string, string>();
        private static List<PropertyInfo> _PropertyInfos;

        public DelegateCommand AddActivity { get; set; }

        #endregion

        public ActivitiesViewModel()
        {
            _Activities = new ObservableCollection<IActivityModel>();

            AddActivity = new DelegateCommand(ExecuteAddActivity, CanExecuteAddActivity);

            //LoadActivities();
        }

        private void ExecuteAddActivity()
        {
            Activities.Add(new ActivityModel(DateTime.Now, this.Text));
        }

        private bool CanExecuteAddActivity()
        {
            return !_Errors.Any();
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

        #region METHODS

        private void LoadActivities()
        {
            _Activities.Add(new ActivityModel(new DateTime(2019, 1, 22, 8, 0, 0), "Issue 5461"));
            _Activities.Add(new ActivityModel(new DateTime(2019, 1, 22, 8, 20, 0), "Issue 5461"));
            _Activities.Add(new ActivityModel(new DateTime(2019, 1, 22, 8, 40, 0), "Issue 5461"));
            _Activities.Add(new ActivityModel(new DateTime(2019, 1, 22, 9, 00, 0), "Sprintretrospektive"));
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
                    var ensureMinLenAttr = prop.GetCustomAttribute<EnsureMinimumElementsAttribute>();

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

                    if (ensureMinLenAttr != null && currentValue != null)
                    {
                        var list = currentValue as IList;

                        if (list != null)
                        {
                            if (list.Count < ensureMinLenAttr.MinElements)
                            {
                                _Errors.Add(prop.Name, ensureMinLenAttr.ErrorMessage);
                            }
                        }
                    }
                });
        }

        #endregion

    }
}
