using Microsoft.AspNetCore.Mvc;
using HotelServiciosAPI.Models;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace HotelWebMVC.Controllers
{
    public class ClientesController : Controller
    {
        private readonly HttpClient _httpClient;

        // Inyectamos el HttpClient que configuramos en el Program.cs
        public ClientesController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("HotelAPI");
        }

        // GET: Clientes (Soporta listar todos o filtrar por identificación)
        public async Task<IActionResult> Index(string identificacion)
        {
            List<Cliente> clientes = new List<Cliente>();

            if (!string.IsNullOrEmpty(identificacion))
            {
                // Búsqueda específica por identificación
                var response = await _httpClient.GetAsync($"api/ClientesApi/buscar/{identificacion}");
                if (response.IsSuccessStatusCode)
                {
                    var cliente = await response.Content.ReadFromJsonAsync<Cliente>();
                    if (cliente != null)
                    {
                        clientes.Add(cliente);
                    }
                }
                else
                {
                    ViewBag.Error = "No se encontró ningún cliente con la identificación ingresada.";
                }
            }
            else
            {
                // Traer la lista completa
                var lista = await _httpClient.GetFromJsonAsync<List<Cliente>>("api/ClientesApi");
                if (lista != null)
                {
                    clientes = lista;
                }
            }

            return View(clientes);
        }

        // GET: Clientes/Details/{id} (Buscar por identificación)
        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrEmpty(id)) return BadRequest();

            var response = await _httpClient.GetAsync($"api/ClientesApi/buscar/{id}");
            if (!response.IsSuccessStatusCode) return NotFound();

            var cliente = await response.Content.ReadFromJsonAsync<Cliente>();
            return View(cliente);
        }

        // GET: Clientes/Create (Pantalla del Formulario)
        public IActionResult Create()
        {
            return View();
        }

        // POST: Clientes/Create (Procesar Formulario)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Cliente cliente)
        {
            if (!ModelState.IsValid) return View(cliente);

            var response = await _httpClient.PostAsJsonAsync("api/ClientesApi", cliente);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }

            // Capturar errores de validación de la API (por ejemplo, cédula duplicada)
            var errorMsg = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError(string.Empty, errorMsg);
            return View(cliente);
        }

        // GET: Clientes/Edit/{id} (Pantalla de Editar)
        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id)) return BadRequest();

            var response = await _httpClient.GetAsync($"api/ClientesApi/buscar/{id}");
            if (!response.IsSuccessStatusCode) return NotFound();

            var cliente = await response.Content.ReadFromJsonAsync<Cliente>();
            return View(cliente);
        }

        // POST: Clientes/Edit/{id} (Procesar Edición)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, Cliente cliente)
        {
            if (id != cliente.Identificacion) return BadRequest();
            if (!ModelState.IsValid) return View(cliente);

            var response = await _httpClient.PutAsJsonAsync($"api/ClientesApi/{id}", cliente);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }

            var errorMsg = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError(string.Empty, errorMsg);
            return View(cliente);
        }

        // GET: Clientes/Delete/{id} (Pantalla de Confirmación de Borrado)
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id)) return BadRequest();

            var response = await _httpClient.GetAsync($"api/ClientesApi/buscar/{id}");
            if (!response.IsSuccessStatusCode) return NotFound();

            var cliente = await response.Content.ReadFromJsonAsync<Cliente>();
            return View(cliente);
        }

        // POST: Clientes/Delete/{id} (Procesar Borrado Físico)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var response = await _httpClient.DeleteAsync($"api/ClientesApi/{id}");

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }

            // Si la API rechaza el borrado, atrapamos el error
            var errorMsg = await response.Content.ReadAsStringAsync();
            TempData["ErrorMessage"] = errorMsg;

            return RedirectToAction(nameof(Delete), new { id = id });
        }
    }
}