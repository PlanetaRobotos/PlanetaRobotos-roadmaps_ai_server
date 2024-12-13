namespace CourseAI.Core.Enums;

/// <summary>
/// Proof of Delivery
/// </summary>
public enum PODBehavior
{
    /// <summary>
    /// Signature is required to confirm delivery or collection
    /// </summary>
    RequireSignature,

    /// <summary>
    /// Photo is required to confirm delivery or collection
    /// </summary>
    RequirePhoto,

    /// <summary>
    /// Signature or Photo is required to confirm delivery or collection
    /// </summary>
    RequirePhotoOrSignature,
}