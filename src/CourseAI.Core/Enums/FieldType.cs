namespace CourseAI.Core.Enums;

public enum JobFieldType
{
    /// <summary>
    /// Textbox
    /// </summary>
    Text,

    /// <summary>
    /// Textarea
    /// </summary>
    TextArea,

    /// <summary>
    /// Text array (comma separated)
    /// </summary>
    TextArray,

    Number,
    Address,
    Date,
    Time,
    DateTime,

    /// <summary>
    /// Checkbox
    /// </summary>
    Checkbox,

    /// <summary>
    /// For special fields with custom behaviour
    /// </summary>
    Custom,
}