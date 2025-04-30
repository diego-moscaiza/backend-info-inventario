using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using System.Data;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    // Clases DTO para la solicitud y respuesta
    public class MovimientoFiltroRequest
    {
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public string? TipoMovimiento { get; set; }
        public string? NroDocumento { get; set; }
    }

    // Definición de la clase FiltroFechasRequest que faltaba
    public class FiltroFechasRequest
    {
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public string? TipoMovimiento { get; set; }
        public string? NroDocumento { get; set; }
    }

    public class MovimientoDTO
    {
        public string CodCia { get; set; } = string.Empty;
        public string CompaniaVenta3 { get; set; } = string.Empty;
        public string AlmacenVenta { get; set; } = string.Empty;
        public string TipoMovimiento { get; set; } = string.Empty;
        public string TipoDocumento { get; set; } = string.Empty;
        public string NroDocumento { get; set; } = string.Empty;
        public string CodItem2 { get; set; } = string.Empty;
        public decimal? Cantidad { get; set; }
        public DateTime? FechaTransaccion { get; set; }
        public string? Descripcion { get; set; }  // Campo adicional que podría venir del SP
        public decimal? CostoUnitario { get; set; }
        public decimal? PrecioUnitario { get; set; }
        public List<MovimientoUbicacionDTO> Ubicaciones { get; set; } = new List<MovimientoUbicacionDTO>();
    }

    public class MovimientoUbicacionDTO
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
    }

    public class MovimientoResultadoDTO
    {
        public List<MovimientoDTO> Movimientos { get; set; } = new List<MovimientoDTO>();
    }

    [ApiController]
    [Route("api/[controller]")]
    public class MovInventariosController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<MovInventariosController> _logger;

        public MovInventariosController(AppDbContext context, ILogger<MovInventariosController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // Método existente: GET: api/MovInventarios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MovInventario>>> GetMovInventarios()
        {
            _logger.LogInformation("Obteniendo todos los movimientos de inventario");
            return await _context.MovInventarios.ToListAsync();
        }

        // Método existente: GET: api/MovInventarios/5
        [HttpGet("{codCia}/{companiaVenta3}/{almacenVenta}/{tipoMovimiento}/{tipoDocumento}/{nroDocumento}/{codItem2}")]
        public async Task<ActionResult<MovInventario>> GetMovInventario(string codCia, string companiaVenta3, string almacenVenta,
            string tipoMovimiento, string tipoDocumento, string nroDocumento, string codItem2)
        {
            _logger.LogInformation("Obteniendo movimiento de inventario");

            var movInventario = await _context.MovInventarios
                .Include(m => m.Ubicaciones)
                .FirstOrDefaultAsync(m =>
                    m.CodCia == codCia &&
                    m.CompaniaVenta3 == companiaVenta3 &&
                    m.AlmacenVenta == almacenVenta &&
                    m.TipoMovimiento == tipoMovimiento &&
                    m.TipoDocumento == tipoDocumento &&
                    m.NroDocumento == nroDocumento &&
                    m.CodItem2 == codItem2);

            if (movInventario == null)
            {
                _logger.LogWarning("Movimiento de inventario no encontrado");
                return NotFound();
            }

            return movInventario;
        }

        // NUEVO MÉTODO: POST: api/MovInventarios/FiltrarMovimientos
        [HttpPost("FiltrarMovimientos")]
        public async Task<ActionResult<MovimientoResultadoDTO>> FiltrarMovimientos(MovimientoFiltroRequest filtro)
        {
            try
            {
                _logger.LogInformation("Filtrando movimientos de inventario con fechas: {FechaInicio} - {FechaFin}",
                    filtro.FechaInicio, filtro.FechaFin);

                if (!filtro.FechaInicio.HasValue || !filtro.FechaFin.HasValue)
                {
                    return BadRequest("Se requieren fechas de inicio y fin");
                }

                var resultado = new MovimientoResultadoDTO();
                var movimientosDict = new Dictionary<string, MovimientoDTO>();
                var movInventarios = new List<MovInventario>();

                // Usar SqlConnection directamente para ejecutar el stored procedure
                using (var connection = new SqlConnection(_context.Database.GetConnectionString()))
                {
                    await connection.OpenAsync();

                    using (var command = new SqlCommand("sp_informacion_movimientos_almacen", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Agregar parámetros
                        command.Parameters.Add(new SqlParameter("@fecha_inicio", SqlDbType.DateTime) { Value = filtro.FechaInicio.Value });
                        command.Parameters.Add(new SqlParameter("@fecha_fin", SqlDbType.DateTime) { Value = filtro.FechaFin.Value });

                        // Agregar parámetros opcionales
                        if (!string.IsNullOrEmpty(filtro.TipoMovimiento))
                            command.Parameters.Add(new SqlParameter("@tipo_movimiento", SqlDbType.VarChar, 2) { Value = filtro.TipoMovimiento });
                        else
                            command.Parameters.Add(new SqlParameter("@tipo_movimiento", SqlDbType.VarChar, 2) { Value = DBNull.Value });

                        if (!string.IsNullOrEmpty(filtro.NroDocumento))
                            command.Parameters.Add(new SqlParameter("@nro_documento", SqlDbType.VarChar, 8) { Value = filtro.NroDocumento });
                        else
                            command.Parameters.Add(new SqlParameter("@nro_documento", SqlDbType.VarChar, 8) { Value = DBNull.Value });

                        // Ejecutar y procesar resultados
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                var dto = new MovimientoDTO
                                {
                                    CodCia = reader["COD_CIA"].ToString() ?? string.Empty,
                                    CompaniaVenta3 = reader["COMPANIA_VENTA_3"].ToString() ?? string.Empty,
                                    AlmacenVenta = reader["ALMACEN_VENTA"].ToString() ?? string.Empty,
                                    TipoMovimiento = reader["TIPO_MOVIMIENTO"].ToString() ?? string.Empty,
                                    TipoDocumento = reader["TIPO_DOCUMENTO"].ToString() ?? string.Empty,
                                    NroDocumento = reader["NRO_DOCUMENTO"].ToString() ?? string.Empty,
                                    CodItem2 = reader["COD_ITEM_2"].ToString() ?? string.Empty
                                };

                                if (reader["CANTIDAD"] != DBNull.Value)
                                    dto.Cantidad = Convert.ToDecimal(reader["CANTIDAD"]);

                                if (reader["FECHA_TRANSACCION"] != DBNull.Value)
                                    dto.FechaTransaccion = Convert.ToDateTime(reader["FECHA_TRANSACCION"]);

                                if (reader["COSTO_UNITARIO"] != DBNull.Value)
                                    dto.CostoUnitario = Convert.ToDecimal(reader["COSTO_UNITARIO"]);

                                if (reader["PRECIO_UNITARIO"] != DBNull.Value)
                                    dto.PrecioUnitario = Convert.ToDecimal(reader["PRECIO_UNITARIO"]);

                                var key = $"{dto.CodCia}|{dto.CompaniaVenta3}|{dto.AlmacenVenta}|{dto.TipoMovimiento}|{dto.TipoDocumento}|{dto.NroDocumento}|{dto.CodItem2}";
                                movimientosDict[key] = dto;

                                // Guardar información para consultar ubicaciones
                                var movInventario = new MovInventario
                                {
                                    CodCia = dto.CodCia,
                                    CompaniaVenta3 = dto.CompaniaVenta3,
                                    AlmacenVenta = dto.AlmacenVenta,
                                    TipoMovimiento = dto.TipoMovimiento,
                                    TipoDocumento = dto.TipoDocumento,
                                    NroDocumento = dto.NroDocumento,
                                    CodItem2 = dto.CodItem2
                                };
                                movInventarios.Add(movInventario);
                            }
                        }
                    }
                }

                // Consultar ubicaciones
                if (movimientosDict.Count > 0)
                {
                    foreach (var movimiento in movInventarios)
                    {
                        var ubicaciones = await _context.MovInventariosUbicacion
                            .Where(u =>
                                u.CodCia == movimiento.CodCia &&
                                u.CompaniaVenta3 == movimiento.CompaniaVenta3 &&
                                u.AlmacenVenta == movimiento.AlmacenVenta &&
                                u.TipoMovimiento == movimiento.TipoMovimiento &&
                                u.TipoDocumento == movimiento.TipoDocumento &&
                                u.NroDocumento == movimiento.NroDocumento &&
                                u.CodItem2 == movimiento.CodItem2)
                            .ToListAsync();

                        var key = $"{movimiento.CodCia}|{movimiento.CompaniaVenta3}|{movimiento.AlmacenVenta}|{movimiento.TipoMovimiento}|{movimiento.TipoDocumento}|{movimiento.NroDocumento}|{movimiento.CodItem2}";

                        if (movimientosDict.TryGetValue(key, out var dto))
                        {
                            foreach (var ubicacion in ubicaciones)
                            {
                                dto.Ubicaciones.Add(new MovimientoUbicacionDTO
                                {
                                    CodCia = ubicacion.CodCia,
                                    CompaniaVenta3 = ubicacion.CompaniaVenta3,
                                    AlmacenVenta = ubicacion.AlmacenVenta,
                                    TipoMovimiento = ubicacion.TipoMovimiento,
                                    TipoDocumento = ubicacion.TipoDocumento,
                                    NroDocumento = ubicacion.NroDocumento,
                                    CodItem2 = ubicacion.CodItem2,
                                    CodLote = ubicacion.CodLote,
                                    CodEstado = ubicacion.CodEstado,
                                    Zona = ubicacion.Zona,
                                    Rack = ubicacion.Rack,
                                    Nivel = ubicacion.Nivel,
                                    Casillero = ubicacion.Casillero,
                                    Pallet = ubicacion.Pallet,
                                    Cantidad = ubicacion.Cantidad,
                                    UmMov = ubicacion.UmMov
                                });
                            }
                        }
                    }
                }

                resultado.Movimientos = movimientosDict.Values.ToList();

                if (!resultado.Movimientos.Any())
                {
                    return NotFound("No se encontraron movimientos para los criterios especificados");
                }

                return resultado;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al filtrar movimientos de inventario: {ErrorMessage}", ex.Message);
                return StatusCode(500, "Error interno al procesar la solicitud: " + ex.Message);
            }
        }

        // POST: api/MovInventarios
        [HttpPost]
        public async Task<ActionResult<MovInventario>> CreateMovInventario(MovInventario movInventario)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.MovInventarios.Add(movInventario);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Error al crear movimiento de inventario");

                if (MovInventarioExists(movInventario.CodCia, movInventario.CompaniaVenta3, movInventario.AlmacenVenta,
                    movInventario.TipoMovimiento, movInventario.TipoDocumento, movInventario.NroDocumento, movInventario.CodItem2))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            _logger.LogInformation("Movimiento de inventario creado exitosamente");

            return CreatedAtAction(nameof(GetMovInventario), new
            {
                codCia = movInventario.CodCia,
                companiaVenta3 = movInventario.CompaniaVenta3,
                almacenVenta = movInventario.AlmacenVenta,
                tipoMovimiento = movInventario.TipoMovimiento,
                tipoDocumento = movInventario.TipoDocumento,
                nroDocumento = movInventario.NroDocumento,
                codItem2 = movInventario.CodItem2
            }, movInventario);
        }

        // PUT: api/MovInventarios/5
        [HttpPut("{codCia}/{companiaVenta3}/{almacenVenta}/{tipoMovimiento}/{tipoDocumento}/{nroDocumento}/{codItem2}")]
        public async Task<IActionResult> UpdateMovInventario(string codCia, string companiaVenta3, string almacenVenta,
            string tipoMovimiento, string tipoDocumento, string nroDocumento, string codItem2, MovInventario movInventario)
        {
            if (codCia != movInventario.CodCia ||
                companiaVenta3 != movInventario.CompaniaVenta3 ||
                almacenVenta != movInventario.AlmacenVenta ||
                tipoMovimiento != movInventario.TipoMovimiento ||
                tipoDocumento != movInventario.TipoDocumento ||
                nroDocumento != movInventario.NroDocumento ||
                codItem2 != movInventario.CodItem2)
            {
                return BadRequest();
            }

            _context.Entry(movInventario).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                _logger.LogInformation("Movimiento de inventario actualizado exitosamente");
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, "Error de concurrencia al actualizar movimiento de inventario");

                if (!MovInventarioExists(codCia, companiaVenta3, almacenVenta, tipoMovimiento, tipoDocumento, nroDocumento, codItem2))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/MovInventarios/5
        [HttpDelete("{codCia}/{companiaVenta3}/{almacenVenta}/{tipoMovimiento}/{tipoDocumento}/{nroDocumento}/{codItem2}")]
        public async Task<IActionResult> DeleteMovInventario(string codCia, string companiaVenta3, string almacenVenta,
            string tipoMovimiento, string tipoDocumento, string nroDocumento, string codItem2)
        {
            var movInventario = await _context.MovInventarios
                .Include(m => m.Ubicaciones)
                .FirstOrDefaultAsync(m =>
                    m.CodCia == codCia &&
                    m.CompaniaVenta3 == companiaVenta3 &&
                    m.AlmacenVenta == almacenVenta &&
                    m.TipoMovimiento == tipoMovimiento &&
                    m.TipoDocumento == tipoDocumento &&
                    m.NroDocumento == nroDocumento &&
                    m.CodItem2 == codItem2);

            if (movInventario == null)
            {
                _logger.LogWarning("Intento de eliminar movimiento de inventario inexistente");
                return NotFound();
            }

            _context.MovInventarios.Remove(movInventario);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Movimiento de inventario eliminado exitosamente");
            return NoContent();
        }

        // GET: api/MovInventarios/ByAlmacen/{almacenVenta}
        [HttpGet("ByAlmacen/{almacenVenta}")]
        public async Task<ActionResult<IEnumerable<MovInventario>>> GetMovInventariosByAlmacen(string almacenVenta)
        {
            return await _context.MovInventarios
                .Where(m => m.AlmacenVenta == almacenVenta)
                .ToListAsync();
        }

        // GET: api/MovInventarios/ByItem/{codItem2}
        [HttpGet("ByItem/{codItem2}")]
        public async Task<ActionResult<IEnumerable<MovInventario>>> GetMovInventariosByItem(string codItem2)
        {
            return await _context.MovInventarios
                .Where(m => m.CodItem2 == codItem2)
                .ToListAsync();
        }

        // POST: api/MovInventarios/FiltrarPorFechas
        [HttpPost("FiltrarPorFechas")]
        public async Task<ActionResult<IEnumerable<MovInventario>>> FiltrarPorFechas(FiltroFechasRequest filtro)
        {
            try
            {
                _logger.LogInformation("Filtrando movimientos por rango de fechas: {FechaInicio} a {FechaFin}",
                    filtro.FechaInicio, filtro.FechaFin);

                // Convertir fechas al formato aceptado por SQL Server
                var fechaInicio = filtro.FechaInicio.ToString("yyyy-MM-dd");
                var fechaFin = filtro.FechaFin.ToString("yyyy-MM-dd");

                var result = new List<MovInventario>();

                // Usar SqlConnection directamente para ejecutar el stored procedure
                using (var connection = new SqlConnection(_context.Database.GetConnectionString()))
                {
                    await connection.OpenAsync();

                    using (var command = new SqlCommand("sp_informacion_movimientos_almacen", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Agregar parámetros
                        command.Parameters.Add(new SqlParameter("@fecha_inicio", SqlDbType.DateTime) { Value = filtro.FechaInicio });
                        command.Parameters.Add(new SqlParameter("@fecha_fin", SqlDbType.DateTime) { Value = filtro.FechaFin });

                        // Agregar parámetros opcionales
                        if (!string.IsNullOrEmpty(filtro.TipoMovimiento))
                            command.Parameters.Add(new SqlParameter("@tipo_movimiento", SqlDbType.VarChar, 2) { Value = filtro.TipoMovimiento });
                        else
                            command.Parameters.Add(new SqlParameter("@tipo_movimiento", SqlDbType.VarChar, 2) { Value = DBNull.Value });

                        if (!string.IsNullOrEmpty(filtro.NroDocumento))
                            command.Parameters.Add(new SqlParameter("@nro_documento", SqlDbType.VarChar, 8) { Value = filtro.NroDocumento });
                        else
                            command.Parameters.Add(new SqlParameter("@nro_documento", SqlDbType.VarChar, 8) { Value = DBNull.Value });

                        // Ejecutar y procesar resultados
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                var movInventario = new MovInventario
                                {
                                    CodCia = reader["COD_CIA"].ToString() ?? string.Empty,
                                    CompaniaVenta3 = reader["COMPANIA_VENTA_3"].ToString() ?? string.Empty,
                                    AlmacenVenta = reader["ALMACEN_VENTA"].ToString() ?? string.Empty,
                                    TipoMovimiento = reader["TIPO_MOVIMIENTO"].ToString() ?? string.Empty,
                                    TipoDocumento = reader["TIPO_DOCUMENTO"].ToString() ?? string.Empty,
                                    NroDocumento = reader["NRO_DOCUMENTO"].ToString() ?? string.Empty,
                                    CodItem2 = reader["COD_ITEM_2"].ToString() ?? string.Empty
                                };

                                // Procesar propiedades que pueden ser nulas
                                if (reader["PROVEEDOR"] != DBNull.Value)
                                    movInventario.Proveedor = reader["PROVEEDOR"].ToString();

                                if (reader["ALMACEN_DESTINO"] != DBNull.Value)
                                    movInventario.AlmacenDestino = reader["ALMACEN_DESTINO"].ToString();

                                if (reader["CANTIDAD"] != DBNull.Value)
                                    movInventario.Cantidad = Convert.ToDecimal(reader["CANTIDAD"]);

                                if (reader["COMPANIA_DESTINO"] != DBNull.Value)
                                    movInventario.CompaniaDestino = reader["COMPANIA_DESTINO"].ToString();

                                if (reader["COSTO_UNITARIO"] != DBNull.Value)
                                    movInventario.CostoUnitario = Convert.ToDecimal(reader["COSTO_UNITARIO"]);

                                if (reader["FECHA_TRANSACCION"] != DBNull.Value)
                                    movInventario.FechaTransaccion = Convert.ToDateTime(reader["FECHA_TRANSACCION"]);

                                if (reader["PRECIO_UNITARIO"] != DBNull.Value)
                                    movInventario.PrecioUnitario = Convert.ToDecimal(reader["PRECIO_UNITARIO"]);

                                // Agregar el movimiento a la lista de resultados
                                result.Add(movInventario);
                            }
                        }
                    }

                    // Si se obtuvieron resultados, cargar las ubicaciones relacionadas
                    if (result.Any())
                    {
                        // Crear condiciones para consultar las ubicaciones
                        var claves = result.Select(m => new
                        {
                            m.CodCia,
                            m.CompaniaVenta3,
                            m.AlmacenVenta,
                            m.TipoMovimiento,
                            m.TipoDocumento,
                            m.NroDocumento,
                            m.CodItem2
                        }).ToList();

                        foreach (var movimiento in result)
                        {
                            // Cargar las ubicaciones para cada movimiento
                            movimiento.Ubicaciones = await _context.MovInventariosUbicacion
                                .Where(u =>
                                    u.CodCia == movimiento.CodCia &&
                                    u.CompaniaVenta3 == movimiento.CompaniaVenta3 &&
                                    u.AlmacenVenta == movimiento.AlmacenVenta &&
                                    u.TipoMovimiento == movimiento.TipoMovimiento &&
                                    u.TipoDocumento == movimiento.TipoDocumento &&
                                    u.NroDocumento == movimiento.NroDocumento &&
                                    u.CodItem2 == movimiento.CodItem2)
                                .ToListAsync();
                        }
                    }
                }

                if (!result.Any())
                {
                    _logger.LogWarning("No se encontraron movimientos para el rango de fechas especificado");
                    return NotFound("No se encontraron movimientos para el rango de fechas especificado");
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al filtrar movimientos por fechas: {ErrorMessage}", ex.Message);

                // Devolver detalles de la excepción en el entorno de desarrollo
                return StatusCode(500, $"Error interno al filtrar movimientos: {ex.Message}");
            }
        }

        private bool MovInventarioExists(string codCia, string companiaVenta3, string almacenVenta,
            string tipoMovimiento, string tipoDocumento, string nroDocumento, string codItem2)
        {
            return _context.MovInventarios.Any(e =>
                e.CodCia == codCia &&
                e.CompaniaVenta3 == companiaVenta3 &&
                e.AlmacenVenta == almacenVenta &&
                e.TipoMovimiento == tipoMovimiento &&
                e.TipoDocumento == tipoDocumento &&
                e.NroDocumento == nroDocumento &&
                e.CodItem2 == codItem2);
        }
    }
}