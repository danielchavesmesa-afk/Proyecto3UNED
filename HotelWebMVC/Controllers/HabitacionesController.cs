using Microsoft.AspNetCore.Mvc;
using HotelServiciosAPI.Models;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace HotelWebMVC.Controllers
{
    public class HabitacionesController : Controller
    {
        private readonly HttpClient _httpClient;

        public HabitacionesController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("HotelAPI");
        }

        // GET: Habitaciones (Listar todas o filtrar por número)
        public async Task<IActionResult> Index(string numero)
        {
            List<Habitacion> habitaciones = new List<Habitacion>();

            if (!string.IsNullOrEmpty(numero))
            {
                // Búsqueda específica por número
                var response = await _httpClient.GetAsync($"api/HabitacionesApi/buscar/{numero}");
                if (response.IsSuccessStatusCode)
                {
                    var habitacion = await response.Content.ReadFromJsonAsync<Habitacion>();
                    if (habitacion != null)
                    {
                        habitaciones.Add(habitacion);
                    }
                }
                else
                {
                    ViewBag.Error = "No se encontró ninguna habitación con el número ingresado.";
                }
            }
            else
            {
                // Traer la lista completa
                var lista = await _httpClient.GetFromJsonAsync<List<Habitacion>>("api/HabitacionesApi");
                if (lista != null)
                {
                    habitaciones = lista;
                }
            }

            return View(habitaciones);
        }

        // GET: Habitaciones/Details/{id}
        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrEmpty(id)) return BadRequest();

            var response = await _httpClient.GetAsync($"api/HabitacionesApi/buscar/{id}");
            if (!response.IsSuccessStatusCode) return NotFound();

            var habitacion = await response.Content.ReadFromJsonAsync<Habitacion>();
            return View(habitacion);
        }

        // GET: Habitaciones/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Habitaciones/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Habitacion habitacion)
        {
            if (!ModelState.IsValid) return View(habitacion);

            var response = await _httpClient.PostAsJsonAsync("api/HabitacionesApi", habitacion);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }

            var errorMsg = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError(string.Empty, errorMsg);
            return View(habitacion);
        }

        // GET: Habitaciones/Edit/{id}
        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id)) return BadRequest();

            var response = await _httpClient.GetAsync($"api/HabitacionesApi/buscar/{id}");
            if (!response.IsSuccessStatusCode) return NotFound();

            var habitacion = await response.Content.ReadFromJsonAsync<Habitacion>();
            return View(habitacion);
        }

        // POST: Habitaciones/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, Habitacion habitacion)
        {
            if (id != habitacion.Numero) return BadRequest();
            if (!ModelState.IsValid) return View(habitacion);

            var response = await _httpClient.PutAsJsonAsync($"api/HabitacionesApi/{id}", habitacion);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }

            var errorMsg = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError(string.Empty, errorMsg);
            return View(habitacion);
        }

        // GET: Habitaciones/Delete/{id}
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id)) return BadRequest();

            var response = await _httpClient.GetAsync($"api/HabitacionesApi/buscar/{id}");
            if (!response.IsSuccessStatusCode) return NotFound();

            var habitacion = await response.Content.ReadFromJsonAsync<Habitacion>();
            return View(habitacion);
        }

        // POST: Habitaciones/Delete/{id}
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var response = await _httpClient.DeleteAsync($"api/HabitacionesApi/{id}");

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }

            // Atrapamos la regla de negocio (No borrar si está enlazada a reservación activa)
            var errorMsg = await response.Content.ReadAsStringAsync();
            TempData["ErrorMessage"] = errorMsg;

            return RedirectToAction(nameof(Delete), new { id = id });
        }
    }
}