using System;
using System.Collections;
using System.Collections.Immutable;

namespace GenericValidatorPOC.Core
{
  public static class ValidationError
  {
    public const string DataItemDataKey = "DataItem";

    public const string MesssageDataKey = "Message";

    public static ValidationError<DataItemT>.Builder CreateBuilder<DataItemT>() where DataItemT : new()

    {
      return new();
    }
  }

  public sealed class ValidationError<DataItemT> : Exception where DataItemT : new()
  {
    public sealed class Builder
    {
      internal ImmutableDictionary<string, object?>.Builder _data = ImmutableDictionary.CreateBuilder<string, object?>();

      internal Builder()
      {
      }

      public Builder Message(string? message)
      {
        return Update(ValidationError.MesssageDataKey, message);
      }

      public string? Message()
      {
        return _data.TryGetValueAs<string, object?, string?>(ValidationError.MesssageDataKey);
      }

      public Builder DataItem(DataItemT? item)
      {
        return Update(ValidationError.DataItemDataKey, item);
      }

      public ValidationError<DataItemT> Build()
      {
        return new(this);
      }

      public void Throw()
      {
        throw this.Build();
      }

      private Builder Update(string key, object? value)
      {
        if (_data.ContainsKey(key))
        {
          _data.Remove(key);
        }
        _data.Add(key, value);

        return this;
      }
    }

    private ImmutableDictionary<string, object?> _data;

    public DataItemT? DataItem
    {
      get
      {
        return _data.TryGetValueAs<string, object?, DataItemT?>(ValidationError.DataItemDataKey);
      }
    }

    public override IDictionary Data { get { return _data; } }

    public override string Message
    {
      get
      {
        return _data.TryGetValueAs<string, object?, string?>(ValidationError.DataItemDataKey) ?? base.Message;
      }
    }

    internal ValidationError(Builder builder) : base(builder.Message())
    {
      _data = builder._data.ToImmutable();
    }
  }
}