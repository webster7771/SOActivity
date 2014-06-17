﻿using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ccai.NewRoam.SOActivity.Domain;

namespace Ccai.NewRoam.SOActivity.Test.SOActivitySessionTest
{
    [TestClass]
    public class When_checkin_in_the_afternoon
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
        }

        [TestMethod]
        public void Should_set_LastCheckIn_value()
        {
            Assert.AreEqual(1, _session.SOActivityEvents.Count);
            SOActivityEvent evt = _session.SOActivityEvents.FirstOrDefault();
            Assert.IsNotNull(evt);
            Assert.IsTrue(evt is CheckedInSOActivityEvent);
            CheckedInSOActivityEvent concreteEvt = evt as CheckedInSOActivityEvent;

            Assert.IsFalse(string.IsNullOrEmpty(concreteEvt.ID));
            Assert.AreEqual(_today, concreteEvt.SessionDate);
            Assert.AreEqual(SOActivityType.CheckIn, concreteEvt.SOActivityType);
            Assert.AreEqual(_soNumber, concreteEvt.SONumber);
            Assert.AreEqual(_today.Date, concreteEvt.Timestamp.Date);
            Assert.AreEqual(CheckInCheckOutState.CheckInAfternoon, concreteEvt.CheckInState);

            Assert.IsFalse(_session.FirstCheckIn.HasValue);
            Assert.IsFalse(_session.FirstCheckOut.HasValue);
            Assert.IsTrue(_session.LastCheckIn.HasValue);
            Assert.IsFalse(_session.LastCheckOut.HasValue);
            Assert.IsFalse(_session.CanCheckIn());
            Assert.IsTrue(_session.CanCheckOut());
        }
    }
}