using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Caja_Recaudadora.Models

{
    public class ST_DOCUMENTOS
    {
        public string NDOCTO { get; set; }
        public string MONTO { get; set; }
        public string MONEDA { get; set; }
        public string FECVENCI { get; set; }
        public string CONTROL_CREDITO { get; set; }
        public string CEBE { get; set; }
        public string COND_PAGO { get; set; }
        public string RUTCLI { get; set; }
        public string NOMCLI { get; set; }
        public string ESTADO { get; set; }
        public string ICONO { get; set; }
        public string DIAS_ATRASO { get; set; }
        public string MONTO_ABONADO { get; set; }
        public string MONTO_PAGAR { get; set; }
        public string NREF { get; set; }
        public string FECHA_DOC { get; set; }
        public string COD_CLIENTE { get; set; }
    }
}