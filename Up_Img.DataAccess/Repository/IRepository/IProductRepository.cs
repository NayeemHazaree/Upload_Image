using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Up_Img.Models.Models;

namespace Up_Img.DataAccess.Repository.IRepository
{
    public interface IProductRepository:IRepository<Product>
    {
        Task Update(Product product);
        Task<IEnumerable<SelectListItem>> DropDownList(string obj);
        Task<string> UploadImg(IFormFileCollection fileObj, Guid prodID);
        Task DeletePhoto(Guid prodID);
    }
}
