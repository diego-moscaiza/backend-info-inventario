// Models/MovInventario.cs
using System;
using System.Collections.Generic;

namespace WebApplication1.Models
{
    public class MovInventario
    {
        public string CodCia { get; set; } = string.Empty;
        public string CompaniaVenta3 { get; set; } = string.Empty;
        public string AlmacenVenta { get; set; } = string.Empty;
        public string TipoMovimiento { get; set; } = string.Empty;
        public string TipoDocumento { get; set; } = string.Empty;
        public string NroDocumento { get; set; } = string.Empty;
        public string CodItem2 { get; set; } = string.Empty;
        public string? Proveedor { get; set; }
        public string? AlmacenDestino { get; set; }
        public decimal? Cantidad { get; set; }
        public string? CompaniaDestino { get; set; }
        public decimal? CostoUnitario { get; set; }
        public string? DocRef1 { get; set; }
        public string? DocRef2 { get; set; }
        public DateTime? FechaTransaccion { get; set; }
        public string? Motivo { get; set; }
        public decimal? PrecioUnitario { get; set; }
        public string? TipoDocRef { get; set; }
        public string? UmItem3 { get; set; }
        public string? NroNota { get; set; }
        public string? Usuario { get; set; }
        public string? Moneda { get; set; }
        public decimal? CostoUnitarioMe { get; set; }
        public decimal? CosUnitEst { get; set; }
        public decimal? CosUnitMeEst { get; set; }
        public DateTime? HoraTransaccion { get; set; }
        public DateTime? FOrdencompra { get; set; }
        public int? CSecOc { get; set; }
        public decimal? CSecDetOc { get; set; }
        public string? TipoDocRef2 { get; set; }
        public DateTime? FechaValorizacion { get; set; }
        public string? UsuarioValoriza { get; set; }
        public decimal? Factor { get; set; }
        public decimal? CostosAdicionales { get; set; }
        public decimal? TCambioValoriza { get; set; }
        public string? PeriodoCerrado { get; set; }
        public string? PlanillaConsigna { get; set; }
        public string? DocRef3 { get; set; }
        public string? IngresoSalida { get; set; }
        public decimal? SaldoFinal { get; set; }
        public string? FlagUrgente { get; set; }
        public string? DocRef4 { get; set; }
        public string? DocRef5 { get; set; }
        public string? DocRef6 { get; set; }
        public string? DocRef7 { get; set; }
        public string? DocRef8 { get; set; }

        // Propiedad de navegación para la relación con MovInventarioUbicacion
        public virtual ICollection<MovInventarioUbicacion> Ubicaciones { get; set; } = new List<MovInventarioUbicacion>();
    }
}