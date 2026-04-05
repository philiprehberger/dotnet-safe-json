# Philiprehberger.SafeJson

[![CI](https://github.com/philiprehberger/dotnet-safe-json/actions/workflows/ci.yml/badge.svg)](https://github.com/philiprehberger/dotnet-safe-json/actions/workflows/ci.yml)
[![NuGet](https://img.shields.io/nuget/v/Philiprehberger.SafeJson.svg)](https://www.nuget.org/packages/Philiprehberger.SafeJson)
[![Last updated](https://img.shields.io/github/last-commit/philiprehberger/dotnet-safe-json)](https://github.com/philiprehberger/dotnet-safe-json/commits/main)

Resilient JSON parsing with fallback defaults and path-based value extraction that never throws.

## Installation

```bash
dotnet add package Philiprehberger.SafeJson
```

## Usage

### Parsing and Extracting Values

```csharp
using Philiprehberger.SafeJson;

var node = SafeJson.Parse("""{"user": {"name": "Alice", "age": 30}}""");

string name = node.GetString("user.name", "unknown"); // "Alice"
int age = node.GetInt("user.age", 0);                 // 30
bool active = node.GetBool("user.active", false);     // false (missing → default)
double score = node.GetDouble("user.score", 0.0);     // 0.0 (missing → default)
```

### Safe Handling of Invalid JSON

```csharp
using Philiprehberger.SafeJson;

// Invalid JSON returns an empty node — never throws
var empty = SafeJson.Parse("not valid json");
string safe = empty.GetString("anything", "default"); // "default"
bool has = empty.Has("anything");                     // false
```

### DateTime and GUID Parsing

```csharp
using Philiprehberger.SafeJson;

var json = SafeJson.Parse("""{"event": {"date": "2026-04-05T10:30:00Z", "id": "a1b2c3d4-e5f6-7890-abcd-ef1234567890"}}""");

DateTime date = json.GetDateTime("event.date", DateTime.MinValue); // 2026-04-05T10:30:00Z
Guid id = json.GetGuid("event.id", Guid.Empty);                   // a1b2c3d4-e5f6-7890-abcd-ef1234567890

// Missing or invalid values return the default — never throws
DateTime missing = json.GetDateTime("event.nope", DateTime.MinValue); // DateTime.MinValue
Guid invalid = json.GetGuid("event.date", Guid.Empty);               // Guid.Empty
```

### Array Access and Path Navigation

```csharp
using Philiprehberger.SafeJson;

var json = SafeJson.Parse("""{"items": [{"id": 1}, {"id": 2}]}""");

int id = json.GetInt("items[0].id", 0);       // 1
SafeJsonNode[] items = json.GetArray("items"); // 2 elements
bool exists = json.Has("items[1].id");         // true
```

## API

### SafeJson

| Method | Description |
|--------|-------------|
| `Parse(string json)` | Parse JSON string into a `SafeJsonNode`. Never throws. |

### SafeJsonNode

| Method | Description |
|--------|-------------|
| `GetString(path, defaultValue)` | Extract a string value at the given path. |
| `GetInt(path, defaultValue)` | Extract an int value at the given path. |
| `GetLong(path, defaultValue)` | Extract a long value at the given path. |
| `GetDouble(path, defaultValue)` | Extract a double value at the given path. |
| `GetBool(path, defaultValue)` | Extract a bool value at the given path. |
| `GetDecimal(path, defaultValue)` | Extract a decimal value at the given path. |
| `GetDateTime(path, defaultValue)` | Parse an ISO 8601 date string at the given path. |
| `GetGuid(path, defaultValue)` | Parse a GUID string at the given path. |
| `GetArray(path)` | Extract an array of `SafeJsonNode` at the given path. |
| `Has(path)` | Check whether a value exists at the given path. |
| `this[string key]` | Access a child node by key. |
| `this[int index]` | Access a child node by array index. |

Paths support dot notation (`user.address.city`) and array indexing (`items[0].name`).

## Development

```bash
dotnet build src/Philiprehberger.SafeJson.csproj --configuration Release
```

## Support

If you find this project useful:

⭐ [Star the repo](https://github.com/philiprehberger/dotnet-safe-json)

🐛 [Report issues](https://github.com/philiprehberger/dotnet-safe-json/issues?q=is%3Aissue+is%3Aopen+label%3Abug)

💡 [Suggest features](https://github.com/philiprehberger/dotnet-safe-json/issues?q=is%3Aissue+is%3Aopen+label%3Aenhancement)

❤️ [Sponsor development](https://github.com/sponsors/philiprehberger)

🌐 [All Open Source Projects](https://philiprehberger.com/open-source-packages)

💻 [GitHub Profile](https://github.com/philiprehberger)

🔗 [LinkedIn Profile](https://www.linkedin.com/in/philiprehberger)

## License

[MIT](LICENSE)
