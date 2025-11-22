using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaDeHuerta.Models
{
    public enum TipoCultivo
    {
        Vegetal,
        Frutal,
        Aromatico,
        Medicinal
    }
    public class cultivo
    {
        public int idCultivo { get; set; }
        public TipoCultivo tipoCultivo { get; set; }
        public DateTime fechaSiembra { get; set; }
        public String riego { get; set; }
    }
}