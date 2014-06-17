using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Ccai.NewRoam.SOActivity.Domain.Attributes;

namespace Ccai.NewRoam.SOActivity.Data.Entity
{
    /// <summary>
    /// Helper class that will read SqlDataReadar and create new Entity/Domain Object.
    /// </summary>
    public static class EntityMaper
    {
        /// <summary>
        /// Create new Entity and read the data from SqlDataReader. 
        /// </summary>
        /// <typeparam name="T">The Entity type name.</typeparam>
        /// <param name="dataReader">The SqlDataReader which used to read the entity data.</param>
        /// <returns></returns>
        public static T Map<T>(this SqlDataReader dataReader) where T : class, new()
        {
            T entity = new T();
            
            // Find all properties that has Field attribute
            IList<PropertyInfo> props = entity.GetType().GetProperties().Where(prop =>
                {
                    return prop.GetCustomAttributes(typeof(FieldAttribute)).Any();
                }).ToList();
            
            // For each property that has Field attribute, 
            // set the property value from data reader and use FieldName in Field attribute as field name in dataReader.
            foreach (PropertyInfo prop in props)
            {
                FieldAttribute fieldAttr = prop.GetCustomAttribute(typeof(FieldAttribute)) as FieldAttribute;
                if (fieldAttr is FieldEnumAttribute)
                {
                    FieldEnumAttribute fieldEnum = fieldAttr as FieldEnumAttribute;
                    object val = Enum.Parse(fieldEnum.EnumType, dataReader[fieldEnum.FieldName].ToString());
                    MethodInfo getSetMethod = prop.GetSetMethod();
                    if (getSetMethod != null)
                    {
                        getSetMethod.Invoke(entity, new object[1] { val });
                    }
                }
                else
                {
                    MethodInfo getSetMethod = prop.GetSetMethod();
                    if (getSetMethod != null)
                    {
                        getSetMethod.Invoke(entity, new object[1] { dataReader[fieldAttr.FieldName] });
                    }
                }
            }
            return entity;
        }
    }
}