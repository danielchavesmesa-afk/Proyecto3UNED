using HotelAccesoDatos;
using HotelServiciosAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace HotelServiciosAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservacionesApiController : ControllerBase
    {
        // Instancias directas a la capa Acceso a Datos
        private readonly ReservacionDatos _reservacionDatos = new ReservacionDatos();
        private readonly ClienteDatos _clienteDatos = new ClienteDatos();
        private readonly HabitacionDatos _habitacionDatos = new HabitacionDatos();

        // 1. GET ALL: api/ReservacionesApi
        [HttpGet]
        public IActionResult Get()
        {
            var lista = _reservacionDatos.ObtenerTodas();
            return Ok(lista);
        }

        // 2. GET BY ID / BÚSQUEDA ESPECÍFICA: api/ReservacionesApi/buscar/{codigo}
        [HttpGet("buscar/{codigo}")]
        public IActionResult GetByCodigo(string codigo)
        {
            var reservacion = _reservacionDatos.ObtenerPorCodigo(codigo);
            if (reservacion == null)
                return NotFound("Reservación no encontrada.");

            return Ok(reservacion);
        }

        // 3. GET BY CLIENTE: api/ReservacionesApi/buscarPorCliente/{clienteId}
        [HttpGet("buscarPorCliente/{clienteId}")]
        public IActionResult GetByCliente(string clienteId)
        {
            var reservas = _reservacionDatos.ObtenerTodas()
                                           .Where(r => r.ClienteId == clienteId)
                                           .ToList();
            return Ok(reservas);
        }

        // 4. POST: api/ReservacionesApi
        [HttpPost]
        public IActionResult Post([FromBody] Reservacion reserva)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            // Validar que el cliente y la habitación realmente existan en SQL Server
            var cliente = _clienteDatos.ObtenerPorId(reserva.ClienteId);
            var habitacion = _habitacionDatos.ObtenerPorNumero(reserva.HabitacionNumero);

            if (cliente == null) return BadRequest("El cliente asociado no existe.");
            if (habitacion == null) return BadRequest("La habitación asociada no existe.");

            // Validar fechas
            if (reserva.FechaSalida <= reserva.FechaInicio)
            {
                return BadRequest("La fecha de salida debe ser estrictamente posterior a la fecha de inicio.");
            }

            // Validar disponibilidad (Evitar traslapes desde la lista de la BD)
            var todasLasReservas = _reservacionDatos.ObtenerTodas();
            bool estaTraslapada = todasLasReservas.Any(r =>
                r.HabitacionNumero == reserva.HabitacionNumero &&
                r.Estado != "Cancelada" &&
                ((reserva.FechaInicio >= r.FechaInicio && reserva.FechaInicio < r.FechaSalida) ||
                 (reserva.FechaSalida > r.FechaInicio && reserva.FechaSalida <= r.FechaSalida) ||
                 (reserva.FechaInicio <= r.FechaInicio && reserva.FechaSalida >= r.FechaSalida))
            );

            if (estaTraslapada)
            {
                return BadRequest("La habitación no está disponible para el rango de fechas seleccionado.");
            }

            // Cálculos del Proyecto
            int noches = (reserva.FechaSalida - reserva.FechaInicio).Days;
            reserva.TarifaCalculada = habitacion.PrecioPorNoche * noches;

            decimal montoConDescuento = reserva.TarifaCalculada - (reserva.TarifaCalculada * (reserva.PorcentajeDescuento / 100));
            reserva.MontoTotal = montoConDescuento * 1.13m; // Monto + 13% IVA

            // Autogenerar código único si viene vacío
            if (string.IsNullOrEmpty(reserva.CodigoReservacion))
            {
                reserva.CodigoReservacion = "RES" + Guid.NewGuid().ToString().Substring(0, 8).ToUpper();
            }
            reserva.FechaRealizacion = DateTime.Now;

            // Guardar en la Base de Datos
            bool guardado = _reservacionDatos.Agregar(reserva);

            if (!guardado)
            {
                return StatusCode(500, "Error al guardar la reservación en la base de datos.");
            }

            return Ok(reserva);
        }

        // 5. PUT / EDITAR: api/ReservacionesApi/{codigo}
        [HttpPut("{codigo}")]
        public IActionResult Put(string codigo, [FromBody] Reservacion resActualizada)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var reservaExistente = _reservacionDatos.ObtenerPorCodigo(codigo);
            if (reservaExistente == null) return NotFound("Reservación no encontrada.");

            var habitacion = _habitacionDatos.ObtenerPorNumero(resActualizada.HabitacionNumero);
            if (habitacion == null) return BadRequest("La habitación asociada no existe.");

            // Validar traslape excluyéndose a sí misma
            var todasLasReservas = _reservacionDatos.ObtenerTodas();
            bool estaTraslapada = todasLasReservas.Any(r =>
                r.CodigoReservacion != codigo &&
                r.HabitacionNumero == resActualizada.HabitacionNumero &&
                r.Estado != "Cancelada" &&
                ((resActualizada.FechaInicio >= r.FechaInicio && resActualizada.FechaInicio < r.FechaSalida) ||
                 (resActualizada.FechaSalida > r.FechaInicio && resActualizada.FechaSalida <= r.FechaSalida) ||
                 (resActualizada.FechaInicio <= r.FechaInicio && resActualizada.FechaSalida >= r.FechaSalida))
            );

            if (estaTraslapada)
            {
                return BadRequest("La habitación no está disponible en esas fechas.");
            }

            // Recalcular montos
            int noches = (resActualizada.FechaSalida - resActualizada.FechaInicio).Days;
            resActualizada.TarifaCalculada = habitacion.PrecioPorNoche * noches;

            decimal montoConDescuento = resActualizada.TarifaCalculada - (resActualizada.TarifaCalculada * (resActualizada.PorcentajeDescuento / 100));
            resActualizada.MontoTotal = montoConDescuento * 1.13m;

            // Asignar el código correspondiente
            resActualizada.CodigoReservacion = codigo;

            // Ejecutar el UPDATE real en SQL Server
            bool actualizado = _reservacionDatos.Actualizar(resActualizada);

            if (!actualizado)
            {
                return StatusCode(500, "Error al actualizar la reservación en la base de datos.");
            }

            return Ok(resActualizada);
        }

        // 6. DELETE / ELIMINAR: api/ReservacionesApi/{codigo}
        [HttpDelete("{codigo}")]
        public IActionResult Delete(string codigo)
        {
            var reserva = _reservacionDatos.ObtenerPorCodigo(codigo);
            if (reserva == null) return NotFound("Reservación no encontrada.");

            // Ejecutar el DELETE real en SQL Server
            bool eliminado = _reservacionDatos.Eliminar(codigo);

            if (!eliminado)
            {
                return StatusCode(500, "Error al eliminar la reservación en la base de datos.");
            }

            return Ok("Reservación eliminada correctamente.");
        }
    }
}