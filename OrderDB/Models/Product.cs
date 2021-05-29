namespace OrderDB.Models
{
    /// <summary>
    /// Örnek ürün ve Stok adedi.
    /// </summary>
    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int AvailableStock { get; set; }
    }
}