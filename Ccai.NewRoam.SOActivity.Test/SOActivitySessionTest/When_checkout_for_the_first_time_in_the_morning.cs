using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ccai.NewRoam.SOActivity.Domain;

namespace Ccai.NewRoam.SOActivity.Test.SOActivitySessionTest
{
    [TestClass]
    public class When_checkout_for_the_first_time_in_the_morning
    {
        SOActivitySession _session;
        DateTime _today;
        string _soNumber;

        [TestInitialize]
        public void Because_of()
        {
            _today = DateTime.Today;
            _session = new SOActivitySession(_today);
            _soNumber = "123456";
            _session.CheckIn("AC~" + _soNumber);
            _session.CheckOut();
        }

        [TestMethod]
        public void Should_has_FirstCheckOut_value()
        {
            Assert.AreEqual(2, _session.SOActivityEvents.Count);
            SOActivityEvent evt = _session.SOActivityEvents.LastOrDefault();
            Assert.IsNotNull(evt);
            Assert.IsTrue(evt is CheckedOutSOActivityEvent);
            CheckedOutSOActivityEvent concreteEvt = evt as CheckedOutSOActivityEvent;

            Assert.IsFalse(string.IsNullOrEmpty(concreteEvt.ID));
            Assert.AreEqual(_today, concreteEvt.SessionDate);
            Assert.AreEqual(SOActivityType.CheckOut, concreteEvt.SOActivityType);
            Assert.AreEqual(_soNumber, concreteEvt.SONumber);
            Assert.AreEqual(_today.Date, concreteEvt.Timestamp.Date);
            Assert.AreEqual(CheckInCheckOutState.FirstCheckOutMorning, concreteEvt.CheckOutState);

            Assert.IsTrue(_session.FirstCheckIn.HasValue);
            Assert.IsTrue(_session.FirstCheckOut.HasValue);
            Assert.AreEqual(_today, _session.FirstCheckOut.Value.Date);
            Assert.IsFalse(_session.LastCheckIn.HasValue);
            Assert.IsFalse(_session.LastCheckOut.HasValue);
            Assert.IsTrue(_session.CanCheckIn());
            Assert.IsFalse(_session.CanCheckOut());
        }
    }
}