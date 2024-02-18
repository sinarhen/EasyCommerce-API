using System.ComponentModel.DataAnnotations;

namespace ECommerce.Models.Validation;

public class MaxValueAttribute : ValidationAttribute
{
    private readonly int _maxValue;
    
    public MaxValueAttribute(int maxValue)
    {
        _maxValue = maxValue;
    }
    
    public override bool IsValid(object value)
    {
        if (value is int intValue) return intValue <= _maxValue;
        
        return false;
    }
    
    public override string FormatErrorMessage(string name)
    {
        return $"The field {name} must be less than or equal to {_maxValue}.";
    }
}