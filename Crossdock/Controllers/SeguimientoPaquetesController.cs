using Crossdock.Context.Commands;
using Crossdock.Models;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Web.Mvc;

namespace Crossdock.Controllers
{
    public class SeguimientoPaquetesController : Controller
    {
        private BusquedaGuia _BusquedaGuia = new BusquedaGuia();//llamada de clase
        static string Url = "";

        public ActionResult Index()
        {
            ViewBag.MuestraInformacion = "none";
            return View();
        }

        [HttpPost]
        public ActionResult Index(SeguimientoPaqs seguimientoPaquetes)
        {

            if (seguimientoPaquetes.Guia == null)
            {
                TempData["Alert"] = "Ingresa un número de guia";
                TempData["AlertCssClass"] = "alert-danger";
                ViewBag.MuestraInformacion = "none";
                return View("Index");
            }
            var datosGuia = _BusquedaGuia.Busqueda_Guia(seguimientoPaquetes.Guia);


            if (datosGuia.Count == 0)
            {
                TempData["Alert"] = "Numero de Guia no es valido. Verifiquelo de nuevo.";
                TempData["AlertCssClass"] = "alert-danger";
                ViewBag.MuestraInformacion = "none";
                return View("Index");
            }
            ViewBag.Busqueda = "none";
            ViewBag.MuestraInformacion = "true";
            Url = datosGuia[datosGuia.Count - 1].EvidenciaEntrega;
            datosGuia[datosGuia.Count - 1].Guia = seguimientoPaquetes.Guia;
            return View(datosGuia[datosGuia.Count - 1]);
        }

        public FileContentResult getImage()
        {
            var webClient = new WebClient();
            byte[] imageBytes = null;
            if (Url != "" && Url != null)
            {
                imageBytes = webClient.DownloadData(Url);
            }
            else
            {
                imageBytes = webClient.DownloadData("https://anestesiar.org/WP/uploads/2013/12/no-sin-evidencia.png");
            }
            return new FileContentResult(imageBytes, "image/png");
        }


       
    }
}
