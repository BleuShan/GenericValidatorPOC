using System;

using GenericValidatorPOC.Core.Attributes;

namespace GenericValidatorPOC.Core.Tests.Fixtures
{
  public class AttributeRegisteredModel { }

  [Validator(typeof(AttributeRegisteredModel))]
  public class AttributeRegisteredModelValidator
  {
    public void Validate(AttributeRegisteredModel target)
    {
      throw new NotImplementedException();
    }
  }
}