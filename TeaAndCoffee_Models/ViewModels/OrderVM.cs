using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeaAndCoffee_Utility;

namespace TeaAndCoffee_Models.ViewModels
{
    public class OrderVM
    {
        public OrderHeader OrderHeader { get; set; }
        public IList<OrderDetail> OrderDetails { get; set; }

        public IEnumerable<SelectListItem>? StatusList { get; set; }
    }
}
