using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ccai.NewRoam.SOActivity.Domain;
using Ccai.NewRoam.SOActivity.Domain.Exception;

namespace Ccai.NewRoam.SOActivity.Test.SOActivitySessionTest
{
    [TestClass]
    public class When_checkin_with_invalid_so_number
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
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidSONumberApplicationException))]
        public void Should_be_fail()
        {
            _session.CheckIn("AC" + _soNumber);
        }
    }
}