namespace DustInTheWind.Xtb.Toolkit;

public class ReportDocument
{
	public CashOperationsSection CashOperationsSection { get; set; }

	public ClosedPositionsSection ClosedPositionsSection { get; set; }
	
	public static async Task<ReportDocument> LoadFromFileAsync(string filePath, DocumentSection section, CancellationToken cancellationToken = default)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(filePath);

		try
		{
			using StreamReader streamReader = File.OpenText(filePath);
			return await LoadInternalAsync(streamReader, section, cancellationToken);
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

	public static async Task<ReportDocument> LoadAsync(Stream stream, DocumentSection section, CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(stream);

		try
		{
			using StreamReader streamReader = new(stream);
			return await LoadInternalAsync(streamReader, section, cancellationToken);
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

	public static async Task<ReportDocument> LoadAsync(FileInfo fileInfo, DocumentSection section, CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(fileInfo);

		try
		{
			using StreamReader streamReader = fileInfo.OpenText();
			return await LoadInternalAsync(streamReader, section, cancellationToken);
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

	public static Task<ReportDocument> LoadAsync(StreamReader streamReader, DocumentSection section, CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(streamReader);

		return LoadInternalAsync(streamReader, section, cancellationToken);
	}

	public static Task<ReportDocument> LoadAsync(TextReader textReader, DocumentSection section, CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(textReader);

		return LoadInternalAsync(textReader, section, cancellationToken);
	}

	private static async Task<ReportDocument> LoadInternalAsync(TextReader textReader, DocumentSection section, CancellationToken cancellationToken)
	{
		try
		{
			ReportDocument reportDocument = new();

			if ((section & DocumentSection.CashOperations) == DocumentSection.CashOperations)
			{
				// todo: open the spreadsheet document and extract the Cash Operations information.
				CashOperationsSection cashOperationsSection = null;
				
				reportDocument.CashOperationsSection = cashOperationsSection;
			}

			if ((section & DocumentSection.ClosedPositions) == DocumentSection.ClosedPositions)
			{
				// todo: open the spreadsheet document and extract the Closed Positions information.
				ClosedPositionsSection closedPositionsSection = null;
				
				reportDocument.ClosedPositionsSection = closedPositionsSection;
			} 

			return reportDocument;
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