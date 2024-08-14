using System.ComponentModel.DataAnnotations;

namespace BibliotecaWebApplicationMvc.Models
{
    public class Autor
    {
        [Key]
        public Guid AutorId { get; set; }
        public string Apellidos { get; set; }
        public string Nombres { get; set; }
        public string Nacionalidad { get; set; }

        public Autor() => this.AutorId = Guid.NewGuid();

    }
}
