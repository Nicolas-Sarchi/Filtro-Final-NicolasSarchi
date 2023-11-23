using Domain.Entities;
namespace Domain.Interfaces;

public interface IProducto : IGenericRepository<Producto>
{
        public   Task<IEnumerable<Producto>> GetSinPedidos();
        public   Task<object> GetMas3000();

         public   Task<object> GetMasVendido();

         public   Task<object> Get20MasVendidos();




}