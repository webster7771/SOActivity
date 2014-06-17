using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ccai.NewRoam.SOActivity.Data.Entity
{
    /// <summary>
    /// Represent result of sql statement generation.
    /// </summary>
    public class SqlBuilderResult
    {
        /// <summary>
        /// The sql statement generated from SqlBuilder.
        /// </summary>
        public string Sql { get; set; }

        /// <summary>
        /// The parameter generated from SqlBuilder.
        /// </summary>
        public SqlParameter[] Parameters { get; set; }
    }
}