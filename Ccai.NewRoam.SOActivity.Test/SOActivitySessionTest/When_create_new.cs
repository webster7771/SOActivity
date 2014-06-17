using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ccai.NewRoam.SOActivity.Domain;

namespace Ccai.NewRoam.SOActivity.Test.SOActivitySessionTest
{
    [TestClass]
    public class When_create_new
    {
        SOActivitySession _session;
        DateTime _today;

        [TestInitialize]
        public void Because_of()
        {
            _today = DateTime.Today;
            _session = new SOActivitySession(_today);
        }

        [TestMethod]
        public void Should_has_correct_initial_state()
        {
            Assert.AreEqual(_today, _session.SessionDate);
            Assert.IsFalse(_session.FirstCheckIn.HasValue);
            Assert.IsFalse(_session.FirstCheckOut.HasValue);
            Assert.IsFalse(_session.LastCheckIn.HasValue);
            Assert.IsFalse(_session.LastCheckOut.HasValue);
            Assert.IsTrue(_session.CanCheckIn());
            Assert.IsFalse(_session.CanCheckOut());
            Assert.AreEqual(0, _session.SOActivityEvents.Count);
        }
    }
}