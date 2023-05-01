using CORE.Entities;

namespace Core.Entities;

public class Usuario : BaseEntity
{
    public string Nombres { get; set; }
    public string ApellidoPaterno { get; set; }
    public string ApellidoMaterno { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    //relaciones 1 a muchos
    public ICollection<Rol> Roles { get; set; } = new HashSet<Rol>();//rol
    public ICollection<RefreshToken> RefreshTokens { get; set; } = new HashSet<RefreshToken>();//refrreshtoken
    public ICollection<UsuariosRoles> UsuariosRoles { get; set; }
}
