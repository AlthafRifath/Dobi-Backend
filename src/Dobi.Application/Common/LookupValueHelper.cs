using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Common
{
    public static class LookupValueHelper
    {
        public static string GetCode(object entity)
        {
            return GetStringProperty(
                       entity,
                       "Code",
                       "StatusCode",
                       "TypeCode",
                       "TransferTypeCode",
                       "TransferStatusCode",
                       "AcknowledgementTypeCode",
                       "AcknowledgementCode",
                       "OrderStatusCode",
                       "PaymentStatusCode")
                   ?? entity.GetType().Name;
        }

        public static string GetName(object entity)
        {
            return GetStringProperty(
                       entity,
                       "Name",
                       "StatusName",
                       "TypeName",
                       "TransferTypeName",
                       "TransferStatusName",
                       "AcknowledgementTypeName",
                       "AcknowledgementName",
                       "OrderStatusName",
                       "PaymentStatusName")
                   ?? GetCode(entity);
        }

        private static string? GetStringProperty(
            object entity,
            params string[] propertyNames)
        {
            foreach (var propertyName in propertyNames)
            {
                var property = entity.GetType().GetProperty(propertyName);

                if (property is null)
                {
                    continue;
                }

                var value = property.GetValue(entity);

                if (value is string stringValue && !string.IsNullOrWhiteSpace(stringValue))
                {
                    return stringValue;
                }
            }

            return null;
        }
    }
}
