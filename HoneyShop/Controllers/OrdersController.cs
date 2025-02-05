using GemBox.Document;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Interface;
using System.Text;

namespace HoneyShop.Web.Controllers
{
    [Authorize(Policy = "AdminPolicy")]
    public class OrdersController : Controller
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
            ComponentInfo.SetLicense("FREE-LIMITED-KEY"); // Set Gembox License

        }

        // Action to view all orders
        public async Task<IActionResult> Index()
        {
            var orders = await _orderService.GetAllOrders();
            return View(orders);  // Display orders in a view
        }

        // Action to create invoice for a specific order
        public async Task<IActionResult> CreateInvoice(Guid orderId)
        {
            var order = await _orderService.GetOrderById(orderId);

            if (order == null)
            {
                TempData["ErrorMessage"] = "Order not found.";
                return RedirectToAction(nameof(Index));
            }

            var document = new DocumentModel();

            var section = new Section(document);
            document.Sections.Add(section);

            section.Blocks.Add(new Paragraph(document, "Invoice")
            {
                ParagraphFormat = new ParagraphFormat() { Alignment = HorizontalAlignment.Center, SpaceAfter = 20 }
            });

            section.Blocks.Add(new Paragraph(document, $"Order Number: {order.OrderId}"));
            section.Blocks.Add(new Paragraph(document, $"Order Date: {order.OrderDate:yyyy-MM-dd}"));
            section.Blocks.Add(new Paragraph(document, $"Customer Name: {order.User.FullName}"));

            StringBuilder productsList = new StringBuilder();
            decimal totalPrice = 0;

            foreach (var item in order.OrderProducts)
            {
                productsList.AppendLine($"{item.Product.Name} - {item.Quantity} x {item.Product.Price:C}");
                totalPrice += item.Quantity * item.Product.Price;
            }

            section.Blocks.Add(new Paragraph(document, $"Products: \n{productsList}"));
            section.Blocks.Add(new Paragraph(document, $"Total Price: {totalPrice} $"));

            var stream = new MemoryStream();
            document.Save(stream, new PdfSaveOptions());

            return File(stream.ToArray(), "application/pdf", $"Invoice_{order.OrderId}.pdf");
        }
    }
}
