using Domain.Entities;
namespace Domain.Interfaces;

public interface ICliente : IGenericRepository<Cliente>
{
        public Task<object> TotalPedidosPorCliente();
        public   Task<object> PedidoTardio();

         public   Task<object> GamaPorCliente ();



}