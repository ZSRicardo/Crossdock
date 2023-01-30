using Crossdock.Context.Commands;
using Crossdock.Models;
using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Microsoft.VisualBasic.CompilerServices;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;

namespace Crossdock.Controllers
{
    public class BodegasController : Controller
    {
        private TablaBodegasCommands objBodega = new TablaBodegasCommands();

        // GET: Bodegas
        public ActionResult Index()
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("../Home/login");
            }

            SeparaPermisos(Session["Permisos"].ToString());

            return View();
        }

        public JsonResult Listabodegas()
        {
            var lista = objBodega.Muestra_Bodegas().ToList();
            lista = indice_general(lista);
            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }

        private List<Bodegas> indice_general(List<Bodegas> lista)
        {
            int indice = 1;
            foreach (Bodegas guia in lista)
            {
                guia.ID_Temporal = indice;
                indice++;
            }
            return lista;
        }
        // GET: Bodegas/Details/5
        public ActionResult Details(string id)
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("../Home/login");
            }
            id = JsonConvert.DeserializeObject<string>(id);
            SeparaPermisos(Session["Permisos"].ToString());

            if (Convert.ToInt32(id) == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var list = objBodega.Muestra_BodegasMod(Convert.ToInt32(id)).ToList();//consumo de sp
            var bodega = new Bodegas //asiga los valores del model para el muestreo 
            {
                BodegaID = list[0].BodegaID,
                Nombre = list[0].Nombre,
                Email = list[0].Email,
                Telefono = list[0].Telefono,
                Calle=list[0].Calle,
                NumeroExt=list[0].NumeroExt,
                NumeroInt=list[0].NumeroInt,
                Colonia=list[0].Colonia,
                CodigoPostal=list[0].CodigoPostal,
                HorarioInicio=list[0].HorarioInicio,
                HorarioFinal=list[0].HorarioFinal,
            };
            return View(bodega);
        }

        // GET: Bodegas/Create
        public ActionResult Create()
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("../Home/login");
            }

            SeparaPermisos(Session["Permisos"].ToString());
            _ = objBodega.Muestra_Bodegas().ToList();//consumo de sp
            return View();
        }

        // POST: Bodegas/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Bodegas bod)
        {
            //* Modelo invalido
            if (!ModelState.IsValid)
            {
                //TempData["Alert"] = "Error en crear Bodega";
                //TempData["AlertCssClass"] = "alert-danger";
                // no redirige a nueva accion, solo renderiza de nuevo la vista
                return RedirectToAction("Index");
            }
            try { 

            //* Modelo valido 
            // Guardar u Obtener Destinatario 
            var bodeg = new Bodegas
            {
                BodegaID = 0,
                Nombre = bod.Nombre,
                Email = bod.Email,
                Telefono = bod.Telefono,
                Calle = bod.Calle,
                NumeroExt = bod.NumeroExt,
                NumeroInt = bod.NumeroInt,
                Colonia = bod.Colonia,
                CodigoPostal = bod.CodigoPostal,
                HorarioInicio = bod.HorarioInicio,
                HorarioFinal=bod.HorarioFinal,
            };

            // SP para alta/modificacion de destinatario
            objBodega.Alta_Bodegas(bodeg);
            return RedirectToAction("Index", new { area = "" });
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

        // GET: Bodegas/Edit/5
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
                SeparaPermisos(Session["Permisos"].ToString());

                if (Convert.ToInt32(id) == 0)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                if (Convert.ToInt32(id) == 0)
                {
                    return HttpNotFound();
                }

                var list = objBodega.Muestra_BodegasMod(Convert.ToInt32(id)).ToList();
                var bod = new Bodegas
                {
                    BodegaID = list[0].BodegaID,
                    Nombre = list[0].Nombre,
                    Email = list[0].Email,
                    Telefono = list[0].Telefono,
                    Calle = list[0].Calle,
                    NumeroExt = list[0].NumeroExt,
                    NumeroInt = list[0].NumeroInt,
                    Colonia = list[0].Colonia,
                    CodigoPostal = list[0].CodigoPostal,
                    HorarioInicio = list[0].HorarioInicio,
                    HorarioFinal = list[0].HorarioFinal,
                };

                 return View(bod);
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

        // POST: Bodegas/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Bodegas bodegas)
        {
            if(ModelState.IsValid)
            {
                objBodega.Alta_Bodegas(bodegas);
                return RedirectToAction("Index");
            }

            return View(bodegas);
        }

        // GET: Bodegas/Delete/5
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
                SeparaPermisos(Session["Permisos"].ToString());

                if (Convert.ToInt32(id) == 0)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                var list = objBodega.Muestra_BodegasMod(Convert.ToInt32(id)).ToList();//consumo de sp
                var bodega = new Bodegas //asiga los valores del model para el muestreo 
                {
                    BodegaID = list[0].BodegaID,
                    Nombre = list[0].Nombre,
                    Email = list[0].Email,
                    Telefono = list[0].Telefono,
                    Calle = list[0].Calle,
                    NumeroExt = list[0].NumeroExt,
                    NumeroInt = list[0].NumeroInt,
                    Colonia = list[0].Colonia,
                    CodigoPostal = list[0].CodigoPostal,
                    HorarioInicio = list[0].HorarioInicio,
                    HorarioFinal = list[0].HorarioFinal,
                };

                return View(bodega);
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

        // POST: Bodegas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            try { 
            id = JsonConvert.DeserializeObject<string>(id);
            var id2 = Convert.ToInt32(id);
            objBodega.Elimina_Bodegas(id2);
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
