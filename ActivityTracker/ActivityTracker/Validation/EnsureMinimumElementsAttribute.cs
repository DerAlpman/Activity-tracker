using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace ActivityTracker.Validation
{
    internal class EnsureMinimumElementsAttribute : ValidationAttribute
    {
        #region FIELDS

        private readonly int _MinElements;

        #endregion

        #region CONSTRUCTOR

        internal EnsureMinimumElementsAttribute(int minElements)
        {
            this._MinElements = minElements;
        }

        #endregion

        #region PROPERTIES

        public int MinElements => _MinElements;

        #endregion

        #region ValidationAttribute

        public override bool IsValid(object value)
        {
            var list = value as IList;

            if (list != null)
            {
                return list.Count > _MinElements;
            }

            return false;
        }

        #endregion
    }
}
