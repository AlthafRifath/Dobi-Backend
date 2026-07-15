using Dobi.Application.Abstractions.Persistence;
using Dobi.Contracts.Lookups;
using Dobi.Shared.Constants;
using Dobi.Shared.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dobi.Application.Features.Lookups.GetLookupValues;

public sealed class GetLookupValuesQueryHandler
    : IRequestHandler<GetLookupValuesQuery, IReadOnlyCollection<LookupResponse>>
{
    private readonly IDobiDbContext _dbContext;

    public GetLookupValuesQueryHandler(IDobiDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyCollection<LookupResponse>> Handle(
        GetLookupValuesQuery request,
        CancellationToken cancellationToken)
    {
        var lookupType = request.LookupType.Trim().ToLowerInvariant();

        return lookupType switch
        {
            LookupTypes.CustomerTypes => await ToLookupResponseAsync(
                _dbContext.CustomerTypes,
                cancellationToken),

            LookupTypes.PricingTypes => await ToLookupResponseAsync(
                _dbContext.PricingTypes,
                cancellationToken),

            LookupTypes.Services => await ToLookupResponseAsync(
                _dbContext.Services,
                cancellationToken),

            LookupTypes.ItemCategories => await ToLookupResponseAsync(
                _dbContext.ItemCategories,
                cancellationToken),

            LookupTypes.OrderStatuses => await ToLookupResponseAsync(
                _dbContext.OrderStatuses,
                cancellationToken),

            LookupTypes.PaymentStatuses => await ToLookupResponseAsync(
                _dbContext.PaymentStatuses,
                cancellationToken),

            LookupTypes.InspectionIssueTypes => await ToLookupResponseAsync(
                _dbContext.InspectionIssueTypes,
                cancellationToken),

            LookupTypes.TransferTypes => await ToLookupResponseAsync(
                _dbContext.TransferTypes,
                cancellationToken),

            LookupTypes.TransferStatuses => await ToLookupResponseAsync(
                _dbContext.TransferStatuses,
                cancellationToken),

            LookupTypes.AcknowledgementTypes => await ToLookupResponseAsync(
                _dbContext.AcknowledgementTypes,
                cancellationToken),

            LookupTypes.ProcessingStages => await ToLookupResponseAsync(
                _dbContext.ProcessingStages,
                cancellationToken),

            LookupTypes.ProcessingStageStatuses => await ToLookupResponseAsync(
                _dbContext.ProcessingStageStatuses,
                cancellationToken),

            LookupTypes.QcStatuses => await ToLookupResponseAsync(
                _dbContext.QCStatuses,
                cancellationToken),

            LookupTypes.CollectionModes => await ToLookupResponseAsync(
                _dbContext.CollectionModes,
                cancellationToken),

            LookupTypes.PaymentMethods => await ToLookupResponseAsync(
                _dbContext.PaymentMethods,
                cancellationToken),

            LookupTypes.RefundStatuses => await ToLookupResponseAsync(
                _dbContext.RefundStatuses,
                cancellationToken),

            LookupTypes.NotificationTypes => await ToLookupResponseAsync(
                _dbContext.NotificationTypes,
                cancellationToken),

            LookupTypes.NotificationStatuses => await ToLookupResponseAsync(
                _dbContext.NotificationStatuses,
                cancellationToken),

            _ => throw new BadRequestException($"Invalid lookup type '{request.LookupType}'.")
        };
    }

    private static async Task<IReadOnlyCollection<LookupResponse>> ToLookupResponseAsync<T>(
        IQueryable<T> query,
        CancellationToken cancellationToken)
        where T : class
    {
        var rows = await query
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return rows
            .Where(IsActive)
            .Select(ToLookupResponse)
            .OrderBy(x => x.Name)
            .ToArray();
    }

    private static LookupResponse ToLookupResponse<T>(T entity)
        where T : class
    {
        var id = GetIntProperty(entity, "Id");

        var code = GetStringProperty(
            entity,

            // Generic/common
            "Code",

            // Customer / pricing / service / item
            "CustomerTypeCode",
            "PricingTypeCode",
            "ServiceCode",
            "ItemCategoryCode",
            "CategoryCode",

            // Order / payment statuses
            "OrderStatusCode",
            "PaymentStatusCode",
            "StatusCode",

            // Inspection
            "InspectionIssueTypeCode",
            "IssueTypeCode",
            "IssueCode",

            // Transfer
            "TransferTypeCode",
            "TransferStatusCode",
            "AcknowledgementTypeCode",
            "AcknowledgementCode",

            // Plant processing
            "ProcessingStageCode",
            "StageCode",
            "ProcessingStageStatusCode",
            "QCStatusCode",

            // Collection / payment / refund
            "CollectionModeCode",
            "PaymentMethodCode",
            "MethodCode",
            "RefundStatusCode",

            // Notification
            "NotificationTypeCode",
            "NotificationStatusCode",
            "TypeCode") ?? id.ToString();

        var name = GetStringProperty(
            entity,

            // Generic/common
            "Name",

            // Customer / pricing / service / item
            "CustomerTypeName",
            "PricingTypeName",
            "ServiceName",
            "ItemCategoryName",
            "CategoryName",

            // Order / payment statuses
            "OrderStatusName",
            "PaymentStatusName",
            "StatusName",

            // Inspection
            "InspectionIssueTypeName",
            "IssueTypeName",
            "IssueName",

            // Transfer
            "TransferTypeName",
            "TransferStatusName",
            "AcknowledgementTypeName",
            "AcknowledgementName",

            // Plant processing
            "ProcessingStageName",
            "StageName",
            "ProcessingStageStatusName",
            "QCStatusName",

            // Collection / payment / refund
            "CollectionModeName",
            "PaymentMethodName",
            "MethodName",
            "RefundStatusName",

            // Notification
            "NotificationTypeName",
            "NotificationStatusName",
            "TypeName") ?? code;

        return new LookupResponse(
            id,
            code,
            name);
    }

    private static bool IsActive<T>(T entity)
        where T : class
    {
        var property = entity.GetType().GetProperty("IsActive");

        if (property is null)
        {
            return true;
        }

        var value = property.GetValue(entity);

        return value is not bool isActive || isActive;
    }

    private static int GetIntProperty<T>(T entity, string propertyName)
        where T : class
    {
        var property = entity.GetType().GetProperty(propertyName);

        if (property is null)
        {
            throw new InvalidOperationException(
                $"Property '{propertyName}' was not found on '{entity.GetType().Name}'.");
        }

        var value = property.GetValue(entity);

        if (value is int intValue)
        {
            return intValue;
        }

        throw new InvalidOperationException(
            $"Property '{propertyName}' on '{entity.GetType().Name}' is not an integer.");
    }

    private static string? GetStringProperty<T>(
        T entity,
        params string[] propertyNames)
        where T : class
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