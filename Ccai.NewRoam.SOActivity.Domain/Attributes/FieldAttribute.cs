using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ccai.NewRoam.SOActivity.Domain.Attributes
{
    /// <summary>
    /// This attribute will map property nama in a class to database field.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class FieldAttribute : System.Attribute
    {
        public FieldAttribute(string fieldName)
        {
            this.FieldName = fieldName;
        }

        /// <summary>
        /// Field name in database
        /// </summary>
        public string FieldName { get; set; }
    }
}