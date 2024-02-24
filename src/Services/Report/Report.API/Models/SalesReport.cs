namespace Report.API.Models;

public class SalesReport(string productName, int totalQuantity, decimal amountQuantity)
{
    public string ProductName { get; set; } = productName;
    public int TotalQuantity { get; set; } = totalQuantity;
    public decimal AmountQuantity { get; set; } = amountQuantity;
}