using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ccai.NewRoam.SOActivity.Domain.Attributes
{
    /// <summary>
    /// This attribute will map class name to table name in database.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class TableAttribute : System.Attribute
    {
        public TableAttribute(string name)
        {
            this.TableName = name;
        }

        public string TableName { get; set; }
    }
}
