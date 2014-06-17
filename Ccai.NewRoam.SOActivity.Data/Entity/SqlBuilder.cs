using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Data.SqlClient;
using Ccai.NewRoam.SOActivity.Domain;
using Ccai.NewRoam.SOActivity.Domain.Attributes;
using Ccai.NewRoam.SOActivity.Data.Exception;

namespace Ccai.NewRoam.SOActivity.Data.Entity
{
    /// <summary>
    /// Generate sql statement from object that has TableAttribute and object's properties has FieldAttribute.
    /// It uses Fluent Interface pattern.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SqlBuilder<T> where T : class, new()
    {
        #region Variable

        private T _entity;
        private Filter _filter;
        private CommandType _commandType;

        #endregion

        #region Constructor

        private SqlBuilder(
            T entity, 
            CommandType optionalCommandType = CommandType.Select)
        {
            _entity = entity;
            _commandType = optionalCommandType;
        }

        #endregion

        #region Static Method/Factory Method

        /// <summary>
        /// Factory method to create new SqlBuilder for selection purpose. Use this factory method to create select sql statement.
        /// </summary>
        /// <returns></returns>
        public static SqlBuilder<T> Select()
        {
            T entity = new T();
            return new SqlBuilder<T>(entity);
        }

        /// <summary>
        /// Factory method to create new SqlBuilder for insertion purpose. Use this factory method to create insert sql statement.
        /// </summary>
        /// <param name="entity">The entity object that will be used to generate the sql statement.</param>
        /// <returns></returns>
        public static SqlBuilder<T> Insert(T entity)
        {
            return new SqlBuilder<T>(entity, optionalCommandType: CommandType.Insert);
        }

        #endregion

        #region Method

        /// <summary>
        /// Generate "where" sql statement.
        /// </summary>
        /// <param name="propertyName">Property name of entity object.</param>
        /// <returns></returns>
        public SqlBuilder<T> Where(string propertyName)
        {
            _filter = new Filter();
            _filter.PropertyName = propertyName;
            return this;
        }

        /// <summary>
        /// Used with Where method. Equals will generate operator "=".
        /// </summary>
        /// <param name="value">Value that will be assigned to "=" operator.</param>
        /// <returns></returns>
        public SqlBuilder<T> Equals(Object value)
        {
            if (_filter != null)
            {
                _filter.Operator = "=";
                _filter.Value = value;
            }
            return this;
        }

        /// <summary>
        /// Will generate "and" sql statement and act like Where method.
        /// </summary>
        /// <param name="propertyName">Property name of entity object.</param>
        /// <returns></returns>
        public SqlBuilder<T> And(string propertyName)
        {
            Filter newFilter = new Filter();
            newFilter.PropertyName = propertyName;
            if (_filter == null)
            {
                _filter = newFilter;
            }
            else
            {
                newFilter.NextFilter = _filter;
                _filter = newFilter;
            }
            return this;
        }

        /// <summary>
        /// Build the sql statement.
        /// </summary>
        /// <returns></returns>
        public SqlBuilderResult Build()
        {
            if (_commandType == CommandType.Select)
            {
                return BuildSelectQuery();
            }
            else
            {
                return BuildInsertCommand();
            }
        }

        private SqlBuilderResult BuildSelectQuery()
        {
            List<SqlParameter> sqlParams = new List<SqlParameter>();
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("SELECT ");

            ForEachFieldAttribute((fieldAttr, propInfo, hasNext) =>
            {
                sqlBuilder.Append(fieldAttr.FieldName);
                if (hasNext)
                {
                    sqlBuilder.Append(",");
                }
            });
            sqlBuilder.Append(" FROM ");
            sqlBuilder.Append(GenerateTableName());
            if (_filter != null)
            {
                sqlBuilder.Append(" WHERE ");
                Filter filter = _filter;
                while (filter != null)
                {
                    PropertyInfo propInfo = _entity.GetType().GetProperty(filter.PropertyName);
                    if (propInfo == null)
                    {
                        throw new SqlBuilderApplicationException("There is no property name " + filter.PropertyName + " in " + _entity.GetType().Name);
                    }
                    FieldAttribute fieldAttr = propInfo.GetCustomAttribute(typeof(FieldAttribute)) as FieldAttribute;
                    if(fieldAttr != null)
                    {
                        sqlBuilder.Append(string.Format("{0}{1}@{2}",
                            fieldAttr.FieldName,
                            filter.Operator,
                            fieldAttr.FieldName.ToLower()));
                        sqlParams.Add(new SqlParameter(string.Format("@{0}", fieldAttr.FieldName.ToLower()), filter.Value));
                    }
                    if (filter.NextFilter != null)
                    {
                        sqlBuilder.Append(" AND ");
                    }
                    filter = filter.NextFilter;
                }
            }
            sqlBuilder.Append(";");

            return new SqlBuilderResult
            {
                Sql = sqlBuilder.ToString(),
                Parameters = sqlParams.ToArray()
            };
        }

        private SqlBuilderResult BuildInsertCommand()
        {
            List<SqlParameter> sqlParams = new List<SqlParameter>();
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("INSERT INTO ");
            sqlBuilder.Append(GenerateTableName());
            sqlBuilder.Append(" (");
            ForEachFieldAttribute((fieldAttr, propInfo, hasNext) =>
            {
                sqlBuilder.Append(fieldAttr.FieldName);
                if (hasNext)
                {
                    sqlBuilder.Append(",");
                }
            });
            sqlBuilder.Append(") VALUES (");
            ForEachFieldAttribute((fieldAttr, propInfo, hasNext) =>
            {
                sqlBuilder.Append(string.Format("@{0}",fieldAttr.FieldName.ToLower()));
                if (hasNext)
                {
                    sqlBuilder.Append(",");
                }
                if (fieldAttr is FieldEnumAttribute)
                {
                    FieldEnumAttribute fieldEnum = fieldAttr as FieldEnumAttribute;
                    string val = Enum.GetName(fieldEnum.EnumType, propInfo.GetGetMethod().Invoke(_entity, null));
                    sqlParams.Add(
                        new SqlParameter(string.Format("@{0}", fieldAttr.FieldName.ToLower()),
                        val));
                }
                else
                {
                    sqlParams.Add(new SqlParameter(string.Format("@{0}", fieldAttr.FieldName.ToLower()),
                        propInfo.GetGetMethod().Invoke(_entity, null)));
                }
            });
            sqlBuilder.Append(");");

            return new SqlBuilderResult
            {
                Sql = sqlBuilder.ToString(),
                Parameters = sqlParams.ToArray()
            };
        }

        private string GenerateTableName()
        {
            Type entityType = _entity.GetType();
            if (entityType.GetCustomAttributes(typeof(TableAttribute), true).Any())
            {
                TableAttribute tableAttr =
                    entityType.GetCustomAttributes(typeof(TableAttribute), true).FirstOrDefault() as TableAttribute;
                if (tableAttr == null)
                {
                    return entityType.Name;
                }
                else
                {
                    return tableAttr.TableName;
                }
            }
            else
            {
                return entityType.Name;
            }
        }

        private void ForEachFieldAttribute(Action<FieldAttribute,PropertyInfo,bool> action)
        {
            // Find all properties that has Field attribute and get the Field attribute from the property and 
            // call the action callback with three arguments: 
            // FieldAttribut, PropertyInfo and boolean value to inform the calling code whether it reach the end of iteration (true means doesn't reach end of iteration). 
            
            Type entityType = _entity.GetType();
            List<PropertyInfo> props = entityType.GetProperties().Where(propInfo =>
                {
                    return propInfo.GetCustomAttributes(typeof(FieldAttribute)).Any();
                }).ToList();
            int idx = 1;
            foreach (PropertyInfo propInfo in props)
            {
                FieldAttribute fieldAttr = propInfo.GetCustomAttribute(typeof(FieldAttribute)) as FieldAttribute;
                if (idx < props.Count)
                {
                    action(fieldAttr, propInfo, true);
                }
                else
                {
                    action(fieldAttr, propInfo, false);
                }
                idx++;
            }
        }

        #endregion

        #region Sub class

        private enum CommandType
        {
            Select,
            Insert
        }

        private class Filter
        {
            internal string PropertyName { get; set; }
            internal string Operator { get; set; }
            internal Object Value { get; set; }
            internal Filter NextFilter { get; set; }
        }

        #endregion
    }
}