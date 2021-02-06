using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Components.ActivityTracker
{
    /// <summary>
    /// <para>Describes the type of an <see cref="IActivityModel"/>.</para>
    /// </summary>
    public enum ActivityType
    {
        /// <summary>
        /// <para>A break is an <see cref="IActivityModel"/> where work is done.</para>
        /// </summary>
        WORK,
        /// <summary>
        /// <para>A break is an <see cref="IActivityModel"/> where no work is done.</para>
        /// </summary>
        BREAK
    }
}
