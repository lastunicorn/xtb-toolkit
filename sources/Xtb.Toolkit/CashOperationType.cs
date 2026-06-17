namespace DustInTheWind.Xtb.Toolkit;

public sealed record class CashOperationType
{
	public static readonly CashOperationType StockPurchase = new("Stock purchase");
	public static readonly CashOperationType FreeFundsInterest = new("Free funds interest");
	public static readonly CashOperationType FreeFundsInterestTax = new("Free funds interest tax");
	public static readonly CashOperationType Deposit = new("Deposit");

	public static readonly IReadOnlyCollection<CashOperationType> KnownValues =
	[
		StockPurchase,
		FreeFundsInterest,
		FreeFundsInterestTax,
		Deposit
	];
	
	public string Value { get; }

	public CashOperationType(string value)
	{
		Value = value ?? throw new ArgumentNullException(nameof(value));
	}

	public override string ToString()
	{
		return Value;
	}

	public static implicit operator CashOperationType(string value)
	{
		return value == null
			? null
			: new CashOperationType(value);
	}

	public static implicit operator string(CashOperationType paymentType)
	{
		return paymentType?.Value;
	}
}