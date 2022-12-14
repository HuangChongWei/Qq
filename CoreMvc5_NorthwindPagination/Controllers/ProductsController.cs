using Microsoft.AspNetCore.Mvc;
using CoreMvc5_NorthwindPagination.Models;
using System.Linq;
using System.Collections.Generic;

namespace CoreMvc5_NorthwindPagination.Controllers
{
    public class ProductsController : Controller
    {
        private static int totalRows = -1;
        private readonly NorthwindContext _ctx;
        public ProductsController(NorthwindContext ctx)
        {
            _ctx = ctx;

            if (totalRows == -1)
            {
                totalRows = _ctx.Products.Count();   //計算總筆數
            }

        }
        public IActionResult Index(int id=1)
        {
            int activePage = id; ////目前所在頁
            int pageRows = 10;   //每頁幾筆資料
            //int totalRows = _ctx.Clothing.Count();   //計算總筆數

            //計算Page頁數
            int Pages = 0;
            if (totalRows % pageRows == 0)
            {
                Pages = totalRows / pageRows;
            }
            else
            {
                Pages = (totalRows / pageRows) + 1;
            }

            int startRow = (activePage - 1) * pageRows;  //起始記錄Index
            List<Product> clothing = _ctx.Products.OrderBy(x => x.ProductId).Skip(startRow).Take(pageRows).ToList();


            ViewData["Active"] = 1;    //SidebarActive頁碼
            ViewData["ActivePage"] = id;    //Activec分頁碼
            ViewData["Pages"] = Pages;  //頁數

            return View(clothing);
        }


        public IActionResult FindCategory(int? id)
        {
            if (id==null)
            {
                ViewData["ResultMessage"] = "請提供Product產品分類Id";
                return View("Result");
            }

            var products = _ctx.Products.Where(x => x.CategoryId == id).ToList();

            if (products.Count==0)
            {
                ViewData["ResultMessage"] = "找不到此產品分類Id";
                return View("Result");
            }

            return View(products);
        }
    }
}
