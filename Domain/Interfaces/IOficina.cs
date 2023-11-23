using Domain.Entities;
namespace Domain.Interfaces;

public interface IOficina : IGenericRepository<Oficina>
{
        public  Task<IEnumerable<Oficina>> GetSinEmpleadosVentasFrutales();

}