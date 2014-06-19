using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ccai.NewRoam.SOActivity.Data.Exception
{
    /// <summary>
    /// This exception occur when repository failed convert CheckInCheckOutState field in database to CheckInCheckOutState enum.
    /// </summary>
    [Serializable]
    public sealed class CheckInCheckOutStateParseApplicationException: ApplicationException
    {
        public CheckInCheckOutStateParseApplicationException() :
            base("Unable to parse CheckInCheckOutState, make sure the CheckInCheckOutState name in database match with CheckInCheckOutState enum.")
        {
        }
    }
}