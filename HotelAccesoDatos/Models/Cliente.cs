using System;
using System.ComponentModel.DataAnnotations;

namespace HotelServiciosAPI.Models
{
    public class Cliente
    {
        [Required(ErrorMessage = "El tipo de identificación es obligatorio.")]
        public string TipoIdentificacion { get; set; } // Nacional, Extranjero, Jurídico

        [Required(ErrorMessage = "La identificación es obligatoria.")]
        [StringLength(20, ErrorMessage = "La identificación no puede superar los 20 caracteres.")]
        public string Identificacion { get; set; } // Llave primaria conceptual

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(50, ErrorMessage = "El nombre no puede superar los 50 caracteres.")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El primer apellido es obligatorio.")]
        [StringLength(50, ErrorMessage = "El primer apellido no puede superar los 50 caracteres.")]
        public string PrimerApellido { get; set; }

        [StringLength(50, ErrorMessage = "El segundo apellido no puede superar los 50 caracteres.")]
        public string SegundoApellido { get; set; }

        [Required(ErrorMessage = "La fecha de nacimiento es obligatoria.")]
        [DataType(DataType.Date)]
        public DateTime FechaNacimiento { get; set; }
    }
}