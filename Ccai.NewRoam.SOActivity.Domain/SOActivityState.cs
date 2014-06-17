using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace Ccai.NewRoam.SOActivity.Domain
{
    /// <summary>
    /// This is the state of the SO activity. There are three kind of activity in SO Activity: None, Check In and Check Out.
    /// </summary>
    public enum SOActivityState
    {
        /// <summary>
        /// This state is initial state of SO activity that denote there isn't any activity performed.
        /// </summary>
        None = 0,

        /// <summary>
        /// This is the state when SO perform check in activity.
        /// </summary>
        CheckedIn = 1,

        /// <summary>
        /// This is the state when SO perform check out activity.
        /// </summary>
        CheckedOut = 2
    }
}