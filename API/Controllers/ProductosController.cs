using CORE.Entities;
using Infraestructura.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class ProductosController : BaseApiController
    {
        private readonly TiendaContext _context; // lo annado al controller
        public ProductosController(TiendaContext context) //luego lo introduzco en el contructor
        {
            _context = context;
        }

        //metodo get
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<Producto>>> Get()
        {
            var productos = await _context.Productos
                            .ToListAsync();
            return Ok(productos);
        }

        //get By id
        [HttpGet("{id}")]//indico el parametro
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Get(int id) //es con IActionResult para un id
        {
            var producto = await _context.Productos.FindAsync(id);            
            return Ok(producto);
        }
    }
}
