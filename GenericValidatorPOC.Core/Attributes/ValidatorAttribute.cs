using System;

namespace GenericValidatorPOC.Core.Attributes
{
  [AttributeUsage(AttributeTargets.Class)]
  public sealed class ValidatorAttribute : Attribute
  {
    public readonly Type TargetType;

    public ValidatorAttribute(Type targetType)
    {
      TargetType = targetType;
    }
  }
}