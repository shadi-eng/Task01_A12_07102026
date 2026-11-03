using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Task01_A12_07102026.Models;

namespace Task01_A12_07102026.Controllers
{
    /// <summary>
    /// dfd
    /// </summary>
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> IndexAsync()
        {
            //Test the UpdateProduct Method
            //Product p2 = Product.GetProductById(28);
            //p2.ProductName = "A12";
            //p2.ProductPrice = 650;
            //int a = Product.UpdateProduct(p2);

            //List<Category> categoriesList = Category.GetAllFromDB();
            // GeminiService gemini = new GeminiService();
            // string prompt = "اكتب لي حكمة واحدة قصيرة موجهة للطلاب";
            // var result = await gemini.AskAsync(prompt);
            //// List<Product> L1 = Product.SearchProductsByName("e");
            // ViewBag.AIMSG = result;
            List<Product> productsList = Product.GetAllProducts();
            ViewBag.productsList = productsList;
            return View();
        }



        public IActionResult UpdateProductView(int ProductId)
        {
            Product p1 = Product.GetProductById(ProductId);
            ViewBag.p1 = p1;
            ViewBag.categoriesList = Category.GetAllFromDB();
            
            return View(p1);
        }

        public IActionResult GoSearch(string str)
        {
            List<Product> productsList = Product.SearchProductsByName(str);
            ViewBag.productsList = productsList;
            return View("Index");
        }

        public async Task<IActionResult> UpdateProductActionAsync(Product p1, IFormFile thePhoto)
        {
            if (thePhoto != null)
            {
                string newPhotoName = await UploadFile(thePhoto, "Photos");
                p1.ProductPhoto = newPhotoName;
            }
            int x= Product.UpdateProduct(p1);
            //return View();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> AddAction(Product p1,IFormFile thePhoto)
        {
           

            if (thePhoto != null)
            {
                string newPhotoName = await UploadFile(thePhoto, "Photos");
                p1.ProductPhoto = newPhotoName;
            }
            else
            {
                p1.ProductPhoto = "p1.png";
            }

                int x = Product.AddProductToDB(p1);
            if (x > 0)
                ViewBag.msg = "done";
            else
                ViewBag.msg = "Error";
            List<Product> productsList = Product.GetAllProducts();
            ViewBag.productsList = productsList;
            return View("Index");
        }

        public IActionResult AddProductTest()
        {
            return View();
        }

        public IActionResult ShowCategories()
        {
            List<Category> categoriesList = Category.GetAllFromDB();
            ViewBag.categoriesList = categoriesList;
            return View();
        }


        public IActionResult ShowCategory(int CategoryId)
        {
            Category c1 = Category.GetCategoryById(CategoryId);
            ViewBag.c1 = c1;
            return View("CategoryView");
        }

        public IActionResult DeleteCategory(int CategoryId)
        {
            int result = Category.DeleteCategory(CategoryId);
            if (result > 0)
            {
                ViewBag.Message = "Category deleted successfully.";
            }
            else
            {
                ViewBag.Message = "Error deleting category.";
            }
            List<Category> categoriesList = Category.GetAllFromDB();
            ViewBag.categoriesList = categoriesList;
            return View("ShowCategories");
        }

        public IActionResult AddProductViewNew()
        {
            return View();
        }

        public IActionResult AddProductView()
        {
            //Get All Categories from DB and send to ViewBag
            //List<Category> categoriesList = Category.GetAllFromDB();
            ViewBag.categoriesList = Category.GetAllFromDB();
            return View();
        }
        public IActionResult AddProductAction(Product p)
        {
            return View();
        }


        public IActionResult DeleteProduct(int ProductId)
        {
            int result = Product.DeleteProductById(ProductId);
            if(result > 0)
            {
                ViewBag.Message = "Product deleted successfully.";
            }
            else
            {
                ViewBag.Message = "Error deleting product.";
            }
            List<Product> productsList = Product.GetAllProducts();
            ViewBag.productsList = productsList;
            return View("Index");
        }

        public IActionResult ShowProduct(int ProductId)
        {
            Product p1=Product.GetProductById(ProductId);
            ViewBag.p1 = p1;
            return View("ProductView");
        }

        public IActionResult Privacy()
        {
            List<Product> products = Product.GetAllProducts();
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        private async Task<string> UploadFile(IFormFile f1, string folder)
        {    

            //Where to Save File and change file name
            var baseFolder = "wwwroot";
            var newFileName = DateTime.Now.Ticks.ToString() + "_" + f1.FileName;
            var filePath = baseFolder + "/" + folder + "/" + newFileName;

            ///stream is good for large files size
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                //async meaning in other thread
                await f1.CopyToAsync(stream);
            }
            return newFileName;
        }
    }
}
