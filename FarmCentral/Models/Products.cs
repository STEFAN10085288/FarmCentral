namespace FarmCentral.Models
{
    public class Products
    {
        public int ID { get; set; }
        public string productName { get; set; }
        public string productType { get; set; }
        public double productPrice { get; set; }
        public double productQuantity { get; set; }
        public DateTime productDateAdded { get; set; }

        public Products()
        {
            
        }
    }
}
