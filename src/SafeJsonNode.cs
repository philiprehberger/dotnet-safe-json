using System.Globalization;
using System.Text.Json.Nodes;

namespace Philiprehberger.SafeJson;

/// <summary>
/// A safe wrapper around <see cref="JsonNode"/> that provides typed value extraction
/// with fallback defaults. All accessors return the default value instead of throwing.
/// </summary>
public sealed class SafeJsonNode
{
    private readonly JsonNode? _node;

    /// <summary>
    /// Initializes a new instance of the <see cref="SafeJsonNode"/> class.
    /// </summary>
    /// <param name="node">The underlying JSON node, or null for an empty node.</param>
    internal SafeJsonNode(JsonNode? node)
    {
        _node = node;
    }

    /// <summary>
    /// Extracts a string value at the given dot-notation path.
    /// </summary>
    /// <param name="path">The dot-notation path (e.g., "user.address.city").</param>
    /// <param name="defaultValue">The value to return if the path does not exist or is not a string.</param>
    /// <returns>The string value at the path, or the default value.</returns>
    public string GetString(string path, string defaultValue)
    {
        try
        {
            var node = PathParser.Resolve(_node, path);
            return node?.GetValue<string>() ?? defaultValue;
        }
        catch
        {
            return defaultValue;
        }
    }

    /// <summary>
    /// Extracts an integer value at the given dot-notation path.
    /// </summary>
    /// <param name="path">The dot-notation path.</param>
    /// <param name="defaultValue">The value to return if the path does not exist or is not an integer.</param>
    /// <returns>The integer value at the path, or the default value.</returns>
    public int GetInt(string path, int defaultValue)
    {
        try
        {
            var node = PathParser.Resolve(_node, path);
            return node?.GetValue<int>() ?? defaultValue;
        }
        catch
        {
            return defaultValue;
        }
    }

    /// <summary>
    /// Extracts a long value at the given dot-notation path.
    /// </summary>
    /// <param name="path">The dot-notation path.</param>
    /// <param name="defaultValue">The value to return if the path does not exist or is not a long.</param>
    /// <returns>The long value at the path, or the default value.</returns>
    public long GetLong(string path, long defaultValue)
    {
        try
        {
            var node = PathParser.Resolve(_node, path);
            return node?.GetValue<long>() ?? defaultValue;
        }
        catch
        {
            return defaultValue;
        }
    }

    /// <summary>
    /// Extracts a double value at the given dot-notation path.
    /// </summary>
    /// <param name="path">The dot-notation path.</param>
    /// <param name="defaultValue">The value to return if the path does not exist or is not a double.</param>
    /// <returns>The double value at the path, or the default value.</returns>
    public double GetDouble(string path, double defaultValue)
    {
        try
        {
            var node = PathParser.Resolve(_node, path);
            return node?.GetValue<double>() ?? defaultValue;
        }
        catch
        {
            return defaultValue;
        }
    }

    /// <summary>
    /// Extracts a boolean value at the given dot-notation path.
    /// </summary>
    /// <param name="path">The dot-notation path.</param>
    /// <param name="defaultValue">The value to return if the path does not exist or is not a boolean.</param>
    /// <returns>The boolean value at the path, or the default value.</returns>
    public bool GetBool(string path, bool defaultValue)
    {
        try
        {
            var node = PathParser.Resolve(_node, path);
            return node?.GetValue<bool>() ?? defaultValue;
        }
        catch
        {
            return defaultValue;
        }
    }

    /// <summary>
    /// Extracts a decimal value at the given dot-notation path.
    /// </summary>
    /// <param name="path">The dot-notation path.</param>
    /// <param name="defaultValue">The value to return if the path does not exist or is not a decimal.</param>
    /// <returns>The decimal value at the path, or the default value.</returns>
    public decimal GetDecimal(string path, decimal defaultValue)
    {
        try
        {
            var node = PathParser.Resolve(_node, path);
            return node?.GetValue<decimal>() ?? defaultValue;
        }
        catch
        {
            return defaultValue;
        }
    }

    /// <summary>
    /// Extracts a <see cref="DateTime"/> value at the given dot-notation path by parsing an ISO 8601 string.
    /// </summary>
    /// <param name="path">The dot-notation path.</param>
    /// <param name="defaultValue">The value to return if the path does not exist or cannot be parsed as a date.</param>
    /// <returns>The parsed <see cref="DateTime"/> at the path, or the default value.</returns>
    public DateTime GetDateTime(string path, DateTime defaultValue)
    {
        try
        {
            var node = PathParser.Resolve(_node, path);
            var str = node?.GetValue<string>();

            if (str is null)
            {
                return defaultValue;
            }

            return DateTime.Parse(str, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind);
        }
        catch
        {
            return defaultValue;
        }
    }

    /// <summary>
    /// Extracts a <see cref="Guid"/> value at the given dot-notation path by parsing a GUID string.
    /// </summary>
    /// <param name="path">The dot-notation path.</param>
    /// <param name="defaultValue">The value to return if the path does not exist or cannot be parsed as a GUID.</param>
    /// <returns>The parsed <see cref="Guid"/> at the path, or the default value.</returns>
    public Guid GetGuid(string path, Guid defaultValue)
    {
        try
        {
            var node = PathParser.Resolve(_node, path);
            var str = node?.GetValue<string>();

            if (str is null)
            {
                return defaultValue;
            }

            return Guid.TryParse(str, out var result) ? result : defaultValue;
        }
        catch
        {
            return defaultValue;
        }
    }

    /// <summary>
    /// Extracts an array of <see cref="SafeJsonNode"/> at the given dot-notation path.
    /// Returns an empty array if the path does not exist or is not an array.
    /// </summary>
    /// <param name="path">The dot-notation path.</param>
    /// <returns>An array of <see cref="SafeJsonNode"/> elements.</returns>
    public SafeJsonNode[] GetArray(string path)
    {
        try
        {
            var node = PathParser.Resolve(_node, path);

            if (node is not JsonArray array)
            {
                return [];
            }

            var result = new SafeJsonNode[array.Count];

            for (var i = 0; i < array.Count; i++)
            {
                result[i] = new SafeJsonNode(array[i]);
            }

            return result;
        }
        catch
        {
            return [];
        }
    }

    /// <summary>
    /// Checks whether a value exists at the given dot-notation path.
    /// </summary>
    /// <param name="path">The dot-notation path.</param>
    /// <returns><c>true</c> if a value exists at the path; otherwise, <c>false</c>.</returns>
    public bool Has(string path)
    {
        try
        {
            var node = PathParser.Resolve(_node, path);
            return node is not null;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Accesses a child node by property key.
    /// Returns an empty node if the key does not exist.
    /// </summary>
    /// <param name="key">The property key.</param>
    /// <returns>A <see cref="SafeJsonNode"/> for the child, or an empty node.</returns>
    public SafeJsonNode this[string key]
    {
        get
        {
            try
            {
                if (_node is JsonObject obj)
                {
                    return new SafeJsonNode(obj[key]);
                }

                return new SafeJsonNode(null);
            }
            catch
            {
                return new SafeJsonNode(null);
            }
        }
    }

    /// <summary>
    /// Accesses a child node by array index.
    /// Returns an empty node if the index is out of range.
    /// </summary>
    /// <param name="index">The zero-based array index.</param>
    /// <returns>A <see cref="SafeJsonNode"/> for the element, or an empty node.</returns>
    public SafeJsonNode this[int index]
    {
        get
        {
            try
            {
                if (_node is JsonArray array && index >= 0 && index < array.Count)
                {
                    return new SafeJsonNode(array[index]);
                }

                return new SafeJsonNode(null);
            }
            catch
            {
                return new SafeJsonNode(null);
            }
        }
    }
}
