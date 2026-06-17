# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## What This Is

XTB Toolkit is a .NET library (`DustInTheWind.Xtb.Toolkit`, targeting `net8.0`) for parsing `.xlsx` files exported from the XTB S.A. brokerage platform. The solution contains two projects:

- `sources/Xtb.Toolkit/` — the library, published to NuGet as `DustInTheWind.Xtb.Toolkit`
- `sources/Xtb.Toolkit.Demo/` — a demo CLI app (`net10.0`) showing library usage

Solution file: `Xtb.Toolkit.slnx`

## Commands

```bash
# Restore dependencies
dotnet restore ./Xtb.Toolkit.slnx --configfile ./nuget.config

# Build (Release)
dotnet build ./Xtb.Toolkit.slnx -c Release --no-restore

# Run the demo (requires report.xlsx in sources/Xtb.Toolkit.Demo/)
dotnet run --project sources/Xtb.Toolkit.Demo/Xtb.Toolkit.Demo.csproj

# Pack the NuGet package locally
dotnet pack ./sources/Xtb.Toolkit/Xtb.Toolkit.csproj -c Release -o ./artifacts
```

## Versioning

`Directory.Build.props` sets `<Version>0.0.0.0</Version>` as a placeholder. Real versions are injected at CI build time via `-p:Version=...`. To publish to NuGet, push a `vMAJOR.MINOR.PATCH` git tag — the `publish-nuget.yml` workflow handles the rest.

## Architecture

### Parsing pipeline

The public entry point is `ReportDocument.LoadFromFileAsync` / `LoadAsync`. These methods open a stream and delegate to the internal `XlsxReportDocument` (in `sources/Xtb.Toolkit/Xlsx/`), which uses `DocumentFormat.OpenXml` to open the spreadsheet. `XlsxReportDocument` is internal and disposable; `ReportDocument` wraps all exceptions as `DocumentLoadException`.

The `DocumentSection` flags enum controls which sheets are parsed — callers pass `DocumentSection.CashOperations`, `DocumentSection.ClosedPositions`, or `DocumentSection.All`.

### Sheet parsing conventions

Both sheets share the same fixed row layout:
- Row 1: account number (column B)
- Rows 2–4: metadata (period dates in rows 3–4, column B)
- Row 5: column headers (skipped)
- Rows 6 to N−1: data rows
- Row N (last): summary row (Total / Profit/loss)

Sheet names are hardcoded strings: `"Cash Operations"` and `"Closed Positions"`. OpenXml shared strings are resolved via a pre-loaded `string[]` index; numeric dates are OADate doubles.

### Value object pattern

`CashOperationType` is a `sealed record` value object that wraps a raw string from the spreadsheet. It provides:
- Static well-known instances (`StockPurchase`, `FreeFundsInterest`, etc.)
- A `KnownValues` collection for enumeration
- Implicit conversions to/from `string`

Follow this same pattern when adding new typed fields that map to a fixed set of known string values from XTB.

## Code Conventions

- **No `var`** — always use the explicit type.
- **Linq lambda parameter**: always name it `x`.
- **Object instantiation**: prefer `new()` over `new SomeType()`.
- **Object initializers**: if more than one property, put each on its own line.
- **No braces** for single-line `if`, `for`, or `using` bodies.
- **XML docs**: only on public types that are part of the NuGet package surface; skip internal types.

## Testing Conventions

- One test file per tested method (including constructors): method `Query()` → `QueryTests.cs`.
- All test files for a class live in a directory named after the class: class `Color` → `ColorTests/`.
- Test method naming: `Having<SetupDetails>_When<Action>_Then<ExpectedResult>`.
- `Assert.Throws` lambda must always use a block body `() => { ... }`.
