using System.ComponentModel.DataAnnotations;

namespace BangazonAPI.Models
{
    public class LineItem
    {
        [Key]
        public int LineItemId { get; set; }

        public int OrderId { get; set; }
        public Order Order { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
