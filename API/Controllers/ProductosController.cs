using API.Dtos;
using AutoMapper;
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
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductosController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }    

        //metodo get
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<ProductoListDto>>> Get()
        {
            var productos = await _unitOfWork.Productos
                                        .GetAllAsync();

            return _mapper.Map<List<ProductoListDto>>(productos);
        }

        //get By id
        [HttpGet("{id}")]//indico el parametro
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductoDto>> Get(int id) //es con IActionResult para un id
        {
            var producto = await _unitOfWork.Productos.GetByIdAsync(id);
            if (producto == null)
                return NotFound();

            return _mapper.Map<ProductoDto>(producto);
        }

        //Post: api/Productos
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Producto>> Post(Producto producto)
        {
            _unitOfWork.Productos.Add(producto);
            _unitOfWork.Save();
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
            
            _unitOfWork.Productos.Update(producto);
            _unitOfWork.Save();

            return producto;
        }

        //Post: api/Productos/4
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]        
        public async Task<ActionResult> Delete(int id)
        {
            var producto = await _unitOfWork.Productos.GetByIdAsync(id);
            if (producto == null)
                return NotFound();
            _unitOfWork.Productos.Remove(producto);
            _unitOfWork.Save();

            return NoContent();
        }
    }
}
