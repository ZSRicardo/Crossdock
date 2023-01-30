using Crossdock.Context.Commands;
using Crossdock.Context.Queries;
using Crossdock.Models;
using CrossDockLib;
using CrossDockLib.Models;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using System.Web.UI;
using Excel = Microsoft.Office.Interop.Excel;
using System.IO;
using System.Collections.Generic;
using Ionic.Zip;
using System.Text;
using SpreadsheetLight;
using System.Data;
using ClosedXML.Excel;
using iText.Kernel.Pdf;
using iText.Kernel.Utils;
using Microsoft.VisualBasic.CompilerServices;
using Syncfusion.XlsIO;
using Newtonsoft.Json;

namespace Crossdock.Controllers
{
    public class GuiasController : Controller
    {
        private GenerarGuiaCommands _generarGuiaCommands = new GenerarGuiaCommands();
        private TablaClientesQueries _tablaClientesQueries = new TablaClientesQueries();
        private PDFUtilities _pDFUtilities = new PDFUtilities();
        private S3Service _s3Service = new S3Service();
        static private List<GuiaTaimingoViewModel> LTGuias = new List<GuiaTaimingoViewModel>();
        static private List<LayoutGrid> oListaGuias = new List<LayoutGrid>();
        static private HttpPostedFileBase Excel;
        static private GuiaTaimingoViewModel oGuiaManual = new GuiaTaimingoViewModel();//llamada de clase
        static private EtiquetaIngresoBodega oEtiqueta = new EtiquetaIngresoBodega();


        public ActionResult Index()
        {
            oListaGuias.Clear();
            LTGuias.Clear();
            SeparaPermisos(Session["Permisos"].ToString());
            return View();
        }
        public ActionResult CargaMasiva()
        {
            ViewBag.Guardar = "disabled";
            SeparaPermisos(Session["Permisos"].ToString());
            return View(oListaGuias);
        }

        // Obtiene archivo de descarga de variable TempData

        [HttpPost]
        public ActionResult CargaMasiva(HttpPostedFileBase excelfile)
        {
            try { 
            oListaGuias.Clear();
            LTGuias.Clear();
            var lista = getGuias(excelfile);
            if (lista.Count != 0)
            {
                ViewBag.Guardar = "";
            }
            else
            {
                ViewBag.Guardar = "disabled";
            }
            SeparaPermisos(Session["Permisos"].ToString());
            return View(lista);
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
                SeparaPermisos(Session["Permisos"].ToString());
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public ActionResult Limpiar()
        {
            oListaGuias.Clear();
            LTGuias.Clear();
            SeparaPermisos(Session["Permisos"].ToString());
            return View("CargaMasiva", oListaGuias);
        }

        public FileResult GetFile()
        {
            try { 
            var file = Convert.FromBase64String(TempData["file"].ToString());
            var fileName = TempData["fileName"].ToString();
            SeparaPermisos(Session["Permisos"].ToString());
            return File(file, "application/pdf", fileName);
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


        private List<LayoutGrid> getGuias(HttpPostedFileBase excelfile)
        {
            try
            {
                VerificaTexto oVerificaTexto = new VerificaTexto();
            Excel = excelfile;
            int registro = 1;
            if (Excel == null)
            {
                return muestra_Mensaje_Error("Selecciona un Archivo");
            }

            LayoutGrid oLayout = new LayoutGrid();
            string jsonString = "";
            string path = Server.MapPath("~/Content/" + Excel.FileName);
            string extencion = Path.GetExtension(Excel.FileName);
            if (System.IO.File.Exists(path))
                System.IO.File.Delete(path);

            excelfile.SaveAs(path);

            //Instantiate the spreadsheet creation engine.
            ExcelEngine excelEngine = new ExcelEngine();

            //Instantiate the Excel application object.
            IApplication application = excelEngine.Excel;
            application.DefaultVersion = ExcelVersion.Xlsx;

            //Load the input Excel file.
            FileStream stream = new FileStream(path, FileMode.Open, FileAccess.ReadWrite);
            IWorkbook book = application.Workbooks.Open(stream);
            stream.Close();

            //Access first worksheet.
            IWorksheet worksheet = book.Worksheets[0];

            //Access a range.
            IRange range = worksheet.Range["A1:H10"];

            MemoryStream jsonStream = new MemoryStream();


            book.SaveAsJson(jsonStream, worksheet); //guarda la primer hoja 

            byte[] json = new byte[jsonStream.Length];

            //Lee el Json stream y comvierte a objeto json.
            jsonStream.Position = 0;
            jsonStream.Read(json, 0, (int)jsonStream.Length);
            jsonString = Encoding.UTF8.GetString(json);
            var lista = JsonConvert.DeserializeObject<JsonToModel>(jsonString);
            List<GuiaTaimingoViewModel> listaList = new List<GuiaTaimingoViewModel>();
            GuiaTaimingoViewModel oguia = new GuiaTaimingoViewModel();

            foreach (var p in lista.Guias)
            {
                if (p.Nombre_Destinatario != null || p.Apellido_Paterno_Destinatario != null || p.Celular_Destinatario != null)
                {
                    registro++;

                    var oGuiaTaimingo = new GuiaTaimingoViewModel();

                    if (p.Numero_Pedido != null)// numero de guia 
                    {
                        if (p.Numero_Pedido.Length >= 10)
                        {
                            oGuiaTaimingo.NoPedido = p.Numero_Pedido;
                        }

                    }



                    if (p.Nombre_Destinatario == null)// Nombre de destinatario
                    {
                        return muestra_Mensaje_Error("Verifica el Nombre de Destinatario en la fila " + registro);
                    }
                    else
                    {
                        oGuiaTaimingo.Nombre = oVerificaTexto.corrige_Nombres(p.Nombre_Destinatario);
                    }

                    if (p.Apellido_Paterno_Destinatario == null)//Apellido paterno
                    {
                        return muestra_Mensaje_Error("Verifica el Apellido Paterno en la fila " + registro);
                    }
                    else
                    {
                        oGuiaTaimingo.ApellidoPaterno = oVerificaTexto.corrige_Nombres(p.Apellido_Paterno_Destinatario);
                    }

                    if (p.Apellido_Materno_Destinatario != null)//Apellido materno
                    {
                        oGuiaTaimingo.ApellidoMaterno = oVerificaTexto.corrige_Nombres(p.Apellido_Materno_Destinatario);
                    }

                    if (p.Celular_Destinatario != null)//numero de celular 
                    {
                        if (oVerificaTexto.corrige_celular(p.Celular_Destinatario).Length == 10)
                        {
                            oGuiaTaimingo.Celular = oVerificaTexto.corrige_celular(p.Celular_Destinatario);
                        }
                        else
                        {
                            return muestra_Mensaje_Error("Verifica el Numero de Celular en la fila " + registro);
                        }
                    }
                    else
                    {
                        return muestra_Mensaje_Error("Verifica el Numero de Celular en la fila " + registro);
                    }

                    if (p.Email != null)//Email
                    {
                        oGuiaTaimingo.Email = p.Email;
                    }
                    else 
                    {
                            oGuiaTaimingo.Email = "";
                        }
                    

                    if (p.Calle != null)//calle
                    {
                        oGuiaTaimingo.Calle = oVerificaTexto.corrige_Nombres(p.Calle);
                    }
                    else
                    {
                        return muestra_Mensaje_Error("Verifica la Calle en la fila" + registro);
                    }

                    if (p.Numero_Exterior != null)// numero exterior
                    {
                        oGuiaTaimingo.NumeroExt = oVerificaTexto.corrige_direccion(p.Numero_Exterior);
                    }
                    else
                    {
                        return muestra_Mensaje_Error("Verifica el Numero Exterior en la fila " + registro);
                    }

                    if (p.Numero_Interior != null)//numero interior
                    {
                        oGuiaTaimingo.NumeroInt = oVerificaTexto.corrige_direccion(p.Numero_Interior);
                    }

                    if (p.Colonia != null)// Colonia
                    {
                        oGuiaTaimingo.Colonia = oVerificaTexto.corrige_Nombres(p.Colonia);
                    }
                    else
                    {
                        return muestra_Mensaje_Error("Verifica la Colonia en la fila " + registro);
                    }

                    if (p.Codigo_Postal != null)// Codigo postal
                    {
                        oGuiaTaimingo.CodigoPostal = oVerificaTexto.corrige_celular(p.Codigo_Postal);
                    }
                    else
                    {
                        return muestra_Mensaje_Error("Verifica el Codigo Postal en la fila " + registro);
                    }

                    if (p.Latitud != null)// Latitud
                    {
                        oGuiaTaimingo.Des_Latitud = float.Parse(oVerificaTexto.corrige_celular(p.Latitud));
                    }

                    if (p.Longitud != null)// Latitud
                    {
                        oGuiaTaimingo.Des_Longitud = float.Parse(oVerificaTexto.corrige_celular(p.Longitud));
                    }

                    if (p.Medida_Paquete != null)// medida de paquete
                    {
                        oGuiaTaimingo.Medida = oVerificaTexto.corrige_direccion(p.Medida_Paquete);
                    }
                    else
                    {
                        return muestra_Mensaje_Error("Verifica la Medida de Paquete en la fila " + registro);
                    }

                    if (p.Peso_paquete != null)// peso del paquete
                    {
                        oGuiaTaimingo.Peso = Convert.ToDouble(oVerificaTexto.corrige_celular(p.Peso_paquete));
                    }
                    else
                    {
                        return muestra_Mensaje_Error("Verifica el peso del paquete en la fila " + registro);
                    }

                    if (p.Descripcion_Paquete != null)// descripcion del paquete
                    {
                        oGuiaTaimingo.Descripcion = oVerificaTexto.corrige_direccion(p.Descripcion_Paquete);
                    }

                    if (p.Instrucciones_Paquete != null)// descripcion del paquete
                    {
                        oGuiaTaimingo.Instrucciones = oVerificaTexto.corrige_direccion(p.Instrucciones_Paquete);
                    }
                    if (p.Tipo_Servicio != null)
                    {
                        var tipo_Servicio = Convert.ToInt32(oVerificaTexto.corrige_celular(p.Tipo_Servicio));
                        if (tipo_Servicio == 0 || tipo_Servicio == 1)
                        {
                            oGuiaTaimingo.TServicio = tipo_Servicio;
                        }
                        else
                        {
                            return muestra_Mensaje_Error("Tipo Servicio fuera de rango en la fila " + registro);
                        }

                    }
                    else
                    {
                        oGuiaTaimingo.TServicio = 0;
                    }

                    if (p.Total_de_Paquetes != null)// numero de paquetes por guia
                    {

                             if(Convert.ToInt32(oVerificaTexto.corrige_celular(p.Total_de_Paquetes))==0)
                            {
                                return muestra_Mensaje_Error("Ingresa un Total de paquetes mayor a 0 en el registro " + registro);
                            }
                            else
                            {
                                oGuiaTaimingo.Numero_de_Paquetes_Extra = Convert.ToInt32(oVerificaTexto.corrige_celular(p.Total_de_Paquetes));

                            }

                    }
                        else
                     {
                            return muestra_Mensaje_Error("Ingresa un Total de paquetes mayor a O en el registro " + registro);
                     }

                    //verificamos la cantidad de paquetes extra que hay para la misma guia
                    oGuiaTaimingo.Guia = obtenerGuia(registro - 1);
                    LTGuias.Add(oGuiaTaimingo);
                    var cliente = _tablaClientesQueries.Muestra_ClientesMod((int)Session["UserClienteID"]);

                    oGuiaTaimingo.RazonSocialCliente = cliente.RazonSocial;


                    if (oGuiaTaimingo.Numero_de_Paquetes_Extra > 1)
                    {
                        var guia = oGuiaTaimingo.Guia;

                        for (int i = 1; i < oGuiaTaimingo.Numero_de_Paquetes_Extra; i++)
                        {
                            var copiaDatos = new GuiaTaimingoViewModel
                            {

                                NoPedido = oGuiaTaimingo.NoPedido,
                                Nombre = oGuiaTaimingo.Nombre,
                                ApellidoMaterno = oGuiaTaimingo.ApellidoMaterno,
                                ApellidoPaterno = oGuiaTaimingo.ApellidoPaterno,
                                NumeroExt = oGuiaTaimingo.NumeroExt,
                                NumeroInt = oGuiaTaimingo.NumeroInt,
                                Calle = oGuiaTaimingo.Calle,
                                Colonia = oGuiaTaimingo.Colonia,
                                CodigoPostal = oGuiaTaimingo.CodigoPostal,
                                Celular = oGuiaTaimingo.Celular,
                                Descripcion = oGuiaTaimingo.Descripcion,
                                Instrucciones = oGuiaTaimingo.Instrucciones,
                                TServicio = oGuiaTaimingo.TServicio,
                                Medida = oGuiaTaimingo.Medida,
                                Peso = oGuiaTaimingo.Peso,
                                Email = oGuiaTaimingo.Email,
                                Des_Latitud = oGuiaTaimingo.Des_Latitud,
                                Des_Longitud = oGuiaTaimingo.Des_Longitud,
                                RazonSocialCliente = oGuiaTaimingo.RazonSocialCliente,
                                Guia = guia + "-" + i.ToString()

                            };

                            LTGuias.Add(copiaDatos);
                        }
                    }
                    LayoutGrid layout = new LayoutGrid();
                    layout.Guia = oGuiaTaimingo.Guia;
                    layout.NoPedido = oGuiaTaimingo.NoPedido;
                    //Guardamos la informacion que mostraremos en el Grid 
                    layout.Destinatario = oGuiaTaimingo.Nombre + " " + oGuiaTaimingo.ApellidoPaterno + " " + oGuiaTaimingo.ApellidoMaterno
                                        + ", " + oGuiaTaimingo.Celular.ToString() + ", " + oGuiaTaimingo.Email;
                    layout.DireccionDes = oGuiaTaimingo.Calle + ", " + oGuiaTaimingo.NumeroExt + ", " + oGuiaTaimingo.NumeroInt + ", " + oGuiaTaimingo.Colonia +
                     ", " + oGuiaTaimingo.CodigoPostal;
                    layout.InformacionPaq = oGuiaTaimingo.Medida + ", " + oGuiaTaimingo.Peso.ToString() + ", " + oGuiaTaimingo.Descripcion + ", "
                     + oGuiaTaimingo.Instrucciones;
                    //atraves del sp agregaremos el delivery
                    // oLayout.CodigoDelivery = oGuiaTaimingo.Del_id.ToString();
                    layout.CodigoUsuario = oGuiaTaimingo.Usu_id.ToString();
                    layout.CodigoCliente = oGuiaTaimingo.Cli_id.ToString();
                    layout.TServicio = oGuiaTaimingo.TServicio;
                    layout.Paquetes_Extra = oGuiaTaimingo.Numero_de_Paquetes_Extra;
                    oListaGuias.Add(layout);
                }

            }
            return oListaGuias;
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

        public string obtenerGuia(int incremento)
        {
            try { 
            TablaGuiasCommands og = new TablaGuiasCommands();
            var log = og.Muestra_Guias();
            var ultimaguia = log[log.Count - 1].Guia;
            string[] separaGuia = ultimaguia.Split('-');
            separaGuia[0] = separaGuia[0].Substring(separaGuia[0].Length - 4);
            int ul = Convert.ToInt32(separaGuia[0]);
            ul = ul + incremento;
            var diaj = Convert.ToString(DateTime.Now.DayOfYear);//dia juliano
            var cli_Identificador = (string)Session["ClienteIdentificador"];
            string ceros = "";
            if (ul >= 1000)
            {
                ceros = "00";
            }
            if (ul >= 10000)
            {
                ceros = "0";
            }
            if (ul >= 100000)
            {
                ceros = "";
            }

            string gui = "TAIMEX" + cli_Identificador.Substring(cli_Identificador.Length - 3) + diaj + ceros + ul;

            return gui;
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

        public List<LayoutGrid> muestra_Mensaje_Error(string mensaje)
        {
            List<LayoutGrid> listavacia = new List<LayoutGrid>();
            ViewBag.Message = mensaje;
            return listavacia;
        }

        [HttpPost]
        public async Task<ActionResult> GuardaGuias()
        {
            try { 
            if (oListaGuias.Count == 0)
            {
                // ViewBag.Message = string.Format("Selecciona un archivo");
                ViewBag.Message = string.Format("formato invalido");
                SeparaPermisos(Session["Permisos"].ToString());
                return View("CargaMasiva", oListaGuias);
            }
            else
            {
                List<byte[]> L_guias = new List<byte[]>();
                List<string> L_nombre_guia = new List<string>();

                GuiaTaimingoViewModel oGuias = new GuiaTaimingoViewModel();


                // Guardar u Obtener Destinatario 
                for (int l = 0; l <= LTGuias.Count - 1; l++)
                {
                    oGuias = LTGuias[l];

                    var destinatario = new Destinatarios
                    {
                        DestinatarioID = 0,
                        Nombre = oGuias.Nombre,
                        ApellidoP = oGuias.ApellidoPaterno,
                        ApellidoM = oGuias.ApellidoMaterno,
                        Celular = (long)Convert.ToDouble(oGuias.Celular),
                        Email = oGuias.Email,
                        Calle = oGuias.Calle,
                        NumeroExt = oGuias.NumeroExt,
                        NumeroInt = oGuias.NumeroInt,
                        Colonia = oGuias.Colonia,
                        CodigoPostal = oGuias.CodigoPostal,
                        Latitud = oGuias.Des_Latitud,
                        Longitud = oGuias.Des_Longitud,
                    };

                    // SP para alta/modificacion de destinatario
                    var destinatarioID = _generarGuiaCommands.VerificaDestinatario(destinatario);

                    // Mapear ViewModel a Model

                    var guiaTaimingo = new Guias
                    {
                        GuiaID = 0,
                        FechaCreacion = DateTime.Now,
                        Medida = oGuias.Medida,
                        Peso = oGuias.Peso,
                        Descripcion = oGuias.Descripcion,
                        Instrucciones = oGuias.Instrucciones,
                        DestinatarioID = destinatarioID,
                        ClienteID = Convert.ToInt32(Session["UserClienteID"]),
                        UsuarioID = Convert.ToInt32(Session["UserID"]),
                        //  DeliveryID = oGuias.Del_id,
                        NoPedido = oGuias.NoPedido,
                        Tipo_Guia = Convert.ToBoolean(oGuias.TServicio),
                    };
                    guiaTaimingo.Guia = oGuias.Guia;
                    //validamos si existe cobertura en esa zona 
                    var oZonas = muestra_zona(destinatario.CodigoPostal);
                    var lzona = "";
                    if (oZonas.Count == 0)
                    {
                        lzona = "K";
                    }
                    else
                    {
                        lzona = oZonas[0];
                    }

                    // Modelo para llenado de pdf
                    DatosPDFGuia datosPDFGuia = new DatosPDFGuia
                    {
                        Calle = destinatario.Calle,
                        NumeroExt = destinatario.NumeroExt,
                        NumeroInt = destinatario.NumeroInt,
                        Colonia = destinatario.Colonia,
                        CodigoPostal = destinatario.CodigoPostal,
                        NombreDestinatario = $"{destinatario.Nombre} {destinatario.ApellidoP}  {destinatario.ApellidoM}",
                        NumeroGuia = guiaTaimingo.Guia,
                        ClienteRazonSocial = oGuias.RazonSocialCliente,
                        Peso = guiaTaimingo.Peso,
                        Medida = guiaTaimingo.Medida,
                        guiaExterna = guiaTaimingo.NoPedido,
                        Zona = lzona,
                        id_paqueteria = 1,//falta definir delivery para las zonas donde no hay covertura 
                    };

                    byte[] pdfArchivo = _pDFUtilities.CrearPDFGuia(datosPDFGuia);
                    //crearemos un solo pdf para descargargarlo en vez de un zip
                    L_guias.Add(pdfArchivo);


                    //var pdfFileName = $"pdf_{guiaTaimingo.Guia}.pdf";
                    var pdfFileNameS3 = $"pdf_{guiaTaimingo.Guia}";
                    L_nombre_guia.Add(pdfFileNameS3);

                    // Subir Documento/Archivo Guia a S3 y guardar URL devuelta

                    //   guiaTaimingo.Url = await _s3Service.UploadFileToS3(pdfArchivo, pdfFileNameS3, "application/pdf");

                    guiaTaimingo.Url = "https://crossdock-guias.s3.amazonaws.com/" + pdfFileNameS3;
                    // SP para alta de guia
                    _generarGuiaCommands.AltaGuia(guiaTaimingo);
                    var Spdf = Encoding.ASCII.GetString(pdfArchivo);

                }
                //subir archivos a s3
                for (int i = 0; i < L_guias.Count; i++)
                {
                    await _s3Service.UploadFileToS3(L_guias[i], L_nombre_guia[i], "application/pdf");
                }


                byte[] pdfArchivo1 = Combine(L_guias);
                var pdfFileName = $"pdf_Guias.pdf";

                TempData["file"] = Convert.ToBase64String(pdfArchivo1);
                TempData["fileName"] = pdfFileName;

                //obtenemos el id del cliente 
                int cID = Convert.ToInt32(Session["UserClienteID"].ToString());
                TablaClientesCommands oC = new TablaClientesCommands();
                var C_Cliente = oC.Muestra_ClientesMod(cID);
                var rz = C_Cliente[0].RazonSocial;

                /*****************************/ //cambiar correo de prueba, poner correo de administrativos

                // **********Envio de notificacion de mensajes
                EnviarMensaje msn = new EnviarMensaje();
                msn.EnvioMensaje("5614640937", "Notificacion. Guias Generadas. La empresa " + rz + " genero nuevas guias.");

                EnviarMail objMail = new EnviarMail();
                objMail.EnviaMail("vcastro@taimingo.com", "Notificacion. Guias Generadas. <No responder>", "Nuevas Guias Generadas", objMail.GeneraBody_NuevasGuias(rz));/*+*/
                objMail.EnviaMail("dsandoval@taimingo.com", "Notificacion. Guias Generadas. <No responder>", "Nuevas Guias Generadas", objMail.GeneraBody_NuevasGuias(rz));/*+*/
                SeparaPermisos(Session["Permisos"].ToString());
                return RedirectToAction("Index", "Guias");


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
                return null;
            }
        }

        [HttpPost]
        public JsonResult Layout_muestra()
        {
            try { 
            var fileName = "Layout" + ".xlsx";
            DataTable dt = new DataTable();
            string fullPath = Path.Combine(Server.MapPath("~/"), fileName);
            dt.Locale = new System.Globalization.CultureInfo("es-PE");
            //   string handle = Guid.NewGuid().ToString();
            dt.Columns.Add("Numero_Pedido", typeof(string));
            //dt.Columns.Add("Numero de Guia", typeof(string));
            dt.Columns.Add("Nombre_Destinatario", typeof(string));
            dt.Columns.Add("Apellido_Paterno_Destinatario", typeof(string));
            dt.Columns.Add("Apellido_Materno_Destinatario", typeof(string));
            dt.Columns.Add("Celular_Destinatario", typeof(string));
            dt.Columns.Add("Email", typeof(string));
            dt.Columns.Add("Calle", typeof(string));
            dt.Columns.Add("Numero_Exterior", typeof(string));
            dt.Columns.Add("Numero_Interior", typeof(string));
            dt.Columns.Add("Colonia", typeof(string));
            dt.Columns.Add("Codigo_Postal", typeof(string));
            dt.Columns.Add("Latitud", typeof(string));
            dt.Columns.Add("Longitud", typeof(string));
            dt.Columns.Add("Medida_Paquete", typeof(string));
            dt.Columns.Add("Peso_paquete", typeof(string));
            dt.Columns.Add("Descripcion_Paquete", typeof(string));
            dt.Columns.Add("Instrucciones_Paquete", typeof(string));
            dt.Columns.Add("Tipo_Servicio", typeof(string));
            dt.Columns.Add("Total_de_Paquetes", typeof(string));
            dt.TableName = "Guias";//tabla de objetos tipo altas 

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
            SeparaPermisos(Session["Permisos"].ToString());
            return Json(new { fileName = fileName }, JsonRequestBehavior.AllowGet);
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

        public byte[] Combine(List<byte[]> pdfs)
        {
            using (var writerMemoryStream = new MemoryStream())
            {
                using (var writer = new PdfWriter(writerMemoryStream))
                {
                    using (var mergedDocument = new PdfDocument(writer))
                    {
                        var merger = new PdfMerger(mergedDocument);

                        foreach (var pdfBytes in pdfs)
                        {
                            using (var copyFromMemoryStream = new MemoryStream(pdfBytes))
                            {
                                using (var reader = new PdfReader(copyFromMemoryStream))
                                {
                                    using (var copyFromDocument = new PdfDocument(reader))
                                    {
                                        merger.Merge(copyFromDocument, 1, copyFromDocument.GetNumberOfPages());
                                    }
                                }
                            }
                        }
                    }
                }

                return writerMemoryStream.ToArray();
            }
        }


        public List<string> muestra_zona(string codigo)
        {
            //traemos la lista de colonias que corresponden al codigo postal 
            ConsultaColonias oColonias = new ConsultaColonias();

            List<string> zonas = new List<string>();
            //comprobamos que el codigo ingresado sea de 5 digitos y que el primer digito sea distinto de 0
            if (codigo != null)
            {
                if (codigo.Length == 5)
                {
                    var Acodigo = codigo.ToCharArray();
                    if (Acodigo[0] == '0')
                    {
                        codigo = codigo.Substring(1);
                    }
                }
                zonas = oColonias.consultaColonias(codigo);
            }
            return zonas;
        }

        // Llenado manual
        // GET: Guias/GenerarGuia

        public ActionResult GenerarGuia()
        {
            SeparaPermisos(Session["Permisos"].ToString());
            return View();
        }



        // POST: Guias/GenerarGuia
        [HttpPost]
        public ActionResult GenerarGuia(GuiaTaimingoViewModel guiaTaimingoViewModel)
        {

            //* Modelo invalido
            if (!ModelState.IsValid)
            {
                TempData["Alert"] = "Error en generar guia";
                TempData["AlertCssClass"] = "alert-danger";

                SeparaPermisos(Session["Permisos"].ToString());
                GenerarGuiaCommands oVerificaDestinatario = new GenerarGuiaCommands();

                // no redirige a nueva accion, solo renderiza de nuevo la vista
                return View("GenerarGuia");
            }

            try
            {
                oGuiaManual = guiaTaimingoViewModel;
                // Obtener razon/nombre de cliente           
                var cliente = _tablaClientesQueries.Muestra_ClientesMod((int)Session["UserClienteID"]);
                oEtiqueta.Remitente = cliente.RazonSocial;//indicamos el remitente
                guiaTaimingoViewModel.RazonSocialCliente = cliente.RazonSocial;
                SeparaPermisos(Session["Permisos"].ToString());
                //* Modelo valido
                return View("GenerarGuiaConf", guiaTaimingoViewModel);
            }
            catch (Exception ex) // Pendiente manejo de excepcion
            {
                SeparaPermisos(Session["Permisos"].ToString());
                return View("GenerarGuia");
            }

        }

        // POST: Guias/GenerarGuia
        public async Task<ActionResult> GenerarGuiaConf(GuiaTaimingoViewModel guiaTaimingoViewModel)
        {
            VerificaTexto verificaTexto = new VerificaTexto();
            //* Modelo invalido
            if (!ModelState.IsValid)
            {
                TempData["Alert"] = "Error en generar guia";
                TempData["AlertCssClass"] = "alert-danger";
                SeparaPermisos(Session["Permisos"].ToString());
                // no redirige a nueva accion, solo renderiza de nuevo la vista
                return View("GenerarGuia", guiaTaimingoViewModel);
            }

            //* Modelo valido
            try
            {
                // Sanitizacion de valores PENDIENTE

                // Guardar u Obtener Destinatario 

                var pdfFileName = "";
                var pdfFileNameS3 = "";
                byte[] pdfArchivo;
                var destinatario = new Destinatarios
                {
                    DestinatarioID = 0,
                    Nombre = verificaTexto.corrige_Nombres(guiaTaimingoViewModel.Nombre),
                    ApellidoP = verificaTexto.corrige_Nombres(guiaTaimingoViewModel.ApellidoPaterno),
                    ApellidoM = verificaTexto.corrige_Nombres(guiaTaimingoViewModel.ApellidoMaterno),
                    Celular = (long)Convert.ToDouble(guiaTaimingoViewModel.Celular),
                    Email = guiaTaimingoViewModel.Email,
                    Calle = verificaTexto.corrige_direccion(guiaTaimingoViewModel.Calle),
                    NumeroExt = verificaTexto.corrige_direccion(guiaTaimingoViewModel.NumeroExt),
                    NumeroInt = verificaTexto.corrige_direccion(guiaTaimingoViewModel.NumeroInt),
                    Colonia = verificaTexto.corrige_Nombres(guiaTaimingoViewModel.Colonia),
                    CodigoPostal = guiaTaimingoViewModel.CodigoPostal,
                    Latitud = 0,
                    Longitud = 0,
                };
                //revisamos si existe covertura 
                var oZona = muestra_zona(guiaTaimingoViewModel.CodigoPostal);
                var lzona = "";
                if (oZona.Count == 0)
                {
                    lzona = "K";
                }
                else
                {
                    lzona = oZona[0];
                }
                // SP para alta/modificacion de destinatario
                var destinatarioID = _generarGuiaCommands.VerificaDestinatario(destinatario);
                // Mapear ViewModel a Model
                var guiaTaimingo = new Guias
                {
                    GuiaID = 0,
                    FechaCreacion = DateTime.Now,
                    Medida = guiaTaimingoViewModel.Medida,
                    Peso = guiaTaimingoViewModel.Peso,
                    Descripcion = guiaTaimingoViewModel.Descripcion,
                    Instrucciones = guiaTaimingoViewModel.Instrucciones,
                    DestinatarioID = destinatarioID,
                    ClienteID = (int)Session["UserClienteID"],
                    UsuarioID = (int)Session["UserID"],
                    DeliveryID = null,
                    NoPedido = guiaTaimingoViewModel.NoPedido,
                    Tipo_Guia = Convert.ToBoolean(guiaTaimingoViewModel.TServicio),

                };
                // obtenemos las guias 
                var gui = obtenerGuia(1);
                guiaTaimingo.Guia = gui;
                List<byte[]> L_guias = new List<byte[]>();
                List<string> L_nombre_guia = new List<string>();
                // Modelo para llenado de pdf
                DatosPDFGuia datosPDFGuia = new DatosPDFGuia
                {
                    Calle = destinatario.Calle,
                    NumeroExt = destinatario.NumeroExt,
                    NumeroInt = destinatario.NumeroInt,
                    Colonia = destinatario.Colonia,
                    CodigoPostal = destinatario.CodigoPostal,
                    NombreDestinatario = $"{destinatario.Nombre} {destinatario.ApellidoP}",
                    NumeroGuia = gui,
                    ClienteRazonSocial = guiaTaimingoViewModel.RazonSocialCliente,
                    Peso = guiaTaimingo.Peso,
                    Medida = guiaTaimingo.Medida,
                    id_paqueteria = 1,
                    guiaExterna = oGuiaManual.NoPedido,
                    Zona = lzona,
                };
                if (guiaTaimingoViewModel.Numero_de_Paquetes_Extra == 1)
                {
                    pdfArchivo = _pDFUtilities.CrearPDFGuia(datosPDFGuia);
                    pdfFileName = $"pdf_{guiaTaimingo.Guia}.pdf";
                    pdfFileNameS3 = $"pdf_{guiaTaimingo.Guia}";

                    // Subir Documento/Archivo Guia a S3 y guardar URL devuelta
                    guiaTaimingo.Url = await _s3Service.UploadFileToS3(pdfArchivo, pdfFileNameS3, "application/pdf");

                    // SP para alta de guia
                    _generarGuiaCommands.AltaGuia(guiaTaimingo);
                }
                else
                {

                    pdfArchivo = _pDFUtilities.CrearPDFGuia(datosPDFGuia);
                    L_guias.Add(pdfArchivo);
                    pdfFileName = $"pdf_{guiaTaimingo.Guia}.pdf";
                    pdfFileNameS3 = $"pdf_{guiaTaimingo.Guia}";
                    guiaTaimingo.Guia = gui;
                    // Subir Documento/Archivo Guia a S3 y guardar URL devuelta
                    guiaTaimingo.Url = await _s3Service.UploadFileToS3(pdfArchivo, pdfFileNameS3, "application/pdf");
                    // SP para alta de guia
                    _generarGuiaCommands.AltaGuia(guiaTaimingo);


                    for (int i = 1; i < guiaTaimingoViewModel.Numero_de_Paquetes_Extra; i++)
                    {
                        datosPDFGuia.NumeroGuia = gui + "-" + i;
                        guiaTaimingo.Guia = gui + "-" + i;
                        pdfArchivo = _pDFUtilities.CrearPDFGuia(datosPDFGuia);
                        L_guias.Add(pdfArchivo);
                        pdfFileName = $"pdf_{guiaTaimingo.Guia}.pdf";
                        pdfFileNameS3 = $"pdf_{guiaTaimingo.Guia}";
                        // Subir Documento/Archivo Guia a S3 y guardar URL devuelta
                        guiaTaimingo.Url = await _s3Service.UploadFileToS3(pdfArchivo, pdfFileNameS3, "application/pdf");
                        // SP para alta de guia
                        _generarGuiaCommands.AltaGuia(guiaTaimingo);

                    }


                    pdfArchivo = Combine(L_guias);
                    pdfFileName = $"pdf_Guias.pdf";

                }

                // Guardado para descarga despues de redireccionamiento
                TempData["file"] = Convert.ToBase64String(pdfArchivo);
                TempData["fileName"] = pdfFileName;


                //cambiar correo de prueba, poner correo de usuarios con rol administrativo
                int cID = Convert.ToInt32(Session["UserClienteID"].ToString());
                TablaClientesCommands oC = new TablaClientesCommands();
                var C_Cliente = oC.Muestra_ClientesMod(cID);
                var rz = C_Cliente[0].RazonSocial;
                //******************** Envio de notificacion de mensajes
                EnviarMensaje msn = new EnviarMensaje();
                msn.EnvioMensaje("5614640937", "Notificacion. Guias Generadas. La empresa " + rz + " genero nuevas guias.");

                EnviarMail objMail = new EnviarMail();
                objMail.EnviaMail("vcastro@taimingo.com", "Notificacion. Guias Generadas. <No responder>", "Nuevas Guias Generadas", objMail.GeneraBody_NuevasGuias(rz));/*+*/
                objMail.EnviaMail("dsandoval@taimingo.com", "Notificacion. Guias Generadas. <No responder>", "Nuevas Guias Generadas", objMail.GeneraBody_NuevasGuias(rz));/*+*/
                SeparaPermisos(Session["Permisos"].ToString());
                return View("Imprime_Etiqueta", guiaTaimingoViewModel);
            }
            catch (Exception ex) // Pendiente manejo de excepcion
            {
                Debug.WriteLine(ex);
                SeparaPermisos(Session["Permisos"].ToString());
                return View("GenerarGuia");
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


