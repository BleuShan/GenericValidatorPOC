using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Reflection;
using System.Linq;

namespace GenericValidatorPOC.Core
{
  public class ValidatorProvider
  {
    private static readonly Lazy<ValidatorProvider> _holder = new Lazy<ValidatorProvider>(() => new ValidatorProvider());
    private readonly ConcurrentDictionary<Type, Type> _validators = new ConcurrentDictionary<Type, Type>();

    public static ValidatorProvider Instance
    {
      get
      {
        return _holder.Value;
      }
    }

    private ValidatorProvider()
    {
    }

    public bool HasValidatorOf<TargetT>() where TargetT : new()
    {
      return HasValidatorOf(typeof(TargetT));
    }

    public bool HasValidatorOf(Type targetType)
    {
      return _validators.ContainsKey(targetType);
    }

    public object? Create<TargetT>(params object?[]? args) where TargetT : new()
    {
      var validatorType = _validators.GetValueOrDefault(typeof(TargetT));
      if (validatorType != null)
      {
        return Activator.CreateInstance(validatorType, args);
      }

      return null;
    }

    public ValidatorT? CreateAs<TargetT, ValidatorT>(params object?[]? args) where TargetT : new()
    {
      return (ValidatorT?)Create<TargetT>(args);
    }

    public IValidator<TargetT>? CreateValidatorOf<TargetT>(params object?[]? args) where TargetT : new()
    {
      return CreateAs<TargetT, IValidator<TargetT>>(args);
    }

    public ValidatorProvider Register(Type targetType, Type validatorType)
    {
      if (HasValidatorOf(targetType)) return this;
      _validators.TryAdd(targetType, validatorType);
      return this;
    }

    public ValidatorProvider Register<TargetT, ValidatorT>() where TargetT : new() where ValidatorT : IValidator<TargetT>, new()
    {
      return Register(typeof(TargetT), typeof(ValidatorT));
    }
  }
}