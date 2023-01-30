using BarcodeLib;
using Crossdock.Models;
using CrossDockLib;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Crossdock.Context.Commands
{
    public class GenerarBarCode : Controller
    {
        public byte[] CreateBarCode(string email, string password, string nombre)
        {
            char delimitador = '@';
            string[] valores = email.Split(delimitador);

            var valor = valores[0];

            ////Generación codebar
            Barcode Codigo = new Barcode();
            Codigo.IncludeLabel = true;

            MemoryStream ms = new MemoryStream();

            Image imgFinal = Codigo.Encode(TYPE.CODE128, email.Replace(email, valor) + "/" + password, Color.Black, Color.White, 640, 100);
            imgFinal.Save(ms, ImageFormat.Gif);
            byte[] aByte = ms.ToArray();

            string filePath = @"C:\imgCodigos";
            if (!Directory.Exists(filePath))//&& Directory.Exists(imgCodigos))
            {
                Directory.CreateDirectory(filePath);
            }

            imgFinal.Save(filePath + @"\" + nombre.Replace("code", "") + ".gif");

            return aByte;
        }


    }
}