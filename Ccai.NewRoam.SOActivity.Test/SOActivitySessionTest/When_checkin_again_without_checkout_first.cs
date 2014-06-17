using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ccai.NewRoam.SOActivity.Domain;
using Ccai.NewRoam.SOActivity.Domain.Exception;

namespace Ccai.NewRoam.SOActivity.Test.SOActivitySessionTest
{
    [TestClass]
    public class When_checkin_again_without_checkout_first
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
        [ExpectedException(typeof(SOHasAlreadyCheckedInApplicationException))]
        public void Should_be_fail()
        {
            _session.CheckIn(_soNumber);
        }
    }
}