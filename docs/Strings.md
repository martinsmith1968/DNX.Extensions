# DNX.Extensions.Strings

The biggest namespace in the library. It provides a large set of `string` extension methods
(substring extraction, prefix/suffix handling, splitting, coalescing, case conversion, masking,
etc.), plus regex-based parsing helpers, `StringBuilder` helpers, `bool`/`int` to-text helpers,
a command-line argument parser, and - in the `DNX.Extensions.Strings.Interpolation` sub-namespace
- a named string-interpolation engine that extends C#'s own string interpolation with named
object instances and dictionaries resolved at runtime.

## StringExtensions

The core set of extension methods on `string`. Grouped below by purpose.

### Null and empty checks

| Method Name | Parameters | Description |
| --- | --- | --- |
| IsNullOrEmpty | | `true` if the string is null or `""` (wraps `string.IsNullOrEmpty`) |
| IsNullOrWhiteSpace | | `true` if the string is null, `""`, or only whitespace (wraps `string.IsNullOrWhiteSpace`) |
| NullIfEmpty | | Returns `null` if the string is null or empty, otherwise returns the string unchanged |
| NullIfEmptyOrWhitespace | | Returns `null` if the string is null, empty, or only whitespace, otherwise returns the string unchanged |

```csharp
var a = "".IsNullOrEmpty();          // true
var b = " ".IsNullOrEmpty();         // false
var c = " ".IsNullOrWhiteSpace();    // true

var d = "".NullIfEmpty();            // null
var e = " ".NullIfEmpty();           // " " (whitespace is not "empty")
var f = " ".NullIfEmptyOrWhitespace(); // null
```

### Ensuring and removing prefixes/suffixes

| Method Name | Parameters | Description |
| --- | --- | --- |
| EnsureStartsWith | prefix | Prepends `prefix` if the text doesn't already start with it (no-op if `prefix` is null/empty) |
| EnsureEndsWith | suffix | Appends `suffix` if the text doesn't already end with it (no-op if `suffix` is null/empty) |
| EnsureStartsAndEndsWith | prefixSuffix | `EnsureStartsWith(prefixSuffix)` followed by `EnsureEndsWith(prefixSuffix)` |
| EnsureStartsAndEndsWith | prefix, suffix | `EnsureStartsWith(prefix)` followed by `EnsureEndsWith(suffix)` |
| RemoveStartsWith | prefix | Repeatedly strips `prefix` from the start of the text while present |
| RemoveEndsWith | suffix | Repeatedly strips `suffix` from the end of the text while present |
| RemoveStartsAndEndsWith | prefixSuffix | `RemoveStartsWith(prefixSuffix)` followed by `RemoveEndsWith(prefixSuffix)` |
| RemoveStartsAndEndsWith | prefix, suffix | `RemoveStartsWith(prefix)` followed by `RemoveEndsWith(suffix)` |

```csharp
var a = "hello".EnsureStartsWith("#");      // "#hello"
var b = "hello".EnsureStartsAndEndsWith("_"); // "_hello_"
var c = "hello".EnsureStartsAndEndsWith("[", "]"); // "[hello]"

var d = "00099".RemoveStartsWith("0");      // "99"
var e = "hello".RemoveStartsAndEndsWith("h", "o"); // "ell"
```

### Extracting substrings

All methods below take an optional `StringComparison comparison = StringComparison.Ordinal` and
return `null` if the requested text/boundary can't be found (or the input is null/empty).

| Method Name | Parameters | Description |
| --- | --- | --- |
| Between | startText, endText, comparison | Text after the first `startText` and before the first `endText` found after it |
| BetweenFirstAndLast | startText, endText, comparison | Text after the first `startText` and before the *last* `endText` |
| Before | endText, comparison | Text before the first occurrence of `endText` |
| BeforeLast | endText, comparison | Text before the last occurrence of `endText` |
| After | startText, comparison | Text after the first occurrence of `startText` |
| AfterLast | startText, comparison | Text after the last occurrence of `startText` |
| From | startText, comparison | Text starting at (and including) the first occurrence of `startText` |
| FromLast | startText, comparison | Text starting at (and including) the last occurrence of `startText` |
| AsFarAs | endText, comparison | Text up to and including the first occurrence of `endText` |
| AsFarAsLast | endText, comparison | Text up to and including the last occurrence of `endText` |

```csharp
var a = "[Section Name]".Between("[", "]");               // "Section Name"
var b = "[Reference[0]]".BetweenFirstAndLast("[", "]");    // "Reference[0]"

var c = "This is some text".Before("some");   // "This is "
var d = "This is some text".After("some");    // " text"

var e = "bobob".From("o");      // "obob"
var f = "bobob".FromLast("o");  // "ob"

var g = "bobob".AsFarAs("o");      // "bo"
var h = "bobob".AsFarAsLast("o");  // "bobo"
```

### Searching and filtering characters

| Method Name | Parameters | Description |
| --- | --- | --- |
| ContainsText | searchText, comparison = `CurrentCultureIgnoreCase` | `true` if `searchText` is found in the text (empty `searchText` always matches; `null` text never matches) |
| ContainsOnly | characters (string) | `true` if every character in the text is one of `characters` |
| ContainsOnly | characters (`IList<char>`) | As above, taking a character list |
| RemoveAny | charsToRemove (string) | Removes every occurrence of any of the given characters |
| RemoveAny | charsToRemove (`char[]`) | As above, taking a character array |
| RemoveAnyExcept | charsToKeep (string) | Keeps only the characters that appear in `charsToKeep`, removing everything else |
| RemoveAnyExcept | charsToKeep (`IList<char>`) | As above, taking a character list |
| CountOccurrences | searchText, comparison = `Ordinal` | Counts non-overlapping occurrences of `searchText` |
| CountOccurrences | value (char) | Counts occurrences of a single character |

```csharp
var a = "BOB".ContainsText("o");                 // true  (CurrentCultureIgnoreCase by default)
var b = "BOB".ContainsText("o", StringComparison.CurrentCulture); // false

var c = "1,234.56".ContainsOnly("1234567890.");  // false (contains a comma)

var d = "obviously this piece of text contains at least one of every vowel".RemoveAny("aeiou");
// "bvsly ths pc f txt cntns t lst n f vry vwl"

var e = "The amount to pay is: 123,456.00".RemoveAnyExcept("1234567890.");
// "123456.00"

var f = "ababab".CountOccurrences("ab", StringComparison.Ordinal); // 3
var g = "banana".CountOccurrences('a');                            // 3
```

### Splitting

`SplitText` honours a `SplitDelimiterType` that controls whether the `delimiters` string is a set
of individual delimiter characters (`Any`, the default) or must match in its entirety (`All`).

| Name | Values | Description |
| --- | --- | --- |
| SplitDelimiterType.Any | 0 (default) | Any one of the characters in `delimiters` splits the text |
| SplitDelimiterType.All | 1 | The entire `delimiters` string must match as a single delimiter |

| Method Name | Parameters | Description |
| --- | --- | --- |
| SplitText | delimiters, options = `None`, delimiterType = `Any` | Splits using `Any` (`string.Split` on individual chars) or `All` (delegates to `SplitByText`); throws `ArgumentOutOfRangeException` for an invalid `delimiterType` |
| SplitByText | delimiterText, options = `None`, comparison = `CurrentCulture` | Splits on a literal multi-character delimiter string |

```csharp
var a = "a-b[c]d=e".SplitText("-[]=");
// ["a", "b", "c", "d", "e"]   (SplitDelimiterType.Any - each character is its own delimiter)

var b = "a-b----d-e".SplitText("--", StringSplitOptions.None, SplitDelimiterType.All);
// ["a-b", "", "d-e"]          (the literal "--" is the delimiter)

var c = "Line 1/Line 2//Line 4".Split('/');
var d = string.Join(Environment.NewLine, c)
    .SplitByText(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
// ["Line 1", "Line 2", "Line 4"]
```

### Coalescing

Each family has a `...With` extension that keeps the original `text` if it already qualifies, and
a plain list-based version used to pick the first qualifying value from a list. The three families
differ only in what counts as "missing": null only, null-or-empty, or null-or-whitespace.

| Method Name | Parameters | Description |
| --- | --- | --- |
| CoalesceNullWith | `params string[]` items | Returns `text` if not null, else the first non-null item |
| CoalesceNullWith | `IList<string>` items | As above, taking a list |
| CoalesceNull | (on `IList<string>`) | First non-null value in the list, or `null` |
| CoalesceNullOrEmptyWith | `params string[]` items | Returns `text` if not null/empty, else the first non-null/empty item |
| CoalesceNullOrEmptyWith | `IList<string>` items | As above, taking a list |
| CoalesceNullOrEmpty | (on `IList<string>`) | First non-null/empty value in the list, or `null` |
| CoalesceNullOrWhitespaceWith | `params string[]` items | Returns `text` if not null/whitespace, else the first non-null/whitespace item |
| CoalesceNullOrWhitespaceWith | `IList<string>` items | As above, taking a list |
| CoalesceNullOrWhitespace | (on `IList<string>`) | First non-null/whitespace value in the list, or `null` |

```csharp
string text = null;

var a = text.CoalesceNullWith("", "", "");             // "" (first non-null item, even though it's empty)
var b = text.CoalesceNullOrEmptyWith("", "", "");       // null (all items are empty too)
var c = text.CoalesceNullOrWhitespaceWith("", " ", "c"); // "c" (empty and whitespace-only are skipped)
```

### Validating numbers

| Method Name | Parameters | Description |
| --- | --- | --- |
| IsValidNumber | | `true` if the text is a valid number formatted for `CultureInfo.DefaultThreadCurrentCulture` (group/decimal separators, optional `+`/`-` sign) |
| IsValidNumber | cultureInfo | As above, validated against the supplied `CultureInfo` instead |

```csharp
var gb = CultureInfo.GetCultureInfo("en-gb");
var de = CultureInfo.GetCultureInfo("de-DE");

var a = "3,123.451".IsValidNumber(gb); // true  (comma group separator, dot decimal separator)
var b = "3,123.451".IsValidNumber(de); // false (German culture expects "3.123,451")
var c = "3.123,451".IsValidNumber(de); // true
```

### Changing case and wording

| Method Name | Parameters | Description |
| --- | --- | --- |
| Wordify | | Inserts a space before each capital letter that borders a lowercase letter, splitting PascalCase/camelCase into words (keeps runs of capitals, e.g. acronyms, together) |
| Wordify | preservedWords (`IList<string>`) | As above, then re-joins any of the given words that `Wordify` would otherwise have split apart (matched via their own wordified form) |
| ToPascalCase | | Splits on spaces/tabs/newlines/`_`/`-`, then upper-cases the first letter of each word and lower-cases the rest |
| ToCamelCase | | `ToPascalCase()` with the very first character lower-cased |
| ToSnakeCase | | Lower-cases and joins words with `_`, inserting `_` at case boundaries and in place of whitespace/`-`/`.`; collapses repeated `_` and trims them from the ends |
| ToKebabCase | | `ToSnakeCase()` with every `_` replaced by `-` |

```csharp
var a = "SouthendUnitedFCAreRubbish".Wordify();
// "Southend United F C Are Rubbish"
var b = "SouthendUnitedFCAreRubbish".Wordify(new[] { "FC" });
// "Southend United FC Are Rubbish"

var c = "hello_world".ToPascalCase(); // "HelloWorld"
var d = "Hello World".ToCamelCase();  // "helloWorld"
var e = "Item1Value".ToSnakeCase();   // "item1_value"
var f = "HelloWorld".ToKebabCase();   // "hello-world"
```

### Reversing, truncating and masking

| Method Name | Parameters | Description |
| --- | --- | --- |
| Reverse | | Reverses the characters in the string (null/whitespace-only input returned unchanged) |
| Left | length | The leftmost `length` characters (or the whole string if shorter); a negative `length` delegates to `Right(-length)`; `0` returns `""` |
| Right | length | The rightmost `length` characters (or the whole string if shorter); a negative `length` delegates to `Left(-length)`; `0` returns `""` |
| Truncate | maxLength, ellipsis = `"..."` | Truncates to `maxLength` (including the ellipsis) and appends `ellipsis` when truncation occurs; `maxLength <= 0` returns `""` |
| Mask | visibleStart = 0, visibleEnd = 0, maskChar = `'*'` | Replaces the characters between the visible start/end portions with `maskChar`; returns the text unchanged if the visible portions cover the whole string |
| MaskEmail | maskChar = `'*'` | Masks the local part of an email address after its first character (keeps the domain and any text without `@` untouched; a 1-character local part is left alone) |

```csharp
var a = "hello world".Reverse();     // "dlrow olleh"
var b = "Some text".Left(5);         // "Some "
var c = "Some text".Right(-5);       // "Some " (negative length -> Left(5))

var d = "Hello world".Truncate(8);          // "Hello..."
var e = "Hello world".Truncate(7, "---");   // "Hell---"

var f = "abcdef".Mask(1, 1);               // "a****f"
var g = "john@example.com".MaskEmail();    // "j***@example.com"
var h = "j@example.com".MaskEmail();       // "j@example.com" (1-char local part is untouched)
```

### Encoding

| Method Name | Parameters | Description |
| --- | --- | --- |
| ToHexString | format = `"X2"` | Converts each character's code point to hex (via `((int)c).ToString(format)`) and concatenates the results |
| GetBytes | | Converts the string to a `byte[]` using UTF-8 |
| GetBytes | encoding | As above, with the specified `Encoding` (falls back to UTF-8 if `encoding` is `null`) |

```csharp
var a = "ABCDEF".ToHexString();        // "414243444546"
var b = "AB".ToHexString("D");         // "6566" (decimal code points instead of hex)

var c = "ABCDEF".GetBytes();           // [65, 66, 67, 68, 69, 70]
```

## BuiltInTypesExtensions

Small set of extensions that render `bool`/`int` values as text.

| Method Name | Parameters | Description |
| --- | --- | --- |
| ToText | trueText = `"True"`, falseText = `"False"` | Renders a `bool` as one of the two supplied strings |
| ToYesNo | | `ToText("Yes", "No")` |
| ToHexString | format = `"X2"` | Renders an `int` as hexadecimal text (`input.ToString(format)`) |

```csharp
var a = true.ToText();                  // "True"
var b = false.ToText("Way", "No Way");  // "No Way"
var c = true.ToYesNo();                 // "Yes"

var d = 255.ToHexString();     // "FF"
var e = 255.ToHexString("X4"); // "00FF"
```

## RegexStringExtensions

Helpers for pulling structured data out of text using regular expressions.

| Method Name | Parameters | Description |
| --- | --- | --- |
| IsMatch | pattern | `true` if `Regex.Match(text, pattern)` succeeds anywhere in the text |
| ParseToKeyValuePair | regExpression, keyGroupName = `"1"`, valueGroupName = `"2"` | Matches `text` against `regExpression` and returns the named/numbered groups `keyGroupName`/`valueGroupName` as a `KeyValuePair<string, string>` |
| ParseToDictionary | (on `IEnumerable<string>`) regExpression, keyGroupName = `"1"`, valueGroupName = `"2"` | Applies `ParseToKeyValuePair` to every item, dropping any with an empty key, and collects the results into a `Dictionary<string, string>` |
| ParseToDictionaryList | regExpression | Runs `Regex.Matches` against `text` and returns one `Dictionary<string, string>` per match, keyed by group name (or the group's index as a string, for unnamed groups) |
| ParseFirstMatchToDictionary | regExpression | `ParseToDictionaryList(...)` and returns only the first match's dictionary (or `null`) |
| GetGroupName | (on `Regex`) index | Returns the name of the group at `index` (numbered groups return their index as a string); negative/out-of-range indexes resolve via `IsIndexValid`/`GetAbsoluteIndex` and fall back to `index.ToString()` |

```csharp
var kvp = "UseFlag=Yes".ParseToKeyValuePair("(?<name>[^=]+)=(?<value>.*)", "name", "value");
// kvp.Key == "UseFlag", kvp.Value == "Yes"

var dict = "CustomField[Blah]".ParseFirstMatchToDictionary(
    @"(?<FieldName>[A-Za-z0-9]+)[\[]*(?<IndexerName>[A-Za-z0-9]*)[\]]*");
// dict["FieldName"] == "CustomField", dict["IndexerName"] == "Blah"

var regex = new Regex(@"(?<FieldName>[A-Za-z0-9]+)[\[]*(?<IndexerName>[A-Za-z0-9]*)[\]]*");
var g0 = regex.GetGroupName(0); // "0"             (group 0 is the whole match, unnamed)
var g1 = regex.GetGroupName(1); // "FieldName"
var g3 = regex.GetGroupName(3); // "3"             (no group 3 - falls back to the index)
```

## StringBuilderExtensions

Guard helpers for appending to a `StringBuilder` only when necessary.

| Method Name | Parameters | Description |
| --- | --- | --- |
| AppendIfNotPresent | text | Appends `text` unless the builder's current content already ends with it (a `null` builder is returned untouched) |
| AppendSeparator | text = `" "` | Appends `text` as a separator, but only if the builder is non-empty and doesn't already end with it (a `null` builder is returned untouched) |

```csharp
var sb = new StringBuilder("Hello");
sb.AppendIfNotPresent("Bob");   // "HelloBob"
sb.AppendIfNotPresent("Bob");   // "HelloBob" (already ends with "Bob", no change)

var sb2 = new StringBuilder("Hello");
sb2.AppendSeparator();          // "Hello " (default separator is a single space)
sb2.AppendSeparator();          // "Hello " (already ends with the separator, no change)
```

## ArgumentParserExtensions

| Method Name | Parameters | Description |
| --- | --- | --- |
| ParseToIndividualArguments | | Splits a raw command-line string into its individual arguments, honouring double-quoted segments (which may contain spaces) and stripping the surrounding quotes |

```csharp
var args = "bob -c 4 \"-a 1\" \"a note\"".ParseToIndividualArguments();
// ["bob", "-c", "4", "-a 1", "a note"]
```

## Strings.Interpolation namespace

`DNX.Extensions.Strings.Interpolation` extends C#'s own string interpolation syntax
(`{PropertyName}` / `{PropertyName:format}`) so that the named values can be supplied at runtime -
from one or more arbitrary object instances and/or dictionaries - rather than only from variables
visible at compile time.

### INamedInstance interface

Describes a named source of values by property name. It is not currently implemented by any class
in this namespace (`NamedInstance`, below, uses reflection directly instead), but is available for
custom value providers that want to plug into the same shape.

| Property | Type | Description |
| --- | --- | --- |
| Name | string | The name used to qualify this instance's values (e.g. the `person` in `{person.FirstName}`) |

| Method Name | Parameters | Description |
| --- | --- | --- |
| GetValue | propertyName | Returns the value for the named property |

### NamedInstance class

Wraps an arbitrary object (or dictionary) together with an optional name, and can convert it into a
flat dictionary of interpolation values for that object's public instance and static properties
and fields (instance values win over static ones of the same name).

| Property | Type | Description |
| --- | --- | --- |
| Instance | object | The wrapped object |
| Name | string | The optional name used to prefix keys (`"{Name}.{PropertyName}"`); when null/whitespace, keys are unprefixed |

| Method Name | Parameters | Description |
| --- | --- | --- |
| ToDictionary | | Builds an `IDictionary<string, object>` of the instance's public property/field names (optionally prefixed with `Name.`) to their current values; returns `null` if `Instance` is `null` |

```csharp
var person = new Person { FirstName = "Bob", LastName = "Carolgees" };

var unnamed = new NamedInstance(person).ToDictionary();
// unnamed["FirstName"] == "Bob"

var named = new NamedInstance(person, "owner").ToDictionary();
// named["owner.FirstName"] == "Bob", named["owner.LastName"] == "Carolgees"

var empty = new NamedInstance(null, "Missing").ToDictionary(); // null
```

### StringInterpolator class

| Method Name | Parameters | Description |
| --- | --- | --- |
| InterpolateWith | instance, namePrefix = null, ignoreErrors = false | Wraps `instance` in a `NamedInstance` (named `namePrefix`) and interpolates `text` against its public property/field values |
| InterpolateWith | `NamedInstance` namedInstance, ignoreErrors = false | As above, using an already-constructed `NamedInstance` |
| InterpolateWithAll | `IList<NamedInstance>` namedInstances, ignoreErrors = false | Merges the dictionaries of several named instances (earlier entries in the list win on name collisions), then interpolates |
| InterpolateWithAll\<T\> | `IDictionary<string, T>` paramValues, ignoreErrors = false | Interpolates directly from a dictionary of parameter name/value pairs; placeholders not present in `paramValues` are left as-is, and dictionary entries not referenced by any placeholder are simply unused |

Placeholders follow standard composite-format syntax, so `{Name:format}` applies a format string to
the resolved value (e.g. a `DateTime` or a number) exactly as `string.Format` would. When
`ignoreErrors` is `true`, any exception raised while formatting causes the original, un-interpolated
`text` to be returned instead of throwing.

```csharp
var person = new Person
{
    FirstName = "Martin",
    LastName = "Smith",
    DateOfBirth = new DateTime(1968, 08, 11),
    AgeInYears = 29
};

var a = "Hello {FirstName}".InterpolateWith(person);
// "Hello Martin"

var b = "{FirstName} was born in {DateOfBirth:MMMM yyyy} and is {AgeInYears:0000}".InterpolateWith(person);
// "Martin was born in August 1968 and is 0029"

var c = "Hello {person.FirstName} {person.LastName}".InterpolateWith(person, "person");
// "Hello Martin Smith"

// Multiple named instances in one format string
var player = new Person { FirstName = "Harry", LastName = "Kane" };
var club = new Club { Name = "Spurs" };

var instances = new List<NamedInstance>
{
    new NamedInstance(player, "player"),
    new NamedInstance(club, "club"),
};

var d = "{player.FirstName} {player.LastName} plays for {club.Name}".InterpolateWithAll(instances);
// "Harry Kane plays for Spurs"

// Plain dictionary - unused entries are simply ignored
var values = new Dictionary<string, object> { { "FirstName", "Martin" }, { "LastName", "Smith" } };
var e = "Hello {FirstName}!".InterpolateWithAll(values);
// "Hello Martin!"

// ignoreErrors swallows formatting failures and returns the original text instead of throwing
// (here, formatting "{Age:0000}" throws because the value's IFormattable.ToString rejects the format)
var badValues = new Dictionary<string, object> { { "Age", new ThrowingFormattable() } };
var f = "Hello {Age:0000}".InterpolateWithAll(badValues, ignoreErrors: true);
// "Hello {Age:0000}"
```
