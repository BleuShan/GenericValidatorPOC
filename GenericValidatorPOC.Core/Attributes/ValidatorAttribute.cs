using System;

namespace GenericValidatorPOC.Core.Attributes {
  [AttributeUsage(AttributeTargets.Class)]
  public sealed class ValidatorAttribute : Attribute {
    public readonly Type TargetType;

    public ValidatorAttribute(Type targetType) {
      if (targetType == null)
        throw new ArgumentNullException($"{nameof(TargetType)} must not be null");

      TargetType = targetType;
    }
  }
}