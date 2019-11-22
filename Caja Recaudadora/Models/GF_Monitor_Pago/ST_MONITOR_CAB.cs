using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Caja_Recaudadora.Models
{
    public class ST_MONITOR_CAB
    {
        public string MANDT { get; set; }
        public string ID_OPERACION { get; set; }
        public string SYS { get; set; }
        public string CENTRO { get; set; }
        public string ESTATUS { get; set; }
        public string TIPO_DOCUMENTO { get; set; }
        public string CEBE { get; set; }
        public string COND_PAGO { get; set; }
        public string COD_CLIENTE { get; set; }
        public string RUT { get; set; }
        public string NOMBRE { get; set; }
        public string CONTROL_CRED { get; set; }
        public string FECHA_DOC { get; set; }
        public string FECHA_VENC_DOC { get; set; }
        public string MONEDA { get; set; }
        public string NRO_REFERENCIA { get; set; }
        public string NRO_DOCUMENTO { get; set; }
        public string SOCIEDAD { get; set; }
        public string TEXTO_CABECERA { get; set; }
        public string EXCEPCION { get; set; }
        public string CREDITO { get; set; }
        public string NUM_CUOTA { get; set; }
        public string MONTO_DOC { get; set; }
        public string MONTO_CAPITAL { get; set; }
        public string MONTO_INTERES { get; set; }
        public string MONTO_MORA { get; set; }
        public string MONTO_COB { get; set; }
        public string FECHA_REG { get; set; }
        public string HORA_REG { get; set; }
        public string CORRELATIVO { get; set; }
        public string FOLIO { get; set; }
        public string EST_PAGO_CUOTA { get; set; }
    }

}