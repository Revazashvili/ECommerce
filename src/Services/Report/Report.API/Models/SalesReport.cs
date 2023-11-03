namespace Report.API.Models;

public class SalesReport
{
    public SalesReport(string productName, int totalQuantity, decimal amountQuantity)
    {
        ProductName = productName;
        TotalQuantity = totalQuantity;
        AmountQuantity = amountQuantity;
    }

    public string ProductName { get; set; }
    public int TotalQuantity { get; set; }
    public decimal AmountQuantity { get; set; }
}