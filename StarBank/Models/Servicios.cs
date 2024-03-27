using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarBank.Models
{
    internal class Servicios
    {
        public String N_Factrua { get; set; }
        public String Nombre { get; set; }

        public String Fecha { get; set; }

        public String Consumo{ get; set; }

        public String Mora { get; set; }

        public String MontoConsumo { get; set; }

        public String TotalPagar { get; set; }
        public String Estado { get; set; }
    }
}
