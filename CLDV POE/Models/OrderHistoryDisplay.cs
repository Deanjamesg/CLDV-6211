using Microsoft.AspNetCore.Mvc;

namespace CLDV_POE.Models
{
    public class OrderHistoryDisplay 
    {
        public int OrderID { get; set; }
        public int CartID { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal CartTotal { get; set; }


    }
}
