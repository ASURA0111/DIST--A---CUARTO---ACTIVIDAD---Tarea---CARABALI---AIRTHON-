using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vehiculo_Tarea_A.Models
{
    [Table("Vehiculo")]
    public class Vehiculo
    {
        [Key]
        [Column("IdVehiculo")]
        public int IdVehiculo { get; set; }

        [Column("IdCategoria")]
        public int IdCategoria { get; set; }

        [Column("Marca")]
        public string Marca { get; set; }

        [Column("Modelo")]
        public string Modelo { get; set; }

        // DECIMAL en SQL se mapea a decimal en C#
        [Column("Precio")]
        public decimal Precio { get; set; }

        [Column("Stock")]
        public int Stock { get; set; }

        // BIT en SQL se mapea a bool en C#
        [Column("Estado")]
        public bool Estado { get; set; }
    }
}