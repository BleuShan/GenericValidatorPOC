using System;

namespace GenericValidatorPOC.Core.Tests.ValidatorErrorFixtures
{
  public class TestModel
  {
    public int Value { get; set; }
  }

  public class ValidationErrorFormatter : IValidationErrorMessageFormatter
  {
    public object? GetFormat(Type? formatType)
    {
      throw new NotImplementedException();
    }
  }
}