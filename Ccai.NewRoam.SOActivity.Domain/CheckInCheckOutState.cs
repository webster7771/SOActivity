using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Ccai.NewRoam.SOActivity.Domain.Attributes;

namespace Ccai.NewRoam.SOActivity.Domain
{
    /// <summary>
    /// This is the state of Sales Office (SO) checkin/checkout activity eventC:\Users\Oetawan\SkyDrive\Documents\CCAI\Ccai.NewRoam.SOActivity\Ccai.NewRoam.SOActivity.Domain\Exception\ that occured when SO perfom check in or check out activity.
    /// </summary>
    public enum CheckInCheckOutState
    {
        /// <summary>
        /// This state indicate that SO has checked in for the firt time in the morning below 12:00PM.
        /// </summary>
        FirstCheckInMorning,
        
        /// <summary>
        /// This state indicate that SO has checked out from his/her first check in but the check out date is below 12:00PM.
        /// </summary>
        FirstCheckOutMorning,

        /// <summary>
        /// This state indicate that SO has checked in more than one in the morning below 12:00PM.
        /// </summary>
        CheckInMorning,

        /// <summary>
        /// This state indicate that SO has checked out more than one in the same day and check out date is below 12:00PM.
        /// </summary>
        CheckOutMorning,

        /// <summary>
        /// This state indicate that SO has checked in above 12:00PM.
        /// </summary>
        CheckInAfternoon,

        /// <summary>
        /// This state indicate that SO has checked out from the last check in above 12:00PM and the check out date above 12:00PM.
        /// </summary>
        CheckOutAfternoon
    }
}