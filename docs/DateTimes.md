# DNX.Extensions.DateTimes

Extension methods and helpers for working with `DateTime` values.

## DateTimeExtensions

| Constant | Value | Description |
| --- | --- | --- |
| CalendarMinValue | `new DateTime(1953, 1, 1)` | A practical "minimum" calendar date, for use as a sentinel bound in date-range logic |
| CalendarMaxValue | `new DateTime(9998, 12, 31)` | A practical "maximum" calendar date, for use as a sentinel bound in date-range logic |

| Method Name | Parameters | Description |
| --- | --- | --- |
| ParseDateAsUtc | | Parses the string with `DateTime.Parse` (current culture) and marks the result as `DateTimeKind.Utc`. This does **not** convert the value from local time to UTC - it reinterprets the same year/month/day/hour/etc. values as if they were already UTC |
| ParseDateAsUtc | `IFormatProvider` formatProvider | As above, parsing with the supplied format provider |
| ParseDateAsUtc | `DateTime` defaultDateTime | As the first overload, but returns `defaultDateTime` (with its `Kind` forced to `Utc`) instead of throwing if parsing fails |
| ParseDateAsUtc | `IFormatProvider` formatProvider, `DateTime` defaultDateTime | Combines the two overloads above - parses with a format provider, falling back to `defaultDateTime` on failure |
| MinOf (static) | `params DateTime[]` values | Returns the earliest of the supplied dates, or `DateTime.MinValue` if none are supplied |
| MaxOf (static) | `params DateTime[]` values | Returns the latest of the supplied dates, or `DateTime.MaxValue` if none are supplied |
| GetMonthsSpan | `DateTime` other | Returns the (inclusive) number of calendar months between this date and `other`, regardless of which one is earlier |
| GetQuarter | | Returns the calendar quarter (1-4) that the date's month falls in |
| SetYear | year | Returns a new `DateTime` with the year changed, keeping month/day/time-of-day/`Kind` the same |
| SetMonth | month, maintainYear (default `true`), maintainDay (default `true`) | Returns a new `DateTime` with the month changed. When `maintainDay` is `true` (the default), the day is **clamped** to the last valid day of the target month rather than rolling over into the next month. `maintainYear` has no distinct effect - a month-only change can never alter the year on its own; the parameter exists purely for signature symmetry with `SetDay` |
| SetDay | day, maintainYear (default `true`), maintainMonth (default `true`) | Returns a new `DateTime` with the day changed. When `maintainMonth` is `true` (the default), the day is **clamped** to the last valid day of the current month rather than rolling into the next month. When `maintainMonth` is `false`, `day` is instead treated as a day-of-month offset and is allowed to roll into adjacent months (and, unless `maintainYear` is also `false`, the year is then forced back to the original) |
| ResetHours / ResetMinutes / ResetSeconds / ResetMilliseconds | | Returns a new `DateTime` with that one component zeroed out, leaving everything else unchanged |
| SetHours / SetMinutes / SetSeconds | hours / minutes / seconds | Resets that component to zero, then adds the supplied value - equivalent to replacing it outright under normal (non-rollover) values |
| SetMilliseconds | milliseconds (`double`) | Resets the milliseconds to zero, then adds the supplied value. A `DateTime`'s precision means this is only meaningful for values up to 999 |

`SetMonth` and `SetDay` both clamp rather than roll over when the target month is shorter than
the requested day - e.g. `new DateTime(2023, 3, 31).SetMonth(2)` lands on 28 February 2023, not
1 March. This is a common gotcha if you're expecting `DateTime.AddMonths`-style rollover
behaviour.

Examples:

```csharp
// ParseDateAsUtc - the wall-clock values are kept, only Kind changes
var parsed = "2023-05-15 14:30:00".ParseDateAsUtc();
// parsed.Kind = DateTimeKind.Utc, parsed = 2023-05-15 14:30:00

var fallback = "not a date".ParseDateAsUtc(new DateTime(2000, 1, 1));
// fallback = 2000-01-01, Kind = Utc

// MinOf / MaxOf
var earliest = DateTimeExtensions.MinOf(
    new DateTime(2023, 5, 15),
    new DateTime(2023, 1, 1));
// earliest = 2023-01-01

// GetMonthsSpan
var span = new DateTime(2000, 1, 1).GetMonthsSpan(new DateTime(2000, 12, 31)); // 12

// GetQuarter
var quarter = new DateTime(2023, 8, 1).GetQuarter(); // 3

// SetMonth clamps rather than rolling over
var result = new DateTime(2023, 3, 31).SetMonth(2);
// result = 2023-02-28 (not 2023-03-01/02)

// SetDay clamps when maintainMonth is true (the default)
var clamped = new DateTime(2023, 4, 15).SetDay(31);
// clamped = 2023-04-30 (April only has 30 days)

// ResetHours / SetHours
var dateTime = new DateTime(2023, 5, 15, 8, 30, 0);
var midnight = dateTime.ResetHours(); // 2023-05-15 00:30:00
var evening = dateTime.SetHours(20);  // 2023-05-15 20:30:00
```
