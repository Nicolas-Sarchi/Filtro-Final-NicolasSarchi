using System.Linq.Expressions;
using Domain.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;

namespace Application.Repository
{
    public class DetallePedidoRepository : GenericRepository<DetallePedido>, IDetallePedido
    {
        private readonly JardineriaContext _context;
        public DetallePedidoRepository(JardineriaContext context) : base(context)
        {
            _context = context;
        }

        public override async Task<IEnumerable<DetallePedido>> GetAllAsync()
        {
            return await _context.DetallePedidos.ToListAsync();
        }
    }
}