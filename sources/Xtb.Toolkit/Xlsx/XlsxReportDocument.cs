using System.Globalization;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace DustInTheWind.Xtb.Toolkit.Xlsx;

internal class XlsxReportDocument : IDisposable
{
	private readonly Stream stream;
	private readonly SpreadsheetDocument spreadsheetDocument;
	private readonly WorkbookPart workbookPart;

	public XlsxReportDocument(Stream stream)
	{
		this.stream = stream ?? throw new ArgumentNullException(nameof(stream));

		spreadsheetDocument = SpreadsheetDocument.Open(stream, isEditable: false);

		workbookPart = spreadsheetDocument.WorkbookPart
			?? throw new DocumentLoadException("The spreadsheet document has no workbook part.");
	}

	public CashOperationsSection GetCashOperationsSection()
	{
		try
		{
			string[] sharedStrings = LoadSharedStrings(workbookPart);

			WorksheetPart worksheetPart = GetWorksheetPart(workbookPart, "Cash Operations");
			return ParseCashOperationsSection(worksheetPart.Worksheet, sharedStrings);
		}
		catch (DocumentLoadException)
		{
			throw;
		}
		catch (Exception ex)
		{
			throw new DocumentLoadException(ex);
		}
	}

	public ClosedPositionsSection GetClosedPositionsSection()
	{
		try
		{
			string[] sharedStrings = LoadSharedStrings(workbookPart);

			WorksheetPart worksheetPart = GetWorksheetPart(workbookPart, "Closed Positions");
			return ParseClosedPositionsSection(worksheetPart.Worksheet, sharedStrings);
		}
		catch (DocumentLoadException)
		{
			throw;
		}
		catch (Exception ex)
		{
			throw new DocumentLoadException(ex);
		}
	}

	private static string[] LoadSharedStrings(WorkbookPart workbookPart)
	{
		SharedStringTablePart sharedStringTablePart = workbookPart.SharedStringTablePart;

		if (sharedStringTablePart == null)
			return [];

		return sharedStringTablePart.SharedStringTable
			.Elements<SharedStringItem>()
			.Select(x => x.InnerText)
			.ToArray();
	}

	private static WorksheetPart GetWorksheetPart(WorkbookPart workbookPart, string sheetName)
	{
		Sheet sheet = workbookPart.Workbook?.Sheets?.Elements<Sheet>()
				.FirstOrDefault(x => string.Equals(x.Name?.Value, sheetName, StringComparison.OrdinalIgnoreCase))
			?? throw new DocumentLoadException($"Sheet '{sheetName}' not found in the spreadsheet document.");

		return (WorksheetPart)workbookPart.GetPartById(sheet.Id!);
	}

	private static CashOperationsSection ParseCashOperationsSection(Worksheet worksheet, string[] sharedStrings)
	{
		CashOperationsSection section = new();
		SheetData sheetData = worksheet.GetFirstChild<SheetData>()
			?? throw new DocumentLoadException("The 'Cash Operations' sheet contains no data.");
		List<Row> rows = sheetData.Elements<Row>().ToList();

		// Row 1: "Account number" | <account>
		section.AccountNumber = GetStringValue(FindCell(rows[0], "B"), sharedStrings);

		// Row 3: "Date from (UTC)" | <date>
		section.DateFrom = GetDateValue(FindCell(rows[2], "B"));

		// Row 4: "Date to (UTC)" | <date>
		section.DateTo = GetDateValue(FindCell(rows[3], "B"));

		// Row 5 (index 4) is the column header — skip.
		// Rows 6..N-1 are data rows; the final row holds the "Total" label and sum.
		for (int i = 5; i < rows.Count - 1; i++)
		{
			Row row = rows[i];
			CashOperation cashOperation = new()
			{
				Type = GetStringValue(FindCell(row, "A"), sharedStrings),
				Ticker = GetStringValue(FindCell(row, "B"), sharedStrings),
				Instrument = GetStringValue(FindCell(row, "C"), sharedStrings),
				Time = GetDateValue(FindCell(row, "D")),
				Amount = GetDecimalValue(FindCell(row, "E")),
				Id = GetStringValue(FindCell(row, "F"), sharedStrings),
				Comment = GetStringValue(FindCell(row, "G"), sharedStrings),
				Product = GetStringValue(FindCell(row, "H"), sharedStrings),
			};
			section.CashOperations.Add(cashOperation);
		}

		// Last row: "Total" label in A, total amount in E.
		section.Total = GetDecimalValue(FindCell(rows[^1], "E"));

		return section;
	}

	private static ClosedPositionsSection ParseClosedPositionsSection(Worksheet worksheet, string[] sharedStrings)
	{
		ClosedPositionsSection section = new();
		SheetData sheetData = worksheet.GetFirstChild<SheetData>()
			?? throw new DocumentLoadException("The 'Closed Positions' sheet contains no data.");
		List<Row> rows = sheetData.Elements<Row>().ToList();

		// Row 1: "Account" | <account>
		section.AccountNumber = GetStringValue(FindCell(rows[0], "B"), sharedStrings);

		// Row 3: "Date from (UTC)" | <date>
		section.DateFrom = GetDateValue(FindCell(rows[2], "B"));

		// Row 4: "Date to (UTC)" | <date>
		section.DateTo = GetDateValue(FindCell(rows[3], "B"));

		// Row 5 (index 4) is the column header — skip.
		// Rows 6..N-1 are data rows; the final row holds the "Profit/loss" label and sum.
		for (int i = 5; i < rows.Count - 1; i++)
		{
			Row row = rows[i];
			ClosedPosition closedPosition = new()
			{
				Instrument = GetStringValue(FindCell(row, "A"), sharedStrings),
				Category = GetStringValue(FindCell(row, "B"), sharedStrings),
				Ticker = GetStringValue(FindCell(row, "C"), sharedStrings),
				Type = GetStringValue(FindCell(row, "D"), sharedStrings),
				Volume = GetStringValue(FindCell(row, "E"), sharedStrings),
				OpenPrice = GetDecimalValue(FindCell(row, "F")),
				OpenTime = GetDateValue(FindCell(row, "G")),
				ClosePrice = GetDecimalValue(FindCell(row, "H")),
				CloseTime = GetDateValue(FindCell(row, "I")),
				Product = GetStringValue(FindCell(row, "J"), sharedStrings),
				ProfitOrLoss = GetStringValue(FindCell(row, "K"), sharedStrings),
				GrossProfit = GetStringValue(FindCell(row, "L"), sharedStrings),
				PurchaseValue = GetDecimalValue(FindCell(row, "M")),
				SaleValue = GetDecimalValue(FindCell(row, "N")),
				StopLoss = GetStringValue(FindCell(row, "O"), sharedStrings),
				TakeProfit = GetStringValue(FindCell(row, "P"), sharedStrings),
				Commission = GetStringValue(FindCell(row, "Q"), sharedStrings),
				Margin = GetStringValue(FindCell(row, "R"), sharedStrings),
				Swap = GetStringValue(FindCell(row, "S"), sharedStrings),
				Rollover = GetStringValue(FindCell(row, "T"), sharedStrings),
				OpenConversionRate = GetStringValue(FindCell(row, "U"), sharedStrings),
				CloseConversionRate = GetStringValue(FindCell(row, "V"), sharedStrings),
				CloseOrigin = GetStringValue(FindCell(row, "W"), sharedStrings),
				PositionId = GetStringValue(FindCell(row, "X"), sharedStrings),
				Comment = GetStringValue(FindCell(row, "Y"), sharedStrings),
			};
			section.ClosedPositions.Add(closedPosition);
		}

		// Last row: "Profit/loss" label in A, total in K.
		section.ProfitOrLoss = GetDecimalValue(FindCell(rows[^1], "K"));

		return section;
	}

	private static Cell FindCell(Row row, string column)
	{
		string cellReference = column + row.RowIndex;
		return row.Elements<Cell>().FirstOrDefault(x => x.CellReference?.Value == cellReference);
	}

	private static string GetStringValue(Cell cell, string[] sharedStrings)
	{
		if (cell == null || string.IsNullOrEmpty(cell.InnerText))
			return null;

		if (cell.DataType?.Value == CellValues.SharedString)
		{
			int index = int.Parse(cell.InnerText);
			return index < sharedStrings.Length ? sharedStrings[index] : null;
		}

		return cell.InnerText;
	}

	private static DateTime GetDateValue(Cell cell)
	{
		if (cell == null || string.IsNullOrEmpty(cell.InnerText))
			return default;

		double oaDate = double.Parse(cell.InnerText, CultureInfo.InvariantCulture);
		return DateTime.FromOADate(oaDate);
	}

	private static decimal GetDecimalValue(Cell cell)
	{
		if (cell == null || string.IsNullOrEmpty(cell.InnerText))
			return 0m;

		return decimal.Parse(cell.InnerText, CultureInfo.InvariantCulture);
	}

	public void Dispose()
	{
		spreadsheetDocument?.Dispose();
		stream?.Dispose();
	}
}