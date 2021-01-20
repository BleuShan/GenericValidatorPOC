using System;
using GenericValidatorPOC.Core.Attributes;

namespace GenericValidatorPOC.Core.Tests.ValidatorProviderFixtures {
  public class MockHostInitializer : IHostInitializer {
    public void Configure(ValidatorProvider.Builder builder) {
      builder
       .Register<MockHostInitializerRegisteredModel, MockHostInitializerRegisteredModelValidator>();
    }
  }

  public class MockHostInitializerRegisteredModel {
    public string? Message;
  }

  public class InvalidMockHostInitializerRegisteredModelMessageError : Exception {
    public readonly MockHostInitializerRegisteredModel DataItem;

    public InvalidMockHostInitializerRegisteredModelMessageError(
      MockHostInitializerRegisteredModel item) :
      base($"{nameof(item.Message)} must not be empty") =>
      DataItem = item;
  }

  public class
    MockHostInitializerRegisteredModelValidator : IValidator<MockHostInitializerRegisteredModel> {
    public void Validate(MockHostInitializerRegisteredModel model) {
      if (string.IsNullOrWhiteSpace(model.Message))
        throw new InvalidMockHostInitializerRegisteredModelMessageError(model);
    }
  }

  public class AttributeRegisteredModel {
    public string? Kind { get; set; }
  }

  [Validator(typeof(AttributeRegisteredModel))]
  public class AttributeRegisteredModelValidator {
    public readonly string? Tag;

    public AttributeRegisteredModelValidator() { }

    public AttributeRegisteredModelValidator(string tag) => Tag = tag;

    public static bool IsValid(AttributeRegisteredModel target) => target is not null;
  }

  public class UnregisteredModel { }

  public class UnregisteredValidator : IValidator<UnregisteredModel> {
    public void Validate(UnregisteredModel target) {
      throw new NotImplementedException();
    }
  }
}