using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NorthWindCRUD.Models;
using NorthWindCRUD.Models.ViewsModel;

namespace NorthWindCRUD.Controllers
{
    public class ProductController : Controller
    {
        private readonly NorthWindContext db;
        public ProductController(NorthWindContext db)
        {
            this.db = db;
            model.Products = GetAllProducts();
        }
        ProductVM model = new ProductVM();
        public IActionResult Index()
        {
            return View(model);
        }
        public IActionResult Detay(int id)
        {
            model.Product_Dtos=model.Products.FirstOrDefault(x=>x.ProductId.Equals(id));
            return View(model);
        }
        public IActionResult Guncelle(int id)
        {
            model.Categories = new SelectList(db.Categories.ToList(), "CategoryId", "CategoryName");
            model.Suppliers = new SelectList(db.Suppliers.ToList(), "SupplierId", "CompanyName");
            model.Product = db.Products.FirstOrDefault(x => x.ProductId.Equals(id));
            return View(model);
        }

        [HttpPost]
        public IActionResult Guncelle(int id, ProductVM vm)
        {
            Product updateProduct = db.Products.Find(id);
            updateProduct.ProductName = vm.Product.ProductName;
            updateProduct.SupplierId = vm.Product.SupplierId;
            updateProduct.CategoryId = vm.Product.CategoryId;
            updateProduct.QuantityPerUnit = vm.Product.QuantityPerUnit;
            updateProduct.UnitPrice = vm.Product.UnitPrice;
            updateProduct.UnitsInStock = vm.Product.UnitsInStock;
            updateProduct.UnitsOnOrder = vm.Product.UnitsOnOrder;
            updateProduct.ReorderLevel = vm.Product.ReorderLevel;
            updateProduct.Discontinued = vm.Product.Discontinued;

            db.SaveChanges();
			TempData["result"] = "Kayıt Güncellendi";
			return RedirectToAction(nameof(Index));
        }

        public IActionResult Ekle()
        {
            model.Categories = new SelectList(db.Categories.ToList(), "CategoryId", "CategoryName");
            model.Suppliers = new SelectList(db.Suppliers.ToList(), "SupplierId", "CompanyName");
            return View(model);
        }
        [HttpPost]
        public IActionResult Ekle(Product product)
        {
            db.Products.Add(product);
            db.SaveChanges();
            TempData["result"] = product.ProductName + " isminde yeni bir kayıt eklendi.";
            return RedirectToAction("Index");
        }
        public IActionResult Sil(int id)
        {
            db.Products.Remove(db.Products.Find(id));
            db.SaveChanges();
            TempData["result"] = "Silme Başarılı";
            return RedirectToAction(nameof(Index));
        }
        public List<Product_Dto> GetAllProducts()
        {
            List<Product_Dto> productList = db.Products
                .Include(p => p.Category)
                .Include(p => p.Supplier)
                .Select(p => new Product_Dto
                {
                    ProductId = p.ProductId,
                    ProductName = p.ProductName,
                    CompanyName = p.Supplier.CompanyName,
                    CategoryName = p.Category.CategoryName,
                    UnitPrice = Convert.ToDecimal(p.UnitPrice),
                    Discontinued = p.Discontinued

                })
                .ToList();
            return productList;
        }
    }
}
