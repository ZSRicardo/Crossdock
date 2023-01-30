using Crossdock.Context.Commands;
using Crossdock.Models;
using System.Linq;
using System.Net;
using System;
using System.Web.Mvc;
using Microsoft.VisualBasic.CompilerServices;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;

namespace Crossdock.Controllers
{
    public class OperadoresController : Controller
    {
        private TablaUnidadesCommands objUnidades = new TablaUnidadesCommands();//llamada de clase
        private TablaOperadoresCommands objOperador = new TablaOperadoresCommands();
        private TablaTipoOperadoresCommands objTop = new TablaTipoOperadoresCommands();

        // GET: Operadores
        public ActionResult Index()
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("../Home/login");
            }

            SeparaPermisos(Session["Permisos"].ToString());


            return View();
        }

        public JsonResult Listaoperadores()
        {
            var lista = objOperador.Muestra_Operadores().ToList();
            lista = indice_general(lista);
            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }
        private List<Operadores> indice_general(List<Operadores> lista)
        {
            int indice = 1;
            foreach (var registro in lista)
            {
                registro.ID_Temporal = indice;
                indice++;
            }
            return lista;
        }

        // GET: Operadores/Details/5
        public ActionResult Details(string id)
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("../Home/login");
            }
            try { 
            SeparaPermisos(Session["Permisos"].ToString());
            id = JsonConvert.DeserializeObject<string>(id);
            var id2 = Convert.ToInt32(id);
            if (id2 == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var list1 = objOperador.Muestra_OperadoresMod(id2).ToList();
            var list2 = objTop.Muestra_TipoOperadoresMod(list1[0].TipoOperadorID).ToList();
            var list3 = objUnidades.Muestra_UnidadesMod(list1[0].UnidadID).ToList();

            var Operador = new Operadores
            {
                OperadorID = list1[0].OperadorID,
                Nombre = list1[0].Nombre,
                ApellidoP = list1[0].ApellidoP,
                ApellidoM = list1[0].ApellidoM,
                Celular = list1[0].Celular,
                Email = list1[0].Email,
                Calle = list1[0].Calle,
                NumeroExt = list1[0].NumeroExt,
                NumeroInt = list1[0].NumeroInt,
                Colonia = list1[0].Colonia,
                CodigoPostal = list1[0].CodigoPostal,
                TipoOperadorID = list1[0].TipoOperadorID,
                TipoOperadorDescripcion = list2[0].Descripcion,
                UnidadID = list1[0].UnidadID,
                UnidadMarca = list3[0].Marca,
                UnidadModelo = list3[0].Modelo,
                UnidadPlacas = list3[0].Placas,
            };
            return View(Operador);
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

        // GET: Operadores/Create
        public ActionResult Create()
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("../Home/login");
            }

            SeparaPermisos(Session["Permisos"].ToString());

            var ListTOP = objTop.Muestra_TipoOperadores().ToList();//consumo de sp
            ViewBag.TipoOperadorID = new SelectList(ListTOP, dataValueField: "TipoOperadorID", dataTextField: "Descripcion").ToList();//llena el DropDownlist de clientes

            var ListUn = objUnidades.Muestra_Unidades().ToList();//consumo de sp
            ViewBag.UnidadID = new SelectList(ListUn, dataValueField: "UnidadID", dataTextField: "Placas").ToList();//llena el DropDownlist            

            //ViewBag.UnidadesList = GetUnidades(null);
            return View();
        }

        // POST: Operadores/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Operadores cl)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }
            //* Modelo valido
            // Guardar u Obtener Destinatario 
            var cli = new Operadores
            {
                OperadorID = 0,
                Nombre=cl.Nombre,
                ApellidoP=cl.ApellidoP,
                ApellidoM=cl.ApellidoM,
                Celular=cl.Celular,
                Email=cl.Email,
                Calle=cl.Calle,
                NumeroExt=cl.NumeroExt,
                NumeroInt=cl.NumeroInt,
                Colonia=cl.Colonia,
                CodigoPostal=cl.CodigoPostal,
                TipoOperadorID=cl.TipoOperadorID,
                UnidadID=cl.UnidadID,
            };

            // SP para alta/modificacion de destinatario
            objOperador.Alta_Operadores(cli);
            return RedirectToAction("Index", new { area = "" });
        }

        // GET: Operadores/Edit/5
        public ActionResult Edit(string id)
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
            if (id2 == 0)
            {
                return HttpNotFound();
            }

            var list1 = objOperador.Muestra_OperadoresMod(id2).ToList();
            var list2 = objTop.Muestra_TipoOperadoresMod(list1[0].TipoOperadorID).ToList();
            var list3 = objUnidades.Muestra_UnidadesMod(list1[0].UnidadID).ToList();

            var Operador = new Operadores
            {
                OperadorID = list1[0].OperadorID,
                Nombre = list1[0].Nombre,
                ApellidoP = list1[0].ApellidoP,
                ApellidoM = list1[0].ApellidoM,
                Celular=list1[0].Celular,
                Email = list1[0].Email,
                Calle=list1[0].Calle,
                NumeroExt=list1[0].NumeroExt,
                NumeroInt=list1[0].NumeroInt,
                Colonia=list1[0].Colonia,
                CodigoPostal=list1[0].CodigoPostal,
                TipoOperadorID=list1[0].TipoOperadorID,
                TipoOperadorDescripcion=list2[0].Descripcion,
                UnidadID=list1[0].UnidadID,
                UnidadMarca=list3[0].Marca,
                UnidadModelo=list3[0].Modelo,
                UnidadPlacas=list3[0].Placas,
            };

            var ListTOP = objTop.Muestra_TipoOperadores().ToList();//consumo de sp
            ViewBag.TipoOperadorID = new SelectList(ListTOP, dataValueField: "TipoOperadorID", dataTextField: "Descripcion", selectedValue: list1[0].TipoOperadorID).ToList();//llena el DropDownlist de clientes

            var ListUn = objUnidades.Muestra_Unidades().ToList();//consumo de sp
            ViewBag.UnidadID = new SelectList(ListUn, dataValueField: "UnidadID", dataTextField: "Placas", selectedValue: list1[0].UnidadID).ToList();//llena el DropDownlist  

            return View(Operador);
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

        // POST: Operadores/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Operadores Operadores)
        {
            if (ModelState.IsValid)
            {
                objOperador.Alta_Operadores(Operadores);
                return RedirectToAction("Index");
            }
            return View(Operadores);
        }

        // GET: Operadores/Delete/5
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
            var list1 = objOperador.Muestra_OperadoresMod(id2).ToList();
            var list2 = objTop.Muestra_TipoOperadoresMod(list1[0].TipoOperadorID).ToList();
            var list3 = objUnidades.Muestra_UnidadesMod(list1[0].UnidadID).ToList();

            var Operador = new Operadores
            {
                OperadorID = list1[0].TipoOperadorID,
                Nombre = list1[0].Nombre,
                ApellidoP = list1[0].ApellidoP,
                ApellidoM = list1[0].ApellidoM,
                Celular=list1[0].Celular,
                Email = list1[0].Email,
                Calle=list1[0].Calle,
                NumeroExt=list1[0].NumeroExt,
                NumeroInt=list1[0].NumeroInt,
                Colonia=list1[0].Colonia,
                CodigoPostal=list1[0].CodigoPostal,
                TipoOperadorID=list1[0].TipoOperadorID,
                TipoOperadorDescripcion=list2[0].Descripcion,
                UnidadID=list1[0].UnidadID,
                UnidadMarca=list3[0].Marca,
                UnidadModelo=list3[0].Modelo,
                UnidadPlacas=list3[0].Placas,
            };
            return View(Operador);
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

        // POST: Operadores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            try { 
            id = JsonConvert.DeserializeObject<string>(id);
            var id2 = Convert.ToInt32(id);
            objOperador.Elimina_Operadores(id2);
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
