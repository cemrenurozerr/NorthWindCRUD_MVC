using Microsoft.AspNetCore.Mvc;
using NorthWindCRUD.Models;
using NorthWindCRUD.Models.ViewsModel;

namespace NorthWindCRUD.Controllers
{
	public class CategoryController : Controller
	{
		private readonly NorthWindContext db;

        public CategoryController(NorthWindContext db)
        {
            this.db = db;
        }
		CategoryVM model = new CategoryVM();
        public IActionResult Index(string search)
		{
            if (search is null)
            {
                search = "";
			}
			model.cList=db.Categories.Where(x=>x.CategoryName.Contains(search)).ToList();
			return View(model);
		}
		public IActionResult Detay(int id)
		{
            model.Category = db.Categories.FirstOrDefault(x => x.CategoryId.Equals(id));
			return View(model);
		}
        public IActionResult Guncelle(int id)
        {
            model.Category = db.Categories.FirstOrDefault(x => x.CategoryId.Equals(id));
            return View(model);
        }
        [HttpPost]
        public IActionResult Guncelle(int id,CategoryVM vm)
        {
            Category category = db.Categories.Find(id);
            category.CategoryName = vm.Category.CategoryName;
            category.Description = vm.Category.Description;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Ekle()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Ekle(Category category)
        {
            db.Categories.Add(category);
            db.SaveChanges();
            TempData["result"] = category.CategoryName + " isminde yeni bir kayıt eklendi.";
            return RedirectToAction("Index");
        }
        public IActionResult Sil(int id)
        {
            db.Categories.Remove(db.Categories.Find(id));
            db.SaveChanges();
			TempData["result"] ="Silme başarılı.";
			return RedirectToAction("Index");
        }
    }
}
