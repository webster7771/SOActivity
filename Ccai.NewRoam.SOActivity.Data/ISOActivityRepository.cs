using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ccai.NewRoam.SOActivity.Domain;

namespace Ccai.NewRoam.SOActivity.Data
{
    /// <summary>
    /// This interface is the Data Access Object interface that retrieves data from database and convert it to Domain Model and stores Domain Model state to database.
    /// It uses repository pattern.
    /// </summary>
    public interface ISOActivityRepository
    {
        /// <summary>
        /// Rebuild SOActivitySession state from SOActivityEvent that retrieved from database.
        /// </summary>
        /// <param name="sessionDate">Session date of SO Activity</param>
        /// <returns>It will return SOActivitySession object.</returns>
        SOActivitySession GetSOActivitySession(DateTime sessionDate);
        
        /// <summary>
        /// Save the session state to database as SOActivityEvent.
        /// </summary>
        /// <param name="session">SOActivitySession object that will be stored.</param>
        void SaveSOActivitySession(SOActivitySession session);
    }
}