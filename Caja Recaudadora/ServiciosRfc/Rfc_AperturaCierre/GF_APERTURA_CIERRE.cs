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
    public class GF_APERTURA_CIERRE
    {

        public string errormessage = "";
        string id_apertura = "";
        public string Mensaje = "";
        public string Sociedad = "";
        public string status = "";
        public string cajeroresp = "";

        ConexionController connectorSap = new ConexionController();

        public List<PARAMS_USUARIO> ObjDatosLogin = new List<PARAMS_USUARIO>();
        public List<ESTADO> Retorno = new List<ESTADO>();
        public List<DEF_CAJA> T_MaestroCajas = new List<DEF_CAJA>();
        public List<DEF_DENOM> T_Denominacion = new List<DEF_DENOM>();
        public List<DET_VIA_PAGO> T_Detviapago = new List<DET_VIA_PAGO>();
        public List<ST_SUBTOTAL_VIAS> T_Subtotalvias = new List<ST_SUBTOTAL_VIAS>();
        public List<ST_SUBTOTAL_ACC> T_Subtotalacc = new List<ST_SUBTOTAL_ACC>();


        public void Apertura(string p_user, string p_pass, string p_idcaja, string p_monto, string p_moneda, string p_tipo_registro)
        {
            ObjDatosLogin.Clear();
            Retorno.Clear();

            try
            {
                IRfcStructure lt_retorno;
                ESTADO retorno_resp;

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

                if (!string.IsNullOrEmpty(p_user) && !string.IsNullOrEmpty(p_pass)) retval = connectorSap.connectionsSAP(user, pass);

                //Si el valor de retorno es nulo o vacio, hay conexion a SAP y la RFC trae datos   
                if (string.IsNullOrEmpty(retval))
                {
                    RfcDestination SapRfcDestination = RfcDestinationManager.GetDestination(connectorSap.connectorConfig);
                    RfcRepository SapRfcRepository = SapRfcDestination.Repository;

                    IRfcFunction BapiGetUser = SapRfcRepository.CreateFunction("ZWD_CAJA02_APERTURA");

                    BapiGetUser.SetValue("USUARIO", p_user);
                    BapiGetUser.SetValue("ID_CAJA", p_idcaja);
                    if (p_monto == "")
                    {
                        p_monto = "0";
                    }
                    BapiGetUser.SetValue("MONTO", p_monto);
                    BapiGetUser.SetValue("MONEDA", p_moneda);
                    BapiGetUser.SetValue("TIPO_REGISTRO", p_tipo_registro);


                    BapiGetUser.Invoke(SapRfcDestination);

                    lt_retorno = BapiGetUser.GetStructure("ESTATUS");

                    if (lt_retorno.Count > 0)
                    {
                        retorno_resp = new ESTADO();
                        for (int i = 0; i < lt_retorno.Count(); i++)
                        {
                            if (i == 0)
                            {
                                status = lt_retorno.GetString("TYPE");
                            }
                            retorno_resp.TYPE = lt_retorno.GetString("TYPE");
                            retorno_resp.ID = lt_retorno.GetString("ID");
                            retorno_resp.NUMBER = lt_retorno.GetString("NUMBER");
                            retorno_resp.MESSAGE = lt_retorno.GetString("MESSAGE");
                            retorno_resp.LOG_NO = lt_retorno.GetString("LOG_NO");
                            retorno_resp.LOG_MSG_NO = lt_retorno.GetString("LOG_MSG_NO");
                            retorno_resp.MESSAGE = lt_retorno.GetString("MESSAGE");
                            retorno_resp.MESSAGE_V1 = lt_retorno.GetString("MESSAGE_V1");
                            if (i == 0)
                            {
                                Mensaje = Mensaje + " - " + lt_retorno.GetString("MESSAGE") + " " + lt_retorno.GetString("MESSAGE_V1");
                                id_apertura = lt_retorno.GetString("MESSAGE_V1");
                            }
                            retorno_resp.MESSAGE_V2 = lt_retorno.GetString("MESSAGE_V2");
                            retorno_resp.MESSAGE_V3 = lt_retorno.GetString("MESSAGE_V3");
                            retorno_resp.MESSAGE_V4 = lt_retorno.GetString("MESSAGE_V4");
                            retorno_resp.PARAMETER = lt_retorno.GetString("PARAMETER");
                            retorno_resp.ROW = lt_retorno.GetString("ROW");
                            retorno_resp.FIELD = lt_retorno.GetString("FIELD");
                            retorno_resp.SYSTEM = lt_retorno.GetString("SYSTEM");
                            Retorno.Add(retorno_resp);
                        }
                    }
                    GC.Collect();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("{0} Exception caught.", ex);
            }
        }

        public void Cierre(string p_user, string p_pass, string p_idcaja, string p_monto_cierre, string p_monto_dif, string p_com_dif)
        {
            ObjDatosLogin.Clear();
            Retorno.Clear();

            try
            {
                IRfcStructure lt_retorno;
                IRfcTable lt_det_efect;
                ESTADO retorno_resp;

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

                    IRfcFunction BapiGetUser = SapRfcRepository.CreateFunction("ZWD_CAJA02_CIERRE");

                    BapiGetUser.SetValue("USUARIO", p_user);
                    BapiGetUser.SetValue("ID_CAJA", p_idcaja);
                    BapiGetUser.SetValue("MONTO_CIERRE", p_monto_cierre);
                    BapiGetUser.SetValue("MONTO_DIF", p_monto_dif);
                    BapiGetUser.SetValue("COMENTARIO_DIF", p_com_dif);

                    BapiGetUser.Invoke(SapRfcDestination);

                    lt_retorno = BapiGetUser.GetStructure("ESTATUS");

                    if (lt_retorno.Count > 0)
                    {
                        retorno_resp = new ESTADO();
                        for (int i = 0; i < lt_retorno.Count(); i++)
                        {
                            if (i == 0)
                            {
                                status = lt_retorno.GetString("TYPE");
                            }
                            retorno_resp.TYPE = lt_retorno.GetString("TYPE");
                            retorno_resp.ID = lt_retorno.GetString("ID");
                            retorno_resp.NUMBER = lt_retorno.GetString("NUMBER");
                            retorno_resp.MESSAGE = lt_retorno.GetString("MESSAGE");
                            retorno_resp.LOG_NO = lt_retorno.GetString("LOG_NO");
                            retorno_resp.LOG_MSG_NO = lt_retorno.GetString("LOG_MSG_NO");
                            retorno_resp.MESSAGE = lt_retorno.GetString("MESSAGE");
                            retorno_resp.MESSAGE_V1 = lt_retorno.GetString("MESSAGE_V1");
                            if (i == 0)
                            {
                                Mensaje = Mensaje + " - " + lt_retorno.GetString("MESSAGE") + " " + lt_retorno.GetString("MESSAGE_V1");
                                id_apertura = lt_retorno.GetString("MESSAGE_V1");
                            }
                            retorno_resp.MESSAGE_V2 = lt_retorno.GetString("MESSAGE_V2");
                            retorno_resp.MESSAGE_V3 = lt_retorno.GetString("MESSAGE_V3");
                            retorno_resp.MESSAGE_V4 = lt_retorno.GetString("MESSAGE_V4");
                            retorno_resp.PARAMETER = lt_retorno.GetString("PARAMETER");
                            retorno_resp.ROW = lt_retorno.GetString("ROW");
                            retorno_resp.FIELD = lt_retorno.GetString("FIELD");
                            retorno_resp.SYSTEM = lt_retorno.GetString("SYSTEM");
                            Retorno.Add(retorno_resp);
                        }
                    }
                    GC.Collect();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("{0} Exception caught.", ex);
            }
        }

        public void maestrocajas(string p_user, string p_pass)
        {
            try
            {
                T_MaestroCajas.Clear();
                IRfcTable lt_cajas;
                DEF_CAJA st_def_caja;

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

                    IRfcFunction BapiGetUser = SapRfcRepository.CreateFunction("ZWD_CAJA02_DEF_CAJA");

                    BapiGetUser.Invoke(SapRfcDestination);

                    lt_cajas = BapiGetUser.GetTable("T_CAJAS");

                    for (int i = 0; i < lt_cajas.Count(); i++)
                    {
                        lt_cajas.CurrentIndex = i;
                        st_def_caja = new DEF_CAJA();
                        st_def_caja.ID_CAJA = lt_cajas.GetString("ID_CAJA");
                        st_def_caja.NOM_CAJA = lt_cajas.GetString("NOM_CAJA");
                        st_def_caja.IMPRESORA = lt_cajas.GetString("IMPRESORA");
                        T_MaestroCajas.Add(st_def_caja);
                    }
                }
            }

            catch (InvalidCastException ex)
            {
                Console.WriteLine("{0} Exception caught.", ex);
            }
        }

        public void obtiene_denominacion(string p_user, string p_pass)
        {
            try
            {
                T_Denominacion.Clear();
                IRfcTable lt_denominacion;
                DEF_DENOM st_def_denom;


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

                    IRfcFunction BapiGetUser = SapRfcRepository.CreateFunction("ZWD_CAJA02_GET_DENOM");

                    BapiGetUser.Invoke(SapRfcDestination);

                    lt_denominacion = BapiGetUser.GetTable("DENOM");

                    for (int i = 0; i < lt_denominacion.Count(); i++)
                    {
                        lt_denominacion.CurrentIndex = i;
                        st_def_denom = new DEF_DENOM();
                        st_def_denom.ID_DENOMINACION = lt_denominacion.GetString("ID_DENOMINACION");
                        st_def_denom.TEXTO = lt_denominacion.GetString("TEXTO");
                        st_def_denom.VALOR = lt_denominacion.GetString("VALOR");
                        T_Denominacion.Add(st_def_denom);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("{0} Exception caught.", e);
            }
        }

        public void DetallePagosCierre(string p_user, string p_pass , string p_idcaja, string p_viapago)
        {
            IRfcTable lt_detallePagoscierre;
            DET_VIA_PAGO st_detviapago;

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

                    IRfcFunction BapiGetUser = SapRfcRepository.CreateFunction("ZWD_CAJA02_GET_DET_PAGOS_CIE");

                    BapiGetUser.SetValue("ID_CAJA", p_idcaja);
                    BapiGetUser.SetValue("VIA_PAGO", p_viapago);

                    BapiGetUser.Invoke(SapRfcDestination);

                    lt_detallePagoscierre = BapiGetUser.GetTable("DETALLE");

                    for (int i = 0; i < lt_detallePagoscierre.Count(); i++)
                    {

                        lt_detallePagoscierre.CurrentIndex = i;
                        st_detviapago = new DET_VIA_PAGO();

                        st_detviapago.SOCIEDAD = lt_detallePagoscierre[i].GetString("SOCIEDAD");
                        st_detviapago.ID_COMPROBANTE = lt_detallePagoscierre[i].GetString("ID_COMPROBANTE");
                        st_detviapago.VIA_PAGO = lt_detallePagoscierre[i].GetString("VIA_PAGO");
                        st_detviapago.MONTO = lt_detallePagoscierre[i].GetString("MONTO");
                        st_detviapago.MONEDA = lt_detallePagoscierre[i].GetString("MONEDA");
                        st_detviapago.BANCO = lt_detallePagoscierre[i].GetString("BANCO");
                        st_detviapago.EMISOR = lt_detallePagoscierre[i].GetString("EMISOR");
                        st_detviapago.NUM_CHEQUE = lt_detallePagoscierre[i].GetString("NUM_CHEQUE");
                        st_detviapago.COD_AUTORIZACION = lt_detallePagoscierre[i].GetString("COD_AUTORIZACION");
                        st_detviapago.NUM_CUOTAS = lt_detallePagoscierre[i].GetString("NUM_CUOTAS");
                        st_detviapago.FECHA_VENC = lt_detallePagoscierre[i].GetString("FECHA_VENC");
                        st_detviapago.TEXTO_POSICION = lt_detallePagoscierre[i].GetString("TEXTO_POSICION");
                        st_detviapago.ANEXO = lt_detallePagoscierre[i].GetString("ANEXO");
                        T_Detviapago.Add(st_detviapago);
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
                lt_detallePagoscierre = null;
                st_detviapago = null;
            }
        }

        public void DetallePreCierre(string p_user, string p_pass, string p_idcaja)
        {
            IRfcTable lt_subtotal_vias;
            IRfcTable lt_subtotal_acc;
            ST_SUBTOTAL_VIAS st_sub_vias;
            ST_SUBTOTAL_ACC st_sub_acc;

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

                    IRfcFunction BapiGetUser = SapRfcRepository.CreateFunction("ZWD_CAJA02_GET_PRE_CIERRE");

                    BapiGetUser.SetValue("ID_CAJA", p_idcaja);

                    BapiGetUser.Invoke(SapRfcDestination);

                    lt_subtotal_vias = BapiGetUser.GetTable("SUBTOTAL_VIAS");

                    for (int i = 0; i < lt_subtotal_vias.Count(); i++)
                    {

                        lt_subtotal_vias.CurrentIndex = i;
                        st_sub_vias = new ST_SUBTOTAL_VIAS();

                        st_sub_vias.VIA_PAGO = lt_subtotal_vias[i].GetString("VIA_PAGO");
                        st_sub_vias.DESCRIPCION = lt_subtotal_vias[i].GetString("DESCRIPCION");
                        st_sub_vias.TOTAL = lt_subtotal_vias[i].GetString("TOTAL");
                        T_Subtotalvias.Add(st_sub_vias);
                    }

                    lt_subtotal_acc = BapiGetUser.GetTable("SUBTOTAL_ACC");

                    for (int i = 0; i < lt_subtotal_acc.Count(); i++)
                    {

                        lt_subtotal_acc.CurrentIndex = i;
                        st_sub_acc = new ST_SUBTOTAL_ACC();
                        st_sub_acc.ACC = lt_subtotal_acc[i].GetString("ACC");
                        st_sub_acc.DESCRIPCION = lt_subtotal_acc[i].GetString("DESCRIPCION");
                        st_sub_acc.TOTAL = lt_subtotal_acc[i].GetString("TOTAL");
   
                        T_Subtotalacc.Add(st_sub_acc);
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
                lt_subtotal_vias = null;
                lt_subtotal_acc = null;
                st_sub_vias = null;
                st_sub_acc = null;
            }
        }

    }
}