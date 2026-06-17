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

# Run the demo
dotnet run --project sources/Xtb.Toolkit.Demo/Xtb.Toolkit.Demo.csproj

# Pack the NuGet package locally
dotnet pack ./sources/Xtb.Toolkit/Xtb.Toolkit.csproj -c Release -o ./artifacts
```

## Versioning

`Directory.Build.props` sets `<Version>0.0.0.0</Version>` as a placeholder. Real versions are injected at CI build time via `-p:Version=...`. To publish to NuGet, push a `vMAJOR.MINOR.PATCH` git tag — the `publish-nuget.yml` workflow handles the rest.

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

## WPF (if applicable)

Avoid code-behind in windows or user controls. Use attached behaviors instead.
