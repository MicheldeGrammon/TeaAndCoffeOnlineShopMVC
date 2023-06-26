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
                OrderDetails = _orderDRepo.GetAll(x => x.OrderHeaderId == id, includeProperties: "Product"),
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
        public IActionResult UpdateStatus(OrderVM orderVM)
        {
            if (WC.listStatus.Contains(orderVM.OrderHeader.OrderStatus))
            {
                var objFromDB = _orderHRepo.FirstOrDefault(x => x.Id == orderVM.OrderHeader.Id);
                if (objFromDB != null) 
                {
                    objFromDB.OrderStatus = orderVM.OrderHeader.OrderStatus;
                    
                    _orderHRepo.Update(objFromDB);
                    _orderHRepo.Save();
                    TempData[WC.Success] = "Status order updated";
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
    }
}
