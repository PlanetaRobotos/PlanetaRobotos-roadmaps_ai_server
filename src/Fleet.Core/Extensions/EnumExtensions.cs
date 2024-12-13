using Fleet.Core.Enums;

namespace Fleet.Core.Extensions;

public static class EnumExtensions
{
    public static bool ToBoolean(this JobFieldState fieldState)
    {
        return fieldState is JobFieldState.Checked or JobFieldState.Required;
    }
}