using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Up_Img.DataAccess.Data;
using Up_Img.DataAccess.Repository.IRepository;
using Up_Img.Models.Models;

namespace Up_Img.DataAccess.Repository
{
    public class BrandRepository : Repository<Brand>, IBrandRepository
    {
        private readonly ApplicationDbContext _db;
        public BrandRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task Update(Brand brand)
        {
            var brandItem = await _db.Brand.FirstOrDefaultAsync(x => x.Id == brand.Id);
            if(brandItem != null)
            {
                brandItem.Name = brand.Name;
                brandItem.Status = brand.Status;
            }
        }
    }
}
