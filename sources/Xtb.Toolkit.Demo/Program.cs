using System.Globalization;
using DustInTheWind.ConsoleTools.Controls;
using DustInTheWind.ConsoleTools.Controls.Tables;

namespace DustInTheWind.Xtb.Toolkit.Demo;

internal static class Program
{
	public static async Task Main(string[] args)
	{
		const string fileName = "report.xlsx";

		try
		{
			ReportDocument document = await ReportDocument.LoadFromFileAsync(fileName, DocumentSection.All);

			Display(document.CashOperationsSection);
			Console.WriteLine();
			Display(document.ClosedPositionsSection);
		}
		catch (DocumentLoadException ex)
		{
			await Console.Error.WriteLineAsync($"Failed to read '{fileName}': {ex.Message}");
			Environment.ExitCode = 1;
		}
		catch (Exception ex)
		{
			await Console.Error.WriteLineAsync($"Unexpected error: {ex.Message}");
			Environment.ExitCode = 1;
		}
	}

	private static void Display(CashOperationsSection document)
	{
		if (document == null)
		{
			Console.Error.WriteLine("No Cash Operations document loaded.");
			return;
		}
		
		DataGrid dataGrid = new()
		{
			Title = new[]
			{
				$"Cash Operations for {document.AccountNumber}",
				$"{document.DateFrom:yyyy-MM-dd HH:mm:ss} - {document.DateTo:yyyy-MM-dd HH:mm:ss}"
			},
			BorderTemplate = BorderTemplate.PlusMinusBorderTemplate,
			Footer = new[]
			{
				$"Count: {document.CashOperations.Count}",
				$"Total: {document.Total}"
			}
		};

		dataGrid.Columns.Add("Type");
		dataGrid.Columns.Add("Ticker");
		dataGrid.Columns.Add("Instrument");
		dataGrid.Columns.Add("Time", HorizontalAlignment.Right);
		dataGrid.Columns.Add("Amount", HorizontalAlignment.Right);
		dataGrid.Columns.Add("ID");
		dataGrid.Columns.Add("Comment");
		dataGrid.Columns.Add("Product");

		foreach (CashOperation cashOperation in document.CashOperations)
			dataGrid.Rows.Add(
				cashOperation.Type,
				cashOperation.Ticker,
				cashOperation.Instrument,
				cashOperation.Time.ToString("yyyy-MM-dd HH:mm:ss"),
				cashOperation.Amount.ToString(CultureInfo.CurrentCulture),
				cashOperation.Id,
				cashOperation.Comment,
				cashOperation.Product);

		dataGrid.Display();
	}

	private static void Display(ClosedPositionsSection document)
	{
		if (document == null)
		{
			Console.Error.WriteLine("No Closed Positions document loaded.");
			return;
		}
		
		DataGrid dataGrid = new()
		{
			Title = new[]
			{
				$"Closed Positions for {document.AccountNumber}",
				$"{document.DateFrom:yyyy-MM-dd HH:mm:ss} - {document.DateTo:yyyy-MM-dd HH:mm:ss}"
			},
			BorderTemplate = BorderTemplate.PlusMinusBorderTemplate,
			Footer = new[]
			{
				$"Count: {document.ClosedPositions.Count}",
				$"Profit/Loss: {document.ProfitOrLoss}"
			}
		};

		dataGrid.Columns.Add("Instrument");
		dataGrid.Columns.Add("Category");
		dataGrid.Columns.Add("Ticker");
		dataGrid.Columns.Add("Type");
		dataGrid.Columns.Add("Volume");
		dataGrid.Columns.Add("Open Price", HorizontalAlignment.Right);
		dataGrid.Columns.Add("Open Time", HorizontalAlignment.Right);
		dataGrid.Columns.Add("Close Price", HorizontalAlignment.Right);
		dataGrid.Columns.Add("Close Time", HorizontalAlignment.Right);
		dataGrid.Columns.Add("Product");
		dataGrid.Columns.Add("Profit/Loss", HorizontalAlignment.Right);
		dataGrid.Columns.Add("Gross Profit", HorizontalAlignment.Right);
		dataGrid.Columns.Add("Purchase Value", HorizontalAlignment.Right);
		dataGrid.Columns.Add("Sale Value", HorizontalAlignment.Right);
		dataGrid.Columns.Add("Stop Loss", HorizontalAlignment.Right);
		dataGrid.Columns.Add("Take Profit", HorizontalAlignment.Right);
		dataGrid.Columns.Add("Commission", HorizontalAlignment.Right);
		dataGrid.Columns.Add("Margin", HorizontalAlignment.Right);
		dataGrid.Columns.Add("Swap", HorizontalAlignment.Right);
		dataGrid.Columns.Add("Rollover", HorizontalAlignment.Right);
		dataGrid.Columns.Add("Open Conv. Rate", HorizontalAlignment.Right);
		dataGrid.Columns.Add("Close Conv. Rate", HorizontalAlignment.Right);
		dataGrid.Columns.Add("Close Origin");
		dataGrid.Columns.Add("Position ID");
		dataGrid.Columns.Add("Comment");

		foreach (ClosedPosition closedOperation in document.ClosedPositions)
			dataGrid.Rows.Add(
				closedOperation.Instrument,
				closedOperation.Category,
				closedOperation.Ticker,
				closedOperation.Type,
				closedOperation.Volume,
				closedOperation.OpenPrice.ToString(CultureInfo.CurrentCulture),
				closedOperation.OpenTime.ToString("yyyy-MM-dd HH:mm:ss"),
				closedOperation.ClosePrice.ToString(CultureInfo.CurrentCulture),
				closedOperation.CloseTime.ToString("yyyy-MM-dd HH:mm:ss"),
				closedOperation.Product,
				closedOperation.ProfitOrLoss,
				closedOperation.GrossProfit,
				closedOperation.PurchaseValue.ToString(CultureInfo.CurrentCulture),
				closedOperation.SaleValue.ToString(CultureInfo.CurrentCulture),
				closedOperation.StopLoss,
				closedOperation.TakeProfit,
				closedOperation.Commission,
				closedOperation.Margin,
				closedOperation.Swap,
				closedOperation.Rollover,
				closedOperation.OpenConversionRate,
				closedOperation.CloseConversionRate,
				closedOperation.CloseOrigin,
				closedOperation.PositionId,
				closedOperation.Comment);

		dataGrid.Display();
	}
}