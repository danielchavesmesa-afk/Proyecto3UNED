using Microsoft.AspNetCore.Mvc;
using HotelServiciosAPI.Models;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace HotelWebMVC.Controllers
{
    public class EmpleadosController : Controller
    {
        private readonly HttpClient _httpClient;

        public EmpleadosController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("HotelAPI");
        }

        // GET: Empleados (Listar todos o filtrar por identificación)
        public async Task<IActionResult> Index(string identificacion)
        {
            List<Empleado> empleados = new List<Empleado>();

            if (!string.IsNullOrEmpty(identificacion))
            {
                // Búsqueda específica por identificación
                var response = await _httpClient.GetAsync($"api/EmpleadosApi/buscar/{identificacion}");
                if (response.IsSuccessStatusCode)
                {
                    var empleado = await response.Content.ReadFromJsonAsync<Empleado>();
                    if (empleado != null)
                    {
                        empleados.Add(empleado);
                    }
                }
                else
                {
                    ViewBag.Error = "No se encontró ningún empleado con la identificación ingresada.";
                }
            }
            else
            {
                // Traer la lista completa
                var lista = await _httpClient.GetFromJsonAsync<List<Empleado>>("api/EmpleadosApi");
                if (lista != null)
                {
                    empleados = lista;
                }
            }

            return View(empleados);
        }

        // GET: Empleados/Details/{id}
        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrEmpty(id)) return BadRequest();

            var response = await _httpClient.GetAsync($"api/EmpleadosApi/buscar/{id}");
            if (!response.IsSuccessStatusCode) return NotFound();

            var empleado = await response.Content.ReadFromJsonAsync<Empleado>();
            return View(empleado);
        }

        // GET: Empleados/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Empleados/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Empleado empleado)
        {
            if (!ModelState.IsValid) return View(empleado);

            var response = await _httpClient.PostAsJsonAsync("api/EmpleadosApi", empleado);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }

            var errorMsg = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError(string.Empty, errorMsg);
            return View(empleado);
        }

        // GET: Empleados/Edit/{id}
        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id)) return BadRequest();

            var response = await _httpClient.GetAsync($"api/EmpleadosApi/buscar/{id}");
            if (!response.IsSuccessStatusCode) return NotFound();

            var empleado = await response.Content.ReadFromJsonAsync<Empleado>();
            return View(empleado);
        }

        // POST: Empleados/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, Empleado empleado)
        {
            if (id != empleado.Identificacion) return BadRequest();
            if (!ModelState.IsValid) return View(empleado);

            var response = await _httpClient.PutAsJsonAsync($"api/EmpleadosApi/{id}", empleado);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }

            var errorMsg = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError(string.Empty, errorMsg);
            return View(empleado);
        }

        // GET: Empleados/Delete/{id}
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id)) return BadRequest();

            var response = await _httpClient.GetAsync($"api/EmpleadosApi/buscar/{id}");
            if (!response.IsSuccessStatusCode) return NotFound();

            var empleado = await response.Content.ReadFromJsonAsync<Empleado>();
            return View(empleado);
        }

        // POST: Empleados/Delete/{id}
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var response = await _httpClient.DeleteAsync($"api/EmpleadosApi/{id}");

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }

            var errorMsg = await response.Content.ReadAsStringAsync();
            TempData["ErrorMessage"] = errorMsg;

            return RedirectToAction(nameof(Delete), new { id = id });
        }
    }
}