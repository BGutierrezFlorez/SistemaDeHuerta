using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaDeHuerta.Models
{
    public class comentario
    {
        public int idComentario { get; set; }
        public String contenido { get; set; }
        public DateTime fechaPublicacion { get; set; }
        public int idUsuario { get; set; }
        public int idCultivo { get; set; }
    }
}