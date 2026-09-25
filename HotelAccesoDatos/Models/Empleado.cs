using System.ComponentModel.DataAnnotations;

namespace HotelServiciosAPI.Models
{
    public class Empleado
    {
        [Required(ErrorMessage = "La identificación es obligatoria.")]
        [StringLength(20, ErrorMessage = "La identificación no puede superar los 20 caracteres.")]
        public string Identificacion { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(50, ErrorMessage = "El nombre no puede superar los 50 caracteres.")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El primer apellido es obligatorio.")]
        [StringLength(50, ErrorMessage = "El primer apellido no puede superar los 50 caracteres.")]
        public string PrimerApellido { get; set; }

        [StringLength(50, ErrorMessage = "El segundo apellido no puede superar los 50 caracteres.")]
        public string SegundoApellido { get; set; }

        [Required(ErrorMessage = "El puesto es obligatorio.")]
        public string Puesto { get; set; }

        [Required(ErrorMessage = "La provincia es obligatoria.")]
        public string Provincia { get; set; }

        [Required(ErrorMessage = "El cantón es obligatorio.")]
        public string Canton { get; set; }

        [Required(ErrorMessage = "El distrito es obligatorio.")]
        public string Distrito { get; set; }
    }
}