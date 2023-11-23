using Domain.Entities;
namespace Domain.Interfaces;

public interface IPedido : IGenericRepository<Pedido>
{
        public  Task<object> PedidosTardios();

}