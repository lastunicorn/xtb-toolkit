# XTB Toolkit

[![GitHub Repo](https://img.shields.io/badge/github-repo-blue?logo=github)](https://github.com/lastunicorn/xtb-toolkit) [![GitHub Build](https://img.shields.io/github/actions/workflow/status/lastunicorn/xtb-toolkit/build-master.yml?logo=github)](https://github.com/lastunicorn/xtb-toolkit/actions/workflows/build-master.yml) [![NuGet Version](https://img.shields.io/nuget/v/DustInTheWind.Xtb.Toolkit?logo=nuget)](https://www.nuget.org/packages/DustInTheWind.Xtb.Toolkit) [![NuGet Downloads](https://img.shields.io/nuget/dt/DustInTheWind.Xtb.Toolkit?logo=nuget)](https://www.nuget.org/packages/DustInTheWind.Xtb.Toolkit)

**XTB Toolkit** is a .NET library (`DustInTheWind.Xtb.Toolkit`, targeting `net8.0`) for parsing `.xlsx` files exported from the XTB S.A. brokerage platform.

**XTB S.A.** is a Warsaw brokerage firm that provides products, services, and technology solutions for trading various financial instruments, including foreign exchange (forex) and contracts for difference (CFDs).

- https://en.wikipedia.org/wiki/XTB_S.A.
- https://www.xtb.com/ro

## Installation

Package Manager:

```powershell
Install-Package DustInTheWind.Xtb.Toolkit
```

.NET CLI:

```bash
dotnet add package DustInTheWind.Xtb.Toolkit
```

## Runtime Requirements

- Library target framework: `.NET 8.0` (`net8.0`)

## Features

- **Parse Cash Operations** — reads data only from the "Cash Operation" sheet (section)
- **Parse Closed Positions** — reads data only from the "Closed Positions" sheet (section)
- **Selective loading** — use `DocumentSection` flags to load only the sections you need

## Quick Start

### Step 1 — Export the report from XTB

In XTB XStation5 web application (https://xstation5.xtb.com):

1. Log in.
2. Select the account from the top-right drop-down.
3. Select the history tab ("Istoricul contului").
4. Click "Export" and generate a new report.

You will receive a `.xlsx` file containing one or both of the supported sections.

### Step 2 — Parse the exported file

Load both sections at once:

```csharp
using DustInTheWind.Xtb.Toolkit;

ReportDocument document = await ReportDocument.LoadFromFileAsync("report.xlsx", DocumentSection.All);
```

Or load only the section you need:

```csharp
ReportDocument document = await ReportDocument.LoadFromFileAsync("report.xlsx", DocumentSection.CashOperations);
ReportDocument document = await ReportDocument.LoadFromFileAsync("report.xlsx", DocumentSection.ClosedPositions);
```

### Step 3 — Access the data

**Cash Operations:**

```csharp
CashOperationsSection cash = document.CashOperationsSection;

Console.WriteLine($"Account : {cash.AccountNumber}");
Console.WriteLine($"Period  : {cash.DateFrom:yyyy-MM-dd} – {cash.DateTo:yyyy-MM-dd}");
Console.WriteLine($"Total   : {cash.Total}");

foreach (CashOperation cashOperation in cash.CashOperations)
{
    ...
}
```

**Closed Positions:**

```csharp
ClosedPositionsSection closed = document.ClosedPositionsSection;

Console.WriteLine($"Account      : {closed.AccountNumber}");
Console.WriteLine($"Period       : {closed.DateFrom:yyyy-MM-dd} – {closed.DateTo:yyyy-MM-dd}");
Console.WriteLine($"Profit/Loss  : {closed.ProfitOrLoss}");

foreach (ClosedPosition closedPosition in closed.ClosedPositions)
{
    ...
}
```

## Spreadsheet Structure

The `.xlsx` export contains two sheets. Each sheet begins with four metadata rows followed by a header row, data rows, and a summary row.

### Sheet 1 — Cash Operations

Metadata properties on `CashOperationsSection`:

| Property        | Type       | Description                      |
| --------------- | ---------- | -------------------------------- |
| `AccountNumber` | `string`   | XTB account number               |
| `DateFrom`      | `DateTime` | Start of the report period (UTC) |
| `DateTo`        | `DateTime` | End of the report period (UTC)   |
| `Total`         | `decimal`  | Sum of all amounts in the period |

Cash Operations

| Property | Type | Description |
|---|---|---|
| `Type` | `string` | Operation type (e.g. "Stock purchase", "Free funds interest") |
| `Ticker` | `string` | Instrument ticker symbol |
| `Instrument` | `string` | Instrument name |
| `Time` | `DateTime` | Timestamp of the operation (UTC) |
| `Amount` | `decimal` | Cash amount (negative for purchases) |
| `Id` | `string` | Unique operation identifier |
| `Comment` | `string` | Free-text comment |
| `Product` | `string` | Product category |

### Sheet 2 — Closed Positions

Metadata properties on `ClosedPositionsSection`:

| Property        | Type       | Description                      |
| --------------- | ---------- | -------------------------------- |
| `AccountNumber` | `string`   | XTB account number               |
| `DateFrom`      | `DateTime` | Start of the report period (UTC) |
| `DateTo`        | `DateTime` | End of the report period (UTC)   |
| `ProfitOrLoss`  | `decimal`  | Total profit/loss for the period |

Closed Positions

| Property | Type | Description |
|---|---|---|
| `Instrument` | `string` | Instrument name |
| `Category` | `string` | Instrument category |
| `Ticker` | `string` | Ticker symbol |
| `Type` | `string` | Trade direction (e.g. "Buy") |
| `Volume` | `string` | Trade volume |
| `OpenPrice` | `decimal` | Price at position open |
| `OpenTime` | `DateTime` | Timestamp of position open (UTC) |
| `ClosePrice` | `decimal` | Price at position close |
| `CloseTime` | `DateTime` | Timestamp of position close (UTC) |
| `Product` | `string` | Product category |
| `ProfitOrLoss` | `string` | Realized profit or loss |
| `GrossProfit` | `string` | Gross profit before costs |
| `PurchaseValue` | `decimal` | Total purchase value |
| `SaleValue` | `decimal` | Total sale value |
| `StopLoss` | `string` | Stop loss level |
| `TakeProfit` | `string` | Take profit level |
| `Commission` | `string` | Commission charged |
| `Margin` | `string` | Margin used |
| `Swap` | `string` | Overnight swap cost |
| `Rollover` | `string` | Rollover cost |
| `OpenConversionRate` | `string` | Currency conversion rate at open |
| `CloseConversionRate` | `string` | Currency conversion rate at close |
| `CloseOrigin` | `string` | How the position was closed |
| `PositionId` | `string` | Unique position identifier |
| `Comment` | `string` | Free-text comment |

## Demo Project

The repository includes a sample CLI project in `sources/Xtb.Toolkit.Demo` that:

- loads `report.xlsx` from the working directory
- prints the Cash Operations info in a table
- prints the Closed Positions info in a table

You can use it as a reference implementation for your own tools.

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.
