namespace CLDV_POE.Models
{
    public class ViewCartDisplay
    {
        public int CartID { get; set; }
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public decimal ProductPrice { get; set; }
        public string ProductURL { get; set; }
        public int Quantity { get; set; }
    }
}
