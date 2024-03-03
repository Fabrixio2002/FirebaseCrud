using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarBank.Models
{
    class Usuarios
    {
        //campos de los usuarios
        //aqui manejamos datos generales tambien sus saldos actuales
        //igualmente su numero de cuenta sera añadido aqui.

        public String Correo {  get; set; }
        public String Nombre { get; set; }
        public String Apellidos { get; set; }
        public String DNI { get; set; }
        public String Telefono { get; set; }
        public String N_Cuenta { get; set; }
        public String Saldo { get; set; }


    }
}
