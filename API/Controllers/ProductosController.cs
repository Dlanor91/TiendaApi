using CORE.Entities;
using CORE.Interfaces;
using Infraestructura.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class ProductosController : BaseApiController
    {
        //sustituyo por su repositorio private readonly TiendaContext _context; // lo annado al controller
        private readonly IProductoRepository _productoRepository;
        
        public ProductosController(IProductoRepository productoRepository) //luego lo introduzco en el contructor
        {
            _productoRepository = productoRepository;
        }

        //metodo get
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<Producto>>> Get()
        {
            var productos = await _productoRepository
                            .GetAllAsync();
            return Ok(productos);
        }

        //get By id
        [HttpGet("{id}")]//indico el parametro
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Get(int id) //es con IActionResult para un id
        {
            var producto = await _productoRepository.GetByIdAsync(id);            
            return Ok(producto);
        }
    }
}
