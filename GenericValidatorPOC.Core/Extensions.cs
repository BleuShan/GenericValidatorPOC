using System.Collections.Generic;

namespace GenericValidatorPOC.Core
{
  public static class Extensions
  {
    public static TValue? TryGetValue<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key) where TKey : notnull
    {
      TValue? value;
      dict.TryGetValue(key, out value);
      return value;
    }

    public static TResult? TryGetValueAs<TKey, TValue, TResult>(this IDictionary<TKey, TValue> dict, TKey key) where TKey : notnull where TResult : TValue
    {
      return (TResult?)dict.TryGetValue(key);
    }
  }
}