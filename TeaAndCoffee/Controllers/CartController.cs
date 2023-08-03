using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TeaAndCoffee_DataAccess;
using TeaAndCoffee_Models;
using TeaAndCoffee_Utility;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using TeaAndCoffee_Models.ViewModels;
using TeaAndCoffee_DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Text;

namespace TeaAndCoffee.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly IApplicationUserRepository _userRepo;
        private readonly IProductRepository _prodRepo;
        private readonly IOrderHeaderRepository _orderHRepo;
        private readonly IOrderDetailRepository _orderDRepo;

        private readonly IWebHostEnvironment _webHostEnvironment;

        [BindProperty]
        public ProductUserVM ProductUserVM { get; set; }

        public CartController(IApplicationUserRepository userRepo,
                              IProductRepository prodRepo,
                              IWebHostEnvironment webHostEnvironment,
                              IOrderHeaderRepository orderHRepo,
                              IOrderDetailRepository orderDRepo)
        {
            _userRepo = userRepo;
            _prodRepo = prodRepo;
            _webHostEnvironment = webHostEnvironment;
            _orderHRepo = orderHRepo;
            _orderDRepo = orderDRepo;
        }

        public IActionResult Index()
        {
            var shoppingCartsList = new List<ShoppingCart>();

            if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart) != null
                && HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart).Count() > 0)
            {
                shoppingCartsList = HttpContext.Session.Get<List<ShoppingCart>>(WC.SessionCart);
            }

            List<int> prodInCart = shoppingCartsList.Select(x => x.ProductId).ToList();
            IEnumerable<Product> productListTemp = _prodRepo.GetAll(x => prodInCart.Contains(x.Id));
            IList<Product> productList = new List<Product>();

            foreach (var cartItem in shoppingCartsList)
            {
                Product prodTemp = productListTemp.FirstOrDefault(x => x.Id == cartItem.ProductId);
                prodTemp.TempWeight = cartItem.Weight;
                productList.Add(prodTemp);
            }

            return View(productList);
        }

        public IActionResult Remove(int? id)
        {
            var shoppingCartsList = new List<ShoppingCart>();

            if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart) != null
                && HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart).Count() > 0)
            {
                shoppingCartsList = HttpContext.Session.Get<List<ShoppingCart>>(WC.SessionCart);
            }

            shoppingCartsList.Remove(shoppingCartsList.FirstOrDefault(x => x.ProductId == id));

            HttpContext.Session.Set(WC.SessionCart, shoppingCartsList);
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Index")]
        public IActionResult IndexPost()
        {
            return RedirectToAction(nameof(Summary));
        }

        public IActionResult Summary()
        {
            ApplicationUser applicationUser;

            if (User.IsInRole(WC.AdminRole))
            {
                applicationUser = new ApplicationUser();
            }
            else
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                applicationUser = _userRepo.FirstOrDefault(x => x.Id == claim.Value);
            }

            var shoppingCartsList = new List<ShoppingCart>();

            if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart) != null
                && HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart).Count() > 0)
            {
                shoppingCartsList = HttpContext.Session.Get<List<ShoppingCart>>(WC.SessionCart);
            }

            List<int> prodInCart = shoppingCartsList.Select(x => x.ProductId).ToList();
            IEnumerable<Product> productList = _prodRepo.GetAll(x => prodInCart.Contains(x.Id));

            ProductUserVM = new ProductUserVM()
            {
                ApplicationUser = applicationUser,
            };

            foreach (var cartItem in shoppingCartsList)
            {
                Product prodTemp = _prodRepo.FirstOrDefault(x => x.Id == cartItem.ProductId);
                prodTemp.TempWeight = cartItem.Weight;
                ProductUserVM.ProductList.Add(prodTemp);
            }

            return View(ProductUserVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Summary")]
        public async Task<IActionResult> SummaryPost(ProductUserVM ProductUserVM)
        {
            var claimsIdeintity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdeintity.FindFirst(ClaimTypes.NameIdentifier);

            OrderHeader orderHeader = new OrderHeader()
            {
                CreatedByUserId = claim.Value,
                FinalOrderTotal = ProductUserVM.ProductList.Sum(x => x.TempWeight * x.Price / 100),
                Address = ProductUserVM.ApplicationUser.Address,
                FullName = ProductUserVM.ApplicationUser.FullName,
                Email = ProductUserVM.ApplicationUser.Email,
                PhoneNumber = ProductUserVM.ApplicationUser.PhoneNumber,
                OrderDate = DateTime.Now,
                OrderStatus = WC.StatusPending
            };

            if (User.IsInRole(WC.AdminRole))
            {
                orderHeader.OrderStatus = WC.StatusInProcces;
            }

            _orderHRepo.Add(orderHeader);
            _orderHRepo.Save();

            foreach (var prod in ProductUserVM.ProductList)
            {
                var orderDetail = new OrderDetail()
                {
                    OrderHeaderId = orderHeader.Id,
                    Price = prod.Price,
                    Weight = prod.TempWeight,
                    ProductId = prod.Id
                };

                _orderDRepo.Add(orderDetail);
            }

            _orderDRepo.Save();

            return RedirectToAction(nameof(InquiryConfirmation), new { id = orderHeader.Id });
        }

        public IActionResult InquiryConfirmation(int id = 0)
        {
            OrderHeader orderHeader = _orderHRepo.FirstOrDefault(x => x.Id == id);
            HttpContext.Session.Clear();
            return View(orderHeader);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateCart(IEnumerable<Product> prodList)
        {
            List<ShoppingCart> shoppingCartList = new List<ShoppingCart>();
            foreach (var prod in prodList)
            {
                shoppingCartList.Add(new ShoppingCart() { ProductId = prod.Id, Weight = prod.TempWeight });
            }

            HttpContext.Session.Set(WC.SessionCart, shoppingCartList);
            return RedirectToAction(nameof(Index));
        }
    }
}
