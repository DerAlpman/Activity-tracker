using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StefanAlpers.ActivityTracker.CrossCutting.DataClasses
{
    /// <summary>
    /// <para>Describes the type of an <see cref="Activity"/>.</para>
    /// </summary>
    public enum ActivityType
    {
        /// <summary>
        /// <para>A break is an <see cref="Activity"/> where work is done.</para>
        /// </summary>
        WORK,
        /// <summary>
        /// <para>A break is an <see cref="Activity"/> where no work is done.</para>
        /// </summary>
        BREAK
    }
}
