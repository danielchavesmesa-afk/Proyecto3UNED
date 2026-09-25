using HotelAccesoDatos;
using HotelServiciosAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace HotelServiciosAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmpleadosApiController : ControllerBase
    {
        private readonly EmpleadoDatos _empleadoDatos = new EmpleadoDatos();

        // 1. GET ALL: api/EmpleadosApi (Listar todos los empleados)
        [HttpGet]
        public IActionResult Get()
        {
            var lista = _empleadoDatos.ObtenerTodos();
            return Ok(lista);
        }

        // 2. GET BY ID: api/EmpleadosApi/buscar/{id} (Búsqueda específica)
        [HttpGet("buscar/{id}")]
        public IActionResult GetById(string id)
        {
            var empleado = _empleadoDatos.ObtenerPorId(id);
            if (empleado == null)
                return NotFound("Empleado no encontrado.");

            return Ok(empleado);
        }

        // 3. POST: api/EmpleadosApi (Crear empleado)
        [HttpPost]
        public IActionResult Post([FromBody] Empleado empleado)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Guardar directamente en SQL Server mediante EmpleadoDatos
            bool guardado = _empleadoDatos.Agregar(empleado);

            if (!guardado)
            {
                return StatusCode(500, "Error al guardar el empleado en la base de datos.");
            }

            return Ok(empleado);
        }

        // 4. PUT: api/EmpleadosApi/{id} (Actualizar empleado)
        [HttpPut("{id}")]
        public IActionResult Put(string id, [FromBody] Empleado empActualizado)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var empleadoExistente = _empleadoDatos.ObtenerPorId(id);
            if (empleadoExistente == null)
                return NotFound("Empleado no encontrado.");

            // Mantener el identificador principal
            empActualizado.Identificacion = id;

            // Ejecutar el UPDATE real en SQL Server
            bool actualizado = _empleadoDatos.Actualizar(empActualizado);

            if (!actualizado)
            {
                return StatusCode(500, "Error al actualizar el empleado en la base de datos.");
            }

            return Ok(empActualizado);
        }

        // 5. DELETE: api/EmpleadosApi/{id} (Eliminar empleado)
        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            var empleado = _empleadoDatos.ObtenerPorId(id);
            if (empleado == null)
                return NotFound("Empleado no encontrado.");

            // Ejecutar el DELETE real en SQL Server
            bool eliminado = _empleadoDatos.Eliminar(id);

            if (!eliminado)
            {
                return StatusCode(500, "Error al eliminar el empleado en la base de datos.");
            }

            return Ok("Empleado eliminado correctamente.");
        }
    }
}