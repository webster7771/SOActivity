using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ccai.NewRoam.SOActivity.Domain;

namespace Ccai.NewRoam.SOActivity.Test.SOActivitySessionTest
{
    [TestClass]
    public class When_recheckin_in_the_morning
    {
        SOActivitySession _session;
        DateTime _today;
        string _soNumber;
        CheckedInSOActivityEvent _firstEvent;

        [TestInitialize]
        public void Because_of()
        {
            _today = DateTime.Today;
            _session = new SOActivitySession(_today);
            _soNumber = "123456";
            _session.CheckIn("AC~" + _soNumber);
            _session.CheckOut();
            _firstEvent = _session.SOActivityEvents.First() as CheckedInSOActivityEvent;
            _session.CheckIn("AC~" + _soNumber);
        }

        [TestMethod]
        public void Should_not_change_FirstCheckInMorning_value()
        {
            Assert.AreEqual(3, _session.SOActivityEvents.Count);
            SOActivityEvent evt = _session.SOActivityEvents.LastOrDefault();
            Assert.IsNotNull(evt);
            Assert.IsTrue(evt is CheckedInSOActivityEvent);
            CheckedInSOActivityEvent concreteEvt = evt as CheckedInSOActivityEvent;

            Assert.IsFalse(string.IsNullOrEmpty(concreteEvt.ID));
            Assert.AreEqual(_today, concreteEvt.SessionDate);
            Assert.AreEqual(SOActivityType.CheckIn, concreteEvt.SOActivityType);
            Assert.AreEqual(_soNumber, concreteEvt.SONumber);
            Assert.AreEqual(_today.Date, concreteEvt.Timestamp.Date);
            Assert.AreEqual(CheckInCheckOutState.CheckInMorning, concreteEvt.CheckInState);

            Assert.AreEqual(_session.FirstCheckIn, _firstEvent.Timestamp);
            Assert.IsTrue(_session.FirstCheckIn.HasValue);
            Assert.IsTrue(_session.FirstCheckOut.HasValue);
            Assert.IsFalse(_session.LastCheckIn.HasValue);
            Assert.IsFalse(_session.LastCheckOut.HasValue);
            Assert.IsFalse(_session.CanCheckIn());
            Assert.IsTrue(_session.CanCheckOut());
        }
    }
}