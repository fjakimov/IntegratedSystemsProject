using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Interface;
using System.Security.Claims;

namespace HoneyShop.Web.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _productService.getAllProducts();
            return View(products);
        }

        [Authorize(Policy = "AdminPolicy")]
        public IActionResult CreateNewProduct()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> CreateNewProduct(Product product)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            await _productService.CreateNewProduct(product, userId);
            return RedirectToAction("Index");
        }

        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> UpdateProduct(Guid id)
        {
            var product = await _productService.findProductById(id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [HttpPost]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> UpdateProduct(Product product)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            await _productService.UpdateProduct(product, userId);
            return RedirectToAction("Index");
        }

        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            await _productService.DeleteProduct(id, userId);
            return RedirectToAction("Index");
        }
    }
}
