using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<MovInventario> MovInventarios { get; set; } = null!;
        public DbSet<MovInventarioUbicacion> MovInventariosUbicacion { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuración para MovInventario
            modelBuilder.Entity<MovInventario>(entity =>
            {
                entity.ToTable("MOV_INVENTARIOS");

                // Clave primaria compuesta
                entity.HasKey(e => new
                {
                    e.CodCia,
                    e.CompaniaVenta3,
                    e.AlmacenVenta,
                    e.TipoMovimiento,
                    e.TipoDocumento,
                    e.NroDocumento,
                    e.CodItem2
                });

                // Mapeo de propiedades con sus nombres de columna en SQL Server
                entity.Property(e => e.CodCia).HasColumnName("COD_CIA").HasMaxLength(2).IsRequired();
                entity.Property(e => e.CompaniaVenta3).HasColumnName("COMPANIA_VENTA_3").HasMaxLength(2).IsRequired();
                entity.Property(e => e.AlmacenVenta).HasColumnName("ALMACEN_VENTA").HasMaxLength(2).IsRequired();
                entity.Property(e => e.TipoMovimiento).HasColumnName("TIPO_MOVIMIENTO").HasMaxLength(2).IsRequired();
                entity.Property(e => e.TipoDocumento).HasColumnName("TIPO_DOCUMENTO").HasMaxLength(2).IsRequired();
                entity.Property(e => e.NroDocumento).HasColumnName("NRO_DOCUMENTO").HasMaxLength(8).IsRequired();
                entity.Property(e => e.CodItem2).HasColumnName("COD_ITEM_2").HasMaxLength(20).IsRequired();
                entity.Property(e => e.Proveedor).HasColumnName("PROVEEDOR").HasMaxLength(15);
                entity.Property(e => e.AlmacenDestino).HasColumnName("ALMACEN_DESTINO").HasMaxLength(2);
                entity.Property(e => e.Cantidad).HasColumnName("CANTIDAD").HasColumnType("decimal(13, 2)");
                entity.Property(e => e.CompaniaDestino).HasColumnName("COMPANIA_DESTINO").HasMaxLength(2);
                entity.Property(e => e.CostoUnitario).HasColumnName("COSTO_UNITARIO").HasColumnType("decimal(15, 5)");
                entity.Property(e => e.DocRef1).HasColumnName("DOC_REF_1").HasMaxLength(25);
                entity.Property(e => e.DocRef2).HasColumnName("DOC_REF_2").HasMaxLength(10);
                entity.Property(e => e.FechaTransaccion).HasColumnName("FECHA_TRANSACCION");
                entity.Property(e => e.Motivo).HasColumnName("MOTIVO").HasMaxLength(2);
                entity.Property(e => e.PrecioUnitario).HasColumnName("PRECIO_UNITARIO").HasColumnType("decimal(15, 6)");
                entity.Property(e => e.TipoDocRef).HasColumnName("TIPO_DOC_REF").HasMaxLength(2);
                entity.Property(e => e.UmItem3).HasColumnName("UM_ITEM_3").HasMaxLength(6);
                entity.Property(e => e.NroNota).HasColumnName("NRO_NOTA").HasMaxLength(6);
                entity.Property(e => e.Usuario).HasColumnName("USUARIO").HasMaxLength(10);
                entity.Property(e => e.Moneda).HasColumnName("MONEDA").HasMaxLength(3);
                entity.Property(e => e.CostoUnitarioMe).HasColumnName("COSTO_UNITARIO_ME").HasColumnType("decimal(15, 5)");
                entity.Property(e => e.CosUnitEst).HasColumnName("COS_UNIT_EST").HasColumnType("decimal(15, 5)");
                entity.Property(e => e.CosUnitMeEst).HasColumnName("COS_UNIT_ME_EST").HasColumnType("decimal(15, 5)");
                entity.Property(e => e.HoraTransaccion).HasColumnName("HORA_TRANSACCION");
                entity.Property(e => e.FOrdencompra).HasColumnName("F_ORDENCOMPRA");
                entity.Property(e => e.CSecOc).HasColumnName("C_SEC_OC");
                entity.Property(e => e.CSecDetOc).HasColumnName("C_SEC_DET_OC").HasColumnType("decimal(15, 5)");
                entity.Property(e => e.TipoDocRef2).HasColumnName("TIPO_DOC_REF_2").HasMaxLength(2);
                entity.Property(e => e.FechaValorizacion).HasColumnName("FECHA_VALORIZACION");
                entity.Property(e => e.UsuarioValoriza).HasColumnName("USUARIO_VALORIZA").HasMaxLength(10);
                entity.Property(e => e.Factor).HasColumnName("FACTOR").HasColumnType("decimal(6, 3)");
                entity.Property(e => e.CostosAdicionales).HasColumnName("COSTOS_ADICIONALES").HasColumnType("decimal(15, 5)");
                entity.Property(e => e.TCambioValoriza).HasColumnName("T_CAMBIO_VALORIZA").HasColumnType("decimal(13, 5)");
                entity.Property(e => e.PeriodoCerrado).HasColumnName("PERIODO_CERRADO").HasMaxLength(1);
                entity.Property(e => e.PlanillaConsigna).HasColumnName("PLANILLA_CONSIGNA").HasMaxLength(16);
                entity.Property(e => e.DocRef3).HasColumnName("DOC_REF_3").HasMaxLength(10);
                entity.Property(e => e.IngresoSalida).HasColumnName("INGRESO_SALIDA").HasMaxLength(2);
                entity.Property(e => e.SaldoFinal).HasColumnName("SALDO_FINAL").HasColumnType("decimal(13, 2)");
                entity.Property(e => e.FlagUrgente).HasColumnName("FLAG_URGENTE").HasMaxLength(1);
                entity.Property(e => e.DocRef4).HasColumnName("DOC_REF_4").HasMaxLength(35);
                entity.Property(e => e.DocRef5).HasColumnName("DOC_REF_5").HasMaxLength(30);
                entity.Property(e => e.DocRef6).HasColumnName("DOC_REF_6").HasMaxLength(30);
                entity.Property(e => e.DocRef7).HasColumnName("DOC_REF_7").HasMaxLength(30);
                entity.Property(e => e.DocRef8).HasColumnName("DOC_REF_8").HasMaxLength(30);
            });

            // Configuración para MovInventarioUbicacion
            modelBuilder.Entity<MovInventarioUbicacion>(entity =>
            {
                entity.ToTable("MOV_INVENTARIOS_UBICACION");

                // Clave primaria compuesta
                entity.HasKey(e => new
                {
                    e.CodCia,
                    e.CompaniaVenta3,
                    e.AlmacenVenta,
                    e.TipoMovimiento,
                    e.TipoDocumento,
                    e.NroDocumento,
                    e.CodItem2,
                    e.CodLote,
                    e.CodEstado,
                    e.Zona,
                    e.Rack,
                    e.Nivel,
                    e.Casillero,
                    e.Pallet,
                    e.UmMov
                });

                // Mapeo de propiedades
                entity.Property(e => e.CodCia).HasColumnName("COD_CIA").HasMaxLength(2).IsRequired();
                entity.Property(e => e.CompaniaVenta3).HasColumnName("COMPANIA_VENTA_3").HasMaxLength(2).IsRequired();
                entity.Property(e => e.AlmacenVenta).HasColumnName("ALMACEN_VENTA").HasMaxLength(2).IsRequired();
                entity.Property(e => e.TipoMovimiento).HasColumnName("TIPO_MOVIMIENTO").HasMaxLength(2).IsRequired();
                entity.Property(e => e.TipoDocumento).HasColumnName("TIPO_DOCUMENTO").HasMaxLength(2).IsRequired();
                entity.Property(e => e.NroDocumento).HasColumnName("NRO_DOCUMENTO").HasMaxLength(8).IsRequired();
                entity.Property(e => e.CodItem2).HasColumnName("COD_ITEM_2").HasMaxLength(20).IsRequired();
                entity.Property(e => e.CodLote).HasColumnName("COD_LOTE").IsRequired();
                entity.Property(e => e.CodEstado).HasColumnName("COD_ESTADO").HasMaxLength(2).IsRequired();
                entity.Property(e => e.Zona).HasColumnName("ZONA").HasMaxLength(5).IsRequired();
                entity.Property(e => e.Rack).HasColumnName("RACK").HasMaxLength(5).IsRequired();
                entity.Property(e => e.Nivel).HasColumnName("NIVEL").HasMaxLength(5).IsRequired();
                entity.Property(e => e.Casillero).HasColumnName("CASILLERO").HasMaxLength(5).IsRequired();
                entity.Property(e => e.Pallet).HasColumnName("PALLET").HasMaxLength(5).IsRequired();
                entity.Property(e => e.Cantidad).HasColumnName("CANTIDAD").HasColumnType("decimal(13, 2)");
                entity.Property(e => e.UmMov).HasColumnName("UM_MOV").HasMaxLength(6).IsRequired();
                entity.Property(e => e.Rowversion).HasColumnName("ROWVERSION").HasDefaultValue(0);
                entity.Property(e => e.CantidadUmStock).HasColumnName("CANTIDAD_UM_STOCK").HasColumnType("decimal(13, 2)");

                // Configurar la relación con MovInventario (la tabla principal)
                entity.HasOne(d => d.Inventario)
                    .WithMany(p => p.Ubicaciones)
                    .HasForeignKey(d => new
                    {
                        d.CodCia,
                        d.CompaniaVenta3,
                        d.AlmacenVenta,
                        d.TipoMovimiento,
                        d.TipoDocumento,
                        d.NroDocumento,
                        d.CodItem2
                    })
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });
        }
    }
}