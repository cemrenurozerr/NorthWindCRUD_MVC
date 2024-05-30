using Microsoft.AspNetCore.Mvc.Rendering;

namespace NorthWindCRUD.Models.ViewsModel
{
    public class ProductVM
    {
        public Product Product { get; set; }
        public Product_Dto Product_Dtos { get; set; }
        public List<Product_Dto> Products { get; set; }
        public SelectList Categories { get; set; }
        public SelectList Suppliers { get; set; }
    }
}
