using System.ComponentModel.DataAnnotations;

namespace ClientesAPI.DTOs
{
    public class ClienteCreateDto
    {
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El correo electronico es obligatorio")]
        [EmailAddress(ErrorMessage = "El correo electronico no es valido")]
        public string CorreoElectronico { get; set; }

        [Required(ErrorMessage = "El telefono es obligatorio")]
        [Phone(ErrorMessage = "El telefono no es valido")]
        public string Telefono { get; set; }
    }

    public class ClienteUpdateDto
    {
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El correo electronico es obligatorio")]
        [EmailAddress(ErrorMessage = "El correo electronico no es valido")]
        public string CorreoElectronico { get; set; }

        [Required(ErrorMessage = "El telefono es obligatorio")]
        [Phone(ErrorMessage = "El telefono no es valido")]
        public string Telefono { get; set; }
    }
}