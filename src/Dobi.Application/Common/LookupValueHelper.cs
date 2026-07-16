namespace Dobi.Application.Common;

public static class LookupValueHelper
{
    public static string GetCode(object entity)
    {
        return GetStringProperty(
                   entity,
                   "Code",
                   "StatusCode",
                   "TypeCode",

                   "OrderStatusCode",
                   "PaymentStatusCode",

                   "CustomerTypeCode",

                   "MethodCode",
                   "PaymentMethodCode",

                   "TransferTypeCode",
                   "TransferStatusCode",
                   "AcknowledgementTypeCode",
                   "AcknowledgementCode",

                   "StageCode",
                   "ProcessingStageCode",
                   "ProcessingStageStatusCode",

                   "QCStatusCode",
                   "QcStatusCode",

                   "CollectionModeCode",

                   "NotificationTypeCode",
                   "NotificationStatusCode")
               ?? entity.GetType().Name;
    }

    public static string GetName(object entity)
    {
        return GetStringProperty(
                   entity,
                   "Name",
                   "StatusName",
                   "TypeName",

                   "OrderStatusName",
                   "PaymentStatusName",

                   "CustomerTypeName",

                   "MethodName",
                   "PaymentMethodName",

                   "TransferTypeName",
                   "TransferStatusName",
                   "AcknowledgementTypeName",
                   "AcknowledgementName",

                   "StageName",
                   "ProcessingStageName",
                   "ProcessingStageStatusName",

                   "QCStatusName",
                   "QcStatusName",

                   "CollectionModeName",

                   "NotificationTypeName",
                   "NotificationStatusName")
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