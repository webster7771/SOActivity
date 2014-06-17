using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ccai.NewRoam.SOActivity.Domain.Exception
{
    /// <summary>
    /// If SO number doesn't have AC~ prefix, this exception will be thrown.
    /// </summary>
    public sealed class InvalidSONumberApplicationException: ApplicationException
    {
        public InvalidSONumberApplicationException()
            : base("SO number must begin with AC~.")
        {
        }
    }
}