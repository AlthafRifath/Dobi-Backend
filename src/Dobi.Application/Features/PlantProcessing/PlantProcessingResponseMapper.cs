using Dobi.Application.Common;
using Dobi.Contracts.PlantProcessing;
using PlantProcessingEntity = Dobi.Domain.PlantProcessing.PlantProcessing;

namespace Dobi.Application.Features.PlantProcessing;

internal static class PlantProcessingResponseMapper
{
    public static PlantProcessingResponse Map(PlantProcessingEntity processing)
    {
        var latestStageUpdate = processing.StageUpdates
            .OrderByDescending(x => x.CompletedAt ?? x.StartedAt ?? DateTime.MinValue)
            .FirstOrDefault();

        return new PlantProcessingResponse(
            processing.Id,
            processing.OrderId,
            processing.Order.OrderNo,
            processing.PlantId,
            processing.Plant.PlantName,
            processing.ReceivedAtPlant,
            processing.ReadyDate,
            processing.OverallQCStatusId,
            processing.OverallQCStatus is null
                ? null
                : LookupValueHelper.GetName(processing.OverallQCStatus),
            processing.PlantRemarks,
            processing.Order.CurrentStatusId,
            LookupValueHelper.GetName(processing.Order.CurrentStatus),
            latestStageUpdate?.ProcessingStageId,
            latestStageUpdate is null
                ? null
                : LookupValueHelper.GetName(latestStageUpdate.ProcessingStage),
            latestStageUpdate?.ProcessingStageStatusId,
            latestStageUpdate is null
                ? null
                : LookupValueHelper.GetName(latestStageUpdate.ProcessingStageStatus),
            processing.StageUpdates
                .OrderBy(x => x.CompletedAt ?? x.StartedAt ?? DateTime.MinValue)
                .Select(x => new PlantProcessingStageUpdateResponse(
                    x.Id,
                    x.ProcessingStageId,
                    LookupValueHelper.GetName(x.ProcessingStage),
                    x.ProcessingStageStatusId,
                    LookupValueHelper.GetName(x.ProcessingStageStatus),
                    x.StartedAt,
                    x.CompletedAt,
                    x.UpdatedByUserId,
                    x.Remarks))
                .ToArray(),
            processing.QCRecords
                .OrderBy(x => x.RecordedAt)
                .Select(x => new QcRecordResponse(
                    x.Id,
                    x.OrderItemId,
                    x.QCStatusId,
                    LookupValueHelper.GetName(x.QCStatus),
                    x.IssueDescription,
                    x.ActionTaken,
                    x.LabourChargeAmount,
                    x.RecordedByUserId,
                    x.RecordedAt))
                .ToArray());
    }
}