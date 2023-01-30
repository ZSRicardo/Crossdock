using Crossdock.Context.Commands;
using Crossdock.Context.Queries;
using Crossdock.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web.Mvc;
using Microsoft.VisualBasic.CompilerServices;
using Microsoft.Ajax.Utilities;
using Habanero.Util;
using DocumentFormat.OpenXml.Bibliography;
using System.IO;

namespace Crossdock.Controllers
{
    public class RecepcionController : Controller
    {
        private TablaGuiasQueries _tablaGuiasQueries = new TablaGuiasQueries();//llamada de clase
        private TablaPaquetesCommands _tablaPaquetesCommands = new TablaPaquetesCommands();
        private TablaMotivoDevolucionCommands _tablaMotivodevolucion = new TablaMotivoDevolucionCommands();
        static DatosGuiaViewModel datosGuia = new DatosGuiaViewModel();
        static DatosGuiaViewModel devolucionGuia = new DatosGuiaViewModel();
        public static List<DatosGuiaViewModel> oList = new List<DatosGuiaViewModel>();
        public static List<DatosGuiaViewModel> DList = new List<DatosGuiaViewModel>();
        public static List<DatosGuiaViewModel> SelectList = new List<DatosGuiaViewModel>();
        static List<SelectListItem> list = new List<SelectListItem>();
        // GET: Recepcion
        public ActionResult Index()
        {
            // Recepcion paquetes 
            if (Session["UserID"] == null)
            {
                return RedirectToAction("../Home/login");
            }
            //Devolvemos siempre la misma guia general para que se muestren todos los paquetes ingresados
            var Listageneral = oList.Union(SelectList);
            if (Listageneral == null || Listageneral.Count() == 0)
            {
                ViewBag.habilitar = true;
            }
            else {
                ViewBag.habilitar = false;
            }

            SeparaPermisos(Session["Permisos"].ToString());
            return View("Index", Listageneral.ToList());
        }

        // POST: Recepcion
        [HttpPost]
        public ActionResult Index(string inputGuia)
        {
            try
            {
                // Buscar datos de guia
                datosGuia = _tablaGuiasQueries.Muestra_Guia(inputGuia);


                // Guia no encontrada
                if (string.IsNullOrWhiteSpace(datosGuia.Guia))
                {
                    TempData["Alert"] = "El número de guía ingresada no existe";
                    TempData["AlertCssClass"] = "alert-danger";

                    // no redirige a nueva accion, solo renderiza de nuevo la vista
                    SeparaPermisos(Session["Permisos"].ToString());
                    //Retornamos siempre el listado general para que la vista se actualice con todos los paquetes ingresados
                    var Listageneral = oList.Union(SelectList);
                    if (Listageneral == null || Listageneral.Count() == 0)
                    {
                        ViewBag.habilitar = true;
                    }
                    else
                    {
                        ViewBag.habilitar = false;
                    }
                    return View("Index", Listageneral.ToList());
                }


                for (int i = 0; i < oList.Count(); i++)

                    //Busca si texto se encuentra en ArrayList.
                    if (oList[i].Guia.Contains(inputGuia))
                    {

                        TempData["Alert"] = "ese numero de guia ya fue ingresado";
                        SeparaPermisos(Session["Permisos"].ToString());
                        //Retornamos siempre el listado general para que la vista se actualice con todos los paquetes ingresados
                        var Listageneral = oList.Union(SelectList);
                        if (Listageneral == null || Listageneral.Count() == 0)
                        {
                            ViewBag.habilitar = true;
                        }
                        else
                        {
                            ViewBag.habilitar = false;
                        }
                        return View("Index", Listageneral.ToList());

                    }
                    else if (oList[i].Detalles != 0)
                    {
                        if (oList == null || oList.Count() == 0)
                        {
                            ViewBag.habilitar = true;
                        }
                        else
                        {
                            ViewBag.habilitar = false;
                        }
                        return View("Index", oList);
                    }

                for (int i = 0; i < SelectList.Count(); i++)

                    //Busca si texto se encuentra en ArrayList.
                    if (SelectList[i].Guia.Contains(inputGuia))
                    {

                        TempData["Alert"] = "ese numero de guia ya fue ingresado";
                        SeparaPermisos(Session["Permisos"].ToString());
                        //Retornamos siempre el listado general para que la vista se actualice con todos los paquetes ingresados
                        var Listageneral = oList.Union(SelectList);
                        if (Listageneral == null || Listageneral.Count() == 0)
                        {
                            ViewBag.habilitar = true;
                        }
                        else
                        {
                            ViewBag.habilitar = false;
                        }
                        return View("Index", Listageneral.ToList());

                    }

                //Validaciones por estatus de paquete y numero de intentos
                if (Convert.ToInt32(datosGuia.EstatusPaqueteID) == 1 && Convert.ToInt32(datosGuia.NumeroIntento) <= 2)
                {
                    TempData["Alert"] = "Este paquete ya ha tenido recepcion y esta en espera de asignación";
                    SeparaPermisos(Session["Permisos"].ToString());
                    //Retornamos siempre el listado general para que la vista se actualice con todos los paquetes ingresados
                    var Listageneral = oList.Union(SelectList);
                    if (Listageneral == null || Listageneral.Count() == 0)
                    {
                        ViewBag.habilitar = true;
                    }
                    else
                    {
                        ViewBag.habilitar = false;
                    }
                    return View("Index", Listageneral.ToList());
                }
                else if (Convert.ToInt32(datosGuia.EstatusPaqueteID) == 1 && Convert.ToInt32(datosGuia.NumeroIntento) >= 3)
                {
                    TempData["Alert"] = "El paquete ya cuenta con mas de 2 intentos, por favor marcar para devolucion";
                    SeparaPermisos(Session["Permisos"].ToString());
                    //Retornamos siempre el listado general para que la vista se actualice con todos los paquetes ingresados
                    oList.Add(datosGuia);
                    var Listageneral = oList.Union(SelectList);
                    if (Listageneral == null || Listageneral.Count() == 0)
                    {
                        ViewBag.habilitar = true;
                    }
                    else
                    {
                        ViewBag.habilitar = false;
                    }
                    return View("Index", Listageneral.ToList());

                }
                else if (Convert.ToInt32(datosGuia.EstatusPaqueteID) == 3)
                {
                    TempData["Alert"] = "El paquete con ese numero de guia ya se encuentra en camino";
                    SeparaPermisos(Session["Permisos"].ToString());
                    //Retornamos siempre el listado general para que la vista se actualice con todos los paquetes ingresados
                    var Listageneral = oList.Union(SelectList);
                    if (Listageneral == null || Listageneral.Count() == 0)
                    {
                        ViewBag.habilitar = true;
                    }
                    else
                    {
                        ViewBag.habilitar = false;
                    }
                    return View("Index", Listageneral.ToList());

                }
                else if (Convert.ToInt32(datosGuia.EstatusPaqueteID) == 4)
                {
                    TempData["Alert"] = "El paquete con ese numero de guia ya se encuentra asignado";
                    SeparaPermisos(Session["Permisos"].ToString());
                    //Retornamos siempre el listado general para que la vista se actualice con todos los paquetes ingresados
                    var Listageneral = oList.Union(SelectList);
                    if (Listageneral == null || Listageneral.Count() == 0)
                    {
                        ViewBag.habilitar = true;
                    }
                    else
                    {
                        ViewBag.habilitar = false;
                    }
                    return View("Index", Listageneral.ToList());

                }
                else if (Convert.ToInt32(datosGuia.EstatusPaqueteID) == 5)
                {
                    TempData["Alert"] = "El paquete con ese numero de guia ya ha sido entregado";
                    SeparaPermisos(Session["Permisos"].ToString());
                    //Retornamos siempre el listado general para que la vista se actualice con todos los paquetes ingresados
                    var Listageneral = oList.Union(SelectList);
                    if (Listageneral == null || Listageneral.Count() == 0)
                    {
                        ViewBag.habilitar = true;
                    }
                    else
                    {
                        ViewBag.habilitar = false;
                    }
                    return View("Index", Listageneral.ToList());

                }
                else if (Convert.ToInt32(datosGuia.EstatusPaqueteID) == 6)
                {
                    TempData["Alert"] = "El paquete con ese numero de guia ya ha sido devuelto";
                    SeparaPermisos(Session["Permisos"].ToString());
                    //Retornamos siempre el listado general para que la vista se actualice con todos los paquetes ingresados
                    var Listageneral = oList.Union(SelectList);
                    if (Listageneral == null || Listageneral.Count() == 0)
                    {
                        ViewBag.habilitar = true;
                    }
                    else
                    {
                        ViewBag.habilitar = false;
                    }
                    return View("Index", Listageneral.ToList());

                }
                else if (datosGuia.EstatusPaqueteID == null || Convert.ToInt32(datosGuia.EstatusPaqueteID) == 2 || Convert.ToInt32(datosGuia.EstatusPaqueteID) == 7)
                {
                    //En caso de que el paquete tenga un estatus de recepcion, se encuentre de regreso a bodega o este en bodega se agrega a la lista
                    if (oList == null || oList.Count() == 0)
                    {
                        ViewBag.habilitar = true;
                    }
                    else
                    {
                        ViewBag.habilitar = false;
                    }
                    oList.Add(datosGuia);


                }

                SeparaPermisos(Session["Permisos"].ToString());
                return RedirectToAction("Index");
            }
            catch (Exception ex) // Pendiente manejo de excepcion
            {
                if (oList == null || oList.Count() == 0)
                {
                    ViewBag.habilitar = true;
                }
                else
                {
                    ViewBag.habilitar = false;
                }
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
                SeparaPermisos(Session["Permisos"].ToString());
                return View("Index", oList);
            }
        }


        public ActionResult Remover(string Guia)
        {
            // Acceso a vista sin tener objecto guia
            if (oList.Count == 0)
            {
                SeparaPermisos(Session["Permisos"].ToString());
                RedirectToAction("Index");
            }
            oList = oList.Where(x => x.Guia != Guia).ToList();
            SelectList = SelectList.Where(x => x.Guia != Guia).ToList();
            var Listageneral = oList.Union(SelectList);
            if (Listageneral == null || Listageneral.Count() == 0)
            {
                ViewBag.habilitar = true;
            }
            else
            {
                ViewBag.habilitar = false;
            }
            SeparaPermisos(Session["Permisos"].ToString());

            return View("Index", Listageneral.ToList());
        }

        public ActionResult Resumen(string Guia)
        {
            if (Session["UserID"] == null)
            {
                SeparaPermisos(Session["Permisos"].ToString());
                return RedirectToAction("../Home/login");
            }

            // Acceso a vista sin tener objecto guia
            if (Guia == null)
            {
                var Listageneral = oList.Union(SelectList);
                if (Listageneral == null || Listageneral.Count() == 0)
                {
                    ViewBag.habilitar = true;
                }
                else
                {
                    ViewBag.habilitar = false;
                }
                SeparaPermisos(Session["Permisos"].ToString());
                RedirectToAction("Index", Listageneral.ToList());
            }


            // Buscar datos de guia
            DList = oList.Where(x => x.Guia == Guia).ToList();

            var Lista_Motivos = _tablaMotivodevolucion.Muestra_Devoluciones();//consumo de sp
            list = new SelectList(Lista_Motivos, dataValueField: "DevolucionID", dataTextField: "Detalles").ToList();//llena el DropDownlist
            ViewBag.Lista_Motivos = list;
            ViewBag.L_Motivos = "Selecciona un motivo";
            SeparaPermisos(Session["Permisos"].ToString());
            return View("Resumen", DList);
        }

        // Este Metodo sera ejecutado si un paquete es devuelto (form submit en vista Resumen)
        // POST: Recepcion/Resumen
        public ActionResult DevolucionPost(string Lista_Motivos)
        {
            if (Session["UserID"] == null)
            {

                return RedirectToAction("../Home/login");
            }

            if (Lista_Motivos == null || Lista_Motivos == "")
            {
                TempData["Alert"] = "Selecciona un motivo de devolucion";
                TempData["AlertCssClass"] = "alert-danger";
                ViewBag.Lista_Motivos = list;
                ViewBag.L_Motivos = "Selecciona un motivo";
                SeparaPermisos(Session["Permisos"].ToString());
                return View("Resumen", DList);
            }

            var lista = DList.Last();
            SelectList.Add(lista);
            oList.Remove(lista);
            lista.Detalles = Convert.ToInt32(Lista_Motivos);


            var Listageneral = oList.Union(SelectList);
            if (Listageneral == null || Listageneral.Count() == 0)
            {
                ViewBag.habilitar = true;
            }
            else
            {
                ViewBag.habilitar = false;
            }
            SeparaPermisos(Session["Permisos"].ToString());
            return RedirectToAction("Index", Listageneral.ToList());
        }


        // GET: Recepcion/Recibo       
        public ActionResult Recibo()
        {
            try { 
            var Listageneral = oList.Union(SelectList);
            if (Listageneral.Count() == 0)
            {
                TempData["Alert"] = "Introduce un numero de guia";
                TempData["AlertCssClass"] = "alert-danger";
                if (Listageneral == null || Listageneral.Count() == 0)
                {
                    ViewBag.habilitar = true;
                }
                else
                {
                    ViewBag.habilitar = false;
                }
                return View("Index", Listageneral.ToList());
            }

            SeparaPermisos(Session["Permisos"].ToString());

            if (Listageneral == null || Listageneral.Count() == 0)
            {
                ViewBag.habilitar = true;
            }
            else
            {
                ViewBag.habilitar = false;
            }
            ViewBag.Total = Listageneral.Count();
            ViewBag.Ingresados = oList.Count;
            ViewBag.Devueltos = SelectList.Count;
            return View(Listageneral.ToList());
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
                var Listageneral = oList.Union(SelectList);
                if (Listageneral.Count() == 0)
                {
                    TempData["Alert"] = "Introduce un numero de guia";
                    TempData["AlertCssClass"] = "alert-danger";
                    if (Listageneral == null || Listageneral.Count() == 0)
                    {
                        ViewBag.habilitar = true;
                    }
                    else
                    {
                        ViewBag.habilitar = false;
                    }
                    return View("Index", Listageneral.ToList());
                }
                return View(Listageneral.ToList());
            }
        }

        // GET: Recepcion/Recibo        
        public ActionResult ReciboAceptar()
        {

            try { 
            if (oList.Count() == 0 && SelectList.Count() == 0)
            {
                RedirectToAction("Index", oList);
            }
            else
            {

                // Registrar datos de paquetes escaneados (SP)
                foreach (var item in oList)
                {
                    Debug.WriteLine($"Guias: {item.Guia}");

                    // Mapear datos de guia a modelo paquete y agregar datos faltantes
                    if (item.paqID == null || item.paqID == "")
                    {
                        Paquetes paquete = new Paquetes
                        {
                            FechaRecepcion = DateTime.Now,
                            Guia = item.Guia,
                            CodigoRecoleccion = "",
                            UsuarioID = (int)Session["UserID"],
                            ClienteID = (int)Session["UserClienteID"],
                            EstatusPaqueteID = 1,//paquete en recepcion
                            GuiID = item.GuiaID,
                            MotivoDevolucionID = item.Detalles,
                            Intento = 1,
                            ZonaID = item.Zonaid


                        }; _tablaPaquetesCommands.Alta_Paquetes(paquete);
                    }
                    else if (item.paqID != null || item.paqID != "")
                    {
                        var id = Convert.ToInt32(item.paqID);

                        Paquetes paquete = new Paquetes
                        {
                            FechaRecepcion = DateTime.Now,
                            Guia = item.Guia,
                            CodigoRecoleccion = "",
                            UsuarioID = (int)Session["UserID"],
                            ClienteID = (int)Session["UserClienteID"],
                            EstatusPaqueteID = 1,//paquete en recepcion
                            GuiID = item.GuiaID,
                            MotivoDevolucionID = item.Detalles,
                            Intento = Convert.ToInt32(item.NumeroIntento) + 1,
                            ZonaID = item.Zonaid


                        };
                        _tablaPaquetesCommands.desactivaPaquete(id);
                        _tablaPaquetesCommands.Alta_Paquetes(paquete);
                    }
                }

                if (SelectList != null)
                {
                    foreach (var item in SelectList)
                    {
                        Debug.WriteLine($"Guias: {item.Guia}");

                        // Mapear datos de guia a modelo paquete y agregar datos faltantes

                        var id = Convert.ToInt32(item.paqID);
                        Paquetes paquete = new Paquetes
                        {
                            FechaRecepcion = DateTime.Now,
                            Guia = item.Guia,
                            CodigoRecoleccion = "",
                            UsuarioID = (int)Session["UserID"],
                            ClienteID = (int)Session["UserClienteID"],
                            EstatusPaqueteID = 6,//paquete devuelto
                            GuiID = item.GuiaID,
                            MotivoDevolucionID = item.Detalles,
                            Intento = Convert.ToInt32(item.NumeroIntento),
                            ZonaID = item.Zonaid


                        };
                        _tablaPaquetesCommands.desactivaPaquete(id);
                        _tablaPaquetesCommands.Alta_Paquetes(paquete);
                    }
                }
            }

            oList.Clear();
            DList.Clear();
            SelectList.Clear();
            if (oList.Count() == 0 && DList.Count() == 0)
            {
                ViewBag.habilitar = true;
            }
            else
            {
                ViewBag.habilitar = false;
            }
            return RedirectToAction("Index", oList);
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
                if (oList.Count() == 0 && DList.Count() == 0)
                {
                    ViewBag.habilitar = true;
                }
                else
                {
                    ViewBag.habilitar = false;
                }
                return RedirectToAction("Index", oList);
            }
        }
        public ActionResult Cancelar()
        {
            oList.Clear();
            DList.Clear();
            SelectList.Clear();
            if (DList.Count() == 0 && oList.Count() == 0)
            {
                ViewBag.habilitar = true;
            }
            else
            {
                ViewBag.habilitar = false;
            }
            SeparaPermisos(Session["Permisos"].ToString());
            return View("Index", oList);
        }

        public ActionResult ResumenCancelar()
        {
            var Listageneral = oList.Union(SelectList);
            if (Listageneral.Count() == 0)
            {
                ViewBag.habilitar = true;
            }
            else
            {
                ViewBag.habilitar = false;
            }
            SeparaPermisos(Session["Permisos"].ToString());
            return View("Index", Listageneral.ToList());
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
