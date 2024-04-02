using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarBank.Models
{
    public class Transacciones
    {
        public String Monto { get; set; }
        public String Tipo { get; set; }
        public String Fecha { get; set; }
        public String CuentaO { get; set; }
        public String CuentaD { get; set; }
        public String NombreENVIO { get; set; }
        public String NombreRecibe { get; set; }
    }
}
