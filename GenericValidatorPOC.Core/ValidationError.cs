using System;
using System.Collections;
using System.Collections.Immutable;
using System.ComponentModel;
using System.Linq;

namespace GenericValidatorPOC.Core
{
  public static class ValidationError
  {
    public const string DataItemDataKey = "DataItem";

    public const string MesssageDataKey = "Message";
    public const string MesssageFormatterDataKey = "MessageFormatter";

    public static ValidationError<DataItemT>.Builder CreateBuilder<DataItemT>() where DataItemT : new()

    {
      return new();
    }
  }

  [ImmutableObject(true)]
  public sealed class ValidationError<DataItemT> : Exception where DataItemT : new()
  {
    public sealed class Builder
    {
      internal ImmutableDictionary<string, object?>.Builder _data = ImmutableDictionary.CreateBuilder<string, object?>();

      internal Builder()
      {
      }

      public Builder Message(string message)
      {
        return Update(ValidationError.MesssageDataKey, message);
      }

      public Builder Format(FormattableString message)
      {
        return Update(ValidationError.MesssageDataKey, message);
      }

      public Builder Formatter(IValidationErrorMessageFormatter? formatter)
      {
        return Update(ValidationError.MesssageFormatterDataKey, formatter);
      }

      public string? Message()
      {
        var messageOrFormat = _data.TryGetValue(ValidationError.MesssageDataKey);
        switch (messageOrFormat)
        {
          case string message:
            return message;

          case FormattableString message:
            var args = message.GetArguments().Select(item =>
            {
              switch (item)
              {
                case ValidationErrorMessageDataFieldValuePlaceholder placeholder:
                  return placeholder.SetValue(_data.TryGetValue(ValidationError.DataItemDataKey));

                default:
                  return item;
              }
            }).ToArray();
            return string.Format(message.Format, args);

          default:
            return null;
        }
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
        return base.Message;
      }
    }

    internal ValidationError(Builder builder) : base(builder.Message())
    {
      _data = builder._data.ToImmutable();
    }
  }
}