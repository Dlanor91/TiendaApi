using Core.Entities;
using CORE.Interfaces;

namespace Core.Interfaces;

public interface IUsuarioRepository : IGenericRepository<Usuario> {
    Task<Usuario> GetByUsernameAsync(string username);
    Task<Usuario> GetByRefreshTokenAsync(string refreshToken);
}

