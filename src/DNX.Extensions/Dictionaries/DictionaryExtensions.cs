using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using DNX.Extensions.Exceptions;
using DNX.Extensions.Linq;
using DNX.Extensions.Validation;

namespace DNX.Extensions.Dictionaries;

/// <summary>
/// Dictionary Extensions
/// </summary>
public static class DictionaryExtensions
{
    /// <summary>
    /// Gets the specified key value, or default value if key not found
    /// </summary>
    /// <typeparam name="TK">The type of the k.</typeparam>
    /// <typeparam name="TV">The type of the v.</typeparam>
    /// <param name="dictionary">The dictionary.</param>
    /// <param name="key">The key.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <returns></returns>
    public static TV GetValue<TK, TV>(this IDictionary<TK, TV> dictionary, TK key, TV defaultValue = default)
    {
        Guard.IsNotNull(() => dictionary);

        if (key == null)
        {
            throw new ArgumentNullException(nameof(key));
        }

        return dictionary.TryGetValue(key, out var value)
            ? value
            : defaultValue;
    }

    /// <summary>
    /// Sets the value.
    /// </summary>
    /// <typeparam name="TK">The type of the tk.</typeparam>
    /// <typeparam name="TV">The type of the tv.</typeparam>
    /// <param name="dictionary">The dictionary.</param>
    /// <param name="keyName">Name of the key.</param>
    /// <param name="value">The value.</param>
    /// <exception cref="System.ArgumentNullException">dictionary or keyName</exception>
    public static void SetValue<TK, TV>(this IDictionary<TK, TV> dictionary, TK keyName, TV value)
    {
        Guard.IsNotNull(() => dictionary);

        if (keyName == null)
        {
            throw new ArgumentNullException(nameof(keyName));
        }

        if (dictionary.ContainsKey(keyName))
        {
            dictionary[keyName] = value;
        }
        else
        {
            dictionary.Add(keyName, value);
        }
    }

    /// <summary>
    /// Renames the key.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="dictionary">The dictionary.</param>
    /// <param name="fromKeyName">Name of from key.</param>
    /// <param name="toKeyName">Name of to key.</param>
    /// <exception cref="System.ArgumentNullException">fromKeyName or toKeyName</exception>
    public static void RenameKey<T>(this IDictionary<string, T> dictionary, string fromKeyName, string toKeyName)
    {
        if (fromKeyName == null)
        {
            throw new ArgumentNullException(nameof(fromKeyName));
        }
        if (toKeyName == null)
        {
            throw new ArgumentNullException(nameof(toKeyName));
        }

        if (dictionary == null || !dictionary.ContainsKey(fromKeyName) || dictionary.ContainsKey(toKeyName))
        {
            return;
        }

        var old = dictionary[fromKeyName];
        dictionary.Remove(fromKeyName);
        dictionary.SetValue(toKeyName, old);
    }

    /// <summary>
    /// Converts a string to Dictionary&lt;string, string&gt;
    /// </summary>
    /// <param name="text">The text.</param>
    /// <param name="elementSeparator">The element separator.</param>
    /// <param name="valueSeparator">The value separator.</param>
    /// <returns></returns>
    public static IDictionary<string, string> ToStringDictionary(this string text, string elementSeparator = "|", string valueSeparator = "=")
    {
        var dictionary = (text ?? string.Empty)
            .Split(elementSeparator.ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
            .ToDictionary(
                x => x.Split(valueSeparator.ToCharArray()).FirstOrDefault(),
                x => string.Join(valueSeparator, x.Split(valueSeparator.ToCharArray()).Skip(1))
            );

        return dictionary;
    }

    /// <summary>
    /// Converts a string to Dictionary&lt;string, object&gt;
    /// </summary>
    /// <param name="text">The text.</param>
    /// <param name="elementSeparator">The element separator.</param>
    /// <param name="valueSeparator">The value separator.</param>
    /// <returns></returns>
    public static IDictionary<string, object> ToStringObjectDictionary(this string text, string elementSeparator = "|", string valueSeparator = "=")
    {
        var dictionary = text
            .ToStringDictionary(elementSeparator, valueSeparator)
            .ToDictionary(
                x => x.Key,
                x => (object)x.Value
            );

        return dictionary;
    }

    /// <summary>
    /// Merges this dictionary with another one
    /// </summary>
    /// <typeparam name="TK">The type of the tk.</typeparam>
    /// <typeparam name="TV">The type of the tv.</typeparam>
    /// <param name="dict">The dictionary.</param>
    /// <param name="other">The other.</param>
    /// <param name="mergeTechnique">The merge technique.</param>
    /// <returns>Dictionary&lt;TK, TV&gt;.</returns>
    /// <remarks>Source and target dictionaries are left untouched</remarks>
    public static IDictionary<TK, TV> MergeWith<TK, TV>(this IDictionary<TK, TV> dict, IDictionary<TK, TV> other, MergeTechnique mergeTechnique = MergeTechnique.Unique)
    {
        var result = Merge(mergeTechnique, dict, other);

        return result;
    }

    /// <summary>
    /// Merges dictionaries
    /// </summary>
    /// <typeparam name="TK">The type of the tk.</typeparam>
    /// <typeparam name="TV">The type of the tv.</typeparam>
    /// <param name="mergeTechnique">The merge technique.</param>
    /// <param name="dictionaries">The dictionaries.</param>
    /// <returns>Dictionary&lt;TK, TV&gt;.</returns>
    /// <exception cref="System.ArgumentException">Invalid or unsupported Merge Technique - mergeTechnique</exception>
    public static IDictionary<TK, TV> Merge<TK, TV>(MergeTechnique mergeTechnique, params IDictionary<TK, TV>[] dictionaries)
    {
        return mergeTechnique switch
        {
            MergeTechnique.Unique => MergeUnique(dictionaries),
            MergeTechnique.TakeFirst => MergeFirst(dictionaries),
            MergeTechnique.TakeLast => MergeLast(dictionaries),
            _ => throw new EnumValueException<MergeTechnique>(mergeTechnique)
        };
    }

    /// <summary>
    /// Merges dictionaries assuming all keys are unique
    /// </summary>
    /// <typeparam name="TK">The type of the tk.</typeparam>
    /// <typeparam name="TV">The type of the tv.</typeparam>
    /// <param name="dictionaries">The dictionaries.</param>
    /// <returns>Dictionary&lt;TK, TV&gt;.</returns>
    public static IDictionary<TK, TV> MergeUnique<TK, TV>(params IDictionary<TK, TV>[] dictionaries)
    {
        var dict = dictionaries.SelectMany(d => d)
            .ToDictionary(pair => pair.Key, pair => pair.Value);

        return dict;
    }

    /// <summary>
    /// Merges dictionaries using the first found key value
    /// </summary>
    /// <typeparam name="TK">The type of the tk.</typeparam>
    /// <typeparam name="TV">The type of the tv.</typeparam>
    /// <param name="dictionaries">The dictionaries.</param>
    /// <returns>Dictionary&lt;TK, TV&gt;.</returns>
    public static IDictionary<TK, TV> MergeFirst<TK, TV>(params IDictionary<TK, TV>[] dictionaries)
    {
        var result = dictionaries.SelectMany(dict => dict)
            .ToLookup(pair => pair.Key, pair => pair.Value)
            .ToDictionary(group => group.Key, group => group.First());

        return result;
    }

    /// <summary>
    /// Merges dictionaries using the last found key value
    /// </summary>
    /// <typeparam name="TK">The type of the tk.</typeparam>
    /// <typeparam name="TV">The type of the tv.</typeparam>
    /// <param name="dictionaries">The dictionaries.</param>
    /// <returns>Dictionary&lt;TK, TV&gt;.</returns>
    public static IDictionary<TK, TV> MergeLast<TK, TV>(params IDictionary<TK, TV>[] dictionaries)
    {
        var result = dictionaries.SelectMany(dict => dict)
            .ToLookup(pair => pair.Key, pair => pair.Value)
            .ToDictionary(group => group.Key, group => group.Last());

        return result;
    }

    /// <summary>
    /// Extension method that turns a dictionary of string and object to an ExpandoObject
    /// </summary>
    public static ExpandoObject ToExpando<T>(this IDictionary<string, T> dictionary)
    {
        // TODO: Should really use AsDictionary

        var expando = new ExpandoObject();
        IDictionary<string, object> expandoDict = expando;

        // go through the items in the dictionary and copy over the key value pairs)
        foreach (var kvp in dictionary)
        {
            switch (kvp.Value)
            {
                // if the value can also be turned into an ExpandoObject, then do it!
                case IDictionary<string, object> valueDictionary:
                    {
                        var expandoValue = valueDictionary.ToExpando();
                        expandoDict.Add(kvp.Key, expandoValue);
                        break;
                    }
                case ICollection valueCollection:
                    {
                        // iterate through the collection and convert any string-object dictionaries
                        // along the way into expando objects
                        var itemList = new List<object>();
                        foreach (var item in valueCollection)
                        {
                            if (item is IDictionary<string, object> itemDictionary)
                            {
                                var expandoItem = itemDictionary.ToExpando();
                                itemList.Add(expandoItem);
                            }
                            else
                            {
                                itemList.Add(item);
                            }
                        }

                        expandoDict.Add(kvp.Key, itemList);
                        break;
                    }
                default:
                    expandoDict.Add(kvp.Key, kvp.Value);
                    break;
            }
        }

        return expando;
    }
}
