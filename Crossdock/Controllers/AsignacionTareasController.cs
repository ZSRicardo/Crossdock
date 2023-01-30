using ClosedXML.Excel;
using Crossdock.Context.Commands;
using Crossdock.Models;
using Microsoft.VisualBasic.CompilerServices;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web.Mvc;
namespace Crossdock.Controllers
{
    public class AsignacionTareasController : Controller
    {

        TareasACommands oTareas = new TareasACommands();
        TablaPaquetesCommands oPaquetes = new TablaPaquetesCommands();
        static List<Paquetes> Lista_Paquetes = new List<Paquetes>();
        static List<Tareas> L_Tareas_Asignadas = new List<Tareas>();
        TablaZonasCommands oZonas = new TablaZonasCommands();
        TablaUsuariosCommands oUsuarios = new TablaUsuariosCommands();
        static string Operador = "", fecha_asignacion = "";
        List<Usuario> L_Usuarios = new List<Usuario>();
        TareasACommands ayuda = new TareasACommands();
        TablaEstatusTareasCommands oEstatustar = new TablaEstatusTareasCommands();
        static List<SeguimientoPaquetes> SegPaquetes = new List<SeguimientoPaquetes>();
        TablaOperadoresCommands oOperadores = new TablaOperadoresCommands();
        static List<SelectListItem> L_op = new List<SelectListItem>();
        static List<SelectListItem> L_paq = new List<SelectListItem>();
        static List<SelectListItem> L_zon = new List<SelectListItem>();
        static List<SelectListItem> L_Clientes = new List<SelectListItem>();
        static List<Paquetes> Ejemplo = new List<Paquetes>();
        static List<Tareas> SegTareas = new List<Tareas>();
        static List<Tareas> TareasAsignadas = new List<Tareas>();
        static List<Tareas> Filtro_TareasAsignadas = new List<Tareas>();
        static List<Tareas> L_G_Tareas = new List<Tareas>();
        static int contador = 0, bandera = 0;
        static string s_Operador = "";
        static string s_Paquete = "";
        static List<Tareas> Filtro_SegTareas = new List<Tareas>();
        private TablaClientesCommands tablaClientes = new TablaClientesCommands();
        //primera ves que entras cargas variables 

        public ActionResult Index()
        {
            SeparaPermisos(Session["Permisos"].ToString());
            return View();
        }


        public ActionResult AsignacionAutomatica()
        {
            SeparaPermisos(Session["Permisos"].ToString());
            return View();
        }

        public ActionResult AsignacionManual()
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("../Home/login");
            }

            if (Operador != "")
            {
                ViewBag.DOperador = Operador;
            }
            else
            {
                CargaInicio_AsignacionTareas();
            }

            //obtenemos lista de los operadores
            SeparaPermisos(Session["Permisos"].ToString());
            return View("AsignacionManual", L_Tareas_Asignadas);
        }


        ///Llenamos lista de operadores
        private void Llena_Drop_Operadores()
        {
            ViewBag.L_Operadores = new SelectList(oUsuarios.Muestra_Usuario().Where(x => x.RolID == 2).ToList(), dataValueField: "UsuarioID", dataTextField: "Nombre").ToList();//llena el DropDownlist de clientes
        }

        private void LLena_Drop_Zonas()
        {
            ViewBag.L_Zonas = new SelectList(oZonas.Muestra_Zonas().ToList(), dataValueField: "Descripcion", dataTextField: "Descripcion").ToList();//llena el DropDownlist            
        }

        //Lista de asignacion manual por DATATABLE
        public JsonResult ListaTareasAsignadas()
        {
            TareasAsignadas = oTareas.Tareas_Generales().Where(x => x.Tarea_Activo != "0" && (x.Estatus == "Verificada" || x.Estatus == "Asignada" || x.Estatus == "Pendiente" || x.Estatus == "En curso" || x.Estatus == "Entrega"))
               .OrderBy(x => x.Usuario_Nombre).ToList();
            //agrupamos por operador para mostrar las tareas que tiene asignadas
            if (TareasAsignadas.Count > 0)
            {
                L_Tareas_Asignadas.Add(new Tareas
                {
                    Usuario_Nombre = TareasAsignadas[0].Usuario_Nombre,
                    Usuario_ID = TareasAsignadas[0].Usuario_ID,
                    Fecha_Asigacion = TareasAsignadas[0].Fecha_Asigacion,
                    Palabra_Clave = TareasAsignadas[0].Palabra_Clave,
                    Celular = TareasAsignadas[0].Celular,
                    C_Razon_Social = TareasAsignadas[0].C_Razon_Social,
                    Numero_Tareas = 1,
                    Estatus = TareasAsignadas[0].Estatus,
                    Comentarios = TareasAsignadas[0].Comentarios,
                    Guia = TareasAsignadas[0].Guia,
                    Fecha_Aceptada = TareasAsignadas[0].Fecha_Aceptada,
                    Fecha_Fin = TareasAsignadas[0].Fecha_Fin,
                    Evidencia = TareasAsignadas[0].Evidencia,
                    Tipo_Tarea_Descripcion = TareasAsignadas[0].Tipo_Tarea_Descripcion,
                    Des_Zona = TareasAsignadas[0].Des_Zona,
                    Nombre_Destinatario = TareasAsignadas[0].Nombre_Destinatario,
                    Direccion_Destinatario = TareasAsignadas[0].Direccion_Destinatario

                });
                int bandera = 0, contador = 0;
                foreach (var t in TareasAsignadas)
                {
                    if (bandera != 0)
                    {
                        if (t.Usuario_Nombre == L_Tareas_Asignadas[contador].Usuario_Nombre)
                        {
                            L_Tareas_Asignadas[contador].Numero_Tareas = L_Tareas_Asignadas[contador].Numero_Tareas + 1;
                        }
                        else
                        {
                            L_Tareas_Asignadas.Add(new Tareas
                            {
                                Usuario_Nombre = t.Usuario_Nombre,
                                Usuario_ID = t.Usuario_ID,
                                Fecha_Asigacion = t.Fecha_Asigacion,
                                Palabra_Clave = t.Palabra_Clave,
                                C_Razon_Social = t.C_Razon_Social,
                                Numero_Tareas = 1,
                                Celular = t.Celular,
                                Estatus = t.Estatus,
                                Comentarios = t.Comentarios,
                                Guia = t.Guia,
                                Fecha_Aceptada = t.Fecha_Aceptada,
                                Fecha_Fin = t.Fecha_Fin,
                                Evidencia = t.Evidencia,
                                Tipo_Tarea_Descripcion = t.Tipo_Tarea_Descripcion,
                                Des_Zona = t.Des_Zona,
                                Nombre_Destinatario = t.Nombre_Destinatario,
                                Direccion_Destinatario = t.Direccion_Destinatario
                            });
                            contador++;
                        }
                    }
                    else
                    {
                        bandera++;
                    }
                }
            }
            L_Tareas_Asignadas = indice_general(L_Tareas_Asignadas);
            return Json(new { data = L_Tareas_Asignadas }, JsonRequestBehavior.AllowGet);
        }

        private List<Tareas> indice_general(List<Tareas> lista)
        {
            int indice = 1;
            foreach (Tareas guia in lista)
            {
                guia.ID_Temporal = indice;
                indice++;
            }
            return lista;
        }


        public ActionResult LimpiarSeleccion()
        {
            CargaInicio_AsignacionTareas();
            SeparaPermisos(Session["Permisos"].ToString());
            return View("AsignacionManual", L_Tareas_Asignadas);
        }

        private void CargaInicio_AsignacionTareas()
        {
            Operador = "";
            L_Tareas_Asignadas.Clear();
            ViewBag.DOperador = "Selecciona un Operador";
            Llena_Drop_Operadores();

        }

        [HttpPost]
        public JsonResult Exportar()
        {
            string fileName = "Tareas_Asignadas_" + DateTime.Now.ToString("yyyy-MM-dd") + ".xlsx";
            DataTable dt = new DataTable();

            string fullPath = Path.Combine(Server.MapPath("~/"), fileName);
            dt.Locale = new System.Globalization.CultureInfo("es-PE");
            //   string handle = Guid.NewGuid().ToString();

            dt.Columns.Add("Nombre de Operador", typeof(string));
            dt.Columns.Add("Celular", typeof(string));
            dt.Columns.Add("Fecha de asignacion", typeof(string));
            dt.Columns.Add("Tipo de Tarea", typeof(string));
            dt.Columns.Add("Cliente", typeof(string));
            dt.Columns.Add("Zona", typeof(string));
            dt.Columns.Add("Numero de Guia", typeof(string));
            dt.Columns.Add("Nombre Destinatario", typeof(string));
            dt.Columns.Add("Direccion Destinatario", typeof(string));

            foreach (var rp in L_Tareas_Asignadas)
            {
                foreach (var usu in TareasAsignadas)
                {
                    if (rp.Usuario_ID == usu.Usuario_ID)
                    {
                        dt.Rows.Add(new object[]
                                   {
                     rp.Usuario_Nombre,
                     rp.Celular,
                     rp.Fecha_Asigacion,
                     rp.Tipo_Tarea_Descripcion,
                     rp.C_Razon_Social,
                     rp.Des_Zona,
                     rp.Guia,
                     rp.Nombre_Destinatario,
                     rp.Direccion_Destinatario
                       });
                    }

                }


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

        [HttpGet]
        public ActionResult Download(string file)
        {
            //get the temp folder and file path in server
            string fullPath = Path.Combine(Server.MapPath("~/"), file);
            //return the file for download, this is an Excel 
            //so I set the file content type to "application/vnd.ms-excel"
            return File(fullPath, "application/vnd.ms-excel", file);
        }


        public ActionResult Inicia_Vista_Seleccion()
        {
            Operador = "";
            Lista_Paquetes.Clear();
            return RedirectToAction("Asignar");
        }

        public ActionResult Asignar()
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("../Home/login");
            }
            Llena_Drop_Operadores();
            ViewBag.DOperador = "Selecciona un Operador";
            SeparaPermisos(Session["Permisos"].ToString());
            return View();
        }


        public void inicia_p()
        {
            foreach (Paquetes op in Lista_Paquetes)
            {
                op.p = "";
            }
        }



        [HttpGet]
        public ActionResult Aceptar(string lista, string operador)
        {
            if (lista != "" && operador != "" && bandera == 0)//asignamos tareas
            {
                try
                {
                    bandera++;
                    string[] indices = lista.Split('.');
                    var fecha_asignacion = DateTime.Now;
                    Tareas tarea = new Tareas();

                    foreach (var _indice in indices)
                    {

                        foreach (var paquete in Lista_Paquetes)
                        {
                            if (Convert.ToInt32(_indice) == paquete.ID_Temporal)
                            {
                                tarea.Tarea_ID = 0;
                                tarea.Guia = paquete.Guia;
                                tarea.Estatus = "1";
                                tarea.Intento = 0;
                                tarea.Comentarios = "";
                                tarea.Tipo_Tarea = 2;
                                tarea.Usuario_ID = Convert.ToInt32(operador);
                                tarea.Fecha_Asigacion = fecha_asignacion;
                                oTareas.Alta_Tarea(tarea);
                            }
                        }

                    }
                    return Json(new { fileName = "1" }, JsonRequestBehavior.AllowGet);
                    // return RedirectToAction("../AsignacionTareas/AsignacionManual");
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
                    return null;
                }
            }
            else
            {
                bandera = 0;
                Llena_Drop_Operadores();
                return Json(new { fileName = "0" }, JsonRequestBehavior.AllowGet);
                // return RedirectToAction("../AsignacionTareas/AsignacionManual");
            }


        }

        [HttpGet]
        public JsonResult LLena_tabla()
        {
            Lista_Paquetes = oPaquetes.Muestra_Paquetes_Para_Asignacion().OrderByDescending(x => x.FechaIngresoBodega).ToList();
            Lista_Paquetes = indice_general(Lista_Paquetes);
            inicia_p();
            return Json(new { data = Lista_Paquetes }, JsonRequestBehavior.AllowGet);
        }




        private List<Paquetes> indice_general(List<Paquetes> lista)
        {
            int indice = 1;
            foreach (var registro in lista)
            {
                registro.ID_Temporal = indice;
                indice++;
            }
            return lista;
        }


        public JsonResult Editar(string id)
        {

            return Json(new { data = TareasAsignadas.Where(x => x.Usuario_Nombre == id) }, JsonRequestBehavior.AllowGet);

        }

        public List<Tareas> GeneraLista(string fecha)
        {

            string celular = "";
            foreach (var t in L_Tareas_Asignadas.Where(x => x.Tarea_Activo != "0"))
            {
                if (Convert.ToDateTime(fecha) == t.Fecha_Asigacion)
                {
                    celular = t.Celular;
                }
            }
            return oTareas.TareasAsignadas_Fecha(fecha, celular).Where(x => x.Tarea_Activo != "0").ToList();
        }


        public JsonResult Eliminar_Tarea(string guia, string fecha_asignacion, string operador)
        {
            try
            {
                //eliminamos la tarea que tiene ese numero de guia en la base de datos
                var fecha = Convert.ToDateTime(fecha_asignacion);
                oTareas.Elimina_Tarea(guia, fecha);

                return Json(new { data = TareasAsignadas.Where(x => x.Usuario_Nombre == operador) }, JsonRequestBehavior.AllowGet);

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
                return Json(new { data = TareasAsignadas.Where(x => x.Usuario_Nombre == operador) }, JsonRequestBehavior.AllowGet);
            }

        }




        public ActionResult Detalles(List<int> id)
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("../Home/login");
            }
            //se muestra la lista de tareas asignadas a un operador en la misma fecha
            //de asignacion
            //si la tarea ya tiene un estatus distinto a asignada no se puede editar 
            //corroborar logica de asignacion 
            SeparaPermisos(Session["Permisos"].ToString());
            return View(oTareas.Muestra_Tareas().ToList());
        }
        public ActionResult Historial()//TABLA
        {
            if (Session["UserID"] == null)
            {

                return RedirectToAction("../Home/login");
            }
            //se muestra el historial y el estatus actual de las tareas 
            var lista = oTareas.Tareas_Generales().ToList();
            var lista_usuarios = oUsuarios.Muestra_Usuario().Where(x => x.RolID == 2).ToList();
            L_op = new SelectList(lista_usuarios, dataValueField: "Nombre", dataTextField: "Nombre").ToList();//llena el DropDownlist de clientes
            ViewBag.L_Operadores = L_op;
            ViewBag.DOperador = "Selecciona un operador";//opcion default de dropdown
            var ListaClientes = tablaClientes.Muestra_Clientes();
            L_Clientes = new SelectList(ListaClientes, dataValueField: "RazonSocial", dataTextField: "RazonSocial").ToList();
            ViewBag.DCliente = "Selecciona un Cliente";
            ViewBag.List_Clientes = L_Clientes;

            var Lista_Paquetes = oEstatustar.Muestra_EstatusTareas().Where(x => x.EstatusTareasID == 5 || x.EstatusTareasID == 6);//llena el DropDownlist de estatus
            L_paq = new SelectList(Lista_Paquetes, dataValueField: "Descripcion", dataTextField: "Descripcion").ToList();
            ViewBag.L_Paquetes = L_paq;
            ViewBag.DPaquete = "Selecciona un estatus";
            SeparaPermisos(Session["Permisos"].ToString());
            return View();
        }


        public List<Tareas> indice_temporal(List<Tareas> lista)
        {

            int indice = 1;
            foreach (var l in lista)
            {
                l.ID_Temporal = indice;
                indice++;
            }
            SeparaPermisos(Session["Permisos"].ToString());
            return lista;

        }

        public JsonResult ListaTareas()
        {
            SegTareas = oTareas.Tareas_Generales().Where(x => x.Estatus == "Fallida" || x.Estatus == "Exitosa").OrderByDescending(x => x.Guia).ToList();
            SegTareas = indice_temporal(SegTareas);
            Filtro_SegTareas = SegTareas;
            return Json(new { data = SegTareas }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult ListaTareasFiltro(string fecha_inicio, string fecha_final, string cliente, string termino, string Operador, string Paquete)
        {
            try
            {
                if (Operador == "" && Paquete == "" && cliente == "" && fecha_inicio == "" && fecha_final == "")
                {
                    Filtro_SegTareas = SegTareas.ToList();
                    Filtro_SegTareas = indice_temporal(Filtro_SegTareas);
                    return Json(new { data = Filtro_SegTareas }, JsonRequestBehavior.AllowGet);
                }
                else if (Operador != "" && Paquete == "" && cliente == "" && fecha_inicio == "" && fecha_final == "")
                {
                    Filtro_SegTareas = SegTareas.Where(x => x.Usuario_Nombre == Operador).ToList();
                    Filtro_SegTareas = indice_temporal(Filtro_SegTareas);
                    return Json(new { data = Filtro_SegTareas }, JsonRequestBehavior.AllowGet);
                }
                else if (Operador == "" && Paquete != "" && cliente == "" && fecha_inicio == "" && fecha_final == "")
                {
                    Filtro_SegTareas = SegTareas.Where(x => x.Estatus == Paquete).ToList();
                    Filtro_SegTareas = indice_temporal(Filtro_SegTareas);
                    return Json(new { data = Filtro_SegTareas }, JsonRequestBehavior.AllowGet);
                }
                else if (Operador == "" && Paquete == "" && cliente != "" && fecha_inicio == "" && fecha_final == "")
                {
                    Filtro_SegTareas = SegTareas.Where(x => x.C_Razon_Social == cliente).ToList();
                    Filtro_SegTareas = indice_temporal(Filtro_SegTareas);
                    return Json(new { data = Filtro_SegTareas }, JsonRequestBehavior.AllowGet);
                }
                else if (Operador != "" && Paquete != "" && cliente != "" && fecha_inicio == "" && fecha_final == "")
                {
                    Filtro_SegTareas = SegTareas.Where(x => x.Usuario_Nombre == Operador && x.Estatus == Paquete && x.C_Razon_Social == cliente).ToList();
                    Filtro_SegTareas = indice_temporal(Filtro_SegTareas);
                    return Json(new { data = Filtro_SegTareas }, JsonRequestBehavior.AllowGet);
                }
                if (termino == "on")
                {
                    if (fecha_final != "" && fecha_inicio != "")
                    {
                        var separa = fecha_final.Split('/');
                        var separa2 = fecha_inicio.Split('/');
                        fecha_inicio = separa2[2] + "-" + separa2[0] + "-" + separa2[1];
                        fecha_final = separa[2] + "-" + separa[0] + "-" + separa[1];
                        if (Operador == "" && Paquete == "" && cliente == "")
                        {
                            Filtro_SegTareas = SegTareas.Where(x => x.Fecha_Fin >= Convert.ToDateTime(fecha_inicio) && x.Fecha_Fin <= Convert.ToDateTime(fecha_final)).ToList();
                            Filtro_SegTareas = indice_temporal(Filtro_SegTareas);
                            return Json(new { data = Filtro_SegTareas }, JsonRequestBehavior.AllowGet);
                        }
                        else if (Operador != "" && Paquete == "" && cliente == "")
                        {
                            Filtro_SegTareas = SegTareas.Where(x => x.Fecha_Fin >= Convert.ToDateTime(fecha_inicio) && x.Fecha_Fin <= Convert.ToDateTime(fecha_final) && x.Usuario_Nombre == Operador).ToList();
                            Filtro_SegTareas = indice_temporal(Filtro_SegTareas);
                            return Json(new { data = Filtro_SegTareas }, JsonRequestBehavior.AllowGet);
                        }
                        else if (Operador == "" && Paquete != "" && cliente == "")
                        {
                            Filtro_SegTareas = SegTareas.Where(x => x.Fecha_Fin >= Convert.ToDateTime(fecha_inicio) && x.Fecha_Fin <= Convert.ToDateTime(fecha_final) && x.Estatus == Paquete).ToList();
                            Filtro_SegTareas = indice_temporal(Filtro_SegTareas);
                            return Json(new { data = Filtro_SegTareas }, JsonRequestBehavior.AllowGet);
                        }
                        else if (Operador == "" && Paquete == "" && cliente != "")
                        {
                            Filtro_SegTareas = SegTareas.Where(x => x.Fecha_Fin >= Convert.ToDateTime(fecha_inicio) && x.Fecha_Fin <= Convert.ToDateTime(fecha_final) && x.C_Razon_Social == cliente).ToList();
                            Filtro_SegTareas = indice_temporal(Filtro_SegTareas);
                            return Json(new { data = Filtro_SegTareas }, JsonRequestBehavior.AllowGet);
                        }
                        else if (Operador != "" && Paquete != "" && cliente != "")
                        {
                            Filtro_SegTareas = SegTareas.Where(x => x.Fecha_Fin >= Convert.ToDateTime(fecha_inicio) && x.Fecha_Fin <= Convert.ToDateTime(fecha_final)
                            && x.Usuario_Nombre == Operador && x.Estatus == Paquete && x.C_Razon_Social == cliente).ToList();
                            Filtro_SegTareas = indice_temporal(Filtro_SegTareas);
                            return Json(new { data = Filtro_SegTareas }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else if (fecha_inicio != "" && fecha_final == "")
                    {
                        var separa2 = fecha_inicio.Split('/');
                        fecha_inicio = separa2[2] + "-" + separa2[0] + "-" + separa2[1];

                        if (Operador != "" && Paquete == "" && cliente == "")
                        {
                            Filtro_SegTareas = SegTareas.Where(x => x.Fecha_Fin >= Convert.ToDateTime(fecha_inicio) && x.Usuario_Nombre == Operador).ToList();
                            Filtro_SegTareas = indice_temporal(Filtro_SegTareas);
                            return Json(new { data = Filtro_SegTareas }, JsonRequestBehavior.AllowGet);
                        }
                        else if (Operador == "" && Paquete != "" && cliente == "")
                        {
                            Filtro_SegTareas = SegTareas.Where(x => x.Fecha_Fin >= Convert.ToDateTime(fecha_inicio) && x.Estatus == Paquete).ToList();
                            Filtro_SegTareas = indice_temporal(Filtro_SegTareas);
                            return Json(new { data = Filtro_SegTareas }, JsonRequestBehavior.AllowGet);
                        }
                        else if (Operador == "" && Paquete == "" && cliente != "")
                        {
                            Filtro_SegTareas = SegTareas.Where(x => x.Fecha_Fin >= Convert.ToDateTime(fecha_inicio) && x.C_Razon_Social == cliente).ToList();
                            Filtro_SegTareas = indice_temporal(Filtro_SegTareas);
                            return Json(new { data = Filtro_SegTareas }, JsonRequestBehavior.AllowGet);
                        }
                        else if (Operador != "" && Paquete != "" && cliente != "")
                        {
                            Filtro_SegTareas = SegTareas.Where(x => x.Fecha_Fin >= Convert.ToDateTime(fecha_inicio) && x.Usuario_Nombre == Operador
                            && x.Estatus == Paquete && x.C_Razon_Social == cliente).ToList();
                            Filtro_SegTareas = indice_temporal(Filtro_SegTareas);
                            return Json(new { data = Filtro_SegTareas }, JsonRequestBehavior.AllowGet);
                        }
                        else if (Operador == "" && Paquete == "" && cliente == "")
                        {
                            Filtro_SegTareas = SegTareas.Where(x => x.Fecha_Fin >= Convert.ToDateTime(fecha_inicio)).ToList();
                            Filtro_SegTareas = indice_temporal(Filtro_SegTareas);
                            return Json(new { data = Filtro_SegTareas }, JsonRequestBehavior.AllowGet);
                        }

                    }
                    return Json(new { data = Filtro_SegTareas }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    Filtro_SegTareas = SegTareas.ToList();
                    Filtro_SegTareas = indice_temporal(Filtro_SegTareas);
                    return Json(new { data = Filtro_SegTareas }, JsonRequestBehavior.AllowGet);
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
                return Json(new { data = SegTareas }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult LimpiarT()//boton de limpiar que asigna valores por default y devuelve la lista
        {
            SegTareas = oTareas.Muestra_Tareas().Where(x => x.Estatus == "Fallida" || x.Estatus == "Exitosa").OrderByDescending(x => x.Guia).ToList();
            ViewBag.L_Operadores = L_op;
            ViewBag.L_Paquetes = L_paq;
            contador = 0;
            SeparaPermisos(Session["Permisos"].ToString());
            return RedirectToAction("Historial", SegTareas);
        }//


        [HttpPost]
        public JsonResult ExportarTareas()
        {

            string fileName = "HistorialdeTareas_" + DateTime.Now.ToString("yyyy-MM-dd") + ".xlsx";

            DataTable dt = new DataTable();

            string fullPath = Path.Combine(Server.MapPath("~/"), fileName);
            dt.Locale = new System.Globalization.CultureInfo("es-PE");

            dt.Columns.Add("Cliente", typeof(string));
            dt.Columns.Add("Guia", typeof(string));
            dt.Columns.Add("Estatus de termino", typeof(string));
            dt.Columns.Add("Operador", typeof(string));
            dt.Columns.Add("Estatus", typeof(string));
            dt.Columns.Add("Comentarios", typeof(string));
            dt.Columns.Add("Evidencia", typeof(string));
            //direccion de destinatario 
            dt.Columns.Add("Nombre de Destinatario", typeof(string));
            dt.Columns.Add("Direccion de Destinatario", typeof(string));
            dt.Columns.Add("Latitud", typeof(string));
            dt.Columns.Add("Longitud", typeof(string));

            //si no se realizo una busqueda cargamos todos los datos al documento

            foreach (var rp in SegTareas)
            {

                contador = contador + 1;

                dt.Rows.Add(new object[]
                {
                      rp.C_Razon_Social,
                      rp.Guia,
                      rp.Fecha_Fin,
                      rp.Usuario_Nombre,
                      rp.Estatus,
                      rp.Comentarios,
                      rp.Evidencia,
                      rp.Nombre_Destinatario,
                      rp.Direccion_Destinatario,
                      rp.Latitud,
                      rp.longitud

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