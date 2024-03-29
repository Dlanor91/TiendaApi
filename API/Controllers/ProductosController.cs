﻿using API.Dtos;
using API.Helpers;
using API.Helpers.Errors;
using AutoMapper;
using CORE.Entities;
using CORE.Interfaces;
using Infraestructura.Data;
using Infraestructura.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    //digo aqui las versiones generales
    [ApiVersion("1.0")]
    [ApiVersion("1.1")]
    [Authorize(Roles = "Administrador")]//tengo q tener token de registro aqui ademas defini un rol
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
        public async Task<ActionResult<Pager<ProductoListDto>>> Get([FromQuery] Params productParams)
        {
            var resultado = await _unitOfWork.Productos
                                        .GetAllAsync(productParams.PageIndex, productParams.PageSize,productParams.Search);

            var listaProductosDto = _mapper.Map<List<ProductoListDto>>(resultado.registros);

            Response.Headers.Add("X-InlineCount", resultado.totalRegistros.ToString());

            return new Pager<ProductoListDto>(listaProductosDto, resultado.totalRegistros,
                productParams.PageIndex, productParams.PageSize, productParams.Search);
            //lo quito por el paginado
            //return _mapper.Map<List<ProductoListDto>>(productos);
        }

        //metodo get versionado
        [HttpGet]
        [MapToApiVersion("1.1")]//solo esta es 1.1 el resto es 1.0
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<ProductoDto>>> Get11()
        {
            var productos = await _unitOfWork.Productos
                                        .GetAllAsync();

            return _mapper.Map<List<ProductoDto>>(productos);
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
                return NotFound(new ApiResponse(404,"El producto solicitado no existe"));

            return _mapper.Map<ProductoDto>(producto);
        }

        //Post: api/Productos
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Producto>> Post(ProductoAddUpdateDto productoDto)
        {
            var producto = _mapper.Map<Producto>(productoDto);

            _unitOfWork.Productos.Add(producto);
            await _unitOfWork.SaveAsync();
            if(producto ==null)
            {
                return BadRequest(new ApiResponse(400));
            }

            productoDto.Id = producto.Id;
            return CreatedAtAction(nameof(Post),new {id = producto.Id}, productoDto);
        }

        //Put: api/Productos/4
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ProductoAddUpdateDto>> Put(int id, [FromBody] ProductoAddUpdateDto productoDto)
        {
            if (productoDto ==null)
                return NotFound(new ApiResponse(404, "El producto solicitado no existe"));

            var productoBd = await _unitOfWork.Productos.GetByIdAsync(id);
            if(productoBd == null)
                return NotFound(new ApiResponse(404, "El producto solicitado no existe"));

            var producto = _mapper.Map<Producto>(productoDto);
            _unitOfWork.Productos.Update(producto);
            await _unitOfWork.SaveAsync();

            return productoDto;
        }

        //Delete: api/Productos
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Delete(int id)
        {
            var producto = await _unitOfWork.Productos.GetByIdAsync(id);
            if (producto == null)
                return NotFound(new ApiResponse(404, "El producto solicitado no existe"));
            _unitOfWork.Productos.Remove(producto);
            await _unitOfWork.SaveAsync();

            return NoContent();
        }
    }
}
