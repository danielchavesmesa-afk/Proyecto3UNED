using HotelAccesoDatos;
using HotelServiciosAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace HotelServiciosAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientesApiController : ControllerBase
    {
        private readonly ClienteDatos _clienteDatos = new ClienteDatos();
        private readonly ReservacionDatos _reservacionDatos = new ReservacionDatos();

        // 1. GET ALL: api/ClientesApi (Listar todos)
        [HttpGet]
        public IActionResult Get()
        {
            var lista = _clienteDatos.ObtenerTodos();
            return Ok(lista);
        }

        // 2. GET BY ID: api/ClientesApi/buscar/{id} (Búsqueda específica)
        [HttpGet("buscar/{id}")]
        public IActionResult GetById(string id)
        {
            var cliente = _clienteDatos.ObtenerPorId(id);
            if (cliente == null)
                return NotFound("Cliente no encontrado.");

            return Ok(cliente);
        }

        // 3. POST: api/ClientesApi (Crear)
        [HttpPost]
        public IActionResult Post([FromBody] Cliente cliente)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Insertar directo en SQL Server mediante ClienteDatos
            bool guardado = _clienteDatos.Agregar(cliente);

            if (!guardado)
            {
                return StatusCode(500, "Error al guardar el cliente en la base de datos.");
            }

            return Ok(cliente);
        }

        // 4. PUT: api/ClientesApi/{id} (Actualizar)
        [HttpPut("{id}")]
        public IActionResult Put(string id, [FromBody] Cliente clienteActualizado)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var clienteExistente = _clienteDatos.ObtenerPorId(id);
            if (clienteExistente == null)
                return NotFound("Cliente no encontrado.");

            // Asegurar que mantenga la identificación principal
            clienteActualizado.Identificacion = id;

            // Actualizar en la base de datos
            bool actualizado = _clienteDatos.Actualizar(clienteActualizado);

            if (!actualizado)
            {
                return StatusCode(500, "Error al actualizar el cliente en la base de datos.");
            }

            return Ok(clienteActualizado);
        }

        // 5. DELETE: api/ClientesApi/{id} (Eliminar)
        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            var cliente = _clienteDatos.ObtenerPorId(id);
            if (cliente == null)
                return NotFound("Cliente no encontrado.");

            // Validar que NO tenga reservaciones asociadas en SQL Server antes de eliminar
            var reservas = _reservacionDatos.ObtenerTodas();
            bool tieneReservas = reservas.Any(r => r.ClienteId == id);

            if (tieneReservas)
            {
                return BadRequest("No se puede eliminar el cliente porque tiene reservaciones asociadas.");
            }

            // Eliminar de la base de datos
            bool eliminado = _clienteDatos.Eliminar(id);

            if (!eliminado)
            {
                return StatusCode(500, "Error al eliminar el cliente en la base de datos.");
            }

            return Ok("Cliente eliminado correctamente.");
        }
    }
}