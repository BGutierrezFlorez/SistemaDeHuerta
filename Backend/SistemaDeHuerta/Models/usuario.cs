using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaDeHuerta.Models
{
    public enum TipoUsuario
    {
        Administrador,
        Empleado,
        Cliente
    }
    public class usuario
    {
        public int idUsuario { get; set; }
        public String nombre { get; set; }
        public TipoUsuario tipoUsuario { get; set; }
        public String cedula { get; set; }
        public String correo { get; set; }
        public String contrasena { get; set; }
        public int idCultivo { get; set; }
        public int idComentario { get; set; }
    }
}