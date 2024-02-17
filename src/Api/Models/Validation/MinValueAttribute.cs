using System.ComponentModel.DataAnnotations;

namespace ECommerce.Models.Validation;

public class MinValueAttribute : ValidationAttribute
{
    private readonly int _minValue;

    public MinValueAttribute(int minValue)
    {
        _minValue = minValue;
    }

    public override bool IsValid(object value)
    {
        if (value is int intValue) return intValue >= _minValue;

        return false;
    }

    public override string FormatErrorMessage(string name)
    {
        return $"The field {name} must be greater than or equal to {_minValue}.";
    }
}