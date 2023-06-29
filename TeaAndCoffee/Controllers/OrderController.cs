using Microsoft.AspNetCore.Mvc;
using TeaAndCoffee_DataAccess.Repository.IRepository;
using TeaAndCoffee_Models;
using TeaAndCoffee_Models.ViewModels;
using TeaAndCoffee_Utility;

namespace TeaAndCoffee.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderHeaderRepository _orderHRepo;
        private readonly IOrderDetailRepository _orderDRepo;

        public OrderController(IOrderHeaderRepository orderHRepo, IOrderDetailRepository orderDRepo)
        {
            _orderDRepo = orderDRepo;
            _orderHRepo = orderHRepo;
        }

        public IActionResult Index()
        {
            OrderListVM orderListVM = new OrderListVM()
            {
                OrderHeaderList = _orderHRepo.GetAll(),
            };

            return View(orderListVM);
        }

        [HttpGet]
        public IActionResult GetOrderList()
        {
            return Json(new { data = _orderHRepo.GetAll() });
        }

        public IActionResult Details(int id)
        {
            OrderVM orderVM = new OrderVM()
            {
                OrderHeader = _orderHRepo.FirstOrDefault(x => x.Id == id),
                OrderDetails = (IList<OrderDetail>)_orderDRepo.GetAll(x => x.OrderHeaderId == id, includeProperties: "Product"),
                StatusList = WC.listStatus.ToList().Select(x => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                {
                    Text = x,
                    Value = x
                })
            };

            return View(orderVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateHeader(OrderVM orderVM)
        {
            if (WC.listStatus.Contains(orderVM.OrderHeader.OrderStatus))
            {
                var headerFromDB = _orderHRepo.FirstOrDefault(x => x.Id == orderVM.OrderHeader.Id);
                var detailsFromDB = (IList<OrderDetail>)_orderDRepo.GetAll(x=>x.OrderHeaderId==orderVM.OrderHeader.Id);

                if (headerFromDB != null && detailsFromDB != null) 
                {
                    double totalOrder = 0.0;
                    for (int i = 0; i < detailsFromDB.Count(); i++)
                    {
                        detailsFromDB[i].Weight = orderVM.OrderDetails[i].Weight;
                        totalOrder += detailsFromDB[i].Weight * detailsFromDB[i].Price/100;
                    }

                    headerFromDB.FullName = orderVM.OrderHeader.FullName;
                    headerFromDB.PhoneNumber = orderVM.OrderHeader.PhoneNumber;
                    headerFromDB.Email = orderVM.OrderHeader.Email;
                    headerFromDB.OrderStatus = orderVM.OrderHeader.OrderStatus;
                    headerFromDB.Address = orderVM.OrderHeader.Address;

                    headerFromDB.FinalOrderTotal = totalOrder;

                    

                    _orderHRepo.Update(headerFromDB);
                    _orderHRepo.Save();

                    TempData[WC.Success] = "Order updated";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData[WC.Error] = "Error. Object is null";
                    return RedirectToAction(nameof(Details));
                }
            }
            TempData[WC.Error] = "Error. Status don't updated";
            return RedirectToAction(nameof(Details));
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteOrder(OrderVM orderVM)
        {
            int id = orderVM.OrderHeader.Id;

            var headerFromDB = _orderHRepo.FirstOrDefault(x=>x.Id==id);

            if (headerFromDB == null)
            {
                TempData[WC.Error] = "Error";
                return NotFound();
            }

            _orderHRepo.Remove(headerFromDB);
            _orderHRepo.Save();

            TempData[WC.Warning] = "Order deleted successfully";

            return RedirectToAction("Index");
        }
    }
}
