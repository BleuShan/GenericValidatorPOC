using System;

namespace GenericValidatorPOC.Core.Tests.ValidatorErrorFixtures {
  public class ValidatorErrorTestModel {
    public int Value { get; set; }
  }

  public class ValidatorErrorTestFormatter : IValidationErrorMessageFormatter {
    public string Format(string? format, object? arg, IFormatProvider? formatProvider) =>
      string.Format(formatProvider, "{0}", arg);

    public object? GetFormat(Type? formatType) =>
      GetType().IsAssignableTo(formatType) ? this : null;
  }
}