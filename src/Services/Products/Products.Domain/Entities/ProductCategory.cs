namespace Products.Domain.Entities;

public class ProductCategory
{
    private ProductCategory(){ }
    public ProductCategory(int id)
    {
        Id = id;
    }

    public ProductCategory(string name)
    {
        Name = name;
    }

    public ProductCategory(int id, string name)
    {
        Id = id;
        Name = name;
    }

    public int Id { get; set; }
    public string Name { get; set; }
    public List<Product> Products { get; set; }
}