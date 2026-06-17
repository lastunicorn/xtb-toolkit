using DustInTheWind.Xtb.Toolkit.Xlsx;

namespace DustInTheWind.Xtb.Toolkit;

/// <summary>
/// Represents an XTB report document parsed from an .xlsx export file.
/// </summary>
public class ReportDocument
{
	public CashOperationsSection CashOperationsSection { get; set; }

	public ClosedPositionsSection ClosedPositionsSection { get; set; }

	/// <summary>
	/// Loads a report document from a file path.
	/// </summary>
	public static async Task<ReportDocument> LoadFromFileAsync(string filePath, DocumentSection section, CancellationToken cancellationToken = default)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(filePath);

		try
		{
			await using Stream stream = File.OpenRead(filePath);
			return await LoadInternalAsync(stream, section, cancellationToken);
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

	/// <summary>
	/// Loads a report document from a stream.
	/// </summary>
	public static async Task<ReportDocument> LoadAsync(Stream stream, DocumentSection section, CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(stream);

		try
		{
			return await LoadInternalAsync(stream, section, cancellationToken);
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

	/// <summary>Loads a report document from a <see cref="FileInfo"/>.</summary>
	public static async Task<ReportDocument> LoadAsync(FileInfo fileInfo, DocumentSection section, CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(fileInfo);

		try
		{
			await using Stream stream = fileInfo.OpenRead();
			return await LoadInternalAsync(stream, section, cancellationToken);
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

	private static Task<ReportDocument> LoadInternalAsync(Stream stream, DocumentSection section, CancellationToken cancellationToken)
	{
		try
		{
			ReportDocument reportDocument = new();

			using XlsxReportDocument xlsxReportDocument = new(stream);

			cancellationToken.ThrowIfCancellationRequested();

			if ((section & DocumentSection.CashOperations) == DocumentSection.CashOperations)
				reportDocument.CashOperationsSection = xlsxReportDocument.GetCashOperationsSection();

			cancellationToken.ThrowIfCancellationRequested();

			if ((section & DocumentSection.ClosedPositions) == DocumentSection.ClosedPositions)
				reportDocument.ClosedPositionsSection = xlsxReportDocument.GetClosedPositionsSection();

			return Task.FromResult(reportDocument);
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
}