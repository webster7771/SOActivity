using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ccai.NewRoam.SOActivity.Domain;

namespace Ccai.NewRoam.SOActivity.Service
{
    /// <summary>
    /// Interface for UI and other service or system as a gateway that provides main functionality of SO Activity feature. 
    /// </summary>
    public interface ISOActivityService
    {
        /// <summary>
        /// Get SOActivitySession by date.
        /// </summary>
        /// <param name="sessionDate">The session date when SO start the activity.</param>
        /// <returns></returns>
        SOActivitySession GetSession(DateTime sessionDate);

        /// <summary>
        /// Use this method for check in activity. 
        /// </summary>
        /// <param name="sessionDate">The session date when SO start the activity.</param>
        /// <param name="soNumber">Barcode that scanned from SO id that asssociated with each SO.</param>
        void CheckIn(DateTime sessionDate, string soNumber);

        /// <summary>
        /// Use this method for check out activity.
        /// </summary>
        /// <param name="sessionDate">The session date when SO start the activity.</param>
        void CheckOut(DateTime sessionDate);
    }
}