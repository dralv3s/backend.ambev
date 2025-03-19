namespace Ambev.DeveloperEvaluation.Domain.Entities;

public class SaleItem
{
    public Guid SaleId { get; set; }
    public string Product { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal Discount { get; set; }
    public decimal TotalAmount { get; private set; }
    public bool IsCancelled { get; set; }

    public SaleItem()
    {
        CalculateTotalAmount();
    }

    public void CalculateTotalAmount()
    {
        TotalAmount = Quantity * UnitPrice * (1 - (Discount /100));
    }
}
