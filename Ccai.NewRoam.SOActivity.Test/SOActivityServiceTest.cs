using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NMock2;
using System.Configuration;
using System.Data.SqlClient;
using Ccai.NewRoam.SOActivity.Data;
using Ccai.NewRoam.SOActivity.Service;
using System.Diagnostics;
using Ccai.NewRoam.SOActivity.Domain;

namespace Ccai.NewRoam.SOActivity.Test
{
    [TestClass]
    public class SOActivityServiceTest
    {
        private ISOActivityRepository mockSOActivityRepository;
        private SOActivityService service;
        private DateTime sessionDate;
        private Mockery mocks;
        private string soNumber;

        [TestInitialize]
        public void BeforeEach()
        {
            mocks = new Mockery();
            mockSOActivityRepository = mocks.NewMock<ISOActivityRepository>();
            service = new SOActivityService(mockSOActivityRepository);
            sessionDate = DateTime.Today;
            soNumber = "AC~123456";
        }

        [TestMethod]
        public void GetSession()
        {
            Expect.Once.On(mockSOActivityRepository).
                Method("GetSOActivitySession").
                With(sessionDate).
                Will(Return.Value(new SOActivitySession(sessionDate)));

            SOActivitySession session = service.GetSession(sessionDate);
            
            Assert.IsNotNull(session);
            mocks.VerifyAllExpectationsHaveBeenMet();
        }

        [TestMethod]
        public void CheckIn()
        {
            Expect.AtLeast(2).On(mockSOActivityRepository).
                Method("GetSOActivitySession").
                With(sessionDate).
                Will(Return.Value(new SOActivitySession(sessionDate)));

            SOActivitySession session = service.GetSession(sessionDate);
            
            Expect.Once.On(mockSOActivityRepository).
                Method("SaveSOActivitySession").
                With(session);

            service.CheckIn(sessionDate, soNumber);

            Assert.IsTrue(session.FirstCheckIn.HasValue || session.LastCheckIn.HasValue);

            mocks.VerifyAllExpectationsHaveBeenMet();
        }

        [TestMethod]
        public void CheckOut()
        {
            Expect.AtLeast(3).On(mockSOActivityRepository).
                Method("GetSOActivitySession").
                With(sessionDate).
                Will(Return.Value(new SOActivitySession(sessionDate)));

            SOActivitySession session = service.GetSession(sessionDate);

            Expect.AtLeast(2).On(mockSOActivityRepository).
                Method("SaveSOActivitySession").
                With(session);

            service.CheckIn(sessionDate, "AC~123456");
            service.CheckOut(sessionDate);

            Assert.IsTrue(session.FirstCheckOut.HasValue || session.LastCheckOut.HasValue);

            mocks.VerifyAllExpectationsHaveBeenMet();
        }
    }
}