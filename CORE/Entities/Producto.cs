using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CORE.Entities
{
    public class Producto:BaseEntity
    {
        //public int Id { get; set; }
        public string Nombre { get; set; }
        public decimal Precio { get; set; } 
        public DateTime FechaCreacion { get; set; }
        //Identifica como Foreign Key - Marca
        public int MarcaId { get; set; }
        public Marca Marca { get; set; }
        //Identifica como Foreign Key - Marca
        public int CategoriaId { get; set; }
        public Categoria Categoria { get; set; }


    }
}
