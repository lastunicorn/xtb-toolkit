namespace DustInTheWind.Xtb.Toolkit;

public class CashOperation
{
	public CashOperationType Type { get; set; }

	public string Ticker { get; set; }

	public string Instrument { get; set; }

	public DateTime Time { get; set; }

	public decimal Amount { get; set; }

	public string Id { get; set; }

	public string Comment { get; set; }

	public string Product { get; set; }
}