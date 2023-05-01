using Core.Entities;
using Core.Interfaces;
using Infraestructura.Data;
using Infraestructura.Repositories;
using Infrastructure.Data;
namespace Infrastructure.Repositories;

public class RolRepository : GenericRepository<Rol>, IRolRepository
{
    public RolRepository(TiendaContext context) : base(context)
    {
    }
}
