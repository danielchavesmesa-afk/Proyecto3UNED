using System.ComponentModel.DataAnnotations;

namespace HotelServiciosAPI.Models
{
    public class Habitacion
    {
        [Required(ErrorMessage = "El número de habitación es obligatorio.")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "El número de habitación debe ser puramente numérico.")]
        public string Numero { get; set; }

        [Required(ErrorMessage = "El tipo de habitación es obligatorio.")]
        public string Tipo { get; set; } // Simple, Doble, Suite

        [Required(ErrorMessage = "El precio por noche es obligatorio.")]
        [Range(1, 1000000, ErrorMessage = "El precio debe ser un valor positivo mayor a 0.")]
        public decimal PrecioPorNoche { get; set; }

        [Required(ErrorMessage = "El estado es obligatorio.")]
        public string Estado { get; set; } // Disponible, Mantenimiento, Ocupada
    }
}