using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using SistemaDeHuerta.Data;
using SistemaDeHuerta.Models;
using System.Data.SqlClient;

namespace SistemaDeHuerta.Controllers
{
    [RoutePrefix("api/usuario")]
    public class UsuarioController : ApiController
    {
        // Preflight CORS
        [HttpOptions]
        [Route("")]
        public IHttpActionResult OptionsUsuario() => Ok();
        
        [HttpOptions]
        [Route("{id}")]
        public IHttpActionResult OptionsUsuarioPorId() => Ok();

        [HttpOptions]
        [Route("login")]
        public IHttpActionResult OptionsLogin() => Ok();

        
        // GET api/usuario/test - Test de conexión simple
        [HttpGet]
        [Route("test")]
        public IHttpActionResult Test()
        {
            try
            {
                ConexionBD conexion = new ConexionBD();
                string resultado = "Error desconocido";
                
                // Prueba simple: contar registros
                if (conexion.ConsultarValorUnico("SELECT COUNT(*) FROM Usuario", false))
                {
                    resultado = "Total usuarios: " + conexion.ValorUnico;
                }
                else
                {
                    resultado = "Error: " + conexion.Error;
                }
                
                conexion.CerrarConexion();
                return Ok(new { resultado });
            }
            catch (Exception ex)
            {
                return Ok(new { error = ex.Message, stack = ex.StackTrace });
            }
        }

        // GET api/usuario - Listar todos los usuarios
        [HttpGet]
        [Route("")]
        public IHttpActionResult Get()
        {
            try
            {
                var usuarios = UsuarioData.ListarUsuarios();
                return Ok(usuarios);
            }
            catch (Exception ex)
            {
                return BadRequest("Error: " + ex.Message);
            }
        }

        // POST api/usuario - Registrar un nuevo usuario
        [HttpPost]
        [Route("")]
        public IHttpActionResult Post([FromBody] usuario nuevo)
        {
            if (nuevo == null)
                return BadRequest("Cuerpo de petición vacío o inválido.");

            try
            {
                bool creado = UsuarioData.RegistrarUsuario(nuevo);
                if (creado)
                    return Ok(new { success = true, mensaje = "Usuario registrado." });
                else
                    return BadRequest("No se pudo registrar el usuario.");
            }
            catch (Exception ex)
            {
                return BadRequest("Error al registrar: " + ex.Message);
            }
        }

    }
}