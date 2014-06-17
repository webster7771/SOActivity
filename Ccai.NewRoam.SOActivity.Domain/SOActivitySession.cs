using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Ccai.NewRoam.SOActivity.Domain.Exception;

namespace Ccai.NewRoam.SOActivity.Domain
{
    /// <summary>
    /// This class provides the main functionality of SO Activity module. This is where the domain logic is implemented.
    /// It uses Event Sourcing pattern and this class is the Aggregate Root. There is only one session in one day for SO Activity and this class holds all SO activity in one day.
    /// </summary>
    public class SOActivitySession
    {
        #region Constant
        
        private const string SO_NUMBER_PREFIX = "AC~";
        
        #endregion

        #region Variable

        private DateTime _sessionDate;
        private DateTime _morningActivityTimeBound;
        private List<SOActivityEvent> _soActivityEvents;                //Holds event that occured when CheckIn/CheckOut method is called and will be used and discharded by data access layer to store the event into database.
        private SOActivityState _soActivityState;
        private DateTime _checkInTime;
        private string _soNumber;

        #endregion

        #region Constructor

        /// <summary>
        /// This constructor will initialize initial value of SOActivitySession.
        /// </summary>
        /// <param name="sessionDate"></param>
        public SOActivitySession(DateTime sessionDate)
        {
            _sessionDate = sessionDate;
            _soActivityState = SOActivityState.None;
            _morningActivityTimeBound = new DateTime(sessionDate.Year, sessionDate.Month, sessionDate.Day, 12, 0, 0);
            _soActivityEvents = new List<SOActivityEvent>();
        }

        #endregion

        #region Property

        /// <summary>
        /// This is the SO Activity session date. 
        /// </summary>
        public DateTime SessionDate
        {
            get
            {
                return this._sessionDate;
            }
        }

        /// <summary>
        /// The first check in time. 
        /// </summary>
        public DateTime? FirstCheckIn 
        { 
            get; 
            private set; 
        }
        
        /// <summary>
        /// The first check out from the first check in.
        /// </summary>
        public DateTime? FirstCheckOut 
        { 
            get; 
            private set; 
        }
        
        /// <summary>
        /// This is the last check in time that peformed by SO and always be updated when check in event occured.
        /// </summary>
        public DateTime? LastCheckIn 
        { 
            get; 
            private set; 
        }
        
        /// <summary>
        /// This is the last check out time that peformed by SO and always be updated when check out event occured.
        /// </summary>
        public DateTime? LastCheckOut 
        { 
            get; 
            private set; 
        }

        /// <summary>
        /// This property return all events that has occured on this session.
        /// </summary>
        public IList<SOActivityEvent> SOActivityEvents
        {
            get
            {
                return this._soActivityEvents;
            }
        }

        #endregion

        #region Method

        /// <summary>
        /// This method determines whether SO can perform check in or not, it will return true if check in is allowed on the session. 
        /// </summary>
        public bool CanCheckIn()
        {
            return _soActivityState != SOActivityState.CheckedIn;
        }

        /// <summary>
        /// This method determines whether SO can perform check out or not, it will return true if check out is allowed on the session. 
        /// </summary>
        public bool CanCheckOut()
        {
            return _soActivityState == SOActivityState.CheckedIn;
        }

        /// <summary>
        /// This is the domain logic of check in activity.
        /// This method will raise event CheckedInSOActivityEvent.
        /// </summary>
        /// <param name="soNumber">Barcode that scanned from SO id that asssociated with each SO.</param>
        public void CheckIn(string soNumber)
        {
            if (!CanCheckIn())
            {
                throw new SOHasAlreadyCheckedInApplicationException();
            }
            if (soNumber.Substring(0,3).ToUpper() != SO_NUMBER_PREFIX)
            {
                throw new InvalidSONumberApplicationException();
            }
            DateTime checkInTime = DateTime.Now;
            CheckedInSOActivityEvent soActivityEvent = new CheckedInSOActivityEvent 
            { 
                ID = Guid.NewGuid().ToString().Replace("-",string.Empty),
                SessionDate = _sessionDate,
                SONumber = soNumber.Replace(SO_NUMBER_PREFIX,string.Empty),
                Timestamp = checkInTime
            };
            if (checkInTime <= _morningActivityTimeBound && !FirstCheckIn.HasValue)
            {
                soActivityEvent.CheckInState = CheckInCheckOutState.FirstCheckInMorning;
            }
            else if (checkInTime <= _morningActivityTimeBound && FirstCheckIn.HasValue)
            {
                soActivityEvent.CheckInState = CheckInCheckOutState.CheckInMorning;
            }
            else if (checkInTime > _morningActivityTimeBound)
            {
                soActivityEvent.CheckInState = CheckInCheckOutState.CheckInAfternoon;
            }
            Apply(soActivityEvent);
            _soActivityEvents.Add(soActivityEvent);
        }

        /// <summary>
        /// This is the domain logic of check out activity. This method will raise event CheckedOutSOActivityEvent.
        /// </summary>
        public void CheckOut()
        {
            if (!CanCheckOut())
            {
                throw new UnableToCheckOutApplicationException();
            }
            DateTime checkOutTime = DateTime.Now;
            CheckedOutSOActivityEvent soActivityEvent = new CheckedOutSOActivityEvent
            {
                ID = Guid.NewGuid().ToString().Replace("-",string.Empty),
                SessionDate = _sessionDate,
                SONumber = _soNumber,
                Timestamp = checkOutTime
            };
            if (_checkInTime <= _morningActivityTimeBound && !FirstCheckOut.HasValue)
            {
                soActivityEvent.CheckOutState = CheckInCheckOutState.FirstCheckOutMorning;
            }
            else if (_checkInTime <= _morningActivityTimeBound && FirstCheckOut.HasValue)
            {
                soActivityEvent.CheckOutState = CheckInCheckOutState.CheckOutMorning;
            }
            else if (_checkInTime > _morningActivityTimeBound)
            {
                soActivityEvent.CheckOutState = CheckInCheckOutState.CheckOutAfternoon;
            }
            Apply(soActivityEvent);
            _soActivityEvents.Add(soActivityEvent);
        }

        /// <summary>
        /// This method will apply check in event that has occured and will change state of the session based on the event.
        /// </summary>
        /// <param name="soActivityEvent">Event that raised from CheckIn method in SOActivitySession class. 
        /// This event will be saved to database by Event Store and will be retreived from the Event Store and applied to this method.</param>
        public void Apply(CheckedInSOActivityEvent soActivityEvent)
        {
            _soNumber = soActivityEvent.SONumber;
            _checkInTime = soActivityEvent.Timestamp;
            _soActivityState = SOActivityState.CheckedIn;
            if (soActivityEvent.CheckInState == CheckInCheckOutState.FirstCheckInMorning)
            {
                FirstCheckIn = soActivityEvent.Timestamp;
            }
            else if (soActivityEvent.CheckInState == CheckInCheckOutState.CheckInAfternoon)
            {
                LastCheckIn = soActivityEvent.Timestamp;
                LastCheckOut = null;
            }
        }

        /// <summary>
        /// This method will apply check out event that has occured and will change state of this session based on the event.
        /// </summary>
        /// <param name="soActivityEvent">Event that raised from CheckOut method in SOActivitySession class. 
        /// This event will be saved to database by Event Store and will be retreived from the Event Store and applied to this method.</param>
        public void Apply(CheckedOutSOActivityEvent soActivityEvent)
        {
            _soActivityState = SOActivityState.CheckedOut;
            _soNumber = soActivityEvent.SONumber;
            if (soActivityEvent.CheckOutState == CheckInCheckOutState.FirstCheckOutMorning)
            {
                FirstCheckOut = soActivityEvent.Timestamp;
            }
            else if (soActivityEvent.CheckOutState == CheckInCheckOutState.CheckOutAfternoon)
            {
                LastCheckOut = soActivityEvent.Timestamp;
            }
        }

        /// <summary>
        /// This method will remove all events in this session.
        /// </summary>
        public void ClearSOActivityEvents()
        {
            this._soActivityEvents.Clear();
        }

        #endregion
    }
}