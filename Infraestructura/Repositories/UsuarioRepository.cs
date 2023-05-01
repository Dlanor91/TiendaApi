using Core.Entities;
using Core.Interfaces;
using Infraestructura.Data;
using Infraestructura.Repositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class UsuarioRepository : GenericRepository<Usuario>, IUsuarioRepository
{
    public UsuarioRepository(TiendaContext context) : base(context)
    {
    }

    public async Task<Usuario> GetByUsernameAsync(string username)
    {
        return await _context.Usuarios
                            .Include(u=>u.Roles)
                            .FirstOrDefaultAsync(u=>u.Username.ToLower()==username.ToLower());
    }
}

