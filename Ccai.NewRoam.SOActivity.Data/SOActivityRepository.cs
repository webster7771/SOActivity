using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Ccai.NewRoam.SOActivity.Domain;
using Ccai.NewRoam.SOActivity.Data.Exception;
using Ccai.NewRoam.SOActivity.Data.Entity;

namespace Ccai.NewRoam.SOActivity.Data
{
    /// <summary>
    /// This class is the Data Access Object that retrieves data from database and convert it to Domain Model and stores Domain Model state to database.
    /// It uses repository pattern.
    /// </summary>
    public class SOActivityRepository: ISOActivityRepository
    {
        #region Constant

        private const string CCAI_DATABASE = "CCAI";                 // Represent the databae name;
        private const string SESSION_DATE = "SessionDate";
        private const string SO_ACTIVITY_TYPE = "SOActivityType";
        
        #endregion

        #region Variable

        private SqlConnection _cn;
        private SqlTransaction _transaction;
        
        #endregion

        #region Constructor

        /// <summary>
        /// This constructor will initialise database connection and use connectionString in application configuration file.
        /// </summary>
        public SOActivityRepository()
        {
            _cn = new SqlConnection(ConfigurationManager.ConnectionStrings[CCAI_DATABASE].ConnectionString);
        }

        #endregion

        #region Method

        /// <summary>
        /// Implements GetSOActivitySession method of ISOActivityRepository. 
        /// </summary>
        /// <param name="sessionDate"></param>
        /// <returns></returns>
        public SOActivitySession GetSOActivitySession(DateTime sessionDate)
        {
            SOActivitySession session = new SOActivitySession(sessionDate.Date);
            List<SOActivityEvent> events = new List<SOActivityEvent>();

            // Geneare sql statement and paramater using SqlBuilder

            SqlBuilderResult sqlBuilderResult = SqlBuilder<CheckedInSOActivityEvent>.
                Select().
                Where(SESSION_DATE).
                Equals(sessionDate).
                And(SO_ACTIVITY_TYPE).
                Equals(SOActivityType.CheckIn.ToString()).
                Build();

            Read(sqlBuilderResult, dataReader => 
                {
                    events.Add(dataReader.Map<CheckedInSOActivityEvent>());
                });

            sqlBuilderResult = SqlBuilder<CheckedOutSOActivityEvent>.
                Select().
                Where(SESSION_DATE).
                Equals(sessionDate).
                And(SO_ACTIVITY_TYPE).
                Equals(SOActivityType.CheckOut.ToString()).
                Build();

            Read(sqlBuilderResult, dataReader =>
            {
                events.Add(dataReader.Map<CheckedOutSOActivityEvent>());
            });

            events.Sort(new SOActivityComparer());
            events.ForEach(evt =>
                {
                    if (evt is CheckedInSOActivityEvent)
                    {
                        session.Apply(evt as CheckedInSOActivityEvent);
                    }
                    else
                    {
                        session.Apply(evt as CheckedOutSOActivityEvent);
                    }
                });

            return session;
        }

        /// <summary>
        /// Implements SaveSOActivitySession method of ISOActivityRepository.
        /// </summary>
        /// <param name="session"></param>
        public void SaveSOActivitySession(SOActivitySession session)
        {
            try
            {
                _cn.Open();
                _transaction = _cn.BeginTransaction();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = _cn;
                cmd.Transaction = _transaction;
                foreach (SOActivityEvent evt in session.SOActivityEvents)
                {
                    SqlBuilderResult sqlBuilderResult;
                    if (evt is CheckedInSOActivityEvent)
                    {
                        sqlBuilderResult = SqlBuilder<CheckedInSOActivityEvent>.
                            Insert(evt as CheckedInSOActivityEvent).
                            Build();
                    }
                    else
                    {
                        sqlBuilderResult = SqlBuilder<CheckedOutSOActivityEvent>.
                            Insert(evt as CheckedOutSOActivityEvent).
                            Build();
                    }
                    cmd.CommandText = sqlBuilderResult.Sql;
                    cmd.Parameters.AddRange(sqlBuilderResult.Parameters);
                    cmd.ExecuteNonQuery();
                }
                _transaction.Commit();
            }
            catch(SqlException sqlException)
            {
                if (_transaction != null)
                {
                    _transaction.Rollback();
                }
                throw sqlException;
            }
            finally
            {
                _cn.Close();
                _transaction = null;
            }
        }

        private void Read(
            SqlBuilderResult sqlBuilderResult, 
            Action<SqlDataReader> onEachRow)
        {
            try
            {
                _cn.Open();
                SqlCommand command = new SqlCommand(sqlBuilderResult.Sql, _cn);
                command.Parameters.AddRange(sqlBuilderResult.Parameters.ToArray());
                using (SqlDataReader dataReader = command.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        onEachRow(dataReader);
                    }
                    dataReader.Close();
                }
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

        #endregion
    }
}