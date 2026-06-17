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
		// DataGrid dataGrid = new()
		// {
		// 	Title = "Transactions",
		// 	BorderTemplate = BorderTemplate.PlusMinusBorderTemplate,
		// 	Footer = $"Count: {document.Count}"
		// };
		//
		// dataGrid.Columns.Add("Date");
		// dataGrid.Columns.Add("Transaction ID");
		// dataGrid.Columns.Add("Details");
		// dataGrid.Columns.Add("Turnover", HorizontalAlignment.Right);
		// dataGrid.Columns.Add("Balance", HorizontalAlignment.Right);
		// dataGrid.Columns.Add("Currency", HorizontalAlignment.Right);
		// dataGrid.Columns.Add("Payment Type");
		//
		// foreach (TransactionRecord transaction in document)
		// 	dataGrid.Rows.Add(
		// 		transaction.Date.ToString("yyyy-MM-dd HH:mm:ss"),
		// 		transaction.TransactionId?.Truncate(30),
		// 		transaction.Details?.Truncate(30),
		// 		transaction.Turnover.ToString(),
		// 		transaction.Balance.ToString(),
		// 		transaction.Currency,
		// 		transaction.PaymentType);
		//
		// dataGrid.Display();
	}
}