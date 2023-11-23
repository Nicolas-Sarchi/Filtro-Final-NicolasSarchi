using System.Linq.Expressions;
using Domain.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;

namespace Application.Repository
{
    public class GamaProductoRepository : GenericRepository<GamaProducto> , IGamaProducto
    {
     private readonly JardineriaContext _context;
        public GamaProductoRepository(JardineriaContext context) : base(context)
        {
            _context = context;
        }

   public override async Task<IEnumerable<GamaProducto>> GetAllAsync()
{
 return await _context.GamaProductos.ToListAsync();
}  
}
}