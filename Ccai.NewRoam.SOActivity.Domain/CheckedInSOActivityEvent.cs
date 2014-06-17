using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Ccai.NewRoam.SOActivity.Domain.Attributes;

namespace Ccai.NewRoam.SOActivity.Domain
{
    /// <summary>
    /// Represent an event that raised by SOActivitySession when SO has checked in.
    /// </summary>
    public sealed class CheckedInSOActivityEvent : SOActivityEvent
    {
        /// <summary>
        /// The state of the check in activity.
        /// </summary>
        [FieldEnum(typeof(CheckInCheckOutState), "CheckInCheckOutState")]
        public CheckInCheckOutState CheckInState { get; set; }
        
        /// <summary>
        /// Type of the SO Activity type. There are two types of SO activity: Check In and Check Out.
        /// </summary>
        [FieldEnum(typeof(SOActivityType), "ActivityType")]
        public override SOActivityType SOActivityType
        {
            get
            {
                return SOActivityType.CheckIn;
            }
        }
    }
}