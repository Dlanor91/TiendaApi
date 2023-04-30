using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CORE.Entities
{
    public class Categoria :BaseEntity
    {
        //public int Id { get; set; } lo elimino porq lo heredo
        public string Nombre { get; set; }
        //Guarda lista de Productos
        public ICollection<Producto> Productos { get; set; }
    }
}
