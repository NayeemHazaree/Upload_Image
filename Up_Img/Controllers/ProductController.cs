using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Up_Img.DataAccess.Repository.IRepository;
using Up_Img.Models.Models;
using Up_Img.Utility;

namespace Up_Img.Controllers
{
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public ProductController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IEnumerable<Product> prodList = await _unitOfWork.Product.GetAllAsync(includeProperties:WC.BrandName);
            return View(prodList);
        }

        [HttpGet]
        public async Task<IActionResult> Upsert(Guid id)
        {
            Product product = new Product();
            product.BrandNames = await _unitOfWork.Product.DropDownList(WC.BrandName);
            if(id == Guid.Empty)
            {
                return View(product);
            }
            else
            {
                var productItem = await _unitOfWork.Product.FirstOrDefaultAsync(x => x.Id == id);
                productItem.BrandNames = await _unitOfWork.Product.DropDownList(WC.BrandName);
                return View(productItem);
            }
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Upsert(Product product)
        {
            var files = HttpContext.Request.Form.Files;
            if (product.Id == Guid.Empty)
            {
                
                if (ModelState.IsValid)
                {
                    if (files.Count > 0)
                    {
                        product.Img_Path = await _unitOfWork.Product.UploadImg(files,product.Id);
                    }
                    await _unitOfWork.Product.AddAsync(product);
                }
            }
            else
            {
                if (ModelState.IsValid)
                {
                    if (files.Count > 0)
                    {
                        product.Img_Path = await _unitOfWork.Product.UploadImg(files,product.Id);
                    }
                    else
                    {
                        var existItem = await _unitOfWork.Product.FirstOrDefaultAsync(x => x.Id == product.Id);
                        if(existItem.Img_Path != null)
                        {
                            product.Img_Path = existItem.Img_Path;
                        }
                    }
                    await _unitOfWork.Product.Update(product);
                }
            }
            await _unitOfWork.SaveAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (id != Guid.Empty)
            {
                var delProd = await _unitOfWork.Product.FirstOrDefaultAsync(x => x.Id == id);
                delProd.BrandNames = await _unitOfWork.Product.DropDownList(WC.BrandName);
                return View(delProd);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Product product)
        {
            var delProd = await _unitOfWork.Product.FirstOrDefaultAsync(x => x.Id == product.Id);
            if(delProd != null)
            {
                if(delProd.Img_Path != null)
                {
                    await _unitOfWork.Product.DeletePhoto(delProd.Id);
                }
                await _unitOfWork.Product.Remove(delProd);
                await _unitOfWork.SaveAsync();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return NotFound();
            }
        }
    }
}
