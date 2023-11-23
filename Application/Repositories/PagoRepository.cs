using System.Linq.Expressions;
using Domain.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;

namespace Application.Repository
{
    public class PagoRepository : GenericRepository<Pago> , IPago
    {
     private readonly JardineriaContext _context;
        public PagoRepository(JardineriaContext context) : base(context)
        {
            _context = context;
        }

   public override async Task<IEnumerable<Pago>> GetAllAsync()
{
 return await _context.Pagos.ToListAsync();
}  
}
}