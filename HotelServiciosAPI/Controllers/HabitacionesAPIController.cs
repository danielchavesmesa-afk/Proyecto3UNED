using HotelAccesoDatos;
using HotelServiciosAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace HotelServiciosAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HabitacionesApiController : ControllerBase
    {
        private readonly HabitacionDatos _habitacionDatos = new HabitacionDatos();
        private readonly ReservacionDatos _reservacionDatos = new ReservacionDatos();

        // 1. GET ALL: api/HabitacionesApi
        [HttpGet]
        public IActionResult Get()
        {
            var lista = _habitacionDatos.ObtenerTodas();
            return Ok(lista);
        }

        // 2. GET BY NUMERO: api/HabitacionesApi/buscar/{numero} (Búsqueda específica)
        [HttpGet("buscar/{numero}")]
        public IActionResult GetByNumero(string numero)
        {
            var habitacion = _habitacionDatos.ObtenerPorNumero(numero);
            if (habitacion == null)
                return NotFound("Habitación no encontrada.");

            return Ok(habitacion);
        }

        // 3. POST: api/HabitacionesApi (Crear)
        [HttpPost]
        public IActionResult Post([FromBody] Habitacion habitacion)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Insertar directo en SQL Server
            bool guardado = _habitacionDatos.Agregar(habitacion);

            if (!guardado)
            {
                return StatusCode(500, "Error al guardar la habitación en la base de datos.");
            }

            return Ok(habitacion);
        }

        // 4. PUT: api/HabitacionesApi/{numero} (Actualizar)
        [HttpPut("{numero}")]
        public IActionResult Put(string numero, [FromBody] Habitacion habActualizada)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var habitacionExistente = _habitacionDatos.ObtenerPorNumero(numero);
            if (habitacionExistente == null)
                return NotFound("Habitación no encontrada.");

            // Mantener el número de habitación como llave
            habActualizada.Numero = numero;

            // Actualizar directo en SQL Server
            bool actualizado = _habitacionDatos.Actualizar(habActualizada);

            if (!actualizado)
            {
                return StatusCode(500, "Error al actualizar la habitación en la base de datos.");
            }

            return Ok(habActualizada);
        }

        // 5. DELETE: api/HabitacionesApi/{numero} (Eliminar)
        [HttpDelete("{numero}")]
        public IActionResult Delete(string numero)
        {
            var habitacion = _habitacionDatos.ObtenerPorNumero(numero);
            if (habitacion == null)
                return NotFound("Habitación no encontrada.");

            // Validar que no esté asociada a una reservación en SQL Server
            var reservas = _reservacionDatos.ObtenerTodas();
            bool tieneReservas = reservas.Any(r => r.HabitacionNumero == numero);

            if (tieneReservas)
            {
                return BadRequest("No se puede eliminar la habitación porque cuenta con reservaciones registradas.");
            }

            // Eliminar directo en SQL Server
            bool eliminado = _habitacionDatos.Eliminar(numero);

            if (!eliminado)
            {
                return StatusCode(500, "Error al eliminar la habitación en la base de datos.");
            }

            return Ok("Habitación eliminada correctamente.");
        }
    }
}