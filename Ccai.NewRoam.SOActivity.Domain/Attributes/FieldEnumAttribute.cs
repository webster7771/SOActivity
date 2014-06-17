using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Ccai.NewRoam.SOActivity.Domain.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class FieldEnumAttribute : FieldAttribute
    {
        public Type EnumType { get; private set; }
        public FieldEnumAttribute(Type enumType, string fieldName)
            : base(fieldName)
        {
            EnumType = enumType;
        }
    }
}