using System.Linq.Expressions;
using Domain.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;

namespace Application.Repository
{
    public class ClienteRepository : GenericRepository<Cliente>, ICliente
    {
        private readonly JardineriaContext _context;
        public ClienteRepository(JardineriaContext context) : base(context)
        {
            _context = context;
        }

        public override async Task<IEnumerable<Cliente>> GetAllAsync()
        {
            return await _context.Clientes.ToListAsync();
        }

        public  async Task<object> TotalPedidosPorCliente()
        {
           return await _context.Clientes.
           Select(x => new {
            NombreCliente = x.NombreCliente,
            TotalPedidos = x.Pedidos.Count() 
           }).ToListAsync();
        }

        public  async Task<object> PedidoTardio()
        {
           return await _context.Clientes.
           Where(c => c.Pedidos.Any(p => p.FechaEntrega > p.FechaEsperada)).
           Select(x => new {
            NombreCliente = x.NombreCliente,

           }).ToListAsync();
        }

         public  async Task<object> GamaPorCliente ()
        {
           return await _context.Clientes.Where(e => e.Pedidos.Any()).
           Select(x => new {
            NombreCliente = x.NombreCliente,
            Gama = x.Pedidos
            .SelectMany(p => p.DetallePedidos
            .Select(d => d.CodigoProductoNavigation.Gama))
            .Distinct()
           }).ToListAsync();
        }


    }
}