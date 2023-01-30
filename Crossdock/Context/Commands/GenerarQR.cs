using Crossdock.Models;
using CrossDockLib;
using MessagingToolkit.QRCode.Codec;
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
    public class GenerarQR : Controller
    {
        public byte[] CreateQR(string email, string password, string nombre)
        {
            char delimitador = '@';
            string[] valores = email.Split(delimitador);

            var valor = valores[0];

            //Generar Código QR
            QRCodeEncoder QR = new QRCodeEncoder();
            QR.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.H;
            QR.QRCodeScale = 10;

            MemoryStream ms = new MemoryStream();

            var path = System.AppDomain.CurrentDomain.BaseDirectory + "Common\\Imagenes\\logo3.jpg";
            Image imgQR = QR.Encode(email.Replace(email, valor) + "/" + password);
            System.Drawing.Image logo = System.Drawing.Image.FromFile(path);
            int left = 90;
            int top = 110;

            Graphics g = Graphics.FromImage(imgQR);
            g.DrawImage(logo, new Point(left, top));

            imgQR.Save(ms, ImageFormat.Jpeg);
            byte[] aByte = ms.ToArray();

            //Crear carpeta local
            string filePath = @"C:\imgCodigos";
            if (!Directory.Exists(filePath))//&& Directory.Exists(imgCodigos))
            {
                Directory.CreateDirectory(filePath);
            }

            imgQR.Save(filePath + @"\" + nombre.Replace("code", "") + ".jpg");

            return aByte;

        }
    }
}