using ITRootsTask.Data;
using ITRootsTask.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ITRootsTask.Controllers
{
    public class InvoiceController : Controller
    {
        private readonly ApplicationDbContext _context;
        public InvoiceController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var Invoices = _context.Invoices.Include(i =>i.Products);
            return View(Invoices);
        }

        public IActionResult CreateInvoicePartial()
        {
            return PartialView("_CreateInvoice", new Invoice());
        }

       
        public async Task<IActionResult>  CreateNewInvoice([FromBody] Invoice invoice)
        {
            if (invoice!=null)
            {
        
            Invoice myinvoice = new Invoice();
            myinvoice.Name = invoice.Name;

            List<Product> products= new List<Product>();
            foreach (var item in invoice.Products)
            {
                Product product = new Product();
                product.ProductName = item.ProductName;
                product.Quantity = item.Quantity;
                product.Price = item.Quantity;
                product.InvoiceId = myinvoice.Id;
                products.Add(product);
            }
            myinvoice.Products= products;

            await _context.Invoices.AddAsync(myinvoice);
            await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }
    }
}
