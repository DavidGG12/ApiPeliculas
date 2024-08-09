using ApiPeliculas.Data;
using ApiPeliculas.Modelos;
using ApiPeliculas.Modelos.Dtos;
using ApiPeliculas.Repositorio.IRepositorio;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using XSystem.Security.Cryptography;

namespace ApiPeliculas.Repositorio
{
    public class UsuarioRepositorio : IUsuarioRepositorio
    {
        private readonly ApplicationDbContext _bd;
        private string claveSecreta;

        public UsuarioRepositorio(ApplicationDbContext db, IConfiguration config)
        {
            _bd = db;
            claveSecreta = config.GetValue<string>("ApiSettings:Secreta");
            //claveSecreta = "tu_clave_secreta_de_16_bytes!_holaqtal";
        }

        public ICollection<Usuario> GetUsuario()
        {
            return _bd.Usuario.OrderBy(c => c.NombreUsuario).ToList();
        }

        public Usuario GetUsuario(int UsuarioID)
        {
            return _bd.Usuario.FirstOrDefault(c => c.UsuarioID == UsuarioID);
        }

        public bool IsUniqueUser(string usuario)
        {
            var usuarioBD = _bd.Usuario.FirstOrDefault(u => u.NombreUsuario == usuario);
            if (usuarioBD == null) return true;
            return false;
        }

        /*
         * Los métodos Task son métodos asíncronas, las cuales quieren decir que, mientras se ejecutan,
         * pueden realizar otras acciones sin ningún problema en lo que se ejecutan.
         */
        public async Task<UsuarioLoginRespuestaDto> Login(UsuarioLoginDto usuarioLoginDto)
        {
            var passwordEncriptado = obtenermd5(usuarioLoginDto.Password);
            var usuario = _bd.Usuario.FirstOrDefault(
                    u => u.NombreUsuario.ToLower() == usuarioLoginDto.NombreUsuario.ToLower()
                    && u.Password == passwordEncriptado
                );
            
            //Válidamos si el usuario no existe con la combinación de usuario y contraseña
            if(usuario == null) return new UsuarioLoginRespuestaDto() { Token = "", Usuario = null };

            //Existe el usuario entonces podemos procesar el login
            var manejandoToken = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(claveSecreta);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, usuario.NombreUsuario.ToString()),
                    new Claim(ClaimTypes.Role, usuario.Role)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = manejandoToken.CreateToken(tokenDescriptor);
            UsuarioLoginRespuestaDto usuarioLoginRespuestaDto = new UsuarioLoginRespuestaDto()
            {
                Token = manejandoToken.WriteToken(token),
                Usuario = usuario
            };

            return usuarioLoginRespuestaDto;
        }

        public async Task<Usuario> Registro(UsuarioRegistroDto usuarioRegistroDto)
        {
            var passwordEncriptado = obtenermd5(usuarioRegistroDto.Password);

            Usuario usuario = new Usuario()
            {
                NombreUsuario = usuarioRegistroDto.NombreUsuario,
                Password = passwordEncriptado,
                Nombre = usuarioRegistroDto.Nombre,
                Role = usuarioRegistroDto.Role
            };

            _bd.Usuario.Add(usuario);
            await _bd.SaveChangesAsync();
            usuario.Password = passwordEncriptado;
            
            return usuario;
        }

        public static string obtenermd5(string valor)
        {
            MD5CryptoServiceProvider x = new MD5CryptoServiceProvider(); 
            byte[] data = System.Text.Encoding.UTF8.GetBytes(valor);
            data = x.ComputeHash(data);
            string resp = "";
            for (int i = 0; i < data.Length; i++)
                resp += data[i].ToString("x2").ToLower();
            return resp;
        }
    }
}
