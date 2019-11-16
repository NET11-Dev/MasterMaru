using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SAP.Middleware.Connector;
using Caja_Recaudadora.Controllers;
using Caja_Recaudadora.Models;
using System.Configuration;

namespace Caja_Recaudadora.ServiciosRfc
{
    public class GF_MONITORPAGO
    {
        public string pagomessage = "";
        public string status = "";
        public string comprobante = "";
        public string message = "";
        public string errormessage = "";
        string id_apertura = "";
        public string Mensaje = "";
        public string Sociedad = "";
        public string cajeroresp = "";

        ConexionController connectorSap = new ConexionController();
        public List<ST_MONITOR_VIEW> T_MonitorBuscadocumentos = new List<ST_MONITOR_VIEW>();
        public List<ST_MONITOR_CAB> T_Cabdocumentos = new List<ST_MONITOR_CAB>();
        public List<ST_MONITOR_DET> T_viapagodocu = new List<ST_MONITOR_DET>();

        public void BuscadocMonitor(string p_user, string p_pass, string p_ID_CAJA)
        {
            T_MonitorBuscadocumentos.Clear();
            IRfcTable lt_buscadocumentos;
            ST_MONITOR_VIEW st_buscadocumentos;

            try
            {
                //Conexion a SAP
                string mandante = ConfigurationManager.AppSettings["mandante"];
                string servidor = ConfigurationManager.AppSettings["servidor"];
                string saprouter = ConfigurationManager.AppSettings["saprouter"];
                string num_sist = ConfigurationManager.AppSettings["num_sist"];
                string sysid = ConfigurationManager.AppSettings["sysid"];
                string lenguaje = ConfigurationManager.AppSettings["lenguaje"];

                //Conexion a SAP
                connectorSap.idioma = "ES";
                connectorSap.idSistema = sysid;
                connectorSap.instancia = num_sist;
                connectorSap.mandante = mandante;
                connectorSap.paswr = p_pass;
                connectorSap.sapRouter = saprouter;
                connectorSap.user = p_user;
                connectorSap.server = servidor;

                string retval = "ERROR";

                if (!string.IsNullOrEmpty(p_user) && !string.IsNullOrEmpty(p_pass)) retval = connectorSap.connectionsSAP(p_user, p_pass);
                //Si el valor de retorno es nulo o vacio, hay conexion a SAP y la RFC trae datos   
                if (string.IsNullOrEmpty(retval))
                {
                    RfcDestination SapRfcDestination = RfcDestinationManager.GetDestination(connectorSap.connectorConfig);
                    RfcRepository SapRfcRepository = SapRfcDestination.Repository;

                    IRfcFunction BapiGetUser = SapRfcRepository.CreateFunction("ZWD_CAJA06_GET_DOCS_MONITOR");
                    
                    BapiGetUser.SetValue("ID_CAJA", p_ID_CAJA);

                    BapiGetUser.Invoke(SapRfcDestination);

                    lt_buscadocumentos = BapiGetUser.GetTable("DOCUMENTOS");

                    for (int i = 0; i < lt_buscadocumentos.Count(); i++)
                    {
                        lt_buscadocumentos.CurrentIndex = i;
                        st_buscadocumentos = new ST_MONITOR_VIEW();

                        st_buscadocumentos.ID_OPERACION = lt_buscadocumentos.GetString("ID_OPERACION");
                        st_buscadocumentos.SYS = lt_buscadocumentos.GetString("SYS");
                        st_buscadocumentos.ESTATUS = lt_buscadocumentos.GetString("ESTATUS");
                        st_buscadocumentos.TIPO_DOCUMENTO = lt_buscadocumentos.GetString("TIPO_DOCUMENTO");
                        st_buscadocumentos.COD_CLIENTE = lt_buscadocumentos.GetString("COD_CLIENTE");
                        st_buscadocumentos.RUT = lt_buscadocumentos.GetString("RUT");
                        st_buscadocumentos.NOMBRE = lt_buscadocumentos.GetString("NOMBRE");
                        st_buscadocumentos.CONTROL_CRED = lt_buscadocumentos.GetString("CONTROL_CRED");
                        st_buscadocumentos.FECHA_DOC = lt_buscadocumentos.GetString("FECHA_DOC");
                        st_buscadocumentos.FECHA_VENC_DOC = lt_buscadocumentos.GetString("FECHA_VENC_DOC");
                        st_buscadocumentos.MONEDA = lt_buscadocumentos.GetString("MONEDA");
                        st_buscadocumentos.NRO_REFERENCIA = lt_buscadocumentos.GetString("NRO_REFERENCIA");
                        st_buscadocumentos.NRO_DOCUMENTO = lt_buscadocumentos.GetString("NRO_DOCUMENTO");
                        st_buscadocumentos.MONTO_DOC = lt_buscadocumentos.GetString("MONTO_DOC");
                        T_MonitorBuscadocumentos.Add(st_buscadocumentos);
                    }
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.Write(e.StackTrace);
                throw new Exception();
            }
            finally
            {
                lt_buscadocumentos = null;
                st_buscadocumentos = null;
            }
        }

        public void BuscadocMonitor2(string p_user, string p_pass)
        {
            T_MonitorBuscadocumentos.Clear();
            IRfcTable lt_cabdocum;
            IRfcTable lt_viapagos;
            ST_MONITOR_CAB st_cabdocument;
            ST_MONITOR_DET st_viapagodoc;

            try
            {
                //Conexion a SAP
                string mandante = ConfigurationManager.AppSettings["mandante"];
                string servidor = ConfigurationManager.AppSettings["servidor"];
                string saprouter = ConfigurationManager.AppSettings["saprouter"];
                string num_sist = ConfigurationManager.AppSettings["num_sist"];
                string sysid = ConfigurationManager.AppSettings["sysid"];
                string lenguaje = ConfigurationManager.AppSettings["lenguaje"];

                //Conexion a SAP
                connectorSap.idioma = "ES";
                connectorSap.idSistema = sysid;
                connectorSap.instancia = num_sist;
                connectorSap.mandante = mandante;
                connectorSap.paswr = p_pass;
                connectorSap.sapRouter = saprouter;
                connectorSap.user = p_user;
                connectorSap.server = servidor;

                string retval = "ERROR";

                if (!string.IsNullOrEmpty(p_user) && !string.IsNullOrEmpty(p_pass)) retval = connectorSap.connectionsSAP(p_user, p_pass);
                //Si el valor de retorno es nulo o vacio, hay conexion a SAP y la RFC trae datos   
                if (string.IsNullOrEmpty(retval))
                {
                    RfcDestination SapRfcDestination = RfcDestinationManager.GetDestination(connectorSap.connectorConfig);
                    RfcRepository SapRfcRepository = SapRfcDestination.Repository;

                    IRfcFunction BapiGetUser = SapRfcRepository.CreateFunction("ZWD_CAJA06_GET_DOCUMENTO");

                    BapiGetUser.Invoke(SapRfcDestination);

                    lt_cabdocum = BapiGetUser.GetTable("DOCUMENTOS");

                    for (int i = 0; i < lt_cabdocum.Count(); i++)
                    {
                        lt_cabdocum.CurrentIndex = i;
                        st_cabdocument = new ST_MONITOR_CAB();

                        st_cabdocument.MANDT = lt_cabdocum.GetString("ID_OPERACION");
                        st_cabdocument.ID_OPERACION = lt_cabdocum.GetString("SYS");
                        st_cabdocument.SYS = lt_cabdocum.GetString("ESTATUS");
                        st_cabdocument.CENTRO = lt_cabdocum.GetString("TIPO_DOCUMENTO");
                        st_cabdocument.ESTATUS = lt_cabdocum.GetString("COD_CLIENTE");
                        st_cabdocument.TIPO_DOCUMENTO = lt_cabdocum.GetString("RUT");
                        st_cabdocument.CEBE = lt_cabdocum.GetString("NOMBRE");
                        st_cabdocument.COND_PAGO = lt_cabdocum.GetString("CONTROL_CRED");
                        st_cabdocument.COD_CLIENTE = lt_cabdocum.GetString("FECHA_DOC");
                        st_cabdocument.RUT = lt_cabdocum.GetString("FECHA_VENC_DOC");
                        st_cabdocument.NOMBRE = lt_cabdocum.GetString("MONEDA");
                        st_cabdocument.CONTROL_CRED = lt_cabdocum.GetString("NRO_REFERENCIA");
                        st_cabdocument.FECHA_DOC = lt_cabdocum.GetString("NRO_DOCUMENTO");
                        st_cabdocument.FECHA_VENC_DOC = lt_cabdocum.GetString("MONTO_DOC");
                        st_cabdocument.MONEDA = lt_cabdocum.GetString("MONTO_DOC");
                        st_cabdocument.NRO_REFERENCIA = lt_cabdocum.GetString("MONTO_DOC");
                        st_cabdocument.NRO_DOCUMENTO = lt_cabdocum.GetString("MONTO_DOC");
                        st_cabdocument.SOCIEDAD = lt_cabdocum.GetString("MONTO_DOC");
                        st_cabdocument.TEXTO_CABECERA = lt_cabdocum.GetString("MONTO_DOC");
                        st_cabdocument.EXCEPCION = lt_cabdocum.GetString("MONTO_DOC");
                        st_cabdocument.CREDITO = lt_cabdocum.GetString("MONTO_DOC");
                        st_cabdocument.NUM_CUOTA = lt_cabdocum.GetString("MONTO_DOC");
                        st_cabdocument.MONTO_DOC = lt_cabdocum.GetString("MONTO_DOC");
                        st_cabdocument.MONTO_CAPITAL = lt_cabdocum.GetString("MONTO_DOC");
                        st_cabdocument.MONTO_INTERES = lt_cabdocum.GetString("MONTO_DOC");
                        st_cabdocument.MONTO_MORA = lt_cabdocum.GetString("MONTO_DOC");
                        st_cabdocument.MONTO_COB = lt_cabdocum.GetString("MONTO_DOC");
                        st_cabdocument.FECHA_REG = lt_cabdocum.GetString("MONTO_DOC");
                        st_cabdocument.HORA_REG = lt_cabdocum.GetString("MONTO_DOC");
                        st_cabdocument.CORRELATIVO = lt_cabdocum.GetString("MONTO_DOC");
                        st_cabdocument.FOLIO = lt_cabdocum.GetString("MONTO_DOC");
                        st_cabdocument.EST_PAGO_CUOTA = lt_cabdocum.GetString("MONTO_DOC");
                        T_Cabdocumentos.Add(st_cabdocument);
                    }


                    lt_viapagos = BapiGetUser.GetTable("VIAS_PAGO");

                    for (int i = 0; i < lt_viapagos.Count(); i++)
                    {
                        lt_viapagos.CurrentIndex = i;
                        st_viapagodoc = new ST_MONITOR_DET();

                        st_viapagodoc.MANDT = lt_cabdocum.GetString("MANDT");
                        st_viapagodoc.ID_OPERACION = lt_cabdocum.GetString("ID_OPERACION");
                        st_viapagodoc.ID_DETALLE = lt_cabdocum.GetString("ID_DETALLE");
                        st_viapagodoc.VIA_PAGO = lt_cabdocum.GetString("VIA_PAGO");
                        st_viapagodoc.MONTO = lt_cabdocum.GetString("MONTO");
                        st_viapagodoc.BANCO = lt_cabdocum.GetString("BANCO");
                        st_viapagodoc.EMISOR = lt_cabdocum.GetString("EMISOR");
                        st_viapagodoc.NUM_CHEQUE = lt_cabdocum.GetString("NUM_CHEQUE");
                        st_viapagodoc.COD_AUTORIZACION = lt_cabdocum.GetString("COD_AUTORIZACION");
                        st_viapagodoc.NUM_CUOTAS = lt_cabdocum.GetString("NUM_CUOTAS");
                        st_viapagodoc.FECHA_VENC = lt_cabdocum.GetString("FECHA_VENC");
                        st_viapagodoc.TEXTO_POSICION = lt_cabdocum.GetString("TEXTO_POSICION");
                        st_viapagodoc.ANEXO = lt_cabdocum.GetString("ANEXO");
                        T_viapagodocu.Add(st_viapagodoc);
                    }
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.Write(e.StackTrace);
                throw new Exception();
            }
            finally
            {
                lt_cabdocum = null;
                st_cabdocument = null;
                lt_viapagos = null;
                st_viapagodoc = null;
            }
        }

    }
}