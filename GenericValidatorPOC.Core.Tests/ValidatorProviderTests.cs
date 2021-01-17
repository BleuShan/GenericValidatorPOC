using NUnit.Framework;
using GenericValidatorPOC.Core;
using System.Reflection;

namespace GenericValidatorPOC.Core.Tests
{
  public partial class ValidatorProviderTests
  {
    [Test]
    public void ValidatorProviderShouldHaveHostInitializerRegisteredValidators()
    {
      Assert.IsTrue(ValidatorProvider.Instance.HasValidatorOf<Fixtures.MockHostInitializerModel>());
    }

    [Test]
    public void ValidatorProviderShouldHaveValidatorAttributeRegisteredValidators()
    {
      Assert.IsTrue(ValidatorProvider.Instance.HasValidatorOf<Fixtures.AttributeRegisteredModel>());
    }
  }
}