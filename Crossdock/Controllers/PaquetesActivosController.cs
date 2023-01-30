using Crossdock.Context.Commands;
using Crossdock.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Microsoft.VisualBasic.CompilerServices;
using System.Linq;

namespace Crossdock.Controllers
{
 

    public class PaquetesActivosController : Controller
    {
        static List<Paquetes> Filtro_Paquetes = new List<Paquetes>(); //se agrega la lista paquetes
        static List<Paquetes> lpaquetes = new List<Paquetes>(); //se agrega la lista paquetes
        TablaPaquetesCommands oPaquetes = new TablaPaquetesCommands();  //objeto de paquetescomands

        static string s_Estatus = "";
        static string s_Fechas = "";

        public ActionResult Index()
        {

            if (Session["UserID"] == null)
            {
                return RedirectToAction("../Home/login");
            }

            if (Session["UserID"] == null)
            {
                return RedirectToAction("../Home/login");
            }
            ViewBag.L_Estatus = lpaquetes;
            ViewBag.L_Fechas = lpaquetes;
            if (s_Estatus != "")
                ViewBag.DEstatus = s_Estatus;
            else
                ViewBag.DEstatus = "Selecciona un Estatus";

            if (s_Fechas != "")
                ViewBag.DFecha = s_Fechas;
            else
                ViewBag.DFecha = "Selecciona una Fecha";
                SeparaPermisos(Session["Permisos"].ToString());
                return View();
            
        }

        //filtro para cliente, estatus, fecha (Ricardo)
       
       public JsonResult ListaGuiasFiltro(string estatus, string fecha)
       {


            try
            {
                if (estatus != "" && fecha == "")
                {
                    Filtro_Paquetes = oPaquetes.Muestra_Paquetes().Where(x => x.EstatusPaqueteDescripcion != "Entregado" && x.EstatusPaqueteDescripcion == estatus).ToList();
                    Filtro_Paquetes = indice_general(Filtro_Paquetes);
                    return Json(new { data = Filtro_Paquetes }, JsonRequestBehavior.AllowGet);
                }
                else if (estatus == "" && fecha != "")
                {

                    Filtro_Paquetes = oPaquetes.Muestra_Paquetes().Where(x => x.EstatusPaqueteDescripcion != "Entregado" && x.FechaEntregaFinal == fecha).ToList();
                    Filtro_Paquetes = indice_general(Filtro_Paquetes);
                    return Json(new { data = Filtro_Paquetes }, JsonRequestBehavior.AllowGet);

                }
                else if (estatus != "" && fecha != "")
                {
                    Filtro_Paquetes = oPaquetes.Muestra_Paquetes().Where(x => x.EstatusPaqueteDescripcion != "Entregado" && x.EstatusPaqueteDescripcion == estatus && x.FechaEntregaFinal == fecha).ToList();
                    Filtro_Paquetes = indice_general(Filtro_Paquetes);
                    return Json(new { data = Filtro_Paquetes }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    Filtro_Paquetes = lpaquetes.ToList();
                    Filtro_Paquetes = indice_general(Filtro_Paquetes);
                    return Json(new { data = Filtro_Paquetes }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                /*string filePath = Server.MapPath("~/Error.txt");

                using (StreamWriter writer = new StreamWriter(filePath, true))
                {
                    writer.WriteLine("-----------------------------------------------------------------------------");
                    writer.WriteLine("Date : " + DateTime.Now.ToString());
                    writer.WriteLine("Usuario ID: " + Session["UserID"]);
                    writer.WriteLine("Usuario: " + Session["UserNombre"]);
                    writer.WriteLine();

                    while (ex != null)
                    {
                        writer.WriteLine(ex.GetType().FullName);
                        writer.WriteLine("Message : " + ex.Message);
                        writer.WriteLine("StackTrace : " + ex.StackTrace);

                        ex = ex.InnerException;
                    }*/
            
               
               return Json(new { data = Filtro_Paquetes }, JsonRequestBehavior.AllowGet);
           }
           
    }
       
       

        //finaliza filtro para cliente, estatus, fecha (Ricardo)


        private List<Paquetes> indice_general(List<Paquetes> lista)
        {
            
            int indice = 1;
            foreach (Paquetes guia in lista)
            {
                guia.ID_Temporal = indice;
                indice++;
            }
            return lista;
            
        }

        [HttpGet]
        public JsonResult LLena_tabla()
        {
            
            var listaPaquetes = oPaquetes.Muestra_Paquetes().Where(x => x.EstatusPaqueteDescripcion != "Entregado").ToList();
            listaPaquetes = indice_general(listaPaquetes);
            return Json(new { data = listaPaquetes }, JsonRequestBehavior.AllowGet);
        }

        public void SeparaPermisos(string permisos)
        {
            //Declaración de variables
            int n;
            string valor;
            int f = 0;

            //Devuelve la longitud
            int a = permisos.Length;
            int[] testArray = new int[2];

            //Iteración de permisos con for
            for (n = 0; n <= permisos.Length - 1; n++)
            {
                valor = Conversions.ToString(permisos[n]);
                //Conversión del permiso
                int asm = Convert.ToInt32(valor);
                testArray[f] = asm;

                switch (n)
                {
                    //Guias
                    case 0:
                        {
                            if (testArray[f] == 1)
                                ViewBag.GuiasVisualizar = "true";
                            else
                                ViewBag.GuiasVisualizar = "none";
                            break;
                        }
                    //Historial de Guias
                    case 1:
                        {
                            if (testArray[f] == 1)
                                ViewBag.HistorialGuias = "true";
                            else
                                ViewBag.HistorialGuias = "none";
                            break;
                        }
                    //Guias Prepagadas
                    case 2:
                        {
                            if (testArray[f] == 1)
                                ViewBag.GuiasPrepagadas = "true";
                            else
                                ViewBag.GuiasPrepagadas = "none";
                            break;
                        }
                    //Notificaciones
                    case 3:
                        {
                            if (testArray[f] == 1)
                                ViewBag.Notificaciones = "true";
                            else
                                ViewBag.Notificaciones = "none";
                            break;
                        }
                    //Recepción Paquetes
                    case 4:
                        {
                            if (testArray[f] == 1)
                                ViewBag.RecepcionPaquetes = "true";
                            else
                                ViewBag.RecepcionPaquetes = "none";
                            break;
                        }
                    //Seguimiento de Paquetes
                    case 5:
                        {
                            if (testArray[f] == 1)
                                ViewBag.SeguimientoPaquetes = "true";
                            else
                                ViewBag.SeguimientoPaquetes = "none";
                            break;
                        }
                    //historial de paquetes
                    case 6:
                        {
                            if (testArray[f] == 1)
                                ViewBag.HistorialPaquetes = "true";
                            else
                                ViewBag.HistorialPaquetes = "none";
                            break;

                        }
                    //Asignación de Tareas
                    case 7:
                        {
                            if (testArray[f] == 1)
                                ViewBag.AsignacionTareas = "true";
                            else
                                ViewBag.AsignacionTareas = "none";
                            break;
                        }
                    //Historial de Tareas
                    case 8:
                        {
                            if (testArray[f] == 1)
                                ViewBag.HistorialTareas = "true";
                            else
                                ViewBag.HistorialTareas = "none";
                            break;
                        }
                    //Usuarios
                    case 9:
                        {
                            if (testArray[f] == 1)
                                ViewBag.UsuariosVisualizar = "true";
                            else
                                ViewBag.UsuariosVisualizar = "none";
                            break;
                        }
                    //Bodegas
                    case 10:
                        {
                            if (testArray[f] == 1)
                                ViewBag.BodegasVisualizar = "true";
                            else
                                ViewBag.BodegasVisualizar = "none";
                            break;
                        }
                    //Clientes
                    case 11:
                        {
                            if (testArray[f] == 1)
                                ViewBag.ClientesVisualizar = "true";
                            else
                                ViewBag.ClientesVisualizar = "none";
                            break;
                        }
                    //Condiciones
                    case 12:
                        {
                            if (testArray[f] == 1)
                                ViewBag.CondicionesVisualizar = "true";
                            else
                                ViewBag.CondicionesVisualizar = "none";
                            break;
                        }
                    //Delivery
                    case 13:
                        {
                            if (testArray[f] == 1)
                                ViewBag.DeliveryVisualizar = "true";
                            else
                                ViewBag.DeliveryVisualizar = "none";
                            break;
                        }
                    //Estatus Paquetes
                    case 14:
                        {
                            if (testArray[f] == 1)
                                ViewBag.EstatusPaVisualizar = "true";
                            else
                                ViewBag.EstatusPaVisualizar = "none";
                            break;
                        }
                    //Estatus Tareas
                    case 15:
                        {
                            if (testArray[f] == 1)
                                ViewBag.EstatusTaVisualizar = "true";
                            else
                                ViewBag.EstatusTaVisualizar = "none";
                            break;
                        }
                    //Operadores
                    case 16:
                        {
                            if (testArray[f] == 1)
                                ViewBag.OperadoresVisualizar = "true";
                            else
                                ViewBag.OperadoresVisualizar = "none";
                            break;
                        }
                    //Tipo Operador
                    case 17:
                        {
                            if (testArray[f] == 1)
                                ViewBag.TopVisualizar = "true";
                            else
                                ViewBag.TopVisualizar = "none";
                            break;
                        }
                    //Roles
                    case 18:
                        {
                            if (testArray[f] == 1)
                                ViewBag.RolesVisualizar = "true";
                            else
                                ViewBag.RolesVisualizar = "none";
                            break;
                        }
                    //Tipo Entrega
                    case 19:
                        {
                            if (testArray[f] == 1)
                                ViewBag.TenVisualizar = "true";
                            else
                                ViewBag.TenVisualizar = "none";
                            break;
                        }
                    //Unidades
                    case 20:
                        {
                            if (testArray[f] == 1)
                                ViewBag.UnidadesVisualizar = "true";
                            else
                                ViewBag.UnidadesVisualizar = "none";
                            break;
                        }
                    //Tipo unidades
                    case 21:
                        {
                            if (testArray[f] == 1)
                                ViewBag.TunVisualizar = "true";
                            else
                                ViewBag.TunVisualizar = "none";
                            break;
                        }
                    //Zonas
                    case 22:
                        {
                            if (testArray[f] == 1)
                                ViewBag.ZonasVisualizar = "true";
                            else
                                ViewBag.ZonasVisualizar = "none";
                            break;
                        }
                    //Devolución
                    case 23:
                        {
                            if (testArray[f] == 1)
                                ViewBag.MotivosVisualizar = "true";
                            else
                                ViewBag.MotivosVisualizar = "none";
                            break;
                        }
                    //Coberturas
                    case 24:
                        {
                            if (testArray[f] == 1)
                                ViewBag.CoberturasVisualizar = "true";
                            else
                                ViewBag.CoberturasVisualizar = "none";
                            break;
                        }
                    //Inexistente
                    default:
                        {
                            Console.WriteLine("El catálogo no existe");
                            break;
                        }
                }
            }
        }
    }
}
