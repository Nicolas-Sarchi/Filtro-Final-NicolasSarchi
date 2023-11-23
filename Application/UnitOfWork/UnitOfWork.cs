using Application.Repository;
using Domain.Entities;
using Domain.Interfaces;
using Persistence.Data;

namespace Application.UnitOfWork;
    public class UnitOfWork : IUnitOfWork

    {
        private readonly JardineriaContext context;
        private ClienteRepository _Clientes;
        private DetallePedidoRepository _DetallePedidos;
        private EmpleadoRepository _Empleados;
        private GamaProductoRepository _GamaProductos;
        private OficinaRepository _Oficinas;
        private PagoRepository _Pagos;
        private PedidoRepository _Pedidos;
        private ProductoRepository _Productos;


        public UnitOfWork(JardineriaContext _context)
        {
            context = _context;
        }

        public  ICliente Clientes
        {
            get
            {
                if (_Clientes == null)
                {
                    _Clientes = new ClienteRepository(context);
                }
                return _Clientes;
            }
        }

        public  IDetallePedido DetallePedidos
        {
            get
            {
                if (_DetallePedidos == null)
                {
                    _DetallePedidos = new DetallePedidoRepository(context);
                }
                return _DetallePedidos;
            }
        }


        public  IEmpleado Empleados
        {
            get
            {
                if (_Empleados == null)
                {
                    _Empleados = new EmpleadoRepository(context);
                }
                return _Empleados;
            }
        }

        public  IGamaProducto GamaProductos
        {
            get
            {
                if (_GamaProductos == null)
                {
                    _GamaProductos = new GamaProductoRepository(context);
                }
                return _GamaProductos;
            }
        }

        public  IOficina Oficinas
        {
            get
            {
                if (_Oficinas == null)
                {
                    _Oficinas = new OficinaRepository(context);
                }
                return _Oficinas;
            }
        }

        public  IPago Pagos
        {
            get
            {
                if (_Pagos == null)
                {
                    _Pagos = new PagoRepository(context);
                }
                return _Pagos;
            }
        }

        public  IPedido Pedidos
        {
            get
            {
                if (_Pedidos == null)
                {
                    _Pedidos = new PedidoRepository(context);
                }
                return _Pedidos;
            }
        }
        public  IProducto Productos
        {
            get
            {
                if (_Productos == null)
                {
                    _Productos = new ProductoRepository(context);
                }
                return _Productos;
            }
        }

        public int Save()
        {
            return context.SaveChanges();
        }
        public Task<int> SaveAsync()
        {
            return context.SaveChangesAsync();
        }

        public void Dispose()
        {
            context.Dispose();
        }
    }