using NUnit.Framework;

namespace GenericValidatorPOC.Core.Tests
{
  public partial class ValidationErrorTests
  {
    private ValidationError<ValidatorErrorFixtures.TestModel>.Builder? builder;
    private ValidatorErrorFixtures.TestModel? expectedModel;

    [SetUp]
    public void Setup()
    {
      builder = ValidationError.CreateBuilder<ValidatorErrorFixtures.TestModel>();
      expectedModel = new();
    }

    [Test]
    public void ValidationErrorShouldBeAbleToBeThrown()
    {
      Assert.Throws<ValidationError<ValidatorErrorFixtures.TestModel>>(() => { builder?.Throw(); });
    }

    [Test]
    public void ValidationErrorShouldBeAbleToReportValidatedValue()
    {
      var error = builder?.DataItem(expectedModel).Build();
      Assert.AreSame(expectedModel, error?.DataItem);
    }

    [Test]
    public void ValidationErrorShouldBeAbleToReportMessages()
    {
      var expectedMessage = "Message";
      var error = builder?.Message(expectedMessage).Build();
      Assert.AreEqual(expectedMessage, error?.Message);
    }
  }
}