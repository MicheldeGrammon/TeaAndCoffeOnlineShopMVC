using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using TeaAndCoffee_DataAccess.Repository.IRepository;
using TeaAndCoffee_Models;
using TeaAndCoffee_Models.ViewModels;
using TeaAndCoffee_Utility;

namespace TeaAndCoffee.Controllers
{
    [Authorize(Roles = WC.AdminRole)]
    public class InquiryController : Controller
    {
        private readonly IInquiryHeaderRepository _inqHRepo;
        private readonly IInquiryDetailsRepository _inqDRepo;

        [BindProperty]
        public InquiryVM InquiryVM { get; set; }

        public InquiryController(IInquiryHeaderRepository inquiryHeaderRepository,
                                 IInquiryDetailsRepository inquiryDetailsRepository)
        {
            _inqDRepo= inquiryDetailsRepository;
            _inqHRepo= inquiryHeaderRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetInquiryList() 
        { 
            return Json(new {data = _inqHRepo.GetAll()});
        }

        public IActionResult Details(int id)
        {
            InquiryVM = new InquiryVM()
            {
                InquiryHeader = _inqHRepo.FirstOrDefault(x => x.Id == id),
                InquiryDetails = _inqDRepo.GetAll(x => x.InquiryHeaderId == id, includeProperties: "Product")
            };
            return View(InquiryVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Details()
        {
            InquiryVM.InquiryDetails = _inqDRepo.GetAll(x=>x.InquiryHeaderId==InquiryVM.InquiryHeader.Id);

            List<ShoppingCart> shoppingCartList = new List<ShoppingCart>();

            foreach (var detail in InquiryVM.InquiryDetails)
            {
                ShoppingCart shoppingCart = new ShoppingCart()
                {
                    ProductId = detail.ProductId
                };
                shoppingCartList.Add(shoppingCart);
            }
            HttpContext.Session.Clear();
            HttpContext.Session.Set(WC.SessionCart, shoppingCartList);
            HttpContext.Session.Set(WC.SessionInquiryId, InquiryVM.InquiryHeader.Id);

            return RedirectToAction("index", "Cart");
        }

        [HttpPost]
        public IActionResult Delete()
        {
            InquiryHeader inquiryHeader = _inqHRepo.FirstOrDefault(x => x.Id == InquiryVM.InquiryHeader.Id);
            IEnumerable<InquiryDetails> inquiryDetails = _inqDRepo.GetAll(x => x.InquiryHeaderId == InquiryVM.InquiryHeader.Id);

            _inqDRepo.RemoveRange(inquiryDetails);
            _inqHRepo.Remove(inquiryHeader);

            _inqHRepo.Save();

            return RedirectToAction(nameof(Index));
        }
    }
}
