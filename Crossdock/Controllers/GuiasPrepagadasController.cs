using Crossdock.Context.Commands;
using Crossdock.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.VisualBasic.CompilerServices;

namespace Crossdock.Controllers
{
    public class GuiasPrepagadasController : Controller
    {
        private TablaClientesCommands tablaClientes = new TablaClientesCommands();
        private Tabla_GuiasPrepagadas tablaGuiasP=  new Tabla_GuiasPrepagadas();
        static List<GuiaPrepagada> oListaGuias = new List<GuiaPrepagada>();
        static List<SelectListItem> L_Clientes = new List<SelectListItem>();
        private TablaGuiasCommands tablaGuias=new TablaGuiasCommands();
       
        // GET: GuiasPrepagadas
        public ActionResult Index()
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("../Home/Login");
            }
            SeparaPermisos(Session["Permisos"].ToString());

            var IDCliente = Convert.ToInt32(Session["UserClienteID"]); //instanciamos el ID de cliente del usuario y el rol al que pertenece
            var RolID = Convert.ToInt32(Session["UserRolID"]);
            //llenamos la lista para dropdowlist
            var ListaClientes = tablaClientes.Muestra_Clientes();
            L_Clientes = new SelectList(ListaClientes, dataValueField: "ClienteID", dataTextField: "RazonSocial").ToList();
            ViewBag.L_Clientes = L_Clientes;

            if (RolID == 1)// si tiene rol de administrador
            {              
                ViewBag.habilitar = "true";
                ViewBag.VCliente ="none";
                ViewBag.DCliente = "Selecciona un Cliente";
                var guias =GuiasPorCliente( tablaGuiasP.Muestra_GuiasPrepagadas().OrderBy(x=>x.ClienteID).ToList());
                //obtenemos las guias usadas por cliente 
                SeparaPermisos(Session["Permisos"].ToString());
                return View(guias);
            }
            else// si no tiene rol de administrador
            {
                ViewBag.habilitar = "none";
                ViewBag.VCliente ="true";
                var guias = tablaGuiasP.Muestra_GuiasPrepagadas().Where(x=>x.ClienteID==IDCliente).OrderBy(x=>x.FechaCompra).ToList();
                var totalGuias = 0;
                //obtenemos la cantidad de guias comrpradas
                foreach(var g in guias)
                {
                    totalGuias = totalGuias + g.NumeroGuias;
                }
                ViewBag.TotalGuias = totalGuias;
                //ontenemos la cantidad de guias utilizadas
                if(guias.Count>0)
                {
                    ViewBag.GuiasUtilizadas = tablaGuias.Muestra_Guias().Where(x => x.FechaCreacion >= guias[0].FechaCompra && x.ClienteID == IDCliente).Count();   
                }
                else
                {
                    ViewBag.GuiasUtilizadas = "0";
                }
                SeparaPermisos(Session["Permisos"].ToString());
                return View(indice_temporal(guias));
            }          
        }

        private List<GuiaPrepagada> GuiasPorCliente(List<GuiaPrepagada> guias)
        {
            List<GuiaPrepagada> clientesGuias = new List<GuiaPrepagada>();
            //obtenemos una lista de los clientes con la cantidad de guias compradas
            clientesGuias.Add(
                new GuiaPrepagada
                {
                    ClienteID = guias[0].ClienteID,
                    NumeroGuias = guias[0].NumeroGuias,
                    Cliente_RazonSocial = guias[0].Cliente_RazonSocial,
                    FechaCompra=guias[0].FechaCompra
                }
            );
            int indice = 0, condicion = 0;
            foreach (var cliente in guias)
            {
                if (condicion != 0)
                {
                    if (clientesGuias[indice].ClienteID == cliente.ClienteID)
                    {
                        clientesGuias[indice].NumeroGuias = clientesGuias[indice].NumeroGuias + cliente.NumeroGuias;

                    }
                    else
                    {
                        indice++;
                        clientesGuias.Add(new GuiaPrepagada
                        {
                            ClienteID = cliente.ClienteID,
                            NumeroGuias = cliente.NumeroGuias,
                            Cliente_RazonSocial = cliente.Cliente_RazonSocial,
                            FechaCompra=cliente.FechaCompra
                            
                        });
                    }
                }
                else
                {
                    condicion = 1;
                }

            }

            foreach (var g in clientesGuias)
            {
                g.GuiasUsadas = tablaGuias.Muestra_Guias().Where(x => x.FechaCreacion >= g.FechaCompra && x.ClienteID == g.ClienteID).Count();

            }
            SeparaPermisos(Session["Permisos"].ToString());
            return indice_temporal(clientesGuias);
        }



        public List<GuiaPrepagada> indice_temporal(List<GuiaPrepagada> lista)
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

        public ActionResult AgregarGuias()
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("../Home/login");
            }
            var ListaClientes = tablaClientes.Muestra_Clientes();
            List<GuiaPrepagada> clientes = new List<GuiaPrepagada>();
            foreach(var ocliente in ListaClientes)
            {
                clientes.Add(new GuiaPrepagada
                {
                    Cliente_RazonSocial = ocliente.RazonSocial,
                    ClienteID = ocliente.ClienteID,
                });
            }

            ViewBag.L_Clientes =new SelectList(clientes, dataValueField: "ClienteID", dataTextField: "Cliente_RazonSocial");         
            SeparaPermisos(Session["Permisos"].ToString());       
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AgregarGuias(GuiaPrepagada oguia)
        {
                 
            //* Modelo valido
            // Guardar u Obtener Destinatario 
            var peticion = new GuiaPrepagada
            {
                ClienteID= oguia.ClienteID,
                NumeroGuias=oguia.NumeroGuias
            };

            // SP para alta/modificacion de destinatario
            tablaGuiasP.Alta_GuiasPrepagadas(peticion);
            return RedirectToAction("Index");
        }
        [HttpPost]
        public string Detalles(int id)
        {
            var guias = tablaGuiasP.Muestra_GuiasPrepagadas().Where(x => x.ClienteID == id).OrderBy(x=>x.FechaCompra).ToList();
            guias = indice_temporal(guias);
            string res = "";
            foreach(var compra in guias)
            {
                int numeroGuias= compra.NumeroGuias;
                DateTime fechacompra = (DateTime)compra.FechaCompra;
                res = res+ "<tr><td>"+ numeroGuias + "</td>"
                    +"<td>"+ fechacompra+" </td></tr>";
            }
  
            return res;
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