using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeaAndCoffee_Models
{
    public class OrderHeader
    {
        [Key]
        public int Id { get; set; }

        public string CreatedByUserId { get; set; }

        [ForeignKey("CreatedByUserId")]
        public ApplicationUser CreatedBy { get; set; }


        public DateTime OrderDate { get; set; }
        [Required]
        public DateTime ShippingDate { get; set; }
        [Required]
        public double FinalOrderTotal { get; set; }
        [Required]
        public string OrderStatus { get; set; }

        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string FullName { get; set; }
        public string Email { get; set; }
    }
}
