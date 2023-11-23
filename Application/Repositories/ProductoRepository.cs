using System.Linq.Expressions;
using Domain.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;

namespace Application.Repository
{
    public class ProductoRepository : GenericRepository<Producto>, IProducto
    {
        private readonly JardineriaContext _context;
        public ProductoRepository(JardineriaContext context) : base(context)
        {
            _context = context;
        }

        public override async Task<IEnumerable<Producto>> GetAllAsync()
        {
            return await _context.Productos.ToListAsync();
        }

        public  async Task<IEnumerable<Producto>> GetSinPedidos()
        {
            return await _context.Productos
            .Where(p => !p.DetallePedidos.Any())
            .ToListAsync();
        }

        public  async Task<object> GetMas3000()
        {
            
            return await _context.DetallePedidos.Where(dp => dp.PrecioUnidad * dp.Cantidad > 3000 && dp.CodigoPedidoNavigation.Estado.ToLower() != "rechazado").
            GroupBy(p => p.CodigoProductoNavigation).
            Select(p => new
            {
                NombreProducto = p.Key.Nombre,
                UnidadesVendidas= p.Sum(p => p.Cantidad),
                totalFacturado = p.Sum(p => p.PrecioUnidad) * p.Sum(p => p.Cantidad),
                totalFacturadoIva = (p.Sum(p => p.PrecioUnidad) * p.Sum(p => p.Cantidad)) * (decimal)1.21,

            })
            .ToListAsync();
        }

         public  async Task<object> GetMasVendido()
        {
             return await _context.DetallePedidos.
            GroupBy(p => p.CodigoProductoNavigation)
            .OrderByDescending(p => p.Sum(p => p.Cantidad))
            .Select(p => new
            {
                NombreProducto = p.Key.Nombre
            }).FirstOrDefaultAsync();
            ;
           
        }

         public  async Task<object> Get20MasVendidos()
        {
             return await _context.DetallePedidos.
            GroupBy(p => p.CodigoProductoNavigation)
            .OrderByDescending(p => p.Sum(p => p.Cantidad))
            .Select(p => new
            {
                NombreProducto = p.Key.Nombre,
                CantidadVendida = p.Sum(p => p.Cantidad)
            })
            .OrderByDescending(p => p.CantidadVendida).Take(20).ToListAsync();
            ;
           
        }

        
    }
}