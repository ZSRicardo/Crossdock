using Crossdock.Context.Commands;
using Crossdock.Models;
using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Microsoft.VisualBasic.CompilerServices;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;

namespace Crossdock.Controllers
{
    public class RolesController : Controller
    {
        private TablaRolesCommands objRol = new TablaRolesCommands();//llamada de clase

        // GET: Roles
        public ActionResult Index()
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("../Home/login");
            }

            SeparaPermisos(Session["Permisos"].ToString());


            return View();
        }

        public JsonResult Listaroles()
        {
            var lista = objRol.Muestra_Roles().ToList();
            lista = indice_general(lista);
            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }
        private List<Roles> indice_general(List<Roles> lista)
        {
            int indice = 1;
            foreach (var registro in lista)
            {
                registro.ID_Temporal = indice;
                indice++;
            }
            return lista;
        }

        // GET: Roles

        // GET: Roles/Details/5
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

            var list = objRol.Muestra_RolesMod(id2).ToList();//consumo de sp
                                                            
            Roles roles = new Roles(); 
            roles = PermisosRoles(list[0].Permisos.ToString());//asiga los valores del model para el muestreo 
            roles.Descripcion = list[0].Descripcion.ToString();//asiga los valores del model para el muestreo 
            roles.RolID = Convert.ToInt32(list[0].RolID.ToString());

            return View(roles);
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

        // GET: Roles/Create
        public ActionResult Create()
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("../Home/login");
            }

            SeparaPermisos(Session["permisos"].ToString());

            Roles rol = new Roles();
            return View(rol);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Roles rol)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }

            var rl = new Roles
            {
                RolID = 0,
                Descripcion = rol.Descripcion,

                //Guías
                GeneracionGuias = rol.GeneracionGuias,

                //Historial de guias
                HistorialGuias = rol.HistorialGuias,
                
                // Notificaciones
                Notificaciones=rol.Notificaciones,
                
                //Cedis
                RecepcionPaquetes = rol.RecepcionPaquetes,

                //Area
                AsignacionTareas = rol.AsignacionTareas,

                //Historial Treas
                HistorialTareas = rol.HistorialTareas,

                //Paquetes
                SeguimientoPaquetes = rol.SeguimientoPaquetes,

                //Crossdock Temporal
                HistorialPaquetes = rol.HistorialPaquetes,

                //Usuarios
                UsuariosVisualizar = rol.UsuariosVisualizar,

                //Bodegas
                BodegasVisualizar = rol.BodegasVisualizar,

                //Clientes
                ClientesVisualizar = rol.ClientesVisualizar,

                //Condiciones
                CondicionesVisualizar = rol.CondicionesVisualizar,

                //Delivery
                DeliveryVisualizar = rol.DeliveryVisualizar,

                //Estatus paquetes
                EstatusPaVisualizar = rol.EstatusPaVisualizar,

                //Estatus tareas
                EstatusTaVisualizar = rol.EstatusTaVisualizar,

                //Operadores
                OperadoresVisualizar = rol.OperadoresVisualizar,

                //Tipos de operador
                TopVisualizar = rol.TopVisualizar,

                //Roles
                RolesVisualizar = rol.RolesVisualizar,

                //Tipo de entrega
                TenVisualizar = rol.TenVisualizar,

                //Unidades
                UnidadesVisualizar = rol.UnidadesVisualizar,

                //Tipo de unidades
                TunVisualizar = rol.TunVisualizar,

                //Zonas
                ZonasVisualizar = rol.ZonasVisualizar,

                //Devolución
                MotivosVisualizar = rol.MotivosVisualizar,

                //Coberturas
                CoberturasVisualizar = rol.CoberturasVisualizar,

                //Guias Prepagadas
                GuiasPrepagadas = rol.GuiasPrepagadas,

                Permisos
                = Convert.ToString(Convert.ToInt32(rol.GeneracionGuias))
                + Convert.ToString(Convert.ToInt32(rol.HistorialGuias))
                + Convert.ToString(Convert.ToInt32(rol.GuiasPrepagadas))
                + Convert.ToString(Convert.ToInt32(rol.Notificaciones))
                + Convert.ToString(Convert.ToInt32(rol.RecepcionPaquetes))
                + Convert.ToString(Convert.ToInt32(rol.SeguimientoPaquetes))
                + Convert.ToString(Convert.ToInt32(rol.HistorialPaquetes))
                + Convert.ToString(Convert.ToInt32(rol.AsignacionTareas))
                + Convert.ToString(Convert.ToInt32(rol.HistorialTareas))
                + Convert.ToString(Convert.ToInt32(rol.UsuariosVisualizar))
                + Convert.ToString(Convert.ToInt32(rol.BodegasVisualizar))
                + Convert.ToString(Convert.ToInt32(rol.ClientesVisualizar))
                + Convert.ToString(Convert.ToInt32(rol.CondicionesVisualizar))
                + Convert.ToString(Convert.ToInt32(rol.DeliveryVisualizar))
                + Convert.ToString(Convert.ToInt32(rol.EstatusPaVisualizar))
                + Convert.ToString(Convert.ToInt32(rol.EstatusTaVisualizar))
                + Convert.ToString(Convert.ToInt32(rol.OperadoresVisualizar))
                + Convert.ToString(Convert.ToInt32(rol.TopVisualizar))
                + Convert.ToString(Convert.ToInt32(rol.RolesVisualizar))
                + Convert.ToString(Convert.ToInt32(rol.TenVisualizar))
                + Convert.ToString(Convert.ToInt32(rol.UnidadesVisualizar))
                + Convert.ToString(Convert.ToInt32(rol.TunVisualizar))
                + Convert.ToString(Convert.ToInt32(rol.ZonasVisualizar))
                + Convert.ToString(Convert.ToInt32(rol.MotivosVisualizar))
                + Convert.ToString(Convert.ToUInt32(rol.CoberturasVisualizar))
            };

            objRol.Alta_Roles(rl);
            return RedirectToAction("Index", new { area = "" });
        }

        // GET: Roles/Edit/5
        public ActionResult Edit(string id)
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("../Home/login");
            }
            id = JsonConvert.DeserializeObject<string>(id);
            var id2 = Convert.ToInt32(id);
            SeparaPermisos(Session["Permisos"].ToString());

            if (id2 == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var list = objRol.Muestra_RolesMod(id2).ToList();//consumo de sp

            Roles roles = new Roles();
            roles = PermisosRoles(list[0].Permisos.ToString());//asiga los valores del model para el muestreo 
            roles.Descripcion = list[0].Descripcion.ToString();//asiga los valores del model para el muestreo 
            roles.RolID = Convert.ToInt32(list[0].RolID.ToString());
            SeparaPermisos(Session["permisos"].ToString());
            return View(roles);
        }

        // POST: Roles/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Roles rol)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }

            if (ModelState.IsValid)
            {
                
                var rl = new Roles
                {
                    RolID = rol.RolID,
                    Descripcion = rol.Descripcion,

                    Permisos = Convert.ToString(Convert.ToInt32(rol.GeneracionGuias))
                + Convert.ToString(Convert.ToInt32(rol.HistorialGuias))
                + Convert.ToString(Convert.ToInt32(rol.GuiasPrepagadas))
                + Convert.ToString(Convert.ToInt32(rol.Notificaciones))
                + Convert.ToString(Convert.ToInt32(rol.RecepcionPaquetes))
                + Convert.ToString(Convert.ToInt32(rol.SeguimientoPaquetes))
                + Convert.ToString(Convert.ToInt32(rol.HistorialPaquetes))
                + Convert.ToString(Convert.ToInt32(rol.AsignacionTareas))
                + Convert.ToString(Convert.ToInt32(rol.HistorialTareas))
                + Convert.ToString(Convert.ToInt32(rol.UsuariosVisualizar))
                + Convert.ToString(Convert.ToInt32(rol.BodegasVisualizar))
                + Convert.ToString(Convert.ToInt32(rol.ClientesVisualizar))
                + Convert.ToString(Convert.ToInt32(rol.CondicionesVisualizar))
                + Convert.ToString(Convert.ToInt32(rol.DeliveryVisualizar))
                + Convert.ToString(Convert.ToInt32(rol.EstatusPaVisualizar))
                + Convert.ToString(Convert.ToInt32(rol.EstatusTaVisualizar))
                + Convert.ToString(Convert.ToInt32(rol.OperadoresVisualizar))
                + Convert.ToString(Convert.ToInt32(rol.TopVisualizar))
                + Convert.ToString(Convert.ToInt32(rol.RolesVisualizar))
                + Convert.ToString(Convert.ToInt32(rol.TenVisualizar))
                + Convert.ToString(Convert.ToInt32(rol.UnidadesVisualizar))
                + Convert.ToString(Convert.ToInt32(rol.TunVisualizar))
                + Convert.ToString(Convert.ToInt32(rol.ZonasVisualizar))
                + Convert.ToString(Convert.ToInt32(rol.MotivosVisualizar))
                + Convert.ToString(Convert.ToUInt32(rol.CoberturasVisualizar))

                };

                objRol.Alta_Roles(rl);
                return RedirectToAction("Index");
            }
            SeparaPermisos(Session["permisos"].ToString());
            return View(rol);
        }

        // GET: Roles/Delete/5
        //public ActionResult Delete()
        public ActionResult Delete(string id)
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
            var list = objRol.Muestra_RolesMod(id2).ToList();//consumo de sp

            Roles roles = new Roles();
            roles = PermisosRoles(list[0].Permisos.ToString());//asiga los valores del model para el muestreo 
            roles.Descripcion = list[0].Descripcion.ToString();//asiga los valores del model para el muestreo 
            SeparaPermisos(Session["permisos"].ToString());
            return View(roles);
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

        // POST: Roles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            try { 
            id = JsonConvert.DeserializeObject<string>(id);
            var id2 = Convert.ToInt32(id);
            objRol.Elimina_Roles(id2);
            SeparaPermisos(Session["permisos"].ToString());
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

        public Roles PermisosRoles(string permisos)
        {
            //Declaración de variables
            int n;
            string valor;
            int f = 0;

            //Devuelve la longitud de permisos
            int a = permisos.Length;
            int[] testArray = new int[2];

            //Genera una nueva instancia
            Roles roles = new Roles();

            //Iteración de permisos con for
            for (n = 0; n <= permisos.Length - 1; n++)
            {
                valor = Conversions.ToString(permisos[n]);
                //Conversión de permiso
                int asm = Convert.ToInt32(valor);
                testArray[0] = asm;

                switch (n)
                {
                    //Guías
                    case 0:
                        {
                            if (testArray[f] == 1)
                                roles.GeneracionGuias = true;
                            else
                                roles.GeneracionGuias = false;
                            break;
                        }
                    //Historial de Guias
                    case 1:
                        {
                            if (testArray[f] == 1)
                                roles.HistorialGuias = true;
                            else
                                roles.HistorialGuias = false;
                            break;
                        }
                    //Guias Prepagadas
                    case 2:
                        {
                            if (testArray[f] == 1)
                                roles.GuiasPrepagadas = true;
                            else
                                roles.GuiasPrepagadas = false;
                            break;
                        }
                    //Notificaciones
                    case 3:
                        {
                            if (testArray[f] == 1)
                                roles.Notificaciones = true;
                            else
                                roles.Notificaciones = false;
                            break;
                        }
                    //Recepción Paquetes
                    case 4:
                        {
                            if (testArray[f] == 1)
                                roles.RecepcionPaquetes = true;
                            else
                                roles.RecepcionPaquetes = false;
                            break;
                        }
                    //Seguimiento de Paquetes
                    case 5:
                        {
                            if (testArray[f] == 1)
                                roles.SeguimientoPaquetes = true;
                            else
                                roles.SeguimientoPaquetes = false;
                            break;
                        }
                    //Historial Paquetes
                    case 6:
                        {
                            if (testArray[f] == 1)
                                roles.HistorialPaquetes = true;
                            else
                                roles.HistorialPaquetes = false;
                            break;
                        }
                    //Asignación de tareas
                    case 7:
                        {
                            if (testArray[f] == 1)
                                roles.AsignacionTareas = true;
                            else
                                roles.AsignacionTareas = false;
                            break;
                        }
                    //Historial de tareas
                    case 8:
                        {
                            if (testArray[f] == 1)
                                roles.HistorialTareas = true;
                            else
                                roles.HistorialTareas = false;
                            break;
                        }

                    //Usuarios
                    case 9:
                        {
                            if (testArray[f] == 1)
                                roles.UsuariosVisualizar = true;
                            else
                                roles.UsuariosVisualizar = false;
                            break;
                        }
                    //Bodegas
                    case 10:
                        {
                            if (testArray[f] == 1)
                                roles.BodegasVisualizar = true;
                            else
                                roles.BodegasVisualizar = false;
                            break;
                        }
                    //Clientes
                    case 11:
                        {
                            if (testArray[f] == 1)
                                roles.ClientesVisualizar = true;
                            else
                                roles.ClientesVisualizar = false;
                            break;
                        }
                    //Condiciones
                    case 12:
                        {
                            if (testArray[f] == 1)
                                roles.CondicionesVisualizar = true;
                            else
                                roles.CondicionesVisualizar = false;
                            break;
                        }
                    //Delivery
                    case 13:
                        {
                            if (testArray[f] == 1)
                                roles.DeliveryVisualizar = true;
                            else
                                roles.DeliveryVisualizar = false;
                            break;
                        }
                    //Estatus Paquetes
                    case 14:
                        {
                            if (testArray[f] == 1)
                                roles.EstatusPaVisualizar = true;
                            else
                                roles.EstatusPaVisualizar = false;
                            break;
                        }
                    //Estatus Tareas
                    case 15:
                        {
                            if (testArray[f] == 1)
                                roles.EstatusTaVisualizar = true;
                            else
                                roles.EstatusTaVisualizar = false;
                            break;
                        }
                    //Operadores
                    case 16:
                        {
                            if (testArray[f] == 1)
                                roles.OperadoresVisualizar = true;
                            else
                                roles.OperadoresVisualizar = false;
                            break;
                        }
                    //Tipo Operador
                    case 17:
                        {
                            if (testArray[f] == 1)
                                roles.TopVisualizar = true;
                            else
                                roles.TopVisualizar = false;
                            break;
                        }
                    //Roles
                    case 18:
                        {
                            if (testArray[f] == 1)
                                roles.RolesVisualizar = true;
                            else
                                roles.RolesVisualizar = false;
                            break;
                        }
                    //Tipo Entrega
                    case 19:
                        {
                            if (testArray[f] == 1)
                                roles.TenVisualizar = true;
                            else
                                roles.TenVisualizar = false;
                            break;
                        }
                    //Unidades
                    case 20:
                        {
                            if (testArray[f] == 1)
                                roles.UnidadesVisualizar = true;
                            else
                                roles.UnidadesVisualizar = false;
                            break;
                        }
                    //Tipo Unidades
                    case 21:
                        {
                            if (testArray[f] == 1)
                                roles.TunVisualizar = true;
                            else
                                roles.TunVisualizar = false;
                            break;
                        }
                    //Zonas
                    case 22:
                        {
                            if (testArray[f] == 1)
                                roles.ZonasVisualizar = true;
                            else
                                roles.ZonasVisualizar = false;
                            break;
                        }
                    //Devoluciones
                    case 23:
                        {
                            if (testArray[f] == 1)
                                roles.MotivosVisualizar = true;
                            else
                                roles.MotivosVisualizar = false;
                            break;
                        }
                    //Coberturas
                    case 24:
                        {
                            if (testArray[f] == 1)
                                roles.CoberturasVisualizar = true;
                            else
                                roles.CoberturasVisualizar = false;
                            break;
                        }
                    //Inexistentes
                    default:
                        {
                            Console.WriteLine("El catálogo no existe");
                            break;
                        }
                }
            }
            return roles;
        }
    }
}
