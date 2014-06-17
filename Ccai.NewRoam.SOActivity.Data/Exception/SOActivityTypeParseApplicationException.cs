using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ccai.NewRoam.SOActivity.Data.Exception
{
    /// <summary>
    /// This exception occur when repository failed convert SOActivityType field in database to SOActivityType enum.
    /// </summary>
    public sealed class SOActivityTypeParseApplicationException: ApplicationException
    {
        public SOActivityTypeParseApplicationException() :
            base("Unable to parse SOActivityType, make sure the SOActivityType name in database match with SOActivityType enum.")
        {
        }
    }
}