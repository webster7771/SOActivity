using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Ccai.NewRoam.SOActivity.Domain.Attributes;

namespace Ccai.NewRoam.SOActivity.Domain
{
    /// <summary>
    /// This class represents an event that raised by SOActivitySession when SO perform check in or check out activity.
    /// This is the base class for all events in SO Activity.
    /// </summary>
    [Table("SOActivity")]
    public abstract class SOActivityEvent
    {
        /// <summary>
        /// Represend the event ID and must be unique. This ID should be mapped in database field as primary key.
        /// </summary>
        [Field("Id")]
        public string ID { get; set; }

        /// <summary>
        /// The type of SO Activity. There are three types: None, Check In and Check Out.
        /// </summary>
        public abstract SOActivityType SOActivityType { get; }

        /// <summary>
        /// Represent the session date where the event is occured in.
        /// </summary>
        [Field("Date")]
        public DateTime SessionDate { get; set; }

        /// <summary>
        /// Represent SO Number scanned from SO id barcode.
        /// </summary>
        [Field("SONumber")]
        public string SONumber { get; set; }

        /// <summary>
        /// The time when the event is occured.
        /// </summary>
        [Field("Timestamp")]
        public DateTime Timestamp { get; set; }
    }
}