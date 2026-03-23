using System.Text.Json.Nodes;
using System.Text.RegularExpressions;

namespace Philiprehberger.SafeJson;

/// <summary>
/// Parses dot-notation paths with optional array indexing and resolves them against a <see cref="JsonNode"/> tree.
/// </summary>
internal static partial class PathParser
{
    [GeneratedRegex(@"^(?<key>[^\[]+)(?:\[(?<index>\d+)\])?$")]
    private static partial Regex SegmentPattern();

    /// <summary>
    /// Splits a dot-notation path into individual segments.
    /// Supports paths like "user.address.city" and "items[0].name".
    /// </summary>
    /// <param name="path">The dot-notation path to split.</param>
    /// <returns>An array of path segments.</returns>
    internal static PathSegment[] SplitPath(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            return [];
        }

        var parts = path.Split('.');
        var segments = new List<PathSegment>();

        foreach (var part in parts)
        {
            var match = SegmentPattern().Match(part);

            if (!match.Success)
            {
                segments.Add(new PathSegment(part, null));
                continue;
            }

            var key = match.Groups["key"].Value;
            int? index = match.Groups["index"].Success
                ? int.Parse(match.Groups["index"].Value)
                : null;

            segments.Add(new PathSegment(key, index));
        }

        return segments.ToArray();
    }

    /// <summary>
    /// Resolves a dot-notation path against a <see cref="JsonNode"/> tree.
    /// Returns null if any segment cannot be resolved.
    /// </summary>
    /// <param name="root">The root JSON node to resolve against.</param>
    /// <param name="path">The dot-notation path to resolve.</param>
    /// <returns>The resolved <see cref="JsonNode"/>, or null if not found.</returns>
    internal static JsonNode? Resolve(JsonNode? root, string path)
    {
        if (root is null)
        {
            return null;
        }

        var segments = SplitPath(path);
        var current = root;

        foreach (var segment in segments)
        {
            if (current is null)
            {
                return null;
            }

            // Navigate to the key
            if (current is JsonObject obj)
            {
                current = obj[segment.Key];
            }
            else
            {
                return null;
            }

            // If there's an array index, navigate into it
            if (segment.Index.HasValue)
            {
                if (current is JsonArray array && segment.Index.Value < array.Count)
                {
                    current = array[segment.Index.Value];
                }
                else
                {
                    return null;
                }
            }
        }

        return current;
    }
}

/// <summary>
/// Represents a single segment in a dot-notation path.
/// </summary>
/// <param name="Key">The property key name.</param>
/// <param name="Index">An optional array index.</param>
internal sealed record PathSegment(string Key, int? Index);
