using System;
using System.Collections;
using System.Collections.Immutable;
using System.ComponentModel;
using System.Linq;

namespace GenericValidatorPOC.Core {
  public static class ValidationError {
    public const string DataItemDataKey = "DataItem";

    public const string MessageDataKey = "Message";
    public const string MessageFormatterDataKey = "MessageFormatter";

    public static ValidationError<TDataItemT>.Builder CreateBuilder<TDataItemT>()
      where TDataItemT : new() =>
      new();
  }

  [ImmutableObject(true)]
  public sealed class ValidationError<TDataItemT> : Exception
    where TDataItemT : new() {
    private readonly ImmutableDictionary<string, object?> _data;

    internal ValidationError(Builder builder) : base(builder.Message()) =>
      _data = builder.Data.ToImmutable();

    public TDataItemT? DataItem =>
      _data.TryGetValueAs<string, object?, TDataItemT?>(ValidationError.DataItemDataKey);

    public override IDictionary Data => _data;

    public override string Message => base.Message;

    public sealed class Builder {
      internal ImmutableDictionary<string, object?>.Builder Data =
        ImmutableDictionary.CreateBuilder<string, object?>();

      internal Builder() { }

      public Builder Message(string message) => Update(ValidationError.MessageDataKey, message);

      public Builder Format(FormattableString message) =>
        Update(ValidationError.MessageDataKey, message);

      public Builder Formatter(IValidationErrorMessageFormatter? formatter) =>
        Update(ValidationError.MessageFormatterDataKey, formatter);

      public string? Message() {
        var messageOrFormat = Data.TryGetValue(ValidationError.MessageDataKey);
        switch (messageOrFormat) {
          case string message:
            return message;

          case FormattableString message:
            var args = message.GetArguments()
                              .Select(item => {
                                 switch (item) {
                                   case ValidationErrorMessageDataFieldValuePlaceholder placeholder:
                                     return placeholder.SetValue(Data.TryGetValue(ValidationError
                                                                  .DataItemDataKey));

                                   default:
                                     return item;
                                 }
                               })
                              .ToArray();
            return string.Format(message.Format, args);

          default:
            return null;
        }
      }

      public Builder DataItem(TDataItemT? item) => Update(ValidationError.DataItemDataKey, item);

      public ValidationError<TDataItemT> Build() => new(this);

      public void Throw() => throw Build();

      private Builder Update(string key, object? value) {
        if (Data.ContainsKey(key)) Data.Remove(key);
        Data.Add(key, value);

        return this;
      }
    }
  }
}