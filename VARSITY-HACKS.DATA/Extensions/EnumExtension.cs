using System.ComponentModel;
using System.Reflection;

namespace VARSITY_HACKS.DATA;

public static class EnumExtension
{
    public static string GetDescription(this Enum value)
    {
        var fieldInfo = value.GetType().GetField(value.ToString());
        var attribute = (DescriptionAttribute)fieldInfo!.GetCustomAttribute(typeof(DescriptionAttribute))!;
        return attribute.Description;
    }
}