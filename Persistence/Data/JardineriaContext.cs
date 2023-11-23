using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using System.Reflection;

namespace Persistence.Data;

public partial class JardineriaContext : DbContext
{
    public JardineriaContext()
    {
    }

    public JardineriaContext(DbContextOptions<JardineriaContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Cliente> Clientes { get; set; }

    public virtual DbSet<DetallePedido> DetallePedidos { get; set; }

    public virtual DbSet<Empleado> Empleados { get; set; }

    public virtual DbSet<GamaProducto> GamaProductos { get; set; }

    public virtual DbSet<Oficina> Oficinas { get; set; }

    public virtual DbSet<Pago> Pagos { get; set; }

    public virtual DbSet<Pedido> Pedidos { get; set; }

    public virtual DbSet<Producto> Productos { get; set; }

//     protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
// #warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
//         => optionsBuilder.UseMySql("server=localhost;user=root;password=123456;database=jardineria", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.34-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
       base.OnModelCreating(modelBuilder);

       modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

    }

    
}
