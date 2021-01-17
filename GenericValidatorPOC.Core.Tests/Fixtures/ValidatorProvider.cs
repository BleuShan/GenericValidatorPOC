using GenericValidatorPOC.Core;
using GenericValidatorPOC.Core.Attributes;

namespace GenericValidatorPOC.Core.Tests.ValidatorProviderFixtures
{
  public class MockHostInitializer : IHostInitializer
  {
    public void Configure(ValidatorProvider provider)
    {
      provider.Register<MockHostInitializerModel, MockHostInitializerModelValidator>();
    }
  }

  public class MockHostInitializerModel
  {
    public string? Message;
  }

  public class InvalidMockHostInitializerModelMessageError : System.Exception
  {
    public readonly MockHostInitializerModel DataItem;

    public InvalidMockHostInitializerModelMessageError(MockHostInitializerModel item) : base($"{nameof(item.Message)} must not be empty")
    {
      DataItem = item;
    }
  }

  public class MockHostInitializerModelValidator : IValidator<MockHostInitializerModel>
  {
    public void Validate(MockHostInitializerModel model)
    {
      if (string.IsNullOrWhiteSpace(model.Message))
      {
        throw new InvalidMockHostInitializerModelMessageError(model);
      }
    }
  }

  public class AttributeRegisteredModel { }

  [Validator(typeof(AttributeRegisteredModel))]
  public class AttributeRegisteredModelValidator
  {
    public readonly string? Tag;

    public AttributeRegisteredModelValidator()
    {
    }

    public AttributeRegisteredModelValidator(string tag)
    {
      Tag = tag;
    }

    public bool IsValid(AttributeRegisteredModel target)
    {
      return false;
    }
  }

  public class UnregisteredModel { }

  public class UnregisteredValidator : IValidator<UnregisteredModel>
  {
    public void Validate(UnregisteredModel target)
    {
      throw new System.NotImplementedException();
    }
  }
}