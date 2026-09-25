using System;
using System.ComponentModel.DataAnnotations;

namespace HotelServiciosAPI.Models
{
    public class Reservacion
    {
        [Required(ErrorMessage = "El código de reservación es obligatorio.")]
        public string CodigoReservacion { get; set; }

        [Required(ErrorMessage = "La identificación del cliente es obligatoria.")]
        public string ClienteId { get; set; }

        [Required(ErrorMessage = "El número de habitación es obligatorio.")]
        public string HabitacionNumero { get; set; }

        [Required(ErrorMessage = "La fecha de inicio es obligatoria.")]
        [DataType(DataType.Date)]
        public DateTime FechaInicio { get; set; }

        [Required(ErrorMessage = "La fecha de salida es obligatoria.")]
        [DataType(DataType.Date)]
        public DateTime FechaSalida { get; set; }

        [Required(ErrorMessage = "El estado es obligatorio.")]
        public string Estado { get; set; } // Activa, Cancelada

        [StringLength(250, ErrorMessage = "Las solicitudes no pueden superar los 250 caracteres.")]
        public string SolicitudesEspeciales { get; set; }

        // --- CAMPOS REQUERIDOS POR EL CONTROLADOR DE LA API ---
        public decimal TarifaCalculada { get; set; }
        public decimal PorcentajeDescuento { get; set; }
        public decimal MontoTotal { get; set; }
        public DateTime FechaRealizacion { get; set; } = DateTime.Now;
    }
}