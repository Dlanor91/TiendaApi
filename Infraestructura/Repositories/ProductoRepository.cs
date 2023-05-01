using CORE.Entities;
using CORE.Interfaces;
using Infraestructura.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructura.Repositories
{
    //aqui hereda del repositorio generico y de la interfaz del mismo
    public class ProductoRepository : GenericRepository<Producto>, IProductoRepository
    {
        public ProductoRepository(TiendaContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Producto>> GetProductosMasCaros(int cantidad) =>
                        await _context.Productos
                            .OrderByDescending(p => p.Precio)   //order descendente por precio
                            .Take(cantidad)                     //filtro
                            .ToListAsync();
        //Para poder mapear Datos
        public override async Task<Producto> GetByIdAsync(int id)
        {
            return await _context.Productos
                            .Include(p => p.Marca)
                            .Include(p => p.Categoria)
                            .FirstOrDefaultAsync(p => p.Id == id);

        }

        public override async Task<IEnumerable<Producto>> GetAllAsync()
        {
            return await _context.Productos
                                .Include(u => u.Marca)
                                .Include(u => u.Categoria)
                                .ToListAsync();
        }

        public override async Task<(int totalRegistros, IEnumerable<Producto> registros)> GetAllAsync(
                int pageIndex, int pageSize,string search)
        {
            //para search hasta el if
            var consulta = _context.Productos as IQueryable<Producto>;

            if (!String.IsNullOrEmpty(search))
            {
                consulta= consulta.Where(p => p.Nombre.ToLower().Contains(search));
            }

            var totalRegistros = await consulta
                                        .CountAsync();

            var registros = await consulta
                                    .Include(u => u.Marca)
                                    .Include(u => u.Categoria)
                                    .Skip((pageIndex - 1) * pageSize)
                                    .Take(pageSize)
                                    .ToListAsync();

            return (totalRegistros, registros);
        }

    }
}
