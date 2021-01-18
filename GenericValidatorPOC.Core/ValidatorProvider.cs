using System;
using System.Collections.Generic;
using System.Linq;

namespace GenericValidatorPOC.Core
{
  public sealed class ValidatorProvider
  {
    public sealed class Builder
    {
      internal static readonly Builder Instance = new();
      internal Dictionary<Type, Type> _validators = new();

      internal ValidatorProvider Build()
      {
        return new ValidatorProvider(this);
      }

      public ValidatorProvider.Builder Register(Type targetType, Type validatorType)
      {
        _validators.Add(targetType, validatorType);
        return this;
      }

      public ValidatorProvider.Builder Register<TargetT, ValidatorT>() where TargetT : new() where ValidatorT : IValidator<TargetT>, new()
      {
        return Register(typeof(TargetT), typeof(ValidatorT));
      }
    }

    private static readonly Lazy<ValidatorProvider> _instanceHolder = new(() => Builder.Instance.Build());
    private readonly Dictionary<Type, Type> _validators;

    public static ValidatorProvider Instance
    {
      get
      {
        return _instanceHolder.Value;
      }
    }

    private ValidatorProvider(Builder builder)
    {
      _validators = builder._validators;
    }

    public bool HasValidator<ValidatorT>()
    {
      return HasValidator(typeof(ValidatorT));
    }

    public bool HasValidator(Type targetType)
    {
      return _validators.Values.FirstOrDefault(validatorType => validatorType == targetType) != null;
    }

    public bool HasValidatorOf<TargetT>() where TargetT : new()
    {
      return HasValidatorOf(typeof(TargetT));
    }

    public bool HasValidatorOf(Type targetType)
    {
      return _validators.ContainsKey(targetType);
    }

    public ValidatorT? Create<ValidatorT>(params object?[]? args) where ValidatorT : class, new()
    {
      if (!HasValidator<ValidatorT>())
      {
        throw new KeyNotFoundException($"Could not resolve {typeof(ValidatorT)}");
      }
      return (ValidatorT?)Activator.CreateInstance(typeof(ValidatorT), args);
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
  }
}