using ClosedXML.Excel;
using Crossdock.Context.Commands;
using Crossdock.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Microsoft.VisualBasic.CompilerServices;
using System.Linq.Dynamic;
using Newtonsoft.Json;

namespace Crossdock.Controllers
{
    public class HistorialGuiasController : Controller
    {


        TablaGuiasCommands commandGuias = new TablaGuiasCommands();
        static List<Guias> L_Guias = new List<Guias>();
        static List<Guias> segGuias = new List<Guias>();
        static List<Guias> Filtro_segGuias = new List<Guias>();
        static List<Guias> F_Guias = new List<Guias>();
        static List<SelectListItem> L_cli = new List<SelectListItem>();
        static string s_Cliente = "", s_fFinal = "", s_fInicial = "", mensaje = "", limpiar = "";
        static string pagina = "";

        public ActionResult Index()
        {
            var ClienteID = Convert.ToInt32(Session["UserClienteID"]);
            if (Session["UserID"] == null)
            {
                SeparaPermisos(Session["Permisos"].ToString());
                return RedirectToAction("../Home/login");
            }

            if (Convert.ToInt32(Session["UserRolID"]) != 1)
            {
                TablaClientesCommands oClientes = new TablaClientesCommands();
                var listaClientes = oClientes.Muestra_Clientes().Where(x => x.ClienteID == ClienteID) .ToList();//consumo de sp
                L_cli = new SelectList(listaClientes, dataValueField: "RazonSocial", dataTextField: "RazonSocial").ToList();//llena el DropDownlist            
                ViewBag.L_Clientes = L_cli;
                ViewBag.habilitar = false;
            }
            else
            {
                TablaClientesCommands oClientes = new TablaClientesCommands();
                var listaClientes = oClientes.Muestra_Clientes().ToList();//consumo de sp
                L_cli = new SelectList(listaClientes, dataValueField: "RazonSocial", dataTextField: "RazonSocial").ToList();//llena el DropDownlist            
                ViewBag.L_Clientes = L_cli;
                ViewBag.DCliente = "Selecciona un Cliente";
                ViewBag.habilitar = false;
            }
            SeparaPermisos(Session["Permisos"].ToString());
            return View();
        }

        private List<Guias> indice_general(List<Guias> lista)
        {
            int indice = 1;
            foreach (Guias guia in lista)
            {
                guia.ID_Temporal = indice;
                indice++;
            }
            return lista;
        }

        public JsonResult ListaGuias()
        {
            if (Convert.ToInt32(Session["UserRolID"]) != 1)
            {
                segGuias = commandGuias.Muestra_Guias().Where(x => x.ClienteID == Convert.ToInt32(Session["UserClienteID"])).OrderByDescending(x => x.Fecha).ToList();
                segGuias = indice_general(segGuias);
                Filtro_segGuias = segGuias;
                return Json(new { data = segGuias }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                segGuias = commandGuias.Muestra_Guias().OrderByDescending(x => x.Fecha).ToList();
                segGuias = indice_general(segGuias);
                Filtro_segGuias = segGuias;
                return Json(new { data = segGuias }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult ListaGuiasFiltro(string fecha_inicio, string fecha_final, string Cliente)
        {
            try { 
            if (Cliente == "" && fecha_inicio == "" && fecha_final == "")
            {
                Filtro_segGuias = segGuias.ToList();
                Filtro_segGuias = indice_general(Filtro_segGuias);
                return Json(new { data = Filtro_segGuias }, JsonRequestBehavior.AllowGet);
            }
            else if (Cliente != "" && fecha_inicio == "" && fecha_final == "")
            {
                Filtro_segGuias = segGuias.Where(x => x.Cliente_RZ == Cliente).ToList();
                Filtro_segGuias = indice_general(Filtro_segGuias);
                return Json(new { data = Filtro_segGuias }, JsonRequestBehavior.AllowGet);
            }
            else if (Cliente == "" && fecha_inicio != "" && fecha_final == "")
            {
                var separa2 = fecha_inicio.Split('/');
                fecha_inicio = separa2[2] + "-" + separa2[0] + "-" + separa2[1];
                Filtro_segGuias = segGuias.Where(x => Convert.ToDateTime(x.Fecha) >= Convert.ToDateTime(fecha_inicio)).ToList();
                Filtro_segGuias = indice_general(Filtro_segGuias);
                return Json(new { data = Filtro_segGuias }, JsonRequestBehavior.AllowGet);
            }
            else if (Cliente == "" && fecha_inicio != "" && fecha_final != "")
            {
                var separa = fecha_final.Split('/');
                var separa2 = fecha_inicio.Split('/');
                fecha_inicio = separa2[2] + "-" + separa2[0] + "-" + separa2[1];
                fecha_final = separa[2] + "-" + separa[0] + "-" + separa[1];
                if (Convert.ToDateTime(fecha_inicio) > Convert.ToDateTime(fecha_final))
                {
                    Filtro_segGuias = segGuias.ToList();
                    Filtro_segGuias = indice_general(Filtro_segGuias);
                    return Json(new { data = Filtro_segGuias }, JsonRequestBehavior.AllowGet);
                }
                else { 

                Filtro_segGuias = segGuias.Where(x => x.FechaCreacion >= Convert.ToDateTime(fecha_inicio) && x.FechaCreacion <= Convert.ToDateTime(fecha_final)).ToList();
                Filtro_segGuias = indice_general(Filtro_segGuias);
                return Json(new { data = Filtro_segGuias }, JsonRequestBehavior.AllowGet);
                }
            }
            else if (Cliente != "" && fecha_inicio != "" && fecha_final == "")
            {
                var separa2 = fecha_inicio.Split('/');
                fecha_inicio = separa2[2] + "-" + separa2[0] + "-" + separa2[1];
                Filtro_segGuias = segGuias.Where(x => Convert.ToDateTime(x.Fecha) >= Convert.ToDateTime(fecha_inicio) && x.Cliente_RZ == Cliente).ToList();
                Filtro_segGuias = indice_general(Filtro_segGuias);
                return Json(new { data = Filtro_segGuias }, JsonRequestBehavior.AllowGet);
            }
            else if (Cliente != "" && fecha_inicio != "" && fecha_final != "")
            {
                var separa2 = fecha_inicio.Split('/');
                var separa = fecha_final.Split('/');
                fecha_final = separa[2] + "-" + separa[0] + "-" + separa[1];
                fecha_inicio = separa2[2] + "-" + separa2[0] + "-" + separa2[1];
                Filtro_segGuias = segGuias.Where(x => Convert.ToDateTime(x.Fecha) >= Convert.ToDateTime(fecha_inicio) && x.FechaCreacion <= Convert.ToDateTime(fecha_final).AddDays(1) && x.Cliente_RZ == Cliente).ToList();
                Filtro_segGuias = indice_general(Filtro_segGuias);
                return Json(new { data = Filtro_segGuias }, JsonRequestBehavior.AllowGet);
            }
            else {
                return Json(new { data = Filtro_segGuias }, JsonRequestBehavior.AllowGet);
            }
            }
            catch (Exception ex)
            {
                string filePath = Server.MapPath("~/Error.txt");

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
                    }
                }
                return Json(new { data = Filtro_segGuias }, JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult Descargar(string id)
        {
            id = JsonConvert.DeserializeObject<string>(id);
            int id2 = Convert.ToInt32(id);
            //descarga de guia
            var d_guia = Filtro_segGuias.Where(x => x.GuiaID == id2).ToList();
            var URL = d_guia[0].Url;
            WebClient Client = new WebClient();
            var archivo = Client.DownloadData(URL);
            TempData["file"] = Convert.ToBase64String(archivo);
            TempData["fileName"] = d_guia[0].Guia;
            //   ViewBag.L_Clientes = L_cli;


            if (s_fInicial != "")
                ViewBag.DFechaInicial = s_fInicial;
            if (s_fFinal != "")
                ViewBag.DFechaFinal = s_fFinal;
            if (s_Cliente != "")
            {
                ViewBag.DCliente = s_Cliente;
            }
            else
            {
                ViewBag.DCliente = "Selecciona un Cliente";
            }
            SeparaPermisos(Session["Permisos"].ToString());

            return RedirectToAction("Index");

        }

        public ActionResult Eliminar(string id)// antes de eliminar verificamos que no tenga recepcion 
        {
            id = JsonConvert.DeserializeObject<string>(id);
            //verificamos el estado de la guia, si ya esta en recepcion no puede ser eliminada 
            TablaPaquetesCommands oPaquetes = new TablaPaquetesCommands();
            var oGuia = commandGuias.Muestra_GuiasMod(id);
            var paquete = oPaquetes.Muestra_Paquetes().Where(x => x.Guia == oGuia[0].Guia).ToList();
            if (paquete.Count != 0)
            {
                ViewBag.Eliminar = "none";
                ViewBag.Ocultar = "true";
            }
            else
            {
                ViewBag.Ocultar = "none";
                ViewBag.Eliminar = "true";
            }
            SeparaPermisos(Session["Permisos"].ToString());
            return View(oGuia[0]);
        }

        [HttpPost, ActionName("Eliminar")]
        public ActionResult ConfirmarEliminar(string id)
        {
            TablaPaquetesCommands oPaquetes = new TablaPaquetesCommands();
            var oGuia = commandGuias.Muestra_GuiasMod(id);
            foreach(var eliminaGUia in oGuia)
            {
                commandGuias.Elimina_Guia(eliminaGUia.GuiaID);
            }
            
            ViewBag.L_Clientes = L_cli;
            limpiar = "on";
            return RedirectToAction("Index");
        }

        public ActionResult Regresar()
        {
            ViewBag.L_Clientes = L_cli;
            return RedirectToAction("Index");
        }


        public ActionResult LimpiarSeleccion()
        {
            TablaClientesCommands oClientes = new TablaClientesCommands();
            var listaClientes = oClientes.Muestra_Clientes().ToList();//consumo de sp
            L_cli = new SelectList(listaClientes, dataValueField: "RazonSocial", dataTextField: "RazonSocial").ToList();//llena el DropDownlist     

            ViewBag.DCliente = "Selecciona un Cliente";
            ViewBag.L_Clientes = L_cli;
            s_Cliente = "";
            s_fFinal = "";
            s_fInicial = "";

            SeparaPermisos(Session["Permisos"].ToString());

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Download(string file)
        {
            //get the temp folder and file path in server
            string fullPath = Path.Combine(Server.MapPath("~/"), file);
            //  return the file for download, this is an Excel 
            // so I set the file content type to "application/vnd.ms-excel"
            return File(fullPath, "application/vnd.ms-excel", file);
            //  return View();
        }

        [HttpPost]
        public JsonResult Exportar()
        {

            string fileName = "Guias_" + DateTime.Now.ToString("yyyy-MM-dd") + ".xlsx";


            DataTable dt = new DataTable();

            string fullPath = Path.Combine(Server.MapPath("~/"), fileName);
            dt.Locale = new System.Globalization.CultureInfo("es-PE");
            //   string handle = Guid.NewGuid().ToString();
            dt.Columns.Add("Numero de Guia", typeof(string));
            dt.Columns.Add("Destinatario", typeof(string));
            dt.Columns.Add("Direccion de Destinatario", typeof(string));
            dt.Columns.Add("Cliente", typeof(string));
            dt.Columns.Add("Zona", typeof(string));
            dt.Columns.Add("Fecha de Generacion", typeof(string));

            foreach (var rp in Filtro_segGuias)
            {
                dt.Rows.Add(new object[]
                {
                      rp.Guia,
                      rp.Destinatario,
                      rp.DireccionDestinatario,
                      rp.Cliente_RZ,
                      rp.ZonaDes,
                      rp.FechaCreacion

                 });
            }
            dt.TableName = "Datos";//tabla de objetos tipo altas 

            //usar commands con un for dependiendo la cantidad de registros en la tabla 

            using (XLWorkbook wb = new XLWorkbook())
            {
                var hoja = wb.Worksheets.Add(dt);
                hoja.ColumnsUsed().AdjustToContents();
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    FileStream file = new FileStream(fullPath, FileMode.Create, FileAccess.Write);
                    stream.WriteTo(file);
                    file.Close();
                }
            }


            return Json(new { fileName = fileName }, JsonRequestBehavior.AllowGet);

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
