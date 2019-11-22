using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Caja_Recaudadora.Models
{
    public class ST_DET_VIASPAGO
    {
        public string MANDT { get; set; }
        public string ID_COMPROBANTE { get; set; }
        public string ID_DETALLE { get; set; }
        public string VIA_PAGO { get; set; }
        public string MONTO { get; set; }
        public string MONEDA { get; set; }
        public string BANCO { get; set; }
        public string EMISOR { get; set; }
        public string NUM_CHEQUE { get; set; }
        public string COD_AUTORIZACION { get; set; }
        public string NUM_CUOTAS { get; set; }
        public string FECHA_VENC { get; set; }
        public string TEXTO_POSICION { get; set; }
        public string ANEXO { get; set; }
    }
}