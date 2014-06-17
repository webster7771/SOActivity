using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace Ccai.NewRoam.SOActivity.Domain.Exception
{
    /// <summary>
    /// Custom exception that used to throw exception that occured in this SO Activity module.
    /// </summary>
    public sealed class SOHasAlreadyCheckedInApplicationException: ApplicationException
    {
        public SOHasAlreadyCheckedInApplicationException()
            : base("SO has already checked in.")
        {
        }
    }
}