using System.ComponentModel.DataAnnotations;

namespace BibliotecaWebApplicationMvc.Models
{
    public class Libro
    {
        [Key]
        public Guid libroId { get; set; }
        public string ISBN { get; set; }
        public string Titulo { get; set; }
        public int NumeroPaginas { get; set; }


        public Libro()
        {
            this.libroId = Guid.NewGuid();
        }
        //propiedad de navegacion
        public ICollection<AutorLibro> LibrosAutores { get; set; } = new List<AutorLibro>();
    }
}
