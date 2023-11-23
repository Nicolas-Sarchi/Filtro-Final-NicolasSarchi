using System.Linq.Expressions;
using Domain.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;

namespace Application.Repository
{
    public class EmpleadoRepository : GenericRepository<Empleado>, IEmpleado
    {
        private readonly JardineriaContext _context;
        public EmpleadoRepository(JardineriaContext context) : base(context)
        {
            _context = context;
        }

        public override async Task<IEnumerable<Empleado>> GetAllAsync()
        {
            return await _context.Empleados.ToListAsync();
        }

        public  async Task<object> GetSinClientes()
        {
            return await _context.Empleados.Where(e => !e.Clientes.Any()).
            Select(e => new
            {
                e.Nombre,
                Apellidos = $"{e.Apellido1} {e.Apellido2}",
                Puesto = e.Puesto,
                TelefonoOficina = e.CodigoOficinaNavigation.Telefono
            })
            .ToListAsync();
        }
    }
}