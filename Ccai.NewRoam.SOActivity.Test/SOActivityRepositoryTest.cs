using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Configuration;
using Ccai.NewRoam.SOActivity.Data;
using System.Diagnostics;
using Ccai.NewRoam.SOActivity.Domain;

namespace Ccai.NewRoam.SOActivity.Test
{
    [TestClass]
    public class SOActivityRepositoryTest
    {
        private SqlConnection _cn;
        private SOActivityRepository _repository;
        private string _soNumber;
        private DateTime _sessionDate;

        [TestInitialize]
        public void Before_each()
        {
            string cnString = ConfigurationManager.ConnectionStrings["CCAI"].ConnectionString;
            _cn = new SqlConnection(cnString);
            _repository = new SOActivityRepository();
            _soNumber = "AC~123456";
            _sessionDate = DateTime.Today;
            InitDB();
        }

        [TestMethod]
        public void When_save_session()
        {
            SOActivitySession session = _repository.GetSOActivitySession(_sessionDate);
            session.CheckIn(_soNumber);
            _repository.SaveSOActivitySession(session);
            session = _repository.GetSOActivitySession(_sessionDate);
            Assert.IsTrue(session.FirstCheckIn.HasValue);
        }

        [TestCleanup]
        public void CleanUp()
        {
            try
            {
                _cn.Open();
                SqlCommand cmd = new SqlCommand("DELETE FROM SOActivity;", _cn);
                cmd.ExecuteNonQuery();
            }
            catch
            {
                throw;
            }
            finally
            {
                _cn.Close();
            }
        }

        private void InitDB()
        {
            try
            {
                _cn.Open();
                SqlCommand createTableCommand = new SqlCommand(SqlStatements.SOActivityCreateTableCommand, _cn);
                createTableCommand.ExecuteNonQuery();
            }
            catch
            {
                Debug.Write("Table has already created.");
            }
            finally
            {
                _cn.Close();
            }
        }
    }
}