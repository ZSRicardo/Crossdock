using Crossdock.Context.Commands;
using Crossdock.Context.Queries;
using Crossdock.Models;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Web.Mvc;
using Crossdock.Controllers;
using ClosedXML;
using Microsoft.Ajax.Utilities;

namespace Crossdock.Controllers
{
    public class HomeController : Controller
    {
        private LoginQueries _loginQueries = new LoginQueries();

        private LoginBarCodeQueries _loginBarCodeQueries = new LoginBarCodeQueries();

        private TablaRolesCommands _tblRoles = new TablaRolesCommands();

        public ActionResult Index()
        {


            ViewBag.Message = "Pagina de inicio";

            return View();
        }

        public ActionResult Login()
        {
            ViewBag.Message = "Inicio de sesion";

            return View("Login", "_LoginLayout");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel loginViewModel)
        {
            // Modelo invalido
            if (!ModelState.IsValid && loginViewModel.Email == null || loginViewModel.Password == null)
            {
                TempData["Alert"] = "Datos incorrectos";
                TempData["AlertCssClass"] = "alert-danger";

                return View("Login", "_LoginLayout", loginViewModel);
            }

            // Validar inicio de sesion y obtener datos de usuario
            if (string.IsNullOrEmpty(loginViewModel.Email))
            {
                string cadena = loginViewModel.Password;
                char delimitador = '-';
                string[] valores = cadena.Split(delimitador);

                var email = valores[0];
                var password = valores[1];

                LoginViewModel obj = new LoginViewModel();
                obj.Email = email;
                obj.Password = password;

                var loginObj = _loginBarCodeQueries.LoginBarCode(obj);

                //Credenciales invalidas
                if (string.IsNullOrWhiteSpace(loginObj.UsuarioNombre) || loginObj.UsuarioID == 0)
                {
                    TempData["Alert"] = "No se tiene registro de email o contraseña";
                    TempData["AlertCssClass"] = "alert-danger";

                    return View("Login", "_LoginLayout", loginViewModel);
                }
                // Credenciales validas

                // Datos de usuario y cliente
                Session["UserID"] = loginObj.UsuarioID;
                Session["UserNombre"] = loginObj.UsuarioNombre;
                Session["UserClienteID"] = loginObj.ClienteID;
                Session["Permisos"] = loginObj.Permisos;
                Session["UserIdentificador"] = loginObj.UsuarioIdendtificador;
                Session["UserRolID"] = loginObj.UsuarioRolID;
                Session["ClienteRS"] = loginObj.ClienteRS;
                Session["ClienteIdentificador"] = loginObj.ClienteIdentifiador;

                //Evaluación de roles
                SeparaPermisos(loginObj.Permisos);
            }
            else
            {
                // Credenciales invalidas
                var loginObjUno = _loginQueries.Login(loginViewModel);
                if (string.IsNullOrWhiteSpace(loginObjUno.UsuarioNombre) || loginObjUno.UsuarioID == 0)
                {
                    TempData["Alert"] = "No se tiene registro de email o contraseña";
                    TempData["AlertCssClass"] = "alert-danger";

                    return View("Login", "_LoginLayout", loginViewModel);
                }
                // Credenciales validas

                // Datos de usuario y cliente
                Session["UserID"] = loginObjUno.UsuarioID;
                Session["UserNombre"] = loginObjUno.UsuarioNombre;
                Session["UserClienteID"] = loginObjUno.ClienteID;
                Session["Permisos"] = loginObjUno.Permisos;
                Session["UserIdentificador"] = loginObjUno.UsuarioIdendtificador;
                Session["UserRolID"] = loginObjUno.UsuarioRolID;
                Session["ClienteRS"] = loginObjUno.ClienteRS;
                Session["ClienteIdentificador"] = loginObjUno.ClienteIdentifiador;

                //Validar roles
                SeparaPermisos(loginObjUno.Permisos);
            }

            return RedirectToAction("Dashboard");
        }

        public ActionResult Dashboard()
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login");
            }

            RecepcionController.DList.Clear();
            RecepcionController.oList.Clear();
            RecepcionController.SelectList.Clear();
            SeparaPermisos(Session["Permisos"].ToString());

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
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
