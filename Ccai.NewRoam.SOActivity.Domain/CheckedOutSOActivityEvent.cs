using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Ccai.NewRoam.SOActivity.Domain.Attributes;

namespace Ccai.NewRoam.SOActivity.Domain
{
    /// <summary>
    /// Represent an event that raised by SOActivitySession when SO has checked out.
    /// </summary>
    public sealed class CheckedOutSOActivityEvent : SOActivityEvent
    {
        /// <summary>
        /// The state of the check out activity.
        /// </summary>
        [FieldEnum(typeof(CheckInCheckOutState), "CheckInCheckOutState")]
        public CheckInCheckOutState CheckOutState { get; set; }

        /// <summary>
        /// Type of the SO Activity. There are two types of SO activity: Check In and Check Out.
        /// </summary>
        [FieldEnum(typeof(SOActivityType), "ActivityType")]
        public override SOActivityType SOActivityType
        {
            get
            {
                return SOActivityType.CheckOut;
            }
        }
    }
}