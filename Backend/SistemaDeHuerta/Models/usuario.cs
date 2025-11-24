using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaDeHuerta.Models
{
    public enum TipoUsuario
    {
        Administrador,
        Usuario,
        Supervisor,
        Invitado
    }
    public class usuario
    {
        public int idUsuario { get; set; }
        public String nombre { get; set; }
        public TipoUsuario tipoUsuario { get; set; }
        public String cedula { get; set; }
        public String correo { get; set; }
        public String contraseña { get; set; }
    }
}