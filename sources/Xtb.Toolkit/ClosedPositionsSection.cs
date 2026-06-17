namespace DustInTheWind.Xtb.Toolkit;

public class ClosedPositionsSection
{
	public string AccountNumber { get; set; }

	public DateTime DateFrom { get; set; }
	
	public DateTime DateTo { get; set; }

	public List<ClosedOperation> ClosedOperations { get; } = [];

	public decimal ProfitOrLoss { get; set; }
}