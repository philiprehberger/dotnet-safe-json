using System.Text.Json.Nodes;

namespace Philiprehberger.SafeJson;

/// <summary>
/// Provides resilient JSON parsing that never throws exceptions.
/// </summary>
public static class SafeJson
{
    /// <summary>
    /// Parses a JSON string into a <see cref="SafeJsonNode"/>.
    /// Returns an empty node if the input is null, empty, or invalid JSON.
    /// </summary>
    /// <param name="json">The JSON string to parse.</param>
    /// <returns>A <see cref="SafeJsonNode"/> wrapping the parsed result, or an empty node on failure.</returns>
    public static SafeJsonNode Parse(string? json)
    {
        if (string.IsNullOrWhiteSpace(json))
        {
            return new SafeJsonNode(null);
        }

        try
        {
            var node = JsonNode.Parse(json);
            return new SafeJsonNode(node);
        }
        catch
        {
            return new SafeJsonNode(null);
        }
    }
}
