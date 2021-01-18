using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace GenericValidatorPOC.Core
{
  public class ValidationErrorMessageDataFieldValuePlaceholder : IFormattable
  {
    private readonly string _key;
    private readonly object? _idx;
    private object? _value;

    public ValidationErrorMessageDataFieldValuePlaceholder(string key, object? idx = null)
    {
      _key = key;
      _idx = idx;
    }

    internal ValidationErrorMessageDataFieldValuePlaceholder SetValue(object? dataItem)
    {
      if (dataItem == null) return this;
      var dataItemType = dataItem.GetType();

      var propOrField = dataItemType.GetMembers().FirstOrDefault(member =>
        member.Name == _key &&
        (member.MemberType == MemberTypes.Field || member.MemberType == MemberTypes.Property)) ?? dataItemType.GetProperty("Item");

      switch (propOrField)
      {
        case FieldInfo field:
          _value = field.GetValue(dataItem);
          break;

        case PropertyInfo prop:
          var indexValue = _idx ?? _key;
          var hasMatchingIndex = prop.GetIndexParameters().FirstOrDefault(param => param.ParameterType == indexValue.GetType()) != null;
          var index = hasMatchingIndex ? new object[] { indexValue } : null;
          _value = prop.GetValue(dataItem, index);
          break;

        default:
          throw new KeyNotFoundException($"{_key} could not be found on ${nameof(dataItem)}");
      }

      return this;
    }

    public string ToString(string? format, IFormatProvider? formatProvider)
    {
      if (format != null) return string.Format(formatProvider, $"{{0:{format}}}", _value);
      return _value?.ToString() ?? string.Empty;
    }
  }
}