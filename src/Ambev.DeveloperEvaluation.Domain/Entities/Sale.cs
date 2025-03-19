using Ambev.DeveloperEvaluation.Domain.Common;
using Serilog;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

public class Sale : BaseEntity
{
    public string SaleNumber { get; set; } = string.Empty;
    public DateTime SaleDate { get; set; }
    public string Customer { get; set; } = string.Empty;
    public decimal TotalSaleAmount { get; private set; }
    public string Branch { get; set; } = string.Empty;
    public List<SaleItem> Items { get; set; } = new List<SaleItem>();
    public bool IsCancelled { get; private set; }

    public Sale()
    {
        SaleDate = DateTime.UtcNow;
    }

    public void AddItem(SaleItem item)
    {
        if (item.Quantity > 20)
        {
            throw new DomainException("Cannot sell more than 20 identical items.");
        }

        item.SaleId = this.Id;
        Items.Add(item);
        CalculateTotalSaleAmount();
    }

    public void RemoveItem(SaleItem item)
    {
        item.SaleId = Guid.Empty;
        Items.Remove(item);
        CalculateTotalSaleAmount();
    }

    public void CalculateTotalSaleAmount()
    {
        TotalSaleAmount = Items.Sum(item => item.TotalAmount);
    }

    public void CancelSale()
    {
        IsCancelled = true;
        LogEvent("SaleCancelled");
    }

    public void CancelItem(SaleItem item)
    {
        item.IsCancelled = true;
        CalculateTotalSaleAmount();
        LogEvent("ItemCancelled");
    }

    private void LogEvent(string eventName)
    {
        Log.Information($"{eventName} event logged for Sale: {SaleNumber}");
    }
}
