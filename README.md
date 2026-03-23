# Philiprehberger.SafeJson

[![CI](https://github.com/philiprehberger/dotnet-safe-json/actions/workflows/ci.yml/badge.svg)](https://github.com/philiprehberger/dotnet-safe-json/actions/workflows/ci.yml)
[![NuGet](https://img.shields.io/nuget/v/Philiprehberger.SafeJson.svg)](https://www.nuget.org/packages/Philiprehberger.SafeJson)
[![License](https://img.shields.io/github/license/philiprehberger/dotnet-safe-json)](LICENSE)

Resilient JSON parsing with fallback defaults and path-based value extraction that never throws.

## Installation

```bash
dotnet add package Philiprehberger.SafeJson
```

## Usage

```csharp
using Philiprehberger.SafeJson;

var node = SafeJson.Parse("""{"user": {"name": "Alice", "age": 30}}""");

string name = node.GetString("user.name", "unknown"); // "Alice"
int age = node.GetInt("user.age", 0);                 // 30
string missing = node.GetString("user.email", "n/a");  // "n/a"

// Invalid JSON returns an empty node — never throws
var empty = SafeJson.Parse("not valid json");
string safe = empty.GetString("anything", "default");  // "default"

// Array access
var json = SafeJson.Parse("""{"items": [{"id": 1}, {"id": 2}]}""");
int id = json.GetInt("items[0].id", 0);                // 1

// Check existence
bool has = node.Has("user.name");                      // true

// Get arrays
SafeJsonNode[] items = json.GetArray("items");         // 2 elements
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
| `GetArray(path)` | Extract an array of `SafeJsonNode` at the given path. |
| `Has(path)` | Check whether a value exists at the given path. |
| `this[string key]` | Access a child node by key. |
| `this[int index]` | Access a child node by array index. |

Paths support dot notation (`user.address.city`) and array indexing (`items[0].name`).

## Development

```bash
dotnet build src/Philiprehberger.SafeJson.csproj --configuration Release
```

## License

MIT
