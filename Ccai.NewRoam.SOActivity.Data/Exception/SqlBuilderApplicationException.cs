using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ccai.NewRoam.SOActivity.Data.Exception
{
    public class SqlBuilderApplicationException: ApplicationException
    {
        public SqlBuilderApplicationException(string message)
            : base(message)
        {
        }
    }
}
