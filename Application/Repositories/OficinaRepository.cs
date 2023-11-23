using System.Linq.Expressions;
using Domain.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;

namespace Application.Repository
{
    public class OficinaRepository : GenericRepository<Oficina>, IOficina
    {
        private readonly JardineriaContext _context;
        public OficinaRepository(JardineriaContext context) : base(context)
        {
            _context = context;
        }

        public override async Task<IEnumerable<Oficina>> GetAllAsync()
        {
            return await _context.Oficinas.ToListAsync();
        }


        public  async Task<IEnumerable<Oficina>> GetSinEmpleadosVentasFrutales()
        {
            return await _context.Oficinas
            .Where(o => !o.Empleados
            .Any(e => e.Clientes
            .Any(c => c.Pedidos
            .Any(p => p.DetallePedidos
            .Any(d => d.CodigoProductoNavigation.Gama == "Frutales")))))
            .ToListAsync();
        }
    }
}