using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeaAndCoffee_Models
{
    public class InquiryDetails
    {
        [Key]
        public int Id { get; set; }

        public int InquiryHeaderId { get; set; }
        [ForeignKey(nameof(InquiryHeaderId))]
        public InquiryHeader? InquiryHeader { get;}

        public int ProductId { get; set; }
        [ForeignKey(nameof(ProductId))]
        public Product? Product { get; set;}

    }
}
