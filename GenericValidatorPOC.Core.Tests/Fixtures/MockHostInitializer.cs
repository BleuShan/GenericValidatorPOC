namespace GenericValidatorPOC.Core.Tests.Fixtures
{
  public class MockHostInitializer : IHostInitializer
  {
    public void Configure(ValidatorProvider provider)
    {
      provider.Register<MockHostInitializerModel, MockHostInitializerModelValidator>();
    }
  }

  public class MockHostInitializerModel { }

  public class MockHostInitializerModelValidator : IValidator<MockHostInitializerModel>
  {
    public void Validate(MockHostInitializerModel target)
    {
      throw new System.NotImplementedException();
    }
  }
}