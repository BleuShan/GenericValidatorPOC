using GenericValidatorPOC.Core.Tests.ValidatorErrorFixtures;
using NUnit.Framework;

namespace GenericValidatorPOC.Core.Tests {
  public class ValidationErrorTests {
    private ValidationError<ValidatorErrorTestModel>.Builder? _builder;
    private ValidatorErrorTestModel? _expectedModel;

    [SetUp]
    public void Setup() {
      _builder = ValidationError.CreateBuilder<ValidatorErrorTestModel>();
      _expectedModel = new ValidatorErrorTestModel();
    }

    [Test]
    public void ValidationErrorShouldBeAbleToBeThrown() {
      Assert.Throws<ValidationError<ValidatorErrorTestModel>>(() => _builder?.Throw());
    }

    [Test]
    public void ValidationErrorShouldBeAbleToReportValidatedValue() {
      var error = _builder?.DataItem(_expectedModel).Build();
      Assert.AreSame(_expectedModel, error?.DataItem);
    }

    [Test]
    public void ValidationErrorShouldBeAbleToReportMessages() {
      var expectedMessage = "Message";
      var error = _builder?.Message(expectedMessage).Build();
      Assert.AreEqual(expectedMessage, error?.Message);
    }

    [Test]
    public void ValidationErrorShouldBeAbleToReportFormattedMessages() {
      if (_expectedModel != null) _expectedModel.Value = 5;

      var error = _builder
                ?.Format($"{new ValidationErrorMessageDataFieldValuePlaceholder("Value"):0.00f}")
                 .DataItem(_expectedModel)
                 .Build();
      Assert.AreEqual($"{_expectedModel?.Value:0.00f}", error?.Message);
    }

    [Test]
    public void ValidationErrorShouldBeAbleToReportCustomFormattedMessages() {
      const string expectedMessage = "Integer 5";
      var error = _builder?.Format($"{5:Type}")
                           .Formatter(new ValidatorErrorTestFormatter())
                           .Build();
      Assert.AreEqual(expectedMessage, error?.Message);
    }
  }
}