using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Caja_Recaudadora.Models
{
    public class ST_MONITOR_VIEW
    {
        public string ID_OPERACION { get; set; }
        public string SYS { get; set; }
        public string ESTATUS { get; set; }
        public string TIPO_DOCUMENTO { get; set; }
        public string COD_CLIENTE { get; set; }
        public string RUT { get; set; }
        public string NOMBRE { get; set; }
        public string CONTROL_CRED { get; set; }
        public string FECHA_DOC { get; set; }
        public string FECHA_VENC_DOC { get; set; }
        public string MONEDA { get; set; }
        public string NRO_REFERENCIA { get; set; }
        public string NRO_DOCUMENTO { get; set; }
        public string MONTO_DOC { get; set; }
    }
}