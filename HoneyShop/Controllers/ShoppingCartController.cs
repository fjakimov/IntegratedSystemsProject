using Domain.DTO;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Services.Implementation;
using Services.Interface;
using Stripe;
using Stripe.Checkout;
using System.Security.Claims;

namespace HoneyShop.Web.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly IShoppingCartService _shoppingCartService;
        private readonly IOrderService _orderService;
        public ShoppingCartController(IShoppingCartService shoppingCartService, IOrderService orderService)
        {
            _shoppingCartService = shoppingCartService;
            _orderService = orderService;
        }


        public async Task<IActionResult> AddToCart(AddToCartDTO addToCartDto)
        {
            if (!User.Identity.IsAuthenticated)
            {
                TempData["ErrorMessage"] = "You must be logged in to add products to the cart.";
                return RedirectToAction("Login", "Account");
            }

            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out Guid userGuid))
            {
                TempData["ErrorMessage"] = "You must be logged in to add products to the cart.";
                return RedirectToAction("Login", "Account");
            }
            try
            {
                await _shoppingCartService.AddProductToShoppingCart(userGuid, addToCartDto);
                TempData["SuccessMessage"] = "Product added to cart successfully!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }

            return RedirectToAction("Index", "Products");
        }

        public async Task<IActionResult> Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                TempData["ErrorMessage"] = "You must be logged in to view the cart.";
                return RedirectToAction("Login", "Account");
            }

            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out Guid userGuid))
            {
                TempData["ErrorMessage"] = "You must be logged in to view the cart.";
                return RedirectToAction("Login", "Account");
            }

            var cart = await _shoppingCartService.GetCartByUserId(userGuid);

            return View(cart);

        }
        [HttpPost]
        public async Task<IActionResult> RemoveFromCart(CartItemDTO cartItemDTO)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out Guid userGuid))
            {
                TempData["ErrorMessage"] = "You must be logged in to add products to the cart.";
                return RedirectToAction("Login", "Account");
            }
            await _shoppingCartService.RemoveProductFromShoppingCart(userGuid, cartItemDTO);
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> UpdateCartItem(Guid selectedProductId, int quantity)
        {
            if (!User.Identity.IsAuthenticated)
            {
                TempData["ErrorMessage"] = "You must be logged in to update the cart.";
                return RedirectToAction("Login", "Account");
            }

            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out Guid userGuid))
            {
                TempData["ErrorMessage"] = "Invalid user ID.";
                return RedirectToAction("Login", "Account");
            }

            await _shoppingCartService.UpdateCartItemQuantity(userGuid, selectedProductId, quantity);

            return RedirectToAction("Index");

        }
        public async Task<IActionResult> CreateCheckoutSession()
        {
            if (!User.Identity.IsAuthenticated)
            {
                TempData["ErrorMessage"] = "You must be logged in to proceed with the checkout.";
                return RedirectToAction("Login", "Account");
            }

            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out Guid userGuid))
            {
                TempData["ErrorMessage"] = "Invalid user ID.";
                return RedirectToAction("Login", "Account");
            }

            var cart = await _shoppingCartService.GetCartByUserId(userGuid);
            if (cart == null || cart.Items.Count == 0)
            {
                TempData["ErrorMessage"] = "Your cart is empty!";
                return RedirectToAction("Index");
            }

            var domain = $"{Request.Scheme}://{Request.Host}";
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = cart.Items.Select(item => new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        Currency = "usd",
                        UnitAmount = (long)(item.Price * 100), 
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.ProductName
                        }
                    },
                    Quantity = item.Quantity
                }).ToList(),
                Mode = "payment",
                SuccessUrl = $"{domain}/ShoppingCart/CheckoutSuccess?userId={userId}",
                CancelUrl = $"{domain}/ShoppingCart/Index"
            };

            var service = new SessionService();
            var session = await service.CreateAsync(options);

            return Redirect(session.Url);
        }
        public async Task<IActionResult> CheckoutSuccess(Guid userId)
        {
            if (!Guid.TryParse(userId.ToString(), out _))
            {
                TempData["ErrorMessage"] = "Invalid user ID.";
                return RedirectToAction("Index", "Products");
            }

            try
            {
                var orderResult = await _orderService.Order(userId, "Paid");

                TempData["SuccessMessage"] = "Payment successful! Your order has been placed.";
                return RedirectToAction("Index", "Products");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error: {ex.Message}";
                return RedirectToAction("Index", "Products");
            }
        }
    }
}