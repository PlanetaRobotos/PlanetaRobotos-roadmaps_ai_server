using CourseAI.Core.Enums;
using CourseAI.Core.Enums.Jobs;

namespace CourseAI.Core.Extensions;

public static class EnumExtensions
{
    public static bool ToBoolean(this JobFieldState fieldState)
    {
        return fieldState is JobFieldState.Checked or JobFieldState.Required;
    }
}