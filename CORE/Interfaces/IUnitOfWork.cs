using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CORE.Interfaces
{
    public interface IUnitOfWork
    {
        //introduzco todos los repositorios y los salvo
        IProductoRepository Productos { get; }
        IMarcaRepository Marcas { get; }
        ICategoriaRepository Categorias { get; }
        IRolRepository Roles { get; }
        IUsuarioRepository Usuarios { get; }
        Task<int> SaveAsync();
    }
}
