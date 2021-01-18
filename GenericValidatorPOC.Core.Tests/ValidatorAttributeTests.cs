using System;

using GenericValidatorPOC.Core.Attributes;

using NUnit.Framework;

namespace GenericValidatorPOC.Core.Tests
{
  public partial class ValidatorAttributeTests
  {
    [Test]
    public void ValidatorAttributeShouldNotHaveANullTargetType()
    {
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
      Assert.Throws<ArgumentNullException>(() => new ValidatorAttribute(null));
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
    }
  }
}