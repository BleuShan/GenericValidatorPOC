using System;
using System.Collections.Generic;
using System.Linq;

namespace GenericValidatorPOC.Core {
  public sealed class ValidatorProvider {
    private static readonly Lazy<ValidatorProvider> InstanceHolder =
      new(() => Builder.Instance.Build());

    private readonly Dictionary<Type, Type> _validators;

    private ValidatorProvider(Builder builder) => _validators = builder.Validators;

    public static ValidatorProvider Instance => InstanceHolder.Value;

    public bool HasValidator<TValidatorT>() => HasValidator(typeof(TValidatorT));

    public bool HasValidator(Type targetType) {
      return _validators.Values.FirstOrDefault(validatorType => validatorType == targetType) !=
             null;
    }

    public bool HasValidatorOf<TArgetT>()
      where TArgetT : new() =>
      HasValidatorOf(typeof(TArgetT));

    public bool HasValidatorOf(Type targetType) => _validators.ContainsKey(targetType);

    public TValidatorT? Create<TValidatorT>(params object?[]? args)
      where TValidatorT : class, new() {
      if (!HasValidator<TValidatorT>())
        throw new KeyNotFoundException($"Could not resolve {typeof(TValidatorT)}");
      return (TValidatorT?) Activator.CreateInstance(typeof(TValidatorT), args);
    }

    public object? CreateValidatorInstanceOf<TArgetT>(params object?[]? args)
      where TArgetT : new() {
      var validatorType = _validators[typeof(TArgetT)];
      return Activator.CreateInstance(validatorType, args);
    }

    public TValidatorT? CreateValidatorInstanceOfAs<TArgetT, TValidatorT>(params object?[]? args)
      where TArgetT : new() =>
      (TValidatorT?) CreateValidatorInstanceOf<TArgetT>(args);

    public IValidator<TArgetT>? CreateValidatorOf<TArgetT>(params object?[]? args)
      where TArgetT : new() =>
      CreateValidatorInstanceOfAs<TArgetT, IValidator<TArgetT>>(args);

    public sealed class Builder {
      internal static readonly Builder Instance = new();
      internal Dictionary<Type, Type> Validators = new();

      internal ValidatorProvider Build() => new(this);

      public Builder Register(Type targetType, Type validatorType) {
        Validators.Add(targetType, validatorType);
        return this;
      }

      public Builder Register<TArgetT, TValidatorT>()
        where TArgetT : new()
        where TValidatorT : IValidator<TArgetT>, new() =>
        Register(typeof(TArgetT), typeof(TValidatorT));
    }
  }
}