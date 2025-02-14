using Domain.DTO;
using Microsoft.AspNetCore.Mvc;
using Services.Interface;

namespace HoneyShop.Web.Controllers
{
    public class PartnerStoreController : Controller
    {
        private readonly IPartnerStoreService _partnerStoreService;

        public PartnerStoreController(IPartnerStoreService partnerStoreService)
        {
            _partnerStoreService = partnerStoreService;
        }


        public async Task<IActionResult> Index()
        {
            var books = await _partnerStoreService.GetBooksForDisplayAsync();
            var totalOrders = await _partnerStoreService.GetTotalOrdersAsync();

            var viewModel = new PartnerStoreViewModel 
            {
                Books = books,
                TotalOrders = totalOrders
            };

            return View(viewModel);

        }
    }
}
