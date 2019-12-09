﻿using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace ActivityTracker.Validation
{
    /// <summary>
    /// <para>This attribute can be used to validate if an <see cref="ICollection"/> has a minimum number of elements. </para>
    /// </summary>
    internal class EnsureMinimumElementsAttribute : ValidationAttribute
    {
        #region FIELDS

        private readonly int _MinElements;

        #endregion

        #region CONSTRUCTOR

        /// <summary>
        /// <para>Initializes a new instance of the <see cref="EnsureMinimumElementsAttribute"/> class.</para>
        /// </summary>
        internal EnsureMinimumElementsAttribute(int minElements)
        {
            this._MinElements = minElements;
        }

        #endregion

        #region PROPERTIES

        /// <summary>
        /// <para>The minimum number the <see cref="ICollection"/> must have.</para>
        /// </summary>
        public int MinElements => _MinElements;

        #endregion

        #region ValidationAttribute

        /// <summary>
        /// <para></para>
        /// </summary>
        /// <param name="value">The <see cref="ICollection"/> that is validated.</param>
        /// <returns>True, if <paramref name="value"/> contains more or equal values than required by <see cref="MinElements"/>.</returns>
        public override bool IsValid(object value)
        {
            var list = value as ICollection;

            if (list != null)
            {
                return list.Count >= _MinElements;
            }

            return false;
        }

        #endregion
    }
}
