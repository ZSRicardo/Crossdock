using Crossdock.Context.Commands;
using Crossdock.Models;
using System;
using Microsoft.VisualBasic.CompilerServices;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Collections.Generic;
using ClosedXML.Excel;
using System.IO;
using System.Data;
using System.Web.UI.WebControls;
using System.Linq.Dynamic;

namespace Crossdock.Controllers
{
    public class HistorialPaquetesController : Controller
    {

        //historial paquetes
        private TablaPaquetesCommands objtablaPaquetes = new TablaPaquetesCommands();
        private TablaEstatusPaquetesCommands tablaEstatusPaquetes = new TablaEstatusPaquetesCommands();
        private TablaClientesCommands tablaClientes = new TablaClientesCommands();
        static List<SeguimientoPaquetes> SegPaquetes = new List<SeguimientoPaquetes>();
        static List<SeguimientoPaquetes> Filtro_SegPaquetes = new List<SeguimientoPaquetes>();
        static List<SelectListItem> L_Clientes = new List<SelectListItem>();
        static List<SelectListItem> L_Estatus = new List<SelectListItem>();


        public ActionResult Seleccion()
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("../Home/Login");
            }
            SeparaPermisos(Session["Permisos"].ToString());
            var IDCliente = Convert.ToInt32(Session["UserClienteID"]); //instanciamos el ID de cliente del usuario y el rol al que pertenece
            var RolID = Convert.ToInt32(Session["UserRolID"]);
            if(RolID==1)
            {
                return RedirectToAction("Index_Administrador");
            }
            else
            {
                return RedirectToAction("Index_Cliente");
            }
            
        }

        // GET: HistorialPaquetes
        public ActionResult Index_Administrador()
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("../Home/Login");
            }
            var ListaClientes = tablaClientes.Muestra_Clientes();
            L_Clientes = new SelectList(ListaClientes, dataValueField: "RazonSocial", dataTextField: "RazonSocial").ToList();
            var ListaEstatus = tablaEstatusPaquetes.Muestra_EstatusPaquetes().Where(x => x.EstatusPaqueteID == 5 || x.EstatusPaqueteID == 6 );
            L_Estatus = new SelectList(ListaEstatus, dataValueField: "Descripcion", dataTextField: "Descripcion").ToList();
            ViewBag.List_Clientes = L_Clientes;
            ViewBag.List_Estatus = L_Estatus;
            ViewBag.DCliente = "Selecciona un Cliente";
            ViewBag.DEstatus = "Selecciona un Estatus";
            SeparaPermisos(Session["Permisos"].ToString());
            return View();
        }

        public ActionResult Index_Cliente()
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("../Home/Login");
            }
            var ListaEstatus = tablaEstatusPaquetes.Muestra_EstatusPaquetes().Where(x => x.EstatusPaqueteID == 5 || x.EstatusPaqueteID == 6);
            L_Estatus = new SelectList(ListaEstatus, dataValueField: "Descripcion", dataTextField: "Descripcion").ToList();
            ViewBag.List_Estatus = L_Estatus;
            ViewBag.DEstatus = "Selecciona un Estatus";
            SeparaPermisos(Session["Permisos"].ToString());
            return View();
        }

        public JsonResult ListaPaquetes()
        {
            var IDCliente = Convert.ToInt32(Session["UserClienteID"]); //instanciamos el ID de cliente del usuario y el rol al que pertenece
            var RolID = Convert.ToInt32(Session["UserRolID"]);
            if(RolID==1)
            {
                SegPaquetes = objtablaPaquetes.MuestraHistorialPaquetes().Where(x => x.Estatus == "Entregado" || x.Estatus == "Regreso a origen").OrderByDescending(x => x.Guia).ToList(); //cualquiera que no sea cliente
               
            }
            else 
            {
                SegPaquetes = objtablaPaquetes.MuestraHistorialPaquetes().Where(x=>x.ClienteID==IDCliente).OrderByDescending(x => x.Guia).Where(x => x.Estatus == "Entregado" || x.Estatus == "Regreso a origen").ToList(); //cualquiera que no sea cliente
            }
            SegPaquetes = indice_temporal(SegPaquetes);
            Filtro_SegPaquetes = SegPaquetes;

            return Json(new { data = SegPaquetes }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult ListaPaquetesFiltro(string fecha_inicio, string fecha_final, string recepcion, string entrega, string estatus)
        {
            try { 
            if (recepcion != "on" && entrega != "on" && estatus == "")
            {
                Filtro_SegPaquetes = SegPaquetes.ToList();
                Filtro_SegPaquetes = indice_temporal(Filtro_SegPaquetes);
                return Json(new { data = Filtro_SegPaquetes }, JsonRequestBehavior.AllowGet);
            }
            else if (recepcion != "on" && entrega != "on" && estatus != "")
            {
                Filtro_SegPaquetes = SegPaquetes.Where(x => x.Estatus == estatus).ToList();
                Filtro_SegPaquetes = indice_temporal(Filtro_SegPaquetes);
                return Json(new { data = Filtro_SegPaquetes }, JsonRequestBehavior.AllowGet);
            }
            if (recepcion == "on")
            {
                if (fecha_final != "" && fecha_inicio != "")
                {
                    var separa = fecha_final.Split('/');
                    var separa2 = fecha_inicio.Split('/');
                    fecha_inicio = separa2[2] + "-" + separa2[0] + "-" + separa2[1];
                    fecha_final = separa[2] + "-" + separa[0] + "-" + separa[1];

                    if (Convert.ToDateTime(fecha_inicio) > Convert.ToDateTime(fecha_final))
                    {
                        Filtro_SegPaquetes = SegPaquetes.ToList();
                        Filtro_SegPaquetes = indice_temporal(Filtro_SegPaquetes);
                        return Json(new { data = Filtro_SegPaquetes }, JsonRequestBehavior.AllowGet);
                    }
                    if (estatus == "")
                    {
                        Filtro_SegPaquetes = SegPaquetes.Where(x => x.FechaRecepcion >= Convert.ToDateTime(fecha_inicio)).ToList();
                        Filtro_SegPaquetes = indice_temporal(Filtro_SegPaquetes);
                        return Json(new { data = Filtro_SegPaquetes }, JsonRequestBehavior.AllowGet);
                    }
                    else if (estatus != "")
                    {
                        Filtro_SegPaquetes = SegPaquetes.Where(x => x.FechaRecepcion >= Convert.ToDateTime(fecha_inicio) && x.Estatus == estatus).ToList();
                        Filtro_SegPaquetes = indice_temporal(Filtro_SegPaquetes);
                        return Json(new { data = Filtro_SegPaquetes }, JsonRequestBehavior.AllowGet);
                    }
                }
                else if (fecha_inicio != "" && fecha_final == "")
                {
                    var separa2 = fecha_inicio.Split('/');
                    fecha_inicio = separa2[2] + "-" + separa2[0] + "-" + separa2[1];

                    if (estatus != "")
                    {
                        Filtro_SegPaquetes = SegPaquetes.Where(x => x.FechaRecepcion >= Convert.ToDateTime(fecha_inicio) && x.Estatus == estatus).ToList();
                        Filtro_SegPaquetes = indice_temporal(Filtro_SegPaquetes);
                        return Json(new { data = Filtro_SegPaquetes }, JsonRequestBehavior.AllowGet);
                    }
                    else if (estatus == "")
                    {
                        Filtro_SegPaquetes = SegPaquetes.Where(x => x.FechaRecepcion >= Convert.ToDateTime(fecha_inicio)).ToList();
                        Filtro_SegPaquetes = indice_temporal(Filtro_SegPaquetes);
                        return Json(new { data = Filtro_SegPaquetes }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            else if (entrega == "on") 
            {
                if (fecha_final != "" && fecha_inicio != "")
                {
                    var separa = fecha_final.Split('/');
                    var separa2 = fecha_inicio.Split('/');
                    fecha_inicio = separa2[2] + "-" + separa2[0] + "-" + separa2[1];
                    fecha_final = separa[2] + "-" + separa[0] + "-" + separa[1];

                    if (Convert.ToDateTime(fecha_inicio) > Convert.ToDateTime(fecha_final))
                    {
                        Filtro_SegPaquetes = SegPaquetes.ToList();
                        Filtro_SegPaquetes = indice_temporal(Filtro_SegPaquetes);
                        return Json(new { data = Filtro_SegPaquetes }, JsonRequestBehavior.AllowGet);
                    }
                    if (estatus == "")
                    {
                        Filtro_SegPaquetes = SegPaquetes.Where(x => x.FechaEntrega >= Convert.ToDateTime(fecha_inicio)).ToList();
                        Filtro_SegPaquetes = indice_temporal(Filtro_SegPaquetes);
                        return Json(new { data = Filtro_SegPaquetes }, JsonRequestBehavior.AllowGet);
                    }
                    else if (estatus != "")
                    {
                        Filtro_SegPaquetes = SegPaquetes.Where(x => x.FechaEntrega >= Convert.ToDateTime(fecha_inicio) && x.Estatus == estatus).ToList();
                        Filtro_SegPaquetes = indice_temporal(Filtro_SegPaquetes);
                        return Json(new { data = Filtro_SegPaquetes }, JsonRequestBehavior.AllowGet);
                    }
                }
                else if (fecha_inicio != "" && fecha_final == "")
                {
                    var separa2 = fecha_inicio.Split('/');
                    fecha_inicio = separa2[2] + "-" + separa2[0] + "-" + separa2[1];

                    if (estatus != "")
                    {
                        Filtro_SegPaquetes = SegPaquetes.Where(x => x.FechaEntrega >= Convert.ToDateTime(fecha_inicio) && x.Estatus == estatus).ToList();
                        Filtro_SegPaquetes = indice_temporal(Filtro_SegPaquetes);
                        return Json(new { data = Filtro_SegPaquetes }, JsonRequestBehavior.AllowGet);
                    }
                    else if (estatus == "")
                    {
                        Filtro_SegPaquetes = SegPaquetes.Where(x => x.FechaEntrega >= Convert.ToDateTime(fecha_inicio)).ToList();
                        Filtro_SegPaquetes = indice_temporal(Filtro_SegPaquetes);
                        return Json(new { data = Filtro_SegPaquetes }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
                Filtro_SegPaquetes = SegPaquetes.ToList();
            Filtro_SegPaquetes = indice_temporal(Filtro_SegPaquetes);
            return Json(new { data = Filtro_SegPaquetes }, JsonRequestBehavior.AllowGet);
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
                return Json(new { data = Filtro_SegPaquetes }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpGet]
        public JsonResult ListaPaquetesFiltro_Administrador(string fecha_inicio, string fecha_final, string recepcion, string entrega, string cliente, string estatus)
        {
            try { 
            if (cliente == "" && estatus == "" && fecha_inicio == "" && fecha_final == "")
            {
                Filtro_SegPaquetes = SegPaquetes.ToList();
                Filtro_SegPaquetes = indice_temporal(Filtro_SegPaquetes);
                return Json(new { data = Filtro_SegPaquetes }, JsonRequestBehavior.AllowGet);
            }
            else if (cliente != "" && estatus == "" && fecha_inicio == "" && fecha_final == "")
            {
                Filtro_SegPaquetes = SegPaquetes.Where(x => x.RazonSocial == cliente).ToList();
                Filtro_SegPaquetes = indice_temporal(Filtro_SegPaquetes);
                return Json(new { data = Filtro_SegPaquetes }, JsonRequestBehavior.AllowGet);
            }
            else if (cliente == "" && estatus != "" && fecha_inicio == "" && fecha_final == "")
            {
                Filtro_SegPaquetes = SegPaquetes.Where(x => x.Estatus == estatus).ToList();
                Filtro_SegPaquetes = indice_temporal(Filtro_SegPaquetes);
                return Json(new { data = Filtro_SegPaquetes }, JsonRequestBehavior.AllowGet);
            }
            else if (cliente != "" && estatus != "" && fecha_inicio == "" && fecha_final == "")
            {
                Filtro_SegPaquetes = SegPaquetes.Where(x => x.RazonSocial == cliente && x.Estatus == estatus).ToList();
                Filtro_SegPaquetes = indice_temporal(Filtro_SegPaquetes);
                return Json(new { data = Filtro_SegPaquetes }, JsonRequestBehavior.AllowGet);
            }
            if (recepcion == "on")
            {
                if (fecha_final != "" && fecha_inicio != "")
                {
                    var separa = fecha_final.Split('/');
                    var separa2 = fecha_inicio.Split('/');
                    fecha_inicio = separa2[2] + "-" + separa2[0] + "-" + separa2[1];
                    fecha_final = separa[2] + "-" + separa[0] + "-" + separa[1];

                    if (Convert.ToDateTime(fecha_inicio) > Convert.ToDateTime(fecha_final))
                    {
                        Filtro_SegPaquetes = SegPaquetes.ToList();
                        Filtro_SegPaquetes = indice_temporal(Filtro_SegPaquetes);
                        return Json(new { data = Filtro_SegPaquetes }, JsonRequestBehavior.AllowGet);
                    }
                    else if (cliente == "" && estatus == "")
                    {
                        Filtro_SegPaquetes = SegPaquetes.Where(x => x.FechaRecepcion >= Convert.ToDateTime(fecha_inicio) && x.FechaRecepcion <= Convert.ToDateTime(fecha_final)).ToList();
                        Filtro_SegPaquetes = indice_temporal(Filtro_SegPaquetes);
                        return Json(new { data = Filtro_SegPaquetes }, JsonRequestBehavior.AllowGet);
                    }
                    else if (cliente != "" && estatus == "")
                    {
                        Filtro_SegPaquetes = SegPaquetes.Where(x => x.FechaRecepcion >= Convert.ToDateTime(fecha_inicio) && x.FechaRecepcion <= Convert.ToDateTime(fecha_final) && x.RazonSocial == cliente).ToList();
                        Filtro_SegPaquetes = indice_temporal(Filtro_SegPaquetes);
                        return Json(new { data = Filtro_SegPaquetes }, JsonRequestBehavior.AllowGet);
                    }
                    else if (cliente == "" && estatus != "")
                    {
                        Filtro_SegPaquetes = SegPaquetes.Where(x => x.FechaRecepcion >= Convert.ToDateTime(fecha_inicio) && x.FechaRecepcion <= Convert.ToDateTime(fecha_final) && x.Estatus == estatus).ToList();
                        Filtro_SegPaquetes = indice_temporal(Filtro_SegPaquetes);
                        return Json(new { data = Filtro_SegPaquetes }, JsonRequestBehavior.AllowGet);
                    }
                    else if (cliente != "" && estatus != "")
                    {
                        Filtro_SegPaquetes = SegPaquetes.Where(x => x.FechaRecepcion >= Convert.ToDateTime(fecha_inicio) && x.FechaRecepcion <= Convert.ToDateTime(fecha_final) && x.RazonSocial == cliente && x.Estatus == estatus).ToList();
                        Filtro_SegPaquetes = indice_temporal(Filtro_SegPaquetes);
                        return Json(new { data = Filtro_SegPaquetes }, JsonRequestBehavior.AllowGet);
                    }
                }
                else if (fecha_inicio != "" && fecha_final == "")
                {
                    var separa2 = fecha_inicio.Split('/');
                    fecha_inicio = separa2[2] + "-" + separa2[0] + "-" + separa2[1];

                    if (cliente != "" && estatus == "")
                    {
                        Filtro_SegPaquetes = SegPaquetes.Where(x => x.FechaRecepcion >= Convert.ToDateTime(fecha_inicio) && x.RazonSocial == cliente).ToList();
                        Filtro_SegPaquetes = indice_temporal(Filtro_SegPaquetes);
                        return Json(new { data = Filtro_SegPaquetes }, JsonRequestBehavior.AllowGet);
                    }
                    else if (cliente == "" && estatus != "")
                    {
                        Filtro_SegPaquetes = SegPaquetes.Where(x => x.FechaRecepcion >= Convert.ToDateTime(fecha_inicio) && x.Estatus == estatus).ToList();
                        Filtro_SegPaquetes = indice_temporal(Filtro_SegPaquetes);
                        return Json(new { data = Filtro_SegPaquetes }, JsonRequestBehavior.AllowGet);
                    }
                    else if (cliente != "" && estatus != "")
                    {
                        Filtro_SegPaquetes = SegPaquetes.Where(x => x.FechaRecepcion >= Convert.ToDateTime(fecha_inicio) && x.RazonSocial == cliente && x.Estatus == estatus).ToList();
                        Filtro_SegPaquetes = indice_temporal(Filtro_SegPaquetes);
                        return Json(new { data = Filtro_SegPaquetes }, JsonRequestBehavior.AllowGet);
                    }
                    else if (cliente == "" && estatus == "")
                    {
                        Filtro_SegPaquetes = SegPaquetes.Where(x => x.FechaRecepcion >= Convert.ToDateTime(fecha_inicio)).ToList();
                        Filtro_SegPaquetes = indice_temporal(Filtro_SegPaquetes);
                        return Json(new { data = Filtro_SegPaquetes }, JsonRequestBehavior.AllowGet);
                    }

                }
                return Json(new { data = Filtro_SegPaquetes }, JsonRequestBehavior.AllowGet);
            }
            else if (entrega == "on")
            {
                if (fecha_final != "" && fecha_inicio != "")
                {
                    var separa = fecha_final.Split('/');
                    var separa2 = fecha_inicio.Split('/');
                    fecha_inicio = separa2[2] + "-" + separa2[0] + "-" + separa2[1];
                    fecha_final = separa[2] + "-" + separa[0] + "-" + separa[1];

                    if (Convert.ToDateTime(fecha_inicio) > Convert.ToDateTime(fecha_final))
                    {
                        Filtro_SegPaquetes = SegPaquetes.ToList();
                        Filtro_SegPaquetes = indice_temporal(Filtro_SegPaquetes);
                        return Json(new { data = Filtro_SegPaquetes }, JsonRequestBehavior.AllowGet);
                    }
                    else if (cliente == "" && estatus == "")
                    {
                        Filtro_SegPaquetes = SegPaquetes.Where(x => x.FechaEntrega >= Convert.ToDateTime(fecha_inicio) && x.FechaEntrega <= Convert.ToDateTime(fecha_final)).ToList();
                        Filtro_SegPaquetes = indice_temporal(Filtro_SegPaquetes);
                        return Json(new { data = Filtro_SegPaquetes }, JsonRequestBehavior.AllowGet);
                    }
                    else if (cliente != "" && estatus == "")
                    {
                        Filtro_SegPaquetes = SegPaquetes.Where(x => x.FechaEntrega >= Convert.ToDateTime(fecha_inicio) && x.FechaEntrega <= Convert.ToDateTime(fecha_final) && x.RazonSocial == cliente).ToList();
                        Filtro_SegPaquetes = indice_temporal(Filtro_SegPaquetes);
                        return Json(new { data = Filtro_SegPaquetes }, JsonRequestBehavior.AllowGet);
                    }
                    else if (cliente == "" && estatus != "")
                    {
                        Filtro_SegPaquetes = SegPaquetes.Where(x => x.FechaEntrega >= Convert.ToDateTime(fecha_inicio) && x.FechaEntrega <= Convert.ToDateTime(fecha_final) && x.Estatus == estatus).ToList();
                        Filtro_SegPaquetes = indice_temporal(Filtro_SegPaquetes);
                        return Json(new { data = Filtro_SegPaquetes }, JsonRequestBehavior.AllowGet);
                    }
                    else if (cliente != "" && estatus != "")
                    {
                        Filtro_SegPaquetes = SegPaquetes.Where(x => x.FechaEntrega >= Convert.ToDateTime(fecha_inicio) && x.FechaEntrega <= Convert.ToDateTime(fecha_final) && x.RazonSocial == cliente && x.Estatus == estatus).ToList();
                        Filtro_SegPaquetes = indice_temporal(Filtro_SegPaquetes);
                        return Json(new { data = Filtro_SegPaquetes }, JsonRequestBehavior.AllowGet);
                    }
                }
                else if (fecha_inicio != "" && fecha_final == "")
                {
                    var separa2 = fecha_inicio.Split('/');
                    fecha_inicio = separa2[2] + "-" + separa2[0] + "-" + separa2[1];

                    if (cliente != "" && estatus == "")
                    {
                        Filtro_SegPaquetes = SegPaquetes.Where(x => x.FechaEntrega >= Convert.ToDateTime(fecha_inicio) && x.RazonSocial == cliente).ToList();
                        Filtro_SegPaquetes = indice_temporal(Filtro_SegPaquetes);
                        return Json(new { data = Filtro_SegPaquetes }, JsonRequestBehavior.AllowGet);
                    }
                    else if (cliente == "" && estatus != "")
                    {
                        Filtro_SegPaquetes = SegPaquetes.Where(x => x.FechaEntrega >= Convert.ToDateTime(fecha_inicio) && x.Estatus == estatus).ToList();
                        Filtro_SegPaquetes = indice_temporal(Filtro_SegPaquetes);
                        return Json(new { data = Filtro_SegPaquetes }, JsonRequestBehavior.AllowGet);
                    }
                    else if (cliente != "" && estatus != "")
                    {
                        Filtro_SegPaquetes = SegPaquetes.Where(x => x.FechaEntrega >= Convert.ToDateTime(fecha_inicio) && x.RazonSocial == cliente && x.Estatus == estatus).ToList();
                        Filtro_SegPaquetes = indice_temporal(Filtro_SegPaquetes);
                        return Json(new { data = Filtro_SegPaquetes }, JsonRequestBehavior.AllowGet);
                    }
                    else if (cliente == "" && estatus == "")
                    {
                        Filtro_SegPaquetes = SegPaquetes.Where(x => x.FechaEntrega >= Convert.ToDateTime(fecha_inicio)).ToList();
                        Filtro_SegPaquetes = indice_temporal(Filtro_SegPaquetes);
                        return Json(new { data = Filtro_SegPaquetes }, JsonRequestBehavior.AllowGet);
                    }

                }
            }
            return Json(new { data = Filtro_SegPaquetes }, JsonRequestBehavior.AllowGet);
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
                return Json(new { data = Filtro_SegPaquetes }, JsonRequestBehavior.AllowGet);
            }
        }



            [HttpGet]
        public JsonResult Details(int id)//recibe la idcliente de la tabla enviada por el index a la pagina de details
        {
            SeguimientoPaquetes opaquete = new SeguimientoPaquetes();
           
            if (Session["UserID"] == null)
            {
                return Json(opaquete, JsonRequestBehavior.AllowGet);
            }
            else
            {
                SeparaPermisos(Session["Permisos"].ToString());

                if (id == 0)
                {
                    return Json( opaquete , JsonRequestBehavior.AllowGet);
                }

                opaquete = objtablaPaquetes.MuestraPaquetesMod(id);
                SeparaPermisos(Session["Permisos"].ToString());
                return Json( opaquete , JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        public JsonResult Reporte_Cliente()
        {

            string fileName = "HistorialPaquetes_" + DateTime.Now.ToString("yyyy-MM-dd") + ".xlsx";
            var IDCliente = Convert.ToInt32(Session["UserClienteID"]); //instanciamos el ID de cliente del usuario y el rol al que pertenece
            var RolID = Convert.ToInt32(Session["UserRolID"]);

            DataTable dt = new DataTable();

            string fullPath = Path.Combine(Server.MapPath("~/"), fileName);
            dt.Locale = new System.Globalization.CultureInfo("es-PE");
            dt.Columns.Add("Guia", typeof(string));
            dt.Columns.Add("Fecha de recepcion", typeof(string));
            dt.Columns.Add("Fecha de entrega", typeof(string));
            dt.Columns.Add("Estatus del paquete", typeof(string));
            dt.Columns.Add("Nombre de destinatario", typeof(string));
            dt.Columns.Add("Direccion de destinatario", typeof(string));
            dt.Columns.Add("Evidencias", typeof(string));

            //si no se realizo una busqueda cargamos todos los datos al documento

            foreach (var rp in Filtro_SegPaquetes)
            {
           
                dt.Rows.Add(new object[]
                {   
                      rp.Guia,
                      rp.FechaRecepcion,
                      rp.FechaEntrega,
                      rp.Estatus,
                      rp.NombreDestinatario,
                      rp.Direccion,
                      rp.EvidenciaEntrega

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
            SeparaPermisos(Session["Permisos"].ToString());
            return Json(new { fileName = fileName }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult Reporte_Administrador()
        {

            string fileName = "HistorialPaquetes_" + DateTime.Now.ToString("yyyy-MM-dd") + ".xlsx";
            var IDCliente = Convert.ToInt32(Session["UserClienteID"]); //instanciamos el ID de cliente del usuario y el rol al que pertenece
            var RolID = Convert.ToInt32(Session["UserRolID"]);

            DataTable dt = new DataTable();

            string fullPath = Path.Combine(Server.MapPath("~/"), fileName);
            dt.Locale = new System.Globalization.CultureInfo("es-PE");
            dt.Columns.Add("Cliente", typeof(string));
            dt.Columns.Add("Guia", typeof(string));
            dt.Columns.Add("Palabra Clave", typeof(string));
            dt.Columns.Add("Fecha de recepcion", typeof(string));
            dt.Columns.Add("Fecha de entrega", typeof(string));
            dt.Columns.Add("Estatus del paquete", typeof(string));
            dt.Columns.Add("Nombre de destinatario", typeof(string));
            dt.Columns.Add("Direccion de destinatario", typeof(string));
            dt.Columns.Add("Evidencias", typeof(string));

            //si no se realizo una busqueda cargamos todos los datos al documento

            foreach (var rp in Filtro_SegPaquetes)
            {

                dt.Rows.Add(new object[]
                {
                      rp.RazonSocial,
                      rp.Guia,
                      rp.PalabraClave,
                      rp.FechaRecepcion,
                      rp.FechaEntrega,
                      rp.Estatus,
                      rp.NombreDestinatario,
                      rp.Direccion,
                      rp.EvidenciaEntrega

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
            SeparaPermisos(Session["Permisos"].ToString());
            return Json(new { fileName = fileName }, JsonRequestBehavior.AllowGet);

        }






        [HttpGet]
        public ActionResult Download(string file)
        {
            //get the temp folder and file path in server
            string fullPath = Path.Combine(Server.MapPath("~/"), file);
            //return the file for download, this is an Excel 
            //so I set the file content type to "application/vnd.ms-excel"
            SeparaPermisos(Session["Permisos"].ToString());
            return File(fullPath, "application/vnd.ms-excel", file);
        }
         
        public List<SeguimientoPaquetes> indice_temporal(List<SeguimientoPaquetes> lista)
        {
            int indice = 1;
            foreach(var l in lista)
            {
                l.ID_Temporal= indice;
                indice++;
            }
            return lista;
        }

        public ActionResult LimpiarT()
        {
            var IDCliente = Convert.ToInt32(Session["UserClienteID"]); //instanciamos el ID de cliente del usuario y el rol al que pertenece
            var RolID = Convert.ToInt32(Session["UserRolID"]);
            if (RolID == 1)
            {
                Filtro_SegPaquetes.Clear();
                return RedirectToAction("Index_Administrador");
            }
            else
            {
                Filtro_SegPaquetes.Clear();
                return RedirectToAction("Index_Cliente");
            }
            
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
