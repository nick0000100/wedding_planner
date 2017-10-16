using System;
using System.ComponentModel.DataAnnotations;

public class CustomDateAttribute : ValidationAttribute
{
    public override bool IsValid(object value)
    {
        DateTime d = Convert.ToDateTime(value);
        return d > DateTime.Now;

    }
}