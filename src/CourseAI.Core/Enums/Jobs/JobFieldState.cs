namespace CourseAI.Core.Enums.Jobs;

public enum JobFieldState : byte
{
    /// <summary>
    /// False
    /// </summary>
    Unchecked = 0,

    /// <summary>
    /// True
    /// </summary>
    Checked = 1,


    // /// <summary>
    // /// No, can be checked
    // /// </summary>
    // Hidden = 3,

    /// <summary>
    /// Yes, cannot be unchecked
    /// </summary>
    Required = 2,
}