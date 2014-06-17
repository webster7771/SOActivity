using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace Ccai.NewRoam.SOActivity.Domain
{
    /// <summary>
    /// Represent type of SO Activity.
    /// </summary>
    public enum SOActivityType
    {
        /// <summary>
        /// This is the type of check in activity.
        /// </summary>
        CheckIn = 0,

        /// <summary>
        /// This is the type of check out activity.
        /// </summary>
        CheckOut = 1
    }
}