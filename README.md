# Filtro-Final-NicolasSarchi

## Consultas

* . Devuelve el listado de clientes indicando el nombre del cliente y cuántos 
pedidos ha realizado. Tenga en cuenta que pueden existir clientes que no 
han realizado ningún pedido.

```
http://localhost:5148/api/cliente/Total-Pedidos-Por-Cliente
```

```c#
  public  async Task<object> TotalPedidosPorCliente()
          {
             return await _context.Clientes.
             Select(x => new {
              NombreCliente = x.NombreCliente,
              TotalPedidos = x.Pedidos.Count() 
             }).ToListAsync();
          }
```

En esta consulta se accede a los clientes guerdados, y se crea un objeto anónimo para cada uno de ellos, el cual contiene el nombre del cliente y su total de pedidos. El cálculo para hallar el total de productos por cliente se hace mediente `.Count()` en la lista de pedidos de cada cliente.

<br><br>

*  Devuelve un listado con el código de pedido, código de cliente, fecha 
esperada y fecha de entrega de los pedidos que no han sido entregados a 
tiempo.

```
http://localhost:5148/api/pedido/tardios
```

```c#
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
```

Para Resolver este método primero se aceede a los pedidos que cumplan con la condición de que su `fecha de entrega` sea mayor a su `fecha esperada` y luego se devuelven los datos requeridos mediante un objeto anónimo.
<br><br>

* . Devuelve un listado de los productos que nunca han aparecido en un 
pedido. El resultado debe mostrar el nombre, la descripción y la imagen del 
producto.

```
http://localhost:5148/api/producto/sin-pedidos
```

```c#
 public  async Task<IEnumerable<Producto>> GetSinPedidos()
        {
            return await _context.Productos
            .Where(p => !p.DetallePedidos.Any())
            .ToListAsync();
        }
```

Para esta consulta, se accede a los productos que cumplan la condición de tener la lista de detalles producto vacía, posteriormente, para mostrar los datos especificados se crea un dto [ProductosSinPedidoDto](API\Dtos\ProductoSinPedidosDto.cs) y se mapea usando automapper
<br><br>

* Devuelve las oficinas donde no trabajan ninguno de los empleados que 
hayan sido los representantes de ventas de algún cliente que haya realizado 
la compra de algún producto de la gama Frutales

```
http://localhost:5148/api/oficina/sin-Empleados-Ventas-Frutales
```
```c#
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
```
Para resolver esta consulta, se acceden a las oficinas, luego se verifica que esta oficina NO tenga empleados que dentro de su lista de Clientes no tengan Pedidos que a su vez no tengan un detalle producto con algún producto de la gama `frutales`, para esto se usa el operador `!` .

<br><br>

*  Lista las ventas totales de los productos que hayan facturado más de 3000 
euros. Se mostrará el nombre, unidades vendidas, total facturado y total 
facturado con impuestos (21% IVA).

```
http://localhost:5148/api/producto/Ventas-Mas-3000
```
```c#
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
```

En esta consulta primero se accede a los detalles de producto que cumplan con la condición de que el producto resultado de multiplicar el precio unitario por la cantidad, sea mayor a 3000, y que el pedido no haya sido rechazado. Postreriormente se agrupa el resultado por Codigo de producto y se muestran los datos solicitados.

<br><br>


* Devuelve el nombre, apellidos, puesto y teléfono de la oficina de aquellos 
empleados que no sean representante de ventas de ningún cliente.

```
http://localhost:5148/api/empleado/Sin-Clientes
```

```c#
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
```

Para esta consulta, se accede a los empleados que cumplan con la condición de no tener ningún cliente en su lista de clientes, posteriormente se seleccionan los datos requeridos y se devuelven en un objeto anónimo


<br><br>

*  Devuelve el nombre del producto del que se han vendido más unidades. 
(Tenga en cuenta que tendrá que calcular cuál es el número total de 
unidades que se han vendido de cada producto a partir de los datos de la 
tabla detalle_pedido)

```
http://localhost:5148/api/producto/MasVendido
```

```c#
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
```

En esta condulta se accede a la tabla detalle pedido, luego se agrupa por producto y se ordena por la cantidad vendida, posteriormente se devuelen los datos solicitados en un objeto anónimo

<br><br>

*  Devuelve un listado de los 20 productos más vendidos y el número total de 
unidades que se han vendido de cada uno. El listado deberá estar ordenado 
por el número total de unidades vendidas.


```
http://localhost:5148/api/producto/20MasVendidos
```

```c#
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
```


En esta consulta se accede a la tabla detalle pedido, luego se agrupa por producto y se ordena por la cantidad vendida, posteriormente se cre un objeto anónimo con los datos solicitados, y nuevamente se ordena el resultado por las unidades vendidas

<br><br>

```
 http://localhost:5148/api/cliente/Pedidos-Tardios
```

```c#
public  async Task<object> PedidoTardio()
        {
           return await _context.Clientes.
           Where(c => c.Pedidos.Any(p => p.FechaEntrega > p.FechaEsperada)).
           Select(x => new {
            NombreCliente = x.NombreCliente,

           }).ToListAsync();
        }
```

Para resolver esta consulta, se accede a los clientes, luego a sus pedidos y se verifica si hay alguno en el que su fecha de entrega sea mayor a la fecha esperada, luego se devuelven los datos en un objeto.


<br><br>

* . Devuelve un listado de las diferentes gamas de producto que ha comprado 
cada cliente

```
http://localhost:5148/api/cliente/gamasCliente
```

```c#
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
```

Para solucionar esta consulta, se accede a los clientes que tengan al menos un pedido, luego, mediante un objeto anónimo, se seleccionan los datos. Para mostrar las gamas que ha comprado cada cliente, se accede a la lista de pedidos del cliente y se seleccionan los detalles de pedido, posteriormente, de los de detalles de pedido, se seleccionan las gamas de los productos asociados, luego se eliminan los duplicados con el método `Distinct()`



