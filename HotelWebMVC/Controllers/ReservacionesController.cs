using Microsoft.AspNetCore.Mvc;
using HotelServiciosAPI.Models;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace HotelWebMVC.Controllers
{
    public class ReservacionesController : Controller
    {
        private readonly HttpClient _httpClient;

        public ReservacionesController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("HotelAPI");
        }

        // GET: Reservaciones (Soporta listar todas o filtrar por código)
        public async Task<IActionResult> Index(string codigo)
        {
            List<Reservacion> reservaciones = new List<Reservacion>();

            if (!string.IsNullOrEmpty(codigo))
            {
                // Búsqueda específica por código
                var response = await _httpClient.GetAsync($"api/ReservacionesApi/buscar/{codigo}");
                if (response.IsSuccessStatusCode)
                {
                    var reservacion = await response.Content.ReadFromJsonAsync<Reservacion>();
                    if (reservacion != null)
                    {
                        reservaciones.Add(reservacion);
                    }
                }
                else
                {
                    ViewBag.Error = "No se encontró ninguna reservación con el código ingresado.";
                }
            }
            else
            {
                // Traer la lista completa
                var lista = await _httpClient.GetFromJsonAsync<List<Reservacion>>("api/ReservacionesApi");
                if (lista != null)
                {
                    reservaciones = lista;
                }
            }

            return View(reservaciones);
        }

        // GET: Reservaciones/Details/{id}
        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrEmpty(id)) return BadRequest();

            var response = await _httpClient.GetAsync($"api/ReservacionesApi/buscar/{id}");
            if (!response.IsSuccessStatusCode) return NotFound();

            var reservacion = await response.Content.ReadFromJsonAsync<Reservacion>();
            return View(reservacion);
        }

        // GET: Reservaciones/Create
        public async Task<IActionResult> Create()
        {
            // Cargamos clientes y habitaciones de la API para llenar los comboboxes en la vista
            ViewBag.Clientes = await _httpClient.GetFromJsonAsync<List<Cliente>>("api/ClientesApi");
            ViewBag.Habitaciones = await _httpClient.GetFromJsonAsync<List<Habitacion>>("api/HabitacionesApi");

            return View();
        }

        // POST: Reservaciones/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Reservacion reservacion)
        {
            // Validación de fechas
            if (reservacion.FechaSalida <= reservacion.FechaInicio)
            {
                ModelState.AddModelError(string.Empty, "La fecha de salida debe ser posterior a la fecha de inicio.");
            }

            // Remoción de campos autogenerados para evitar fallos de ModelState
            ModelState.Remove("CodigoReservacion");
            ModelState.Remove("Estado");

            if (!ModelState.IsValid)
            {
                ViewBag.Clientes = await _httpClient.GetFromJsonAsync<List<Cliente>>("api/ClientesApi");
                ViewBag.Habitaciones = await _httpClient.GetFromJsonAsync<List<Habitacion>>("api/HabitacionesApi");
                return View(reservacion);
            }

            // Autogeneración de código único
            reservacion.CodigoReservacion = "RES-" + DateTime.Now.Ticks.ToString().Substring(10);

            // Comunicación asíncrona con la API
            var response = await _httpClient.PostAsJsonAsync("api/ReservacionesApi", reservacion);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }

            var errorMsg = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError(string.Empty, errorMsg);

            ViewBag.Clientes = await _httpClient.GetFromJsonAsync<List<Cliente>>("api/ClientesApi");
            ViewBag.Habitaciones = await _httpClient.GetFromJsonAsync<List<Habitacion>>("api/HabitacionesApi");
            return View(reservacion);
        }

        // GET: Reservaciones/Edit/{id}
        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id)) return BadRequest();

            var response = await _httpClient.GetAsync($"api/ReservacionesApi/buscar/{id}");
            if (!response.IsSuccessStatusCode) return NotFound();

            var reservacion = await response.Content.ReadFromJsonAsync<Reservacion>();
            return View(reservacion);
        }

        // POST: Reservaciones/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, Reservacion reservacion)
        {
            if (id != reservacion.CodigoReservacion) return BadRequest();

            var response = await _httpClient.PutAsJsonAsync($"api/ReservacionesApi/{id}", reservacion);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }

            var errorMsg = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError(string.Empty, errorMsg);
            return View(reservacion);
        }
    }
}