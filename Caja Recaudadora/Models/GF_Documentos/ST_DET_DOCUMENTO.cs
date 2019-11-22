using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Caja_Recaudadora.Models
{
    public class ST_DET_DOCUMENTO
    {
        public string MANDT { get; set; }
        public string ID_COMPROBANTE { get; set; }
        public string POSICION { get; set; }
        public string CLIENTE { get; set; }
        public string TIPO_DOCUMENTO { get; set; }
        public string SOCIEDAD { get; set; }
        public string NRO_DOCUMENTO { get; set; }
        public string NRO_REFERENCIA { get; set; }
        public string ACC { get; set; }
        public string CAJERO_RESP { get; set; }
        public string CAJERO_GEN { get; set; }
        public string ID_CAJA { get; set; }
        public string FECHA_COMP { get; set; }
        public string HORA { get; set; }
        public string NRO_COMPENSACION { get; set; }
        public string TEXTO_CABECERA { get; set; }
        public string NULO { get; set; }
        public string NRO_ANULACION { get; set; }
        public string EXCEPCION { get; set; }
        public string FECHA_DOC { get; set; }
        public string FECHA_VENC_DOC { get; set; }
        public string CREDITO { get; set; }
        public string NUM_CUOTA { get; set; }
        public string MONTO_DOC { get; set; }
        public string MONTO_CAPITAL { get; set; }
        public string MONTO_INTERES { get; set; }
        public string MONTO_MORA { get; set; }
        public string MONTO_COB { get; set; }
        public string TEXTO_EXCEPCION { get; set; }
        public string PARCIAL { get; set; }
        public string TIME { get; set; }
        public string CORRELATIVO { get; set; }
        public string FOLIO { get; set; }
        public string EST_PAGO_CUOTA { get; set; }
        public string NUM_BOLETA { get; set; }
    }
}