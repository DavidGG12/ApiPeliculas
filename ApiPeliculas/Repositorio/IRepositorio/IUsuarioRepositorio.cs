using ApiPeliculas.Modelos;
using ApiPeliculas.Modelos.Dtos;
using System.Collections.Generic;

namespace ApiPeliculas.Repositorio.IRepositorio
{
    public interface IUsuarioRepositorio
    {
        //Esta interface es la estructura que va a tener la clase con la que vamos a trabajar, en este caso, con el repositorio
        //Tenemos las funciones con las que vamos a hacer un CRUD
        ICollection<Usuario>GetUsuario();
        Usuario GetUsuario(int UsuarioID);
        bool IsUniqueUser(string usuario);
        Task<UsuarioLoginRespuestaDto> Login(UsuarioLoginDto usuarioLoginDto);
        Task<Usuario> Registro(UsuarioRegistroDto usuarioRegistroDto);
    }
}
