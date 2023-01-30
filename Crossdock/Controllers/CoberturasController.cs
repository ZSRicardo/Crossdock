using Crossdock.Context.Commands;
using Crossdock.Models;
using Microsoft.VisualBasic.CompilerServices;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace Crossdock.Controllers
{
    public class CoberturasController : Controller
    {
        private TablaCoberturasCommands objCoberturas = new TablaCoberturasCommands();
        private TablaDeliveryCommands objDelivery = new TablaDeliveryCommands();
        private TablaZonasCommands objZona = new TablaZonasCommands();

        // GET: Coberturas
        public ActionResult Index()
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("../Home/login");
            }

            SeparaPermisos(Session["Permisos"].ToString());


            return View();
        }

        public JsonResult Listatareas()
        {
            var lista = objCoberturas.Muestra_Coberturas().ToList();
            lista = indice_general(lista);
            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }
        private List<Coberturas> indice_general(List<Coberturas> lista)
        {
            int indice = 1;
            foreach (Coberturas guia in lista)
            {
                guia.ID_Temporal = indice;
                indice++;
            }
            return lista;
        }

        // GET: Coberturas/Details/5
        public ActionResult Details(string id)
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("../Home/login");
            }
            try { 
            id = JsonConvert.DeserializeObject<string>(id);
            var id2 = Convert.ToInt32(id);
            SeparaPermisos(Session["Permisos"].ToString());

            if (id2 == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var list = objCoberturas.Muestra_CoberturasMod(id2).ToList();//Consumo de sp
            var coberturas = new Coberturas
            {
                CoberturaID = list[0].CoberturaID,
                Colonia = list[0].Colonia,
                CodigoPostal = list[0].CodigoPostal,
                DeliveryNombre = list[0].DeliveryNombre,
                DeliveryTelefono = list[0].DeliveryTelefono,
                DeliveryID = list[0].DeliveryID,
                ZonaID = list[0].ZonaID,
                
            };

            var listDelivery = objDelivery.Muestra_Delivery().ToList();//consumo de sp
            ViewBag.DeliveryID = new SelectList(listDelivery, dataValueField: "DeliveryID", dataTextField: "Nombre", selectedValue: list[0].DeliveryID).ToList();//llena el DropDownlist

            var listCliente = objZona.Muestra_Zonas().ToList();//consumo de sp
            ViewBag.ZonaID = new SelectList(listCliente, dataValueField: "ZonaID", dataTextField: "Descripcion", selectedValue: list[0].ZonaID).ToList();//llena el DropDownlist de clientes

            return View(coberturas);
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
                return RedirectToAction("Index");
            }

        }
        // GET: Coberturas/Create
        public ActionResult Create()
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("../Home/login");
            }

            SeparaPermisos(Session["Permisos"].ToString());

            var listDelivery = objDelivery.Muestra_Delivery().ToList();//consumo sp
            ViewBag.DeliveryID = new SelectList(listDelivery, dataValueField: "DeliveryID", dataTextField: "Nombre").ToList();//Llena el DropDownlist Delivery

            var listZona = objZona.Muestra_Zonas().ToList();//consumo sp
            ViewBag.ZonaID = new SelectList(listZona, dataValueField: "ZonaID", dataTextField: "Descripcion").ToList();//Llena el DropDownList Zonas

            return View();
        }

        // POST: Coberturas/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Coberturas coberturas)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }

            //Modelo valido
            //Guarda destinatario
            var _coberturas = new Coberturas
            {
                CoberturaID = 0,
                Colonia = coberturas.Colonia,
                CodigoPostal = coberturas.CodigoPostal,
                DeliveryID = coberturas.DeliveryID,
                ZonaID = coberturas.ZonaID,
            };

            //sp para alta/modificación 
            objCoberturas.Alta_Coberturas(_coberturas);
            return RedirectToAction("Index", new {area = ""});
        }

        // GET: Coberturas/Edit/5
        public ActionResult Edit(string id)
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("../Home/login");
            }
            else
            {
                try { 
                id = JsonConvert.DeserializeObject<string>(id);
                var id2 = Convert.ToInt32(id);
                SeparaPermisos(Session["Permisos"].ToString());

                if (id2 == 0)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                if (id2 == 0)
                {
                    return HttpNotFound();
                }

                var list = objCoberturas.Muestra_CoberturasMod(id2).ToList();//Consumo de sp
                var coberturas = new Coberturas
                {
                    CoberturaID = list[0].CoberturaID,
                    Colonia = list[0].Colonia,
                    CodigoPostal = list[0].CodigoPostal,
                    DeliveryID = list[0].DeliveryID,
                    ZonaID = list[0].ZonaID
                };

                var listDelivery = objDelivery.Muestra_Delivery().ToList();//consumo de sp
                ViewBag.DeliveryID = new SelectList(listDelivery, dataValueField: "DeliveryID", dataTextField: "Nombre", selectedValue: list[0].DeliveryID).ToList();//llena el DropDownlist Delivery

                var listZonas = objZona.Muestra_Zonas().ToList();//consumo de sp
                ViewBag.ZonaID = new SelectList(listZonas, dataValueField: "ZonaID", dataTextField: "Descripcion", selectedValue: list[0].ZonaID).ToList();//llena el DropDownlist Zonas

                return View(coberturas);
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
                    return RedirectToAction("Index");
                }
            }
        }

        // POST: Coberturas/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Coberturas coberturas)
        {
            if (ModelState.IsValid)
            {
                objCoberturas.Alta_Coberturas(coberturas);
                return RedirectToAction("Index");
            }
            return View(coberturas);
        }

        // GET: Coberturas/Delete/5
        public ActionResult Delete(string id)
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("../Home/login");
            }
            else
            {
                try { 
                id = JsonConvert.DeserializeObject<string>(id);
                var id2 = Convert.ToInt32(id);
                SeparaPermisos(Session["Permisos"].ToString());

                if (id2 == 0)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                var list = objCoberturas.Muestra_CoberturasMod(id2).ToList();//consumo de sp
                var coberturas = new Coberturas //asiga los valores del model para el muestreo 
                {
                    CoberturaID = list[0].CoberturaID,
                    Colonia = list[0].Colonia,
                    CodigoPostal = list[0].CodigoPostal,
                    DeliveryID = list[0].DeliveryID,
                    ZonaID = list[0].ZonaID,
                };

                var listDelivery = objDelivery.Muestra_Delivery().ToList();//consumo de sp
                ViewBag.DeliveryID = new SelectList(listDelivery, dataValueField: "DeliveryID", dataTextField: "Nombre", selectedValue: list[0].DeliveryID).ToList();//llena el DropDownlist

                var listCliente = objZona.Muestra_Zonas().ToList();//consumo de sp
                ViewBag.ZonaID = new SelectList(listCliente, dataValueField: "ZonaID", dataTextField: "Descripcion", selectedValue: list[0].ZonaID).ToList();//llena el DropDownlist de clientes

                return View(coberturas);
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
                    return RedirectToAction("Index");
                }
            }
        }

        // POST: Condiciones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            try { 
            id = JsonConvert.DeserializeObject<string>(id);
            var id2 = Convert.ToInt32(id);
            objCoberturas.Elimina_Coberturas(id2);
            return RedirectToAction("Index");
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
                return RedirectToAction("Index");
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
