namespace DustInTheWind.Xtb.Toolkit;

public class CashOperationsSection
{
	public string AccountNumber { get; set; }

	public DateTime DateFrom { get; set; }

	public DateTime DateTo { get; set; }

	public List<CashOperation> CashOperations { get; } = [];

	public decimal Total { get; set; }
}