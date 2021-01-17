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

    public ValidatorT? Create<ValidatorT>(params object?[]? args) where ValidatorT : new()
    {
      var targetType = typeof(ValidatorT);
      var validatorType = _validators.Values.FirstOrDefault(validatorType => validatorType == targetType);
      if (validatorType == null)
      {
        throw new KeyNotFoundException($"Could not resolve {validatorType}");
      }
      return (ValidatorT?)Activator.CreateInstance(validatorType, args);
    }

    public object? CreateValidatorInstanceOf<TargetT>(params object?[]? args) where TargetT : new()
    {
      var validatorType = _validators[typeof(TargetT)];
      return Activator.CreateInstance(validatorType, args);
    }

    public ValidatorT? CreateValidatorInstanceOfAs<TargetT, ValidatorT>(params object?[]? args) where TargetT : new()
    {
      return (ValidatorT?)CreateValidatorInstanceOf<TargetT>(args);
    }

    public IValidator<TargetT>? CreateValidatorOf<TargetT>(params object?[]? args) where TargetT : new()
    {
      return CreateValidatorInstanceOfAs<TargetT, IValidator<TargetT>>(args);
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