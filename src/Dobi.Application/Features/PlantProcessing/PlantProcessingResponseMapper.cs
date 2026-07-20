using Dobi.Application.Common;
using Dobi.Contracts.PlantProcessing;
using Dobi.Domain.Orders;
using Dobi.Domain.PlantProcessing;

namespace Dobi.Application.Features.PlantProcessing;

internal static class PlantProcessingResponseMapper
{
    public static PlantProcessingResponse Map(Dobi.Domain.PlantProcessing.PlantProcessing plantProcessing)
    {
        var latestStageUpdate = plantProcessing.StageUpdates
            .OrderByDescending(x => x.CompletedAt ?? x.StartedAt ?? DateTime.MinValue)
            .ThenByDescending(x => x.Id)
            .FirstOrDefault();

        return new PlantProcessingResponse(
            plantProcessing.Id,
            plantProcessing.OrderId,
            plantProcessing.Order?.OrderNo,

            plantProcessing.PlantId,
            plantProcessing.Plant?.PlantName,

            plantProcessing.ReceivedAtPlant,
            plantProcessing.ReadyDate,

            plantProcessing.OverallQCStatusId,
            plantProcessing.OverallQCStatus is null
                ? null
                : LookupValueHelper.GetName(plantProcessing.OverallQCStatus),

            plantProcessing.PlantRemarks,

            plantProcessing.Order?.CurrentStatusId ?? 0,
            plantProcessing.Order?.CurrentStatus is null
                ? null
                : LookupValueHelper.GetName(plantProcessing.Order.CurrentStatus),

            latestStageUpdate?.ProcessingStageId,
            latestStageUpdate?.ProcessingStage is null
                ? null
                : LookupValueHelper.GetName(latestStageUpdate.ProcessingStage),

            latestStageUpdate?.ProcessingStageStatusId,
            latestStageUpdate?.ProcessingStageStatus is null
                ? null
                : LookupValueHelper.GetName(latestStageUpdate.ProcessingStageStatus),

            plantProcessing.StageUpdates
                .OrderBy(x => x.ProcessingStage.SortOrder)
                .ThenBy(x => x.Id)
                .Select(MapStageUpdate)
                .ToArray(),

            plantProcessing.QCRecords
                .OrderByDescending(x => x.RecordedAt)
                .ThenByDescending(x => x.Id)
                .Select(MapQcRecord)
                .ToArray(),

            plantProcessing.Order is null
                ? Array.Empty<PlantProcessingOrderItemResponse>()
                : plantProcessing.Order.Items
                    .OrderBy(x => x.Id)
                    .Select(MapOrderItem)
                    .ToArray());
    }

    private static PlantProcessingStageUpdateResponse MapStageUpdate(
        PlantProcessingStageUpdate stageUpdate)
    {
        return new PlantProcessingStageUpdateResponse(
            stageUpdate.Id,
            stageUpdate.ProcessingStageId,
            LookupValueHelper.GetName(stageUpdate.ProcessingStage),
            stageUpdate.ProcessingStageStatusId,
            LookupValueHelper.GetName(stageUpdate.ProcessingStageStatus),
            stageUpdate.StartedAt,
            stageUpdate.CompletedAt,
            stageUpdate.UpdatedByUserId,
            stageUpdate.Remarks);
    }

    private static QcRecordResponse MapQcRecord(QCRecord qcRecord)
    {
        return new QcRecordResponse(
            qcRecord.Id,
            qcRecord.OrderItemId,
            qcRecord.QCStatusId,
            LookupValueHelper.GetName(qcRecord.QCStatus),
            qcRecord.IssueDescription,
            qcRecord.ActionTaken,
            qcRecord.LabourChargeAmount,
            qcRecord.RecordedByUserId,
            qcRecord.RecordedAt);
    }

    private static PlantProcessingOrderItemResponse MapOrderItem(OrderItem orderItem)
    {
        var tags = orderItem.Tags
            .OrderBy(x => x.Id)
            .Select(GetTagValue)
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Select(x => x!)
            .ToArray();

        return new PlantProcessingOrderItemResponse(
            orderItem.Id,

            orderItem.ServiceId,
            orderItem.Service.ServiceName,

            orderItem.ItemCategoryId,
            orderItem.ItemCategory.CategoryName,

            orderItem.PricingTypeId,
            LookupValueHelper.GetCode(orderItem.PricingType),
            LookupValueHelper.GetName(orderItem.PricingType),

            orderItem.Quantity,
            orderItem.WeightKg,

            orderItem.UnitPrice,
            orderItem.LineAmount,
            orderItem.LineAmount,

            orderItem.SpecialNotes,
            tags);
    }

    private static string? GetTagValue(object tag)
    {
        var propertyNames = new[]
        {
            "TagNo",
            "ManualTagNo",
            "TagNumber"
        };

        foreach (var propertyName in propertyNames)
        {
            var property = tag.GetType().GetProperty(propertyName);

            if (property is null)
            {
                continue;
            }

            var value = property.GetValue(tag);

            if (value is string stringValue && !string.IsNullOrWhiteSpace(stringValue))
            {
                return stringValue;
            }
        }

        return null;
    }
}