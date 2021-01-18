using System;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace GenericValidatorPOC.Core
{
  internal static class Module
  {
    [ModuleInitializer]
    public static void Initialize()
    {
      var definedTypes = AppDomain.CurrentDomain.GetAssemblies().SelectMany((assembly) => assembly.DefinedTypes);
      var validatorTypes = definedTypes.Where(typeInfo => typeInfo.GetCustomAttribute<Attributes.ValidatorAttribute>() != null);
      var initializerTypes = definedTypes.Where(typeInfo => typeInfo.ImplementedInterfaces.Contains(typeof(IHostInitializer)));
      var builder = ValidatorProvider.Builder.Instance;

      foreach (var initializerType in initializerTypes)
      {
        var initializer = (IHostInitializer?)Activator.CreateInstance(initializerType);
        initializer?.Configure(builder);
      }

      foreach (var validatorType in validatorTypes)
      {
        var validatorAttribute = validatorType.GetCustomAttribute<Attributes.ValidatorAttribute>();
        if (validatorAttribute != null)
        {
          builder.Register(validatorAttribute.TargetType, validatorType);
        }
      }
    }
  }
}