using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Categoria_Tarea_A.Models
{
    [Table("Categoria")]
    public class Categoria
    {
        [Key]
        [Column("IdCategoria")]
        public int IdCategoria { get; set; }

        [Column("Nombre")]
        public string Nombre { get; set; }

        [Column("Descripcion")]
        public string Descripcion { get; set; }

        [Column("Estado")]
        public bool Estado { get; set; }
    }
}