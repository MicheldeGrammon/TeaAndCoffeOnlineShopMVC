using Microsoft.AspNetCore.Mvc;
using TeaAndCoffee_DataAccess.Repository.IRepository;

namespace TeaAndCoffee.Controllers
{
    public class InquiryController : Controller
    {
        private readonly IInquiryHeaderRepository _inqHRepo;
        private readonly IInquiryDetailsRepository _inqDRepo;

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
    }
}
