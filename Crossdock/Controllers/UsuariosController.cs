using Crossdock.Context.Commands;
using Crossdock.Models;
using CrossDockLib;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.VisualBasic.CompilerServices;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;

namespace Crossdock.Controllers
{
    public class UsuariosController : Controller
    {
        private TablaClientesCommands objCliente = new TablaClientesCommands();//llamada de clase
        private TablaUsuariosCommands objUsuario = new TablaUsuariosCommands();
        private TablaRolesCommands objRol = new TablaRolesCommands();
        private S3Service _s3Service = new S3Service();

        public ActionResult Index()//muestra los usuarios Activos vista
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("../Home/login");
            }

            SeparaPermisos(Session["Permisos"].ToString());

            return View();
        }

        public JsonResult Listausuarios() 
        {
            var lista = objUsuario.Muestra_Usuario().ToList();
            lista = indice_general(lista);
            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }
        private List<Usuario> indice_general(List<Usuario> lista)
        {
            int indice = 1;
            foreach (var registro in lista)
            {
                registro.ID_Temporal = indice;
                indice++;
            }
            return lista;
        }

        // GET: Usuarios

        // GET: Usuarios/Details/5
        public ActionResult Details(string id)//muestra un registro de usuarios basado en el id
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("../Home/login");
            }
            else
            {
                try { 
                SeparaPermisos(Session["Permisos"].ToString());
                id = JsonConvert.DeserializeObject<string>(id);
                if (Convert.ToInt32(id) == 0)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                var list = objUsuario.Muestra_UsuarioMod(Convert.ToInt32(id)).ToList();//consumo de sp
                var user = new Usuario //asiga los valores del model para el muestreo 
                {
                    UsuarioID = list[0].UsuarioID,
                    UsuarioIdentificador = list[0].UsuarioIdentificador,
                    Nombre = list[0].Nombre,
                    Email = list[0].Email,
                    Celular = list[0].Celular,
                    RolID = list[0].RolID,
                    ClienteID = list[0].ClienteID,
                    RolDescripcion = list[0].RolDescripcion,
                    Cliente = list[0].Cliente,
                };

                return View(user);
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

        // GET: Usuarios/Create
        public ActionResult Create()
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("../Home/login");
            }

            SeparaPermisos(Session["Permisos"].ToString());

            var listRol = objRol.Muestra_Roles().ToList();//consumo de sp
            ViewBag.RolID = new SelectList(listRol, dataValueField: "RolID", dataTextField: "Descripcion").ToList();//llena el DropDownlist roles

            var list = objCliente.Muestra_Clientes().ToList();//consumo de sp
            ViewBag.ClienteID = new SelectList(list, dataValueField: "ClienteID", dataTextField: "RazonSocial").ToList();//llena el DropDownlist de clientes
            return View();
        }

        // POST: Usuarios/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.

        // POST: Usuarios/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Usuario user)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }

            var pass = new GeneraContraseña();
            var usu = new Usuario
            {
                UsuarioID = 0,
                Nombre = user.Nombre,
                Password = pass.Password,
                UsuFechaAlta = DateTime.Now,
                UsuFechaBaja = null,
                CodigoVerificacion = null,
                TelefonoVerificado = false,
                Email = user.Email,
                EmailVerificado = false,
                Celular = user.Celular,
                RolID = user.RolID,
                ClienteID = user.ClienteID
            };

            objUsuario.Alta_Usuario(usu);

            //GenerarQR generarQR = new GenerarQR();
            //var aByte = generarQR.CreateQR(usu.Email, usu.Password, usu.Nombre);

            GenerarBarCode barCode = new GenerarBarCode();
            var aByte = barCode.CreateBarCode(usu.Email, usu.Password, usu.Nombre);

            //Subida de codebar a S3
            var CBFileNameS3 = $"BarCode{usu.UsuarioIdentificador.Replace("BarCode", "BarCode")}";
            string URL = await _s3Service.UploadFileToS3(aByte, CBFileNameS3, "");

            //Envio de Mail
            EnviarMail objMail = new EnviarMail();
            objMail.EnviaMail(usu.Email, "Registro Taimingo Crossdock <No responder>", "Tus datos de Acceso, " + usu.Nombre.Split(' ')[0], objMail.GeneraBody(usu.Nombre, usu.Email, usu.Password));/*+*/
            /* "también puedes ingresar al link para obtener tu código de barras de inicio de sesión" + URL)*/

            //Envio del link del codebar de S3 por sms
            //EnviarMensaje msn = new EnviarMensaje();
            // msn.EnvioMensaje(usu.Celular, "Bienvenido " + usu.Nombre + "(@)" + " a Taimingo, Ingresa al link para obtener tu código de barras de inicio de sesion" + " " + URL);

            return RedirectToAction("Index", new { area = "" });
        }

        // GET:/Usuarios/Edit/5
        public ActionResult Edit(string id)
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("../Home/login");
            }
            else
            {
                try { 
                SeparaPermisos(Session["Permisos"].ToString());
                id = JsonConvert.DeserializeObject<string>(id);
                if (Convert.ToInt32(id) == 0)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                if (Convert.ToInt32(id) == 0)
                {
                    return HttpNotFound();
                }
                //  return View(usuario);

                var list = objUsuario.Muestra_UsuarioMod(Convert.ToInt32(id)).ToList();//consumo de sp
                var user = new Usuario //asiga los valores del model para el muestreo 
                {
                    UsuarioID = list[0].UsuarioID,
                    UsuarioIdentificador = list[0].UsuarioIdentificador,
                    Password = list[0].Password,
                    Nombre = list[0].Nombre,
                    Email = list[0].Email,
                    Celular = list[0].Celular,
                    RolID = list[0].RolID,
                    ClienteID = list[0].ClienteID,
                };

                var listRol = objRol.Muestra_Roles().ToList();//consumo de sp
                ViewBag.RolID = new SelectList(listRol, dataValueField: "RolID", dataTextField: "Descripcion", selectedValue: list[0].RolID).ToList();//llena el DropDownlist

                var listCliente = objCliente.Muestra_Clientes().ToList();//consumo de sp
                ViewBag.ClienteID = new SelectList(listCliente, dataValueField: "ClienteID", dataTextField: "RazonSocial", selectedValue: list[0].ClienteID).ToList();//llena el DropDownlist de clientes

                return View(user);
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

        // POST: Usuarios/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                objUsuario.Alta_Usuario(usuario);
                return RedirectToAction("Index");
            }

            return View(usuario);
        }

        // GET: Usuarios/Delete/5
        public ActionResult Delete(string id)
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("../Home/login");
            }
            else
            {
                try { 
                SeparaPermisos(Session["Permisos"].ToString());
                id = JsonConvert.DeserializeObject<string>(id);
                if (Convert.ToInt32(id) == 0)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                if (Convert.ToInt32(id) == 0)
                {
                    return HttpNotFound();
                }
                var list = objUsuario.Muestra_UsuarioMod(Convert.ToInt32(id)).ToList();//consumo de sp
                var user = new Usuario //asiga los valores del model para el muestreo 
                {
                    UsuarioID = list[0].UsuarioID,
                    UsuarioIdentificador = list[0].UsuarioIdentificador,
                    Nombre = list[0].Nombre,
                    Email = list[0].Email,
                    Celular = list[0].Celular,
                    RolID = list[0].RolID,
                    ClienteID = list[0].ClienteID
                };
                var listRol = objRol.Muestra_Roles().ToList();//consumo de sp
                ViewBag.RolID = new SelectList(listRol, dataValueField: "RolID", dataTextField: "Descripcion", selectedValue: list[0].RolID).ToList();//llena el DropDownlist

                var listCliente = objCliente.Muestra_Clientes().ToList();//consumo de sp
                ViewBag.ClienteID = new SelectList(listCliente, dataValueField: "ClienteID", dataTextField: "RazonSocial", selectedValue: list[0].ClienteID).ToList();//llena el DropDownlist de clientes

                return View(user);
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

        // POST: Usuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            try { 
            id = JsonConvert.DeserializeObject<string>(id);
            var id2 = Convert.ToInt32(id);
            objUsuario.Elimina_Usuario(id2);
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
