# Changelog

## 0.2.0 (2026-04-05)

- Add `GetDateTime(path, defaultValue)` accessor for parsing ISO 8601 date strings
- Add `GetGuid(path, defaultValue)` accessor for parsing GUID strings

## 0.1.2 (2026-03-31)

- Standardize README to 3-badge format with emoji Support section
- Update CI actions to v5 for Node.js 24 compatibility
- Add GitHub issue templates, dependabot config, and PR template

## 0.1.1 (2026-03-24)

- Expand README usage section with feature subsections

## 0.1.0 (2026-03-22)

- Initial release
- Safe JSON parsing that never throws
- Path-based value extraction with dot notation and array indexing
- Typed accessors with fallback defaults (string, int, long, double, bool, decimal)
- Array extraction support
