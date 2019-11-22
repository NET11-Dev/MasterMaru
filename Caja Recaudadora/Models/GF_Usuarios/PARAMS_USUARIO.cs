using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Caja_Recaudadora.Models
{
    public class PARAMS_USUARIO
    {
        public string TIPO { get; set; }
        public string MONTO_APERTURA { get; set; }
        public string NOMBRE { get; set; }
        public string EMAIL { get; set; }
        public string CAJERO_RESP { get; set; }
    }
}