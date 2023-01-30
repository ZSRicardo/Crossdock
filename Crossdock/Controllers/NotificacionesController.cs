using ClosedXML.Excel;
using Crossdock.Context.Commands;
using Crossdock.Models;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.VisualBasic.CompilerServices;
using Newtonsoft.Json;
using FSharp.Data.Runtime;
using FSharp.Data.Runtime.BaseTypes;
using System.Security.Policy;

namespace Crossdock.Controllers
{
    public class NotificacionesController : Controller
    {
        static List<SelectListItem> L_zon = new List<SelectListItem>();//llamada de clase
        TablaZonasCommands oZonas = new TablaZonasCommands();
        static List<SelectListItem> L_cli = new List<SelectListItem>();
        TablaClientesCommands oClientes = new TablaClientesCommands();
        TablaPaquetesCommands oPaquetes = new TablaPaquetesCommands();
        TablaGuiasCommands commandGuias = new TablaGuiasCommands();
        //instanciamos TablaGuiasCommands para traer todas las guias.
        TablaGuiasCommands CoGuias = new TablaGuiasCommands();
        static List<Paquetes> l_Paquetes = new List<Paquetes>();
        static List<Guias> L_Guias = new List<Guias>();
        static List<Guias> segGuias = new List<Guias>();
        static List<Guias> Filtro_segGuias = new List<Guias>();


        static string s_Zona = "";
        static string s_Cliente = "";
       // static string pagina = "";
        public ActionResult PaquetesPorLlegar()
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("../Home/login");
            }
            ViewBag.L_Zonas = L_zon;
            ViewBag.L_Clientes = L_cli;
            if (s_Cliente != "")
                ViewBag.DCliente = s_Cliente;
            else
                ViewBag.DCliente = "Selecciona un Cliente";

            if (s_Zona != "")
                ViewBag.DZona = s_Zona;
            else
                ViewBag.DZona = "Selecciona una Zona";

            SeparaPermisos(Session["Permisos"].ToString());
            return View(L_Guias);
        }



       




        public JsonResult ListaPaquetes()
        {
            segGuias.Clear();
            l_Paquetes = oPaquetes.Muestra_PaquetesUno();
        
         
            
            var l_Guias = CoGuias.Muestra_Guias();
            for (int r = 0; r < l_Guias.Count; r++)
            {
                int c = 0;
                for (int i = 0; i < l_Paquetes.Count; i++)
                {
                    if (l_Guias[r].Guia == l_Paquetes[i].Guia)
                    {
                        c++;
                    }
                }
                if (c == 0)
                {
                    segGuias.Add(l_Guias[r]);
                }
            }

            segGuias = indice_general(segGuias);
            Filtro_segGuias = segGuias;

            return Json(new { data = segGuias }, JsonRequestBehavior.AllowGet);
            
          //  var l_Guias= CoGuias.Muestra_Guias().Where(x=>x.ZonaId!=0).ToList();
            //return Json(new { data = L_Guias }, JsonRequestBehavior.AllowGet);
        }

              [HttpGet]
        public JsonResult ListaGuiasFiltro(string cliente, string zona)
        {
            try { 
            if (cliente != "" && zona == "") {
                Filtro_segGuias = segGuias.Where(x => x.Cliente_RZ == cliente).ToList();
                Filtro_segGuias = indice_general(Filtro_segGuias);
                return Json(new { data = Filtro_segGuias }, JsonRequestBehavior.AllowGet);
            }
            else if (cliente == "" && zona != "")
            {
                if (zona == "K") 
                {
                    Filtro_segGuias = segGuias.Where(x => x.ZonaDes == "").ToList();
                    Filtro_segGuias = indice_general(Filtro_segGuias);
                    return Json(new { data = Filtro_segGuias }, JsonRequestBehavior.AllowGet);
                } 
                else
                { 
                Filtro_segGuias = segGuias.Where(x => x.ZonaDes == zona).ToList();
                Filtro_segGuias = indice_general(Filtro_segGuias);
                return Json(new { data = Filtro_segGuias }, JsonRequestBehavior.AllowGet);
                }
            }
            else if (cliente != "" && zona != "")
            {
                if (zona == "K")
                {
                    Filtro_segGuias = segGuias.Where(x => x.ZonaDes == "" && x.Cliente_RZ == cliente).ToList();
                    Filtro_segGuias = indice_general(Filtro_segGuias);
                    return Json(new { data = Filtro_segGuias }, JsonRequestBehavior.AllowGet);
                }
                else { 

                Filtro_segGuias = segGuias.Where(x => x.ZonaDes == zona && x.Cliente_RZ == cliente).ToList();
                Filtro_segGuias = indice_general(Filtro_segGuias);
                return Json(new { data = Filtro_segGuias }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                Filtro_segGuias = segGuias.ToList();
                Filtro_segGuias = indice_general(Filtro_segGuias);
                return Json(new { data = Filtro_segGuias }, JsonRequestBehavior.AllowGet);
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
                return Json(new { data = Filtro_segGuias }, JsonRequestBehavior.AllowGet);
            }
        }    

            public ActionResult Descargar(string id)
            {
            id = JsonConvert.DeserializeObject<string>(id);
            int id2 = Convert.ToInt32(id);
            Guias l = new Guias();

            foreach (var p in Filtro_segGuias)
            {
                if (p.GuiaID == id2)
                {
                    l = p;
                }

            }
            var URL = l.Url;
            WebClient Client = new WebClient();
            var archivo = Client.DownloadData(URL);

            TempData["file"] = Convert.ToBase64String(archivo);
            TempData["fileName"] = l.Guia;

            ViewBag.L_Clientes = L_cli;
            ViewBag.L_Zonas = L_zon;
            if (s_Cliente == "" && s_Zona == "")
            {
                ViewBag.DCliente = "Selecciona un Cliente";
                ViewBag.DZona = "Selecciona una Zona";
            }
            else if (s_Cliente != "" && s_Zona == "")
            {
                ViewBag.DCliente = s_Cliente;
                ViewBag.DZona = "Selecciona una Zona";
            }
            else if (s_Cliente == "" && s_Zona != "")
            {
                ViewBag.DCliente = "Selecciona un Cliente";
                ViewBag.DZona = s_Zona;

            }
            else if (s_Cliente != "" && s_Zona != "")
            {
                ViewBag.DZona = s_Zona;
                ViewBag.DCliente = s_Cliente;
            }
            SeparaPermisos(Session["Permisos"].ToString());
            //  return View("PaquetesPorLlegar", L_Guias);
            return RedirectToAction("PaquetesPorLlegar");
        }

        public ActionResult LimpiarSeleccion()
        {
           
            var listaZonas = oZonas.Muestra_Zonas().ToList();//consumo de sp
            //llenamos la lista con las zonas que vamos a mostrar 
            L_zon = new SelectList(listaZonas, dataValueField: "Descripcion", dataTextField: "Descripcion").ToList();//llena el DropDownlist            
            ViewBag.L_Zonas = L_zon;

            var listaClientes = oClientes.Muestra_Clientes().ToList();//consumo de sp
            //llenamos la lista con las zonas que vamos a mostrar 
            L_cli = new SelectList(listaClientes, dataValueField: "RazonSocial", dataTextField: "RazonSocial").ToList();//llena el DropDownlist            
            ViewBag.L_Clientes = L_cli;

            ViewBag.DCliente = "Selecciona un Cliente";
            ViewBag.DZona = "Selecciona una Zona";

             

            s_Cliente = "";
            s_Zona = "";
            return RedirectToAction("PaquetesPorLlegar");

        }

       





        [HttpPost]
        public JsonResult Exportar(string cadena) //recibimos el valor de cadena de la vista
        {

            

            string fileName = "Notificaciones_" + DateTime.Now.ToString("yyyy-MM-dd") + ".xlsx";


            DataTable dt = new DataTable();


            string fullPath = Path.Combine(Server.MapPath("~/"), fileName);
            dt.Locale = new System.Globalization.CultureInfo("es-PE");
            //   string handle = Guid.NewGuid().ToString();
            dt.Columns.Add("Numero de Guia", typeof(string));
            dt.Columns.Add("Destinatario", typeof(string));
            dt.Columns.Add("Direccion de Destinatario", typeof(string));
            dt.Columns.Add("Cliente", typeof(string));
            dt.Columns.Add("Zona", typeof(string));
            dt.Columns.Add("Fecha de Generacion", typeof(string));
            dt.Columns.Add("Instrucciones", typeof(string));  //se agrega columna Instrucciones (Ricardo)
            dt.Columns.Add("Descripción", typeof(string));  //se agrega columna Descripción (Ricardo)




         
            
            string valor = cadena; //almacenamos lo que recibimos en cadena

            var tbfiltrada = Filtro_segGuias; //aqui ya almacenamos nuestra tabla ya previamente filtrada

            if (valor != null || valor != "") //si lo que recibimos es diferente a nulo o a nada
            {
                //buscamos coincidencias
                tbfiltrada = tbfiltrada.Where(x =>    //ponemos que la variable de tbfiltrada (que ya estaba filtrada o no) reciba el mismo filtro cuando las coincidencias son iguales a la cadena
                x.Guia.Contains(cadena.ToLower()) || x.Guia.Contains(cadena.ToUpper()) ||  //busco coincidencia en guia
                x.Destinatario.Contains(cadena.ToLower()) || x.Destinatario.Contains(cadena.ToUpper()) ||  //en destinatario
                x.DireccionDestinatario.Contains(cadena.ToLower()) || x.DireccionDestinatario.Contains(cadena.ToUpper()) || //dirección
                x.Cliente_RZ.Contains(cadena.ToLower()) || x.Cliente_RZ.Contains(cadena.ToUpper()) ||  //razón social
                x.ZonaDes.Contains(cadena.ToLower()) || x.ZonaDes.Contains(cadena.ToUpper()) || //zona descarga
                x.Fecha.Contains(cadena.ToLower()) || x.Fecha.Contains(cadena.ToUpper())).ToList(); // || //fecha
                
                /*x.Instrucciones.Contains(cadena.ToLower()) || x.Instrucciones.Contains(cadena.ToUpper()) || //instrucciones
                x.Descripcion.Contains(cadena.ToLower()) || x.Descripcion.Contains(cadena.ToUpper())).ToList(); //descripción (y finaliza)
                */

                Filtro_segGuias = indice_general(Filtro_segGuias);




            }
            else
            {
                //en caso de que la cadena sea nula (por tanto el valor es nulo) lanzaremos a tbfilta igual a filtro_seguias  (se queda igual)
                tbfiltrada = Filtro_segGuias = indice_general(Filtro_segGuias);  //se queda como esta sin buscar nada
            }





            var tabla = tbfiltrada; //declaramos a tabla como a tbfiltrada (sea o no filtrada por cliente y zona y o exista una busqueda en la vista)
            





            foreach (var rp in tabla) //aqui se quito el Filto_segGuias
            {



                //se declararon nuevas variables para el almacen de datos de la tabla 
                //se pasaron los datos de la tabla como el valor de nuestras variables
                var guia = rp.Guia;   //guias
                var destinatario = rp.Destinatario;  //destinatario
                var direccion = rp.DireccionDestinatario;  //dirección
                var clienterz = rp.Cliente_RZ; //la razón social del cliente
                var zonades = rp.ZonaDes;  //zona de descarga

                //en caso de que el valor no exista un en ZonaDes (Que en su mayoria es porque la cobertura no se encontro)
                if (zonades == null || zonades == "") //veremos que en el caso de que este sea nulo o no exista
                {
                    zonades = "K";  //en automatico agregamos la zona K
                }
                else  //caso contrario (que si exista un valor)
                {
                    zonades = rp.ZonaDes;   //el valor se mantiene (volvemos a cargar con el registro de la tabla para prevenir problemas)
                }

                var fecha = rp.Fecha; //fecha 

                //aqui fue la razón por la cual se realizo la declaración de variables

                
                var instrucciones = rp.Instrucciones;   //instrucciones

                //verificamos que existan valores en nuestra variable "instrucciones"
                if(instrucciones==null || instrucciones == "") //en caso de que no existan valores (que sea nula)
                {
                    instrucciones = "NO EXISTEN DATOS";  //nuestro valor de varibale cambiara a "NO EXISTEN DATOS"
                }
                else //en caso de que si exista un valor
                {
                    instrucciones = rp.Instrucciones;  //el valor se mantendra (en este caso para evitar problemas se vuelve a obtener el valor)
                   
                }


                var descripcion = rp.Descripcion; //descripción
                if (descripcion == null || descripcion == "") //en caso de que no existan valores en descripción (que sea nula)
                {
                    descripcion = "NO EXISTEN DATOS"; //el valor de la variable cambiara como en instrucciones a "NO EXISTEN DATOS"
                }
                else  //en caso de que si existan valores
                {
                    descripcion = rp.Descripcion;  //el valor se mantendra (igual para evitar problemas se vuelve a obtener nuestro valor)
                   
                }

                dt.Rows.Add(new object[]   //ahora entramos para mostrar nuestros datos
                {


                    //aqui comentamos los datos que venian directamente de la lista por medio de la consulta (ya los tenemos en las variables)

                     /* rp.Guia,
                      rp.Destinatario,
                      rp.DireccionDestinatario,
                      rp.Cliente_RZ,
                      rp.ZonaDes,
                      rp.Fecha,
                      rp.Instrucciones,  //se agrego para imprimir instrucciones(Ricardo)
                      rp.Descripcion   //se agrego para imprimir descrípción (Ricardo)

                  */

                    //solo imprimimos (o mandamos a llamar) a nuestras variables
                     guia,
                     destinatario, 
                     direccion, 
                     clienterz, 
                     zonades, //ya se imprime la zona sin importar si esta nula o no
                     fecha, 
                     instrucciones,  //aqui ya se imprime si instrucciones tiene o no datos (ya tiene valor asignado)
                     descripcion  //aqui tambien ya se imprime si descripcion ya tiene o no datos(ya tiene valor asignado)


                 }

                
               
                
                );
            }
          
            
            //--------------------
            
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

            ViewBag.L_Clientes = L_cli;
            ViewBag.L_Zonas = L_zon;
            if (s_Cliente == "" && s_Zona == "")
            {
                ViewBag.DCliente = "Selecciona un Cliente";
                ViewBag.DZona = "Selecciona una Zona";
            }
            else if (s_Cliente != "" && s_Zona == "")
            {
                ViewBag.DCliente = s_Cliente;
                ViewBag.DZona = "Selecciona una Zona";
            }
            else if (s_Cliente == "" && s_Zona != "")
            {
                ViewBag.DCliente = "Selecciona un Cliente";
                ViewBag.DZona = s_Zona;

            }
            else if (s_Cliente != "" && s_Zona != "")
            {
                ViewBag.DZona = s_Zona;
                ViewBag.DCliente = s_Cliente;
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

        private List<Guias> indice_general(List<Guias> lista)
        {
            int indice = 1;
            foreach (Guias guia in lista)
            {
                guia.ID_Temporal = indice;
                indice++;
            }
            return lista;
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