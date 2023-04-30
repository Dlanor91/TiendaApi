using CORE.Entities;
using CORE.Interfaces;
using Infraestructura.Data;
using Infraestructura.UnitOfWork;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class ProductosController : BaseApiController
    {
        //sustituyo por su repositorio private readonly TiendaContext _context; // lo annado al controller
        private readonly IUnitOfWork _unitofwork;
        
        public ProductosController(IUnitOfWork unitofwork) //luego lo introduzco en el contructor
        {
            _unitofwork = unitofwork;
        }

        //metodo get
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<Producto>>> Get()
        {
            var productos = await _unitofwork.Productos
                            .GetAllAsync();
            return Ok(productos);
        }

        //get By id
        [HttpGet("{id}")]//indico el parametro
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Get(int id) //es con IActionResult para un id
        {
            var producto = await _unitofwork.Productos.GetByIdAsync(id);            
            return Ok(producto);
        }

        //Post: api/Productos
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Producto>> Post(Producto producto)
        {
            _unitofwork.Productos.Add(producto);
            _unitofwork.Save();
            if(producto ==null)
            {
                return BadRequest();
            }

            return CreatedAtAction(nameof(Post),new {id = producto.Id},producto);
        }

        //Post: api/Productos/4
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Producto>> Put(int id, [FromBody]Producto producto)
        {
            if (producto ==null)
                return NotFound();
            
            _unitofwork.Productos.Update(producto);
            _unitofwork.Save();

            return producto;
        }

        //Post: api/Productos/4
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]        
        public async Task<ActionResult> Delete(int id)
        {
            var producto = await _unitofwork.Productos.GetByIdAsync(id);
            if (producto == null)
                return NotFound();
            _unitofwork.Productos.Remove(producto);
            _unitofwork.Save();

            return NoContent();
        }
    }
}
