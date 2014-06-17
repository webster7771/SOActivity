using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ccai.NewRoam.SOActivity.Domain;
using Ccai.NewRoam.SOActivity.Data.Entity;
using Ccai.NewRoam.SOActivity.Data.Exception;

namespace Ccai.NewRoam.SOActivity.Test
{
    [TestClass]
    public class SqlBuilderTest
    {
        [TestMethod]
        public void BuildSelectSqlStatement()
        {
            SqlBuilderResult result = SqlBuilder<CheckedInSOActivityEvent>.Select().Build();
            Assert.AreEqual("SELECT CheckInCheckOutState,ActivityType,Id,Date,SONumber,Timestamp FROM SOActivity;", result.Sql); 
        }

        [TestMethod]
        public void BuildSelectSqlStatementWithFilter()
        {
            SqlBuilderResult result = SqlBuilder<CheckedInSOActivityEvent>.
                Select().
                Where("SessionDate").
                Equals(DateTime.Today).
                And("SONumber").
                Equals("123").
                Build();
            Assert.AreEqual("SELECT CheckInCheckOutState,ActivityType,Id,Date,SONumber,Timestamp FROM SOActivity WHERE SONumber=@sonumber AND Date=@date;", result.Sql);
            Assert.AreEqual(2, result.Parameters.Length);
            Assert.AreEqual("@sonumber", result.Parameters[0].ParameterName);
            Assert.AreEqual("123", result.Parameters[0].Value);
            Assert.AreEqual("@date", result.Parameters[1].ParameterName);
            Assert.AreEqual(DateTime.Today, result.Parameters[1].Value);
        }

        [TestMethod]
        [ExpectedException(typeof(SqlBuilderApplicationException))]
        public void BuildSelectSqlStatementWithFilterThatHasInvalidPropertyName()
        {
            SqlBuilder<CheckedInSOActivityEvent>.
                Select().
                Where("Date").
                Equals(DateTime.Today).
                Build();
        }

        [TestMethod]
        public void BuildInsertSqlStatement()
        {
            CheckedInSOActivityEvent evt = new CheckedInSOActivityEvent
                {
                    ID = Guid.NewGuid().ToString(),
                    SessionDate = DateTime.Today,
                    SONumber = "123456",
                    Timestamp = DateTime.Now,
                    CheckInState = CheckInCheckOutState.CheckInMorning
                };
            SqlBuilderResult result = SqlBuilder<CheckedInSOActivityEvent>.Insert(evt).Build();
            Assert.AreEqual("INSERT INTO SOActivity (CheckInCheckOutState,ActivityType,Id,Date,SONumber,Timestamp) VALUES (@checkincheckoutstate,@activitytype,@id,@date,@sonumber,@timestamp);", result.Sql);
        }
    }
}