using System.Linq.Expressions;
using Domain.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;

namespace Application.Repository
{
    public class PedidoRepository : GenericRepository<Pedido>, IPedido
    {
        private readonly JardineriaContext _context;
        public PedidoRepository(JardineriaContext context) : base(context)
        {
            _context = context;
        }

        public override async Task<IEnumerable<Pedido>> GetAllAsync()
        {
            return await _context.Pedidos.ToListAsync();
        }

        public  async Task<object> PedidosTardios()
        {
            return await _context.Pedidos.Where(p => p.FechaEntrega > p.FechaEsperada)
            .Select(p => new
            {
                p.CodigoPedido,
                p.CodigoCliente,
                p.FechaEsperada,
                p.FechaEntrega

            }).ToListAsync();
        }
    }
}