namespace Fleet.Core.Enums;

/// <summary>
///  Customize the failed collection reasons and reject item reasons.
///  Default list of reasons will be used if no reasons are entered.
/// </summary>
public enum FailureReasonType
{
    // Collections
    FailedCollection,
    RejectCollectionItem,
    FailedCollectionWithSignature,

    // Deliveries
    FailedDelivery,
    RejectDeliveryItem,
    FailedDeliveryWithSignature,
}