using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Up_Img.Models.Models;

namespace Up_Img.DataAccess.Repository.IRepository
{
    public interface IBrandRepository:IRepository<Brand>
    {
        Task Update(Brand brand);
    }
}
