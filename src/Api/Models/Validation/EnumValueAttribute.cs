namespace ECommerce.Models.Validation;

using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

public class EnumValueAttribute : ValidationAttribute
{
    private readonly Type _enumType;

    public EnumValueAttribute(Type enumType)
    {
        _enumType = enumType;
    }

    public override bool IsValid(object value)
    {
        if (value is not string stringValue) return false;

        return Enum.TryParse(_enumType, stringValue, true, out _);
    }

    public override string FormatErrorMessage(string name)
    {
        var validOptions = string.Join(", ", Enum.GetNames(_enumType));
        return $"The field {name} must be one of the following: {validOptions}.";
    }
}