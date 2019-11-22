using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Caja_Recaudadora.Models
{
    public class DET_EFECTIVO
    {
        public string ID_ARQUEO { get; set; }
        public string ID_DENOMINACION { get; set; }
        public string CANTIDAD { get; set; }
        public string MONTO_TOTAL { get; set; }
    }
}