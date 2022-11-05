using Microsoft.AspNetCore.Mvc;
using Up_Img.DataAccess.Repository.IRepository;
using Up_Img.Models.Models;

namespace Up_Img.Controllers
{
    public class BrandController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public BrandController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IEnumerable<Brand> brandList = await _unitOfWork.Brand.GetAllAsync(filter: x=>x.Status == true);
            return View(brandList);
        }

        [HttpGet]
        public async Task<IActionResult> Upsert(Guid id)
        {
            Brand brand = new();
            if(id == Guid.Empty)
            {
                return View(brand);
            }
            else
            {
                var brandItem = await _unitOfWork.Brand.FirstOrDefaultAsync(x => x.Id == id);
                return View(brandItem);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Upsert(Brand brand)
        {
            if (brand.Id == Guid.Empty)
            {
                if (ModelState.IsValid)
                {
                    await _unitOfWork.Brand.AddAsync(brand);
                }
            }
            else
            {
                if (ModelState.IsValid)
                {
                    await _unitOfWork.Brand.Update(brand);
                }
            }
            await _unitOfWork.SaveAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            if(id != Guid.Empty)
            {
                var delItem = await _unitOfWork.Brand.FirstOrDefaultAsync(x => x.Id == id);
                return View(delItem);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Brand brand)
        {
            if(brand.Id != Guid.Empty)
            {
                await _unitOfWork.Brand.Remove(brand);
            }
            else
            {
                return NotFound();
            }
            await _unitOfWork.SaveAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
