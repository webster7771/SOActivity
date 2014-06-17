using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using Ccai.NewRoam.SOActivity.Data;
using Ccai.NewRoam.SOActivity.Domain;
using Ccai.NewRoam.SOActivity.Data.Exception;
using System.Data.SqlClient;
using Ccai.NewRoam.SOActivity.Domain.Exception;

namespace Ccai.NewRoam.SOActivity.Service
{
    /// <summary>
    /// Interface for UI and other service or system as a gateway that provides main functionality of SO Activity feature. 
    /// </summary>
    public class SOActivityService : ISOActivityService
    {
        ILog _logger;
        ISOActivityRepository _soAcvRepo;
        
        public SOActivityService(ISOActivityRepository repository)
        {
            _logger = log4net.LogManager.GetLogger(typeof(SOActivityService));
            _soAcvRepo = repository;
        }

        /// <summary>
        /// Get SOActivitySession by date.
        /// </summary>
        /// <param name="sessionDate"></param>
        /// <returns></returns>
        public SOActivitySession GetSession(DateTime sessionDate)
        {
            try
            {
                return _soAcvRepo.GetSOActivitySession(sessionDate);
            }
            catch (SqlBuilderApplicationException appException)
            {
                _logger.Warn("SqlBuilder exception: " + appException.Message);
                throw appException;
            }
            catch (SqlException sqlException)
            {
                _logger.Warn("SqlException: " + sqlException.Message);
                throw sqlException;
            }
            catch (Exception unknownException)
            {
                _logger.Error(unknownException.Message);
                throw unknownException;
            }
        }

        /// <summary>
        /// Call this method for check in activity. 
        /// </summary>
        /// <param name="sessionDate"></param>
        /// <param name="soNumber"></param>
        public void CheckIn(DateTime sessionDate, string soNumber)
        {
            try
            {
                SOActivitySession session = GetSession(sessionDate);
                session.CheckIn(soNumber);
                _soAcvRepo.SaveSOActivitySession(session);
            }
            catch (InvalidSONumberApplicationException domainException)
            {
                _logger.Warn("Domain exception: " + domainException.Message);
                throw domainException;
            }
            catch (SOHasAlreadyCheckedInApplicationException domainException)
            {
                _logger.Warn("Domain exception: " + domainException.Message);
                throw domainException;
            }
            catch (Exception unknownException)
            {
                _logger.Error(unknownException.Message);
                throw unknownException;
            }
        }

        /// <summary>
        /// Call this method for check out activity.
        /// </summary>
        /// <param name="sessionDate"></param>
        public void CheckOut(DateTime sessionDate)
        {
            try
            {
                SOActivitySession session = GetSession(sessionDate);
                session.CheckOut();
                _soAcvRepo.SaveSOActivitySession(session);
            }
            catch (UnableToCheckOutApplicationException domainException)
            {
                _logger.Warn("Domain exception: " + domainException.Message);
                throw domainException;
            }
            catch (Exception unknownException)
            {
                _logger.Error(unknownException.Message);
                throw unknownException;
            }
        }
    }
}