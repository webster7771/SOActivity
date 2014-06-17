using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace Ccai.NewRoam.SOActivity.Domain
{
    /// <summary>
    /// Compares SOActivityEvent for indexing purpose.
    /// </summary>
    public sealed class SOActivityComparer : IComparer<SOActivityEvent>
    {
        #region IComparer<SOActivityEvent> Members

        /// <summary>
        /// Compares two SOActivityEvent by it's Timestamp in ascending mode.
        /// </summary>
        public int Compare(SOActivityEvent x, SOActivityEvent y)
        {
            return x.Timestamp.CompareTo(y.Timestamp);
        }

        #endregion
    }
}