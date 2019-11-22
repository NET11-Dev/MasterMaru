using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using SAP.Middleware.Connector;
using Caja_Recaudadora.Controllers;
using Caja_Recaudadora.Models;

namespace Caja_Recaudadora.ServiciosRfc.Rfc_AperturaCierre
{
    public class CAJA_LOGIN
    {

        public List<PARAMS_USUARIO> ObjDatosLogin = new List<PARAMS_USUARIO>();
        public List<ESTADO> Retorno = new List<ESTADO>();

        ConexionController connectorSap = new ConexionController();
        public string errormessage = "";
        string id_apertura = "";
        public string Mensaje = "";
        public string Sociedad = "";
        public string status = "";
        public string cajeroresp = "";


        public void usuarioscaja(string user, string pass, string caja, string temporal)
        {
            ObjDatosLogin.Clear();
            Retorno.Clear();

            try
            {
                IRfcStructure lt_USER;
                IRfcStructure lt_retorno;

                PARAMS_USUARIO USER_resp;
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
                connectorSap.paswr = pass;
                connectorSap.sapRouter = saprouter;
                connectorSap.user = user;
                connectorSap.server = pass;

                string retval = "ERROR";

                if (!string.IsNullOrEmpty(user) && !string.IsNullOrEmpty(pass)) retval = connectorSap.connectionsSAP(user, pass);

                //Si el valor de retorno es nulo o vacio, hay conexion a SAP y la RFC trae datos   
                if (string.IsNullOrEmpty(retval))
                {
                    RfcDestination SapRfcDestination = RfcDestinationManager.GetDestination(connectorSap.connectorConfig);
                    RfcRepository SapRfcRepository = SapRfcDestination.Repository;

                    IRfcFunction BapiGetUser = SapRfcRepository.CreateFunction("ZWD_CAJA_LOGIN");

                    BapiGetUser.SetValue("USUARIO", user);
                    BapiGetUser.SetValue("PASSWORD", pass);
                    BapiGetUser.SetValue("CAJA", caja);
                    BapiGetUser.SetValue("TEMPORAL", temporal);


                    BapiGetUser.Invoke(SapRfcDestination);

                    lt_USER = BapiGetUser.GetStructure("PARAMS_USUARIO");
                    lt_retorno = BapiGetUser.GetStructure("ESTATUS");

                    if (lt_USER.Count > 0)
                    {
                        //LLenamos la tabla de salida lt_DatGen
                        for (int i = 0; i < lt_USER.Count(); i++)
                        {
                            USER_resp = new PARAMS_USUARIO();
                            USER_resp.TIPO = lt_USER.GetString("TIPO");
                            USER_resp.MONTO_APERTURA = lt_USER.GetString("MONTO_APERTURA");
                            USER_resp.NOMBRE = lt_USER.GetString("NOMBRE");
                            USER_resp.EMAIL = lt_USER.GetString("EMAIL");
                            USER_resp.CAJERO_RESP = lt_USER.GetString("CAJERO_RESP");
                            ObjDatosLogin.Add(USER_resp);
                        }

                    }
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
    }
}