using System;
using System.Collections.Generic;

using GenericValidatorPOC.Core.Tests.ValidatorProviderFixtures;

using NUnit.Framework;

namespace GenericValidatorPOC.Core.Tests
{
  public partial class ValidatorProviderTests
  {
    [Test]
    public void ValidatorProviderShouldHaveHostInitializerRegisteredValidators()
    {
      Assert.IsTrue(ValidatorProvider.Instance.HasValidatorOf<MockHostInitializerRegisteredModel>());
    }

    [Test]
    public void ValidatorProviderShouldHaveValidatorAttributeRegisteredValidators()
    {
      Assert.IsTrue(ValidatorProvider.Instance.HasValidatorOf<AttributeRegisteredModel>());
    }

    [Test]
    public void ValidatorProviderShouldBeAbleToCreateValidatorsWithATypeSafeApi()
    {
      Assert.Throws<InvalidMockHostInitializerRegisteredModelMessageError>(() =>
      {
        var validator = ValidatorProvider.Instance.CreateValidatorOf<MockHostInitializerRegisteredModel>();
        var model = new MockHostInitializerRegisteredModel();
        validator?.Validate(model);
      });
    }

    [Test]
    public void ValidatorProviderShouldBeAbleToCreateValidatorsWithArguments()
    {
      var expectedTag = "Tag";
      var validator = ValidatorProvider.Instance.Create<AttributeRegisteredModelValidator>(expectedTag);
      Assert.AreEqual(expectedTag, validator?.Tag);
    }

    [Test]
    public void ValidatorProviderShouldThrowWhenCreatingIncompatibleValidatorsWithTheTypeSafeApi()
    {
      Assert.Throws<InvalidCastException>(() =>
      {
        var validator = ValidatorProvider.Instance.CreateValidatorOf<AttributeRegisteredModel>();
      });
    }

    [Test]
    public void ValidatorProviderShouldThrowWithInvalidUseOfTheCastApi()
    {
      Assert.Throws<InvalidCastException>(() =>
      {
        var validator = ValidatorProvider.Instance.CreateValidatorInstanceOfAs<MockHostInitializerRegisteredModel, AttributeRegisteredModelValidator>();
      });
    }

    [Test]
    public void ValidatorProviderShouldThrowWithUnregisteredModelTypes()
    {
      Assert.Throws<KeyNotFoundException>(() =>
      {
        var validator = ValidatorProvider.Instance.CreateValidatorOf<UnregisteredModel>();
      });
    }

    public void ValidatorProviderShouldThrowWithUnregisteredValidatorTypes()
    {
      Assert.Throws<KeyNotFoundException>(() =>
      {
        var validator = ValidatorProvider.Instance.Create<UnregisteredValidator>();
      });
    }
  }
}