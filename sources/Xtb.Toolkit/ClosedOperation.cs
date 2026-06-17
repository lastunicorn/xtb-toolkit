namespace DustInTheWind.Xtb.Toolkit;

public class ClosedOperation
{
	public string Instrument { get; set; }
	
	public string Category { get; set; }
	
	public string Ticker { get; set; }
	
	public string Type { get; set; }
	
	public string Volume { get; set; }
	
	public decimal OpenPrice { get; set; }
	
	public DateTime OpenTime { get; set; }
	
	public decimal ClosePrice { get; set; }
	
	public DateTime CloseTime { get; set; }
	
	public string Product { get; set; }
	
	public string ProfitOrLoss { get; set; }
	
	public string GrossProfit { get; set; }
	
	public decimal PurchaseValue { get; set; }
	
	public decimal SaleValue { get; set; }
	
	public string StopLoss { get; set; }
	
	public string TakeProfit { get; set; }
	
	public string Commission { get; set; }
	
	public string Margin { get; set; }
	
	public string Swap { get; set; }
	
	public string Rollover { get; set; }
	
	public string OpenConversionRate { get; set; }
	
	public string CloseConversionRate { get; set; }
	
	public string CloseOrigin { get; set; }
	
	public string PositionId { get; set; }
	
	public string Comment { get; set; }
}