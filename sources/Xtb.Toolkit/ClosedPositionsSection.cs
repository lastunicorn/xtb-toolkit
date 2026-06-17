namespace DustInTheWind.Xtb.Toolkit;

public class ClosedPositionsSection
{
	public string AccountNumber { get; set; }

	public DateTime DateFrom { get; set; }

	public DateTime DateTo { get; set; }

	public List<ClosedPosition> ClosedPositions { get; } = [];

	public decimal ProfitOrLoss { get; set; }
}