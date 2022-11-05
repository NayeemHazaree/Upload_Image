using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Up_Img.DataAccess.Data;
using Up_Img.DataAccess.Repository.IRepository;
using Up_Img.Models.Models;
using Up_Img.Utility;

namespace Up_Img.DataAccess.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductRepository(ApplicationDbContext db, IWebHostEnvironment webHostEnvironment) : base(db)
        {
            _db = db;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task DeletePhoto(Guid prodID)
        {
            var existProd = await _db.Product.FirstOrDefaultAsync(x => x.Id == prodID);
            string webRootPath = _webHostEnvironment.WebRootPath;
            string upload = webRootPath + WC.ProductImg;
            var path = existProd.Img_Path;
            var imageData = Path.Combine(upload, path.TrimStart('\\'));
            if (System.IO.File.Exists(imageData))
            {
                System.IO.File.Delete(imageData);
            }
        }

        public async Task<IEnumerable<SelectListItem>> DropDownList(string obj)
        {
            if(obj == WC.BrandName)
            {
                return _db.Brand.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                });
            }
            return await Task.FromResult<IEnumerable<SelectListItem>>(null);
        }

        public async Task Update(Product product)
        {
            //_db.Product.Update(product);
            var proditem = await _db.Product.FirstOrDefaultAsync(x => x.Id == product.Id);
            if (proditem != null)
            {
                proditem.Name = product.Name;
                proditem.BrandId = product.BrandId;
                proditem.Img_Path = product.Img_Path;
                proditem.Price = product.Price;
            }
        }

        public async Task<string> UploadImg(IFormFileCollection fileObj,Guid prodID)
        {
            var existProd = await _db.Product.FirstOrDefaultAsync(x => x.Id == prodID);
            string webRootPath = _webHostEnvironment.WebRootPath;
            string upload = webRootPath + WC.ProductImg;
            if (existProd == null)
            {
                string fileName = Guid.NewGuid().ToString();
                string extension = Path.GetExtension(fileObj[0].FileName);
                using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                {
                    fileObj[0].CopyTo(fileStream);
                }
                return fileName + extension;
            }
            else
            {
                //delete the previous photo 
                await DeletePhoto(prodID);

                //upload new one
                string fileName = Guid.NewGuid().ToString();
                string extension = Path.GetExtension(fileObj[0].FileName);
                using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                {
                    fileObj[0].CopyTo(fileStream);
                }
                return fileName + extension;
            }
        }
    }
}
