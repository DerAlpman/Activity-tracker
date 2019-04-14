using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace Library.ActivityTracker.ViewModels
{
#warning Not used, yet
    public class ActivityInputControlsViewModel : IDataErrorInfo
    {
        #region FIELDS

        private string _Text;
        private Dictionary<string, string> _Errors { get; } = new Dictionary<string, string>();
        private static List<PropertyInfo> _PropertyInfos;

        #endregion

        #region PROPERTIES

        [Required(AllowEmptyStrings = false, ErrorMessage = "Es ist keine Aktivität eingetragen.")]
        [MaxLength(40, ErrorMessage = "Es sind maximal 40 Zeichen möglich.")]
        public string Text { get => _Text; set => _Text = value; }

        protected List<PropertyInfo> PropertyInfos
        {
            get
            {
                if (_PropertyInfos == null)
                {
                    _PropertyInfos = GetType()
                        .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                        .Where(prop => prop.IsDefined(typeof(RequiredAttribute), true) || prop.IsDefined(typeof(MaxLengthAttribute), true))
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
                });
        }

        #endregion
    }
}
