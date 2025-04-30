// Models/MovInventarioUbicacion.cs
using System;

namespace WebApplication1.Models
{
    public class MovInventarioUbicacion
    {
        public string CodCia { get; set; } = string.Empty;
        public string CompaniaVenta3 { get; set; } = string.Empty;
        public string AlmacenVenta { get; set; } = string.Empty;
        public string TipoMovimiento { get; set; } = string.Empty;
        public string TipoDocumento { get; set; } = string.Empty;
        public string NroDocumento { get; set; } = string.Empty;
        public string CodItem2 { get; set; } = string.Empty;
        public int CodLote { get; set; }
        public string CodEstado { get; set; } = string.Empty;
        public string Zona { get; set; } = string.Empty;
        public string Rack { get; set; } = string.Empty;
        public string Nivel { get; set; } = string.Empty;
        public string Casillero { get; set; } = string.Empty;
        public string Pallet { get; set; } = string.Empty;
        public decimal? Cantidad { get; set; }
        public string UmMov { get; set; } = string.Empty;
        public long Rowversion { get; set; }
        public decimal? CantidadUmStock { get; set; }

        // Propiedad de navegación para la relación con MovInventario
        public virtual MovInventario Inventario { get; set; } = null!;
    }
}