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
    public class GF_DOCUMENTOS
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

        public List<ST_BANCOS> T_Maestrobancos = new List<ST_BANCOS>();
        public List<ST_SOCIEDADES> T_Maestrosociedades = new List<ST_SOCIEDADES>();
        public List<ST_VIAS_PAGO> T_Maestroviaspago = new List<ST_VIAS_PAGO>();
        public List<ST_VIAS_ACC> T_Maestroviasacc = new List<ST_VIAS_ACC>();
        public List<ST_CLIENTES> T_Maestroclientes = new List<ST_CLIENTES>();
        public List<ST_DOCUMENTOS> T_MaestroBuscadocumentos = new List<ST_DOCUMENTOS>();
        public List<ST_RETORNO> t_retorno = new List<ST_RETORNO>();

        public void maestrobancos(string p_user, string p_pass)
        {
            T_Maestrobancos.Clear();
            IRfcTable lt_BANCOS;
            ST_BANCOS st_maestrobancos;

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

                    IRfcFunction BapiGetUser = SapRfcRepository.CreateFunction("ZWD_CAJA03_MC_BANCOS");

                    BapiGetUser.Invoke(SapRfcDestination);

                    lt_BANCOS = BapiGetUser.GetTable("BANCOS");

                    for (int i = 0; i < lt_BANCOS.Count(); i++)
                    {
                        lt_BANCOS.CurrentIndex = i;
                        st_maestrobancos = new ST_BANCOS();

                        st_maestrobancos.BANCO = lt_BANCOS.GetString("BANCO");
                        st_maestrobancos.DESCRIPCION = lt_BANCOS.GetString("DESCRIPCION");
                        T_Maestrobancos.Add(st_maestrobancos);
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
                lt_BANCOS = null;
                st_maestrobancos = null;
            }
        }

        public void maestrosociedades(string p_user, string p_pass)
        {
            T_Maestrosociedades.Clear();
            IRfcTable lt_sociedades;
            ST_SOCIEDADES st_maestrosociedades;

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

                    IRfcFunction BapiGetUser = SapRfcRepository.CreateFunction("ZWD_CAJA03_MC_SOCIEDADES");

                    BapiGetUser.Invoke(SapRfcDestination);

                    lt_sociedades = BapiGetUser.GetTable("SOCIEDADES");

                    for (int i = 0; i < lt_sociedades.Count(); i++)
                    {
                        lt_sociedades.CurrentIndex = i;
                        st_maestrosociedades = new ST_SOCIEDADES();

                        st_maestrosociedades.BUKRS = lt_sociedades.GetString("BUKRS");
                        st_maestrosociedades.DESCRIPCION = lt_sociedades.GetString("DESCRIPCION");
                        T_Maestrosociedades.Add(st_maestrosociedades);
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
                lt_sociedades = null;
                st_maestrosociedades = null;
            }
        }

        public void maestroviapago(string p_user, string p_pass, string p_excepcion)
        {
            T_Maestroviaspago.Clear();
            T_Maestroviasacc.Clear();

            IRfcTable lt_maestroviapago;
            IRfcTable lt_maestroviaacc;

            ST_VIAS_PAGO st_maestroviapago;
            ST_VIAS_ACC st_maestroviaacc;

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

                    IRfcFunction BapiGetUser = SapRfcRepository.CreateFunction("ZWD_CAJA03_MC_VIAS_PAGO");

                    BapiGetUser.SetValue("EXCEPCION", p_excepcion);

                    BapiGetUser.Invoke(SapRfcDestination);

                    lt_maestroviapago = BapiGetUser.GetTable("SOCIEDADES");

                    for (int i = 0; i < lt_maestroviapago.Count(); i++)
                    {
                        lt_maestroviapago.CurrentIndex = i;
                        st_maestroviapago = new ST_VIAS_PAGO();

                        st_maestroviapago.VIA_PAGO = lt_maestroviapago.GetString("VIA_PAGO");
                        st_maestroviapago.DESCRIPCION = lt_maestroviapago.GetString("DESCRIPCION");
                        T_Maestroviaspago.Add(st_maestroviapago);
                    }

                    lt_maestroviaacc = BapiGetUser.GetTable("CONDICIONES");

                    for (int i = 0; i < lt_maestroviaacc.Count(); i++)
                    {
                        lt_maestroviaacc.CurrentIndex = i;
                        st_maestroviaacc = new ST_VIAS_ACC();

                        st_maestroviaacc.ACC = lt_maestroviapago.GetString("ACC");
                        st_maestroviaacc.CAJA = lt_maestroviapago.GetString("CAJA");
                        st_maestroviaacc.COND_PAGO = lt_maestroviapago.GetString("COND_PAGO");
                        T_Maestroviasacc.Add(st_maestroviaacc);
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
                lt_maestroviapago = null;
                lt_maestroviaacc = null;
                st_maestroviapago = null;
                st_maestroviaacc = null;
            }
        }

        public void Buscaclientes(string p_user, string p_pass, string p_rut, string p_nombre)
        {
            T_Maestroclientes.Clear();
            IRfcTable lt_buscaclientes;
            ST_CLIENTES st_buscaclientes;

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

                    IRfcFunction BapiGetUser = SapRfcRepository.CreateFunction("ZWD_CAJA03_SEARCH_CUST");


                    BapiGetUser.SetValue("RUT", p_rut);
                    BapiGetUser.SetValue("NOMBRE", p_nombre);

                    BapiGetUser.Invoke(SapRfcDestination);

                    lt_buscaclientes = BapiGetUser.GetTable("CLIENTES");

                    for (int i = 0; i < lt_buscaclientes.Count(); i++)
                    {
                        lt_buscaclientes.CurrentIndex = i;
                        st_buscaclientes = new ST_CLIENTES();

                        st_buscaclientes.NAME1 = lt_buscaclientes.GetString("NAME1");
                        st_buscaclientes.ORT01 = lt_buscaclientes.GetString("ORT01");
                        st_buscaclientes.STRAS = lt_buscaclientes.GetString("STRAS");
                        st_buscaclientes.TELF1 = lt_buscaclientes.GetString("TELF1");
                        st_buscaclientes.STCD1 = lt_buscaclientes.GetString("STCD1");
                        st_buscaclientes.KUNNR = lt_buscaclientes.GetString("KUNNR");
                        T_Maestroclientes.Add(st_buscaclientes);
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
                lt_buscaclientes = null;
                st_buscaclientes = null;
            }
        }
        public void Buscadocumentos(string p_user, string p_pass, string p_codcliente, string p_documento, string p_rut, string p_sociedad)
        {
            T_MaestroBuscadocumentos.Clear();
            IRfcTable lt_buscadocumentos;
            ST_DOCUMENTOS st_buscadocumentos;

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

                    IRfcFunction BapiGetUser = SapRfcRepository.CreateFunction("ZWD_CAJA03_SEARCH_CUST");


                    BapiGetUser.SetValue("CODCLIENTE", p_codcliente);
                    BapiGetUser.SetValue("DOCUMENTO", p_documento);
                    BapiGetUser.SetValue("RUT", p_rut);
                    BapiGetUser.SetValue("SOCIEDAD", p_sociedad);

                    BapiGetUser.Invoke(SapRfcDestination);

                    lt_buscadocumentos = BapiGetUser.GetTable("CLIENTES");

                    for (int i = 0; i < lt_buscadocumentos.Count(); i++)
                    {
                        lt_buscadocumentos.CurrentIndex = i;
                        st_buscadocumentos = new ST_DOCUMENTOS();

                        st_buscadocumentos.NDOCTO = lt_buscadocumentos.GetString("NDOCTO");
                        st_buscadocumentos.MONTO = lt_buscadocumentos.GetString("MONTO");
                        st_buscadocumentos.MONEDA = lt_buscadocumentos.GetString("MONEDA");
                        st_buscadocumentos.FECVENCI = lt_buscadocumentos.GetString("FECVENCI");
                        st_buscadocumentos.CONTROL_CREDITO = lt_buscadocumentos.GetString("CONTROL_CREDITO");
                        st_buscadocumentos.CEBE = lt_buscadocumentos.GetString("CEBE");
                        st_buscadocumentos.COND_PAGO = lt_buscadocumentos.GetString("COND_PAGO");
                        st_buscadocumentos.RUTCLI = lt_buscadocumentos.GetString("RUTCLI");
                        st_buscadocumentos.NOMCLI = lt_buscadocumentos.GetString("NOMCLI");
                        st_buscadocumentos.ESTADO = lt_buscadocumentos.GetString("ESTADO");
                        st_buscadocumentos.ICONO = lt_buscadocumentos.GetString("ICONO");
                        st_buscadocumentos.DIAS_ATRASO = lt_buscadocumentos.GetString("DIAS_ATRASO");
                        st_buscadocumentos.MONTO_ABONADO = lt_buscadocumentos.GetString("MONTO_ABONADO");
                        st_buscadocumentos.MONTO_PAGAR = lt_buscadocumentos.GetString("MONTO_PAGAR");
                        st_buscadocumentos.NREF = lt_buscadocumentos.GetString("NREF");
                        st_buscadocumentos.FECHA_DOC = lt_buscadocumentos.GetString("FECHA_DOC");
                        st_buscadocumentos.COD_CLIENTE = lt_buscadocumentos.GetString("COD_CLIENTE");
                        T_MaestroBuscadocumentos.Add(st_buscadocumentos);
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

        public void pagodocumentos(string p_user, string p_pass, string p_total_facturas, string p_total_vias, string p_diferencia, string p_parcial, string p_excepcion, string p_txt_parcial, string p_txt_excepcion,
                                   List<ST_DET_VIASPAGO> P_VIASPAGO, List<ST_DET_DOCUMENTO> P_DOCSAPAGAR, List<ST_DET_ANEXOS> P_ANEXOS)
        {
            IRfcTable lt_PAGO_DOCS;
            IRfcTable lt_PAGO_MESS;
            IRfcTable lt_retorno;

            ST_RETORNO retorno;
            //T_Retorno.Clear();

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

                    IRfcFunction BapiGetUser = SapRfcRepository.CreateFunction("ZWD_CAJA03_PAGAR_DOCS");

                    BapiGetUser.SetValue("TOTAL_FACTURAS", p_total_facturas);
                    BapiGetUser.SetValue("TOTAL_VIAS", p_total_vias);
                    BapiGetUser.SetValue("DIFERENCIA", p_diferencia);
                    BapiGetUser.SetValue("PARCIAL", p_parcial);
                    BapiGetUser.SetValue("EXCEPCION", p_excepcion);
                    BapiGetUser.SetValue("TXT_PARCIAL", p_txt_parcial);
                    BapiGetUser.SetValue("TXT_EXCEPCION", p_txt_excepcion);
                    try
                    {

                        IRfcTable st_viapagos = BapiGetUser.GetTable("VIAS_PAGO");
                        try
                        {
                            for (var i = 0; i < P_VIASPAGO.Count; i++)
                            {
                                st_viapagos.Append();
                                st_viapagos.SetValue("MANDT", P_VIASPAGO[i].MANDT);
                                st_viapagos.SetValue("ID_COMPROBANTE", P_VIASPAGO[i].ID_COMPROBANTE);
                                st_viapagos.SetValue("ID_DETALLE", P_VIASPAGO[i].ID_DETALLE);
                                st_viapagos.SetValue("VIA_PAGO", P_VIASPAGO[i].VIA_PAGO);
                                st_viapagos.SetValue("MONTO", P_VIASPAGO[i].MONTO);
                                st_viapagos.SetValue("MONEDA", P_VIASPAGO[i].MONEDA);
                                st_viapagos.SetValue("BANCO", P_VIASPAGO[i].BANCO);
                                st_viapagos.SetValue("EMISOR", P_VIASPAGO[i].EMISOR);
                                st_viapagos.SetValue("NUM_CHEQUE", P_VIASPAGO[i].NUM_CHEQUE);
                                st_viapagos.SetValue("COD_AUTORIZACION", P_VIASPAGO[i].COD_AUTORIZACION);
                                st_viapagos.SetValue("NUM_CUOTAS", P_VIASPAGO[i].NUM_CUOTAS);
                                st_viapagos.SetValue("FECHA_VENC", P_VIASPAGO[i].FECHA_VENC);
                                st_viapagos.SetValue("TEXTO_POSICION", P_VIASPAGO[i].TEXTO_POSICION);
                                st_viapagos.SetValue("ANEXO", P_VIASPAGO[i].ANEXO);
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message + ex.StackTrace);
                        }
                        BapiGetUser.SetValue("VIAS_PAGO", st_viapagos);

                        IRfcTable st_documento = BapiGetUser.GetTable("DOCUMENTOS");
                        try
                        {
                            for (var i = 0; i < P_DOCSAPAGAR.Count; i++)
                            {
                                st_documento.Append();
                                st_documento.SetValue("MANDT", "");
                                st_documento.SetValue("ID_COMPROBANTE", P_DOCSAPAGAR[i].ID_COMPROBANTE);
                                st_documento.SetValue("POSICION", P_DOCSAPAGAR[i].POSICION);
                                st_documento.SetValue("CLIENTE", P_DOCSAPAGAR[i].CLIENTE);
                                st_documento.SetValue("TIPO_DOCUMENTO", P_DOCSAPAGAR[i].TIPO_DOCUMENTO);
                                st_documento.SetValue("SOCIEDAD", P_DOCSAPAGAR[i].SOCIEDAD);
                                st_documento.SetValue("NRO_DOCUMENTO", P_DOCSAPAGAR[i].NRO_DOCUMENTO);
                                st_documento.SetValue("NRO_REFERENCIA", P_DOCSAPAGAR[i].NRO_REFERENCIA);
                                st_documento.SetValue("ACC", P_DOCSAPAGAR[i].ACC);
                                st_documento.SetValue("CAJERO_RESP", P_DOCSAPAGAR[i].CAJERO_RESP);
                                st_documento.SetValue("CAJERO_GEN", P_DOCSAPAGAR[i].CAJERO_RESP);
                                st_documento.SetValue("ID_CAJA", P_DOCSAPAGAR[i].ID_CAJA);
                                st_documento.SetValue("FECHA_COMP", P_DOCSAPAGAR[i].FECHA_COMP);
                                st_documento.SetValue("HORA", P_DOCSAPAGAR[i].HORA);
                                st_documento.SetValue("NRO_COMPENSACION", P_DOCSAPAGAR[i].NRO_COMPENSACION);
                                st_documento.SetValue("TEXTO_CABECERA", P_DOCSAPAGAR[i].TEXTO_CABECERA);
                                st_documento.SetValue("NULO", P_DOCSAPAGAR[i].NULO);
                                st_documento.SetValue("NRO_ANULACION", P_DOCSAPAGAR[i].NRO_ANULACION);
                                st_documento.SetValue("EXCEPCION", P_DOCSAPAGAR[i].EXCEPCION);
                                st_documento.SetValue("FECHA_DOC", P_DOCSAPAGAR[i].FECHA_DOC);
                                st_documento.SetValue("FECHA_VENC_DOC", P_DOCSAPAGAR[i].FECHA_VENC_DOC);
                                st_documento.SetValue("CREDITO", P_DOCSAPAGAR[i].CREDITO);
                                st_documento.SetValue("NUM_CUOTA", P_DOCSAPAGAR[i].NUM_CUOTA);
                                st_documento.SetValue("MONTO_DOC", P_DOCSAPAGAR[i].MONTO_DOC);
                                st_documento.SetValue("MONTO_CAPITAL", P_DOCSAPAGAR[i].MONTO_CAPITAL);
                                st_documento.SetValue("MONTO_INTERES", P_DOCSAPAGAR[i].MONTO_INTERES);
                                st_documento.SetValue("MONTO_MORA", P_DOCSAPAGAR[i].MONTO_MORA);
                                st_documento.SetValue("MONTO_COB", P_DOCSAPAGAR[i].MONTO_COB);
                                st_documento.SetValue("TEXTO_EXCEPCION", P_DOCSAPAGAR[i].TEXTO_EXCEPCION);
                                st_documento.SetValue("PARCIAL", P_DOCSAPAGAR[i].PARCIAL);
                                st_documento.SetValue("TIME", P_DOCSAPAGAR[i].TIME);
                                st_documento.SetValue("CORRELATIVO", P_DOCSAPAGAR[i].CORRELATIVO);
                                st_documento.SetValue("FOLIO", P_DOCSAPAGAR[i].FOLIO);
                                st_documento.SetValue("EST_PAGO_CUOTA", P_DOCSAPAGAR[i].EST_PAGO_CUOTA);
                                st_documento.SetValue("NUM_BOLETA", P_DOCSAPAGAR[i].NUM_BOLETA);
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message + ex.StackTrace);
                        }
                        BapiGetUser.SetValue("DOCUMENTOS", st_documento);

                        BapiGetUser.Invoke(SapRfcDestination);

                        lt_retorno = BapiGetUser.GetTable("RETURN");

                        for (int i = 0; i < lt_retorno.Count(); i++)
                        {
                            lt_retorno.CurrentIndex = i;
                            retorno = new ST_RETORNO();
                            if (lt_retorno.GetString("TYPE") == "S")
                            {
                                Mensaje = Mensaje + " - " + lt_retorno.GetString("MESSAGE") + "\n";
                            }
                            if (lt_retorno.GetString("TYPE") == "E")
                            {
                                pagomessage = pagomessage + " - " + lt_retorno.GetString("MESSAGE") + "\n";
                            }
                            retorno.TYPE = lt_retorno.GetString("TYPE");
                            retorno.MESSAGE = lt_retorno.GetString("MESSAGE");
                            retorno.LOG_NO = lt_retorno.GetString("LOG_NO");
                            retorno.LOG_MSG_NO = lt_retorno.GetString("LOG_MSG_NO");
                            retorno.MESSAGE_V1 = lt_retorno.GetString("MESSAGE_V1");
                            retorno.MESSAGE_V2 = lt_retorno.GetString("MESSAGE_V2");
                            retorno.MESSAGE_V3 = lt_retorno.GetString("MESSAGE_V3");
                            if (lt_retorno.GetString("MESSAGE_V4") != "")
                            {
                                comprobante = lt_retorno.GetString("MESSAGE_V4");
                            }
                            retorno.MESSAGE_V4 = lt_retorno.GetString("MESSAGE_V4");
                            t_retorno.Add(retorno);
                        }
                    }
               }
            catch (Exception ex)
            {
                Console.WriteLine("{0} Exception caught.", ex);
            }
        }
    }
}