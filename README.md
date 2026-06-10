# XTB Toolkit

[![GitHub Repo](https://img.shields.io/badge/github-repo-blue?logo=github)](https://github.com/lastunicorn/xtb-toolkit) [![GitHub Build](https://img.shields.io/github/actions/workflow/status/lastunicorn/xtb-toolkit/build-master.yml?logo=github)](https://github.com/lastunicorn/xtb-toolkit/actions/workflows/build-master.yml) [![NuGet Version](https://img.shields.io/nuget/v/DustInTheWind.Xtb.Toolkit?logo=nuget)](https://www.nuget.org/packages/DustInTheWind.Xtb.Toolkit) [![NuGet Downloads](https://img.shields.io/nuget/dt/DustInTheWind.Xtb.Toolkit?logo=nuget)](https://www.nuget.org/packages/DustInTheWind.Xtb.Toolkit)

`XTB Toolkit` is a .NET library that helps working with files exported from XTB.

**XTB S.A.** is a Warsaw brokerage firm that provides products, services, and technology  solutions for trading various financial instruments, including foreign  exchange (forex) and contracts for difference (CFDs).

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

- **Parse XTB Cash Operations Documents** - Load and parse CSV files exported directly from the XTB platform

## Quick Start

### a) Export the Transactions CSV File

In XTB XStation5 web application (https://xstation5.xtb.com):

1. Log in.
2. Select the account from the top-right drop-down.
3. Select the history tab ("Istoricul contului")
4. Click "Export" button
5. Generate a new report.

You will get a spreadsheet (.xlsx file) containing cash operations that can be parsed with this toolkit.

### b) Parse the Exported Document

```csharp
using DustInTheWind.Xtb.Toolkit;

CashOperationsDocument document = CashOperationsDocument.LoadFromFile("cash-operations.xlsx");

foreach (TransactionRecord transaction in document)
{
	...
}
```

## Spreadsheet Document

Each row is mapped to a `TransactionRecord` with the following columns:

|      |      |      |      |
|-----------------|----------|--------------------------|-----------------------------------------------------|
|      |      |      |      |
|      |      |      |      |
|      |      |      |      |
|      |      |      |      |
|      |      |      |      |
|      |      |      |      |
|      |      |      |      |

## Demo Project

The repository includes a sample CLI project in `sources/Xtb.Toolkit.Demo` that demonstrates:

- reading `cash-operations.xlsx`
- printing parsed data.

You can use this project as a reference implementation for your own importer/exporter tools.

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.
