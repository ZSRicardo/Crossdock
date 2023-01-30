using CrossDockLib.Models;
using iText.Barcodes;
using iText.IO.Font.Constants;
using iText.IO.Image;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using System;
using System.IO;
using System.Drawing;
//using MessagingToolkit.QRCode.Codec;
//using System.Drawing.Imaging;
using iText.Barcodes.Qrcode;
using System.Collections.Generic;

namespace CrossDockLib
{
    public class PDFUtilities
    {
        public byte[] CrearPDFGuia(DatosPDFGuia datosPDFGuia)
        {
            // Generar pdf en ubicacion o con stream
            var stream = new MemoryStream();
            var writer = new PdfWriter(stream);
            var pdf = new PdfDocument(writer);
            //definimos el tamaño de la etiqueta
            PageSize dd5 = new PageSize(300, 300);

            pdf.SetDefaultPageSize(dd5);
            Document document = new Document(pdf);
            document.SetMargins(0, 0, 0, 0);

            // Estilos
            Style bold = new Style();
            PdfFont boldFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);
            bold.SetFont(boldFont);

            // Contenido

            // Imagenes


            var appDomainPaqueteria = AppDomain.CurrentDomain;
            var basePathPaqueteria = appDomainPaqueteria.RelativeSearchPath ?? appDomainPaqueteria.BaseDirectory;
            //  var pathPaqueteria = System.IO.Path.Combine(basePathPaqueteria, "Common", "imagenes", "logotaimingo1.png");
            var pathPaqueteria = System.AppDomain.CurrentDomain.BaseDirectory + "Common\\Imagenes\\logotaimingo1.png";

            //Agregamos el Logotipo actual de taimingo 
            iText.Layout.Element.Image imgPaqueteria = new iText.Layout.Element.Image(ImageDataFactory.Create(pathPaqueteria)).ScaleToFit(170.0f, 170.0f).SetFixedPosition(66.0f, 250.0f);
            document.Add(imgPaqueteria);

            // Remitente        
            Paragraph from = new Paragraph("Remitente").SetFontSize(9).SetFont(boldFont);
            from.SetFixedPosition(24.0f, 230.0f, 150.0f);
            document.Add(from);
            // Remitente
            //limitamos la informacion de Remitente a 32 caracteres
            string C32Remitente = "";
            if (datosPDFGuia.ClienteRazonSocial != null)
            {
                if (datosPDFGuia.ClienteRazonSocial.Length > 24)
                {
                    C32Remitente = datosPDFGuia.ClienteRazonSocial.Substring(0, 24);
                }
                else
                {
                    C32Remitente = datosPDFGuia.ClienteRazonSocial;
                }
            }


            Paragraph nomRemitente = new Paragraph(C32Remitente).SetFontSize(9);
            nomRemitente.SetFixedPosition(24.0f, 215.0f, 150.0f);
            document.Add(nomRemitente);

            // Destinatario
            Paragraph destinatario = new Paragraph("Destinatario").SetFontSize(9).SetFont(boldFont);
            destinatario.SetFixedPosition(24.0f, 200.0f, 150.0f);
            document.Add(destinatario);

            // nombre destinatario Av Benito Juarez 34 El Mirador 53050 Naucalpan de Juarez
            //limitamos los datos que mostraremos a 32 caracteres.
            string C32NDes = "";

            if (datosPDFGuia.NombreDestinatario.Length > 25)
            {
                C32NDes = datosPDFGuia.NombreDestinatario.Substring(0, 25);
            }
            else
            {
                C32NDes = datosPDFGuia.NombreDestinatario;
            }
            Paragraph nomDestinatario = new Paragraph(C32NDes).SetFontSize(9);
            nomDestinatario.SetFixedPosition(24.0f, 185.0f, 150.0f);
            document.Add(nomDestinatario);

            //Direccion
            //limitamos la direccion que mostraremos a 64 caracteres
            string C64Direccion = datosPDFGuia.Calle + " " + datosPDFGuia.NumeroExt + " " + datosPDFGuia.NumeroInt + " " +
                datosPDFGuia.Colonia + " " + datosPDFGuia.CodigoPostal;


            Paragraph direccion = new Paragraph(C64Direccion).SetFontSize(9);
            if (C64Direccion.Length > 32)
            {
                direccion.SetFixedPosition(24.0f, 140.0f, 150.0f);
                direccion.SetWidth(155.0f);
                direccion.SetHeight(45.0f);
            }
            else
            {
                direccion.SetFixedPosition(24.0f, 155.0f, 150.0f);
                direccion.SetWidth(155.0f);
                direccion.SetHeight(30.0f);

            }


            direccion.SetTextAlignment(TextAlignment.LEFT);
            document.Add(direccion);

            // Codigo de barras
            if (datosPDFGuia.guiaExterna != null)
            {
                if (datosPDFGuia.guiaExterna.Length >= 10)
                {
                    Barcode128 bar = new Barcode128(pdf);
                    bar.SetCode(datosPDFGuia.guiaExterna.ToString());
                    var prueba = bar.CreateFormXObject(pdf);
                    var barcodeImg = new iText.Layout.Element.Image(prueba)
                      .SetHorizontalAlignment(HorizontalAlignment.CENTER).SetFixedPosition(180.0f, 210.0f, 30.0f);
                    barcodeImg.SetWidth(100f);
                    barcodeImg.SetHeight(50f);
                    barcodeImg.SetMaxHeight(30);
                    document.Add(barcodeImg);
                }
            }

            //Logo de taimingo
            // seleccionamos el logo que corresponde a la paqueteria que entregara el paquete 
            string nomPaqueteria = "";
            if (datosPDFGuia.id_paqueteria == 1)
            {
                nomPaqueteria = "AccessPack";
                //   var pathLOGO = System.IO.Path.Combine(basePathPaqueteria, "Common", "imagenes", nomPaqueteria + ".png");
                var pathLOGO = System.AppDomain.CurrentDomain.BaseDirectory + "Common\\Imagenes\\" + nomPaqueteria + ".png";
                iText.Layout.Element.Image imgLogo = new iText.Layout.Element.Image(ImageDataFactory.Create(pathLOGO)).ScaleToFit(70.0f, 70.0f).SetFixedPosition(195.0f, 170.0f);
                document.Add(imgLogo);
            }
            else if (datosPDFGuia.id_paqueteria == 2)
            {
                nomPaqueteria = "DHL";
                //var pathLOGO = System.IO.Path.Combine(basePathPaqueteria, "Common", "imagenes", nomPaqueteria + ".png");
                var pathLOGO = System.AppDomain.CurrentDomain.BaseDirectory + "Common\\Imagenes\\" + nomPaqueteria + ".png";
                iText.Layout.Element.Image imgLogo = new iText.Layout.Element.Image(ImageDataFactory.Create(pathLOGO)).ScaleToFit(70.0f, 70.0f).SetFixedPosition(195.0f, 170.0f);
                document.Add(imgLogo);
            }
            else if (datosPDFGuia.id_paqueteria == 3)
            {
                nomPaqueteria = "RedPack";
                //var pathLOGO = System.IO.Path.Combine(basePathPaqueteria, "Common", "imagenes", nomPaqueteria + ".png");
                var pathLOGO = System.AppDomain.CurrentDomain.BaseDirectory + "Common\\Imagenes\\" + nomPaqueteria + ".png";
                iText.Layout.Element.Image imgLogo = new iText.Layout.Element.Image(ImageDataFactory.Create(pathLOGO)).ScaleToFit(70.0f, 70.0f).SetFixedPosition(195.0f, 170.0f);
                document.Add(imgLogo);
            }




            //qr
            IDictionary<EncodeHintType, Object> hints = new Dictionary<EncodeHintType, object>();

            //default character set (ISO-8859-1)
            hints[EncodeHintType.CHARACTER_SET] = "UTF-8";

            //Qrcode Error correction level L,M,Q,H
            //default ErrorCorrectionLevel.L
            hints[EncodeHintType.ERROR_CORRECTION] = ErrorCorrectionLevel.H;

            iText.Barcodes.BarcodeQRCode qrcode = new BarcodeQRCode(datosPDFGuia.NumeroGuia, hints);

            var codeQrImage = new iText.Layout.Element.Image(qrcode.CreateFormXObject(pdf))
               .SetHorizontalAlignment(HorizontalAlignment.CENTER).SetFixedPosition(3.0f, 0.0f, 100.0f);

            codeQrImage.SetHeight(160.0f);
            codeQrImage.SetWidth(160.0f);

            document.Add(codeQrImage);
            //agregamos imagen al qr
            //  var pathQR = System.IO.Path.Combine(basePathPaqueteria, "Common", "imagenes", "logo3.jpg");
            var pathQR = System.AppDomain.CurrentDomain.BaseDirectory + "Common\\Imagenes\\logo3.jpg";
            iText.Layout.Element.Image imgQR = new iText.Layout.Element.Image(ImageDataFactory.Create(pathQR)).
                ScaleToFit(50.0f, 50.0f).SetFixedPosition(60.0f, 65.0f);
            document.Add(imgQR);

            //agregamos los ultimos 5 caracteres de la guia 
            //extraemos los cinco ultimos datos de la guia
            string cadena1 = datosPDFGuia.NumeroGuia;
            string substr = cadena1.Substring(cadena1.Length - 5);
            if(substr.Contains("-"))
            {
                string[] union=cadena1.Split('-');
                substr= union[0].Substring(union[0].Length-5)+" - "+union[1];
            }

            Text cincoCaracteres = new Text(substr).AddStyle(bold);
            Paragraph to = new Paragraph().SetFontSize(12);

            to.Add(cincoCaracteres).SetFixedPosition(170.0f, 28.0f, 100.0f);
            to.SetRotationAngle(1.5708f);
            document.Add(to);

            //fecha de ingreso a bodega
            DateTime f_formato = DateTime.Now;
            Paragraph fecha = new Paragraph(f_formato.ToString("dd/MM/yyyy")).SetFontSize(12);
            fecha.SetFixedPosition(200.0f, 20.0f, 100.0f);
            document.Add(fecha);

            // var cuadrado = System.IO.Path.Combine(basePathPaqueteria, "Common", "imagenes", "cuadrado.png");
            var cuadrado = System.AppDomain.CurrentDomain.BaseDirectory + "Common\\Imagenes\\cuadrado.png";
            iText.Layout.Element.Image imgCua = new iText.Layout.Element.Image(ImageDataFactory.Create(cuadrado)).ScaleToFit(100.0f, 100.0f).SetFixedPosition(180.0f, 38.0f);
            document.Add(imgCua);

            //zona
            Style oStyle = new Style()
                .SetFontColor(ColorConstants.WHITE);

            //dimensiones del cuadro negro
            //agregar metodo para seleccionar la zona por medio del Codigo postal 
            Text lZona = new Text(datosPDFGuia.Zona);

            Paragraph zona = new Paragraph(lZona).SetFontSize(100).SetBold();

            zona.SetFixedPosition(180.0f, 10.0f, 150.0f).AddStyle(oStyle);
            zona.SetWidth(100);
            zona.SetTextAlignment(TextAlignment.CENTER);
            document.Add(zona);
            // Cerrar        
            pdf.Close();
            writer.Close();
            stream.Close();

            return stream.ToArray();

        }

        //  Agregamos temporalmente la impresion de un codigo de barras para el uso de pistola

        //**  public byte[] CrearPDFGuia(DatosPDFGuia datosPDFGuia)
        //  {
        //      // Generar pdf en ubicacion o con stream
        //      var stream = new MemoryStream();
        //      var writer = new PdfWriter(stream);
        //      var pdf = new PdfDocument(writer);
        //      //definimos el tamaño de la etiqueta
        //      PageSize dd5 = new PageSize(300, 300);

        //      pdf.SetDefaultPageSize(dd5);
        //      Document document = new Document(pdf);
        //      document.SetMargins(0, 0, 0, 0);

        //      // Estilos
        //      Style bold = new Style();
        //      PdfFont boldFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);
        //      bold.SetFont(boldFont);

        //      // Contenido

        //      // Imagenes


        //      var appDomainPaqueteria = AppDomain.CurrentDomain;
        //      var basePathPaqueteria = appDomainPaqueteria.RelativeSearchPath ?? appDomainPaqueteria.BaseDirectory;
        //      //  var pathPaqueteria = System.IO.Path.Combine(basePathPaqueteria, "Common", "imagenes", "logotaimingo1.png");
        //      var pathPaqueteria = System.AppDomain.CurrentDomain.BaseDirectory + "Common\\Imagenes\\logotaimingo1.png";

        //      //Agregamos el Logotipo actual de taimingo 
        //      iText.Layout.Element.Image imgPaqueteria = new iText.Layout.Element.Image(ImageDataFactory.Create(pathPaqueteria)).ScaleToFit(170.0f, 170.0f).SetFixedPosition(66.0f, 250.0f);
        //      document.Add(imgPaqueteria);

        //      // Remitente        
        //    //  Paragraph from = new Paragraph("Remitente").SetFontSize(9);
        //     // from.SetFixedPosition(24.0f, 245.0f, 150.0f);
        //    //  document.Add(from);
        //      // Remitente
        //      //limitamos la informacion de Remitente a 32 caracteres
        //      string C32Remitente = "";
        //      if (datosPDFGuia.ClienteRazonSocial != null)
        //      {
        //          if (datosPDFGuia.ClienteRazonSocial.Length > 32)
        //          {
        //              C32Remitente = datosPDFGuia.ClienteRazonSocial.Substring(0, 32);
        //          }
        //          else
        //          {
        //              C32Remitente = datosPDFGuia.ClienteRazonSocial;
        //          }
        //      }


        //      Paragraph nomRemitente = new Paragraph(C32Remitente).SetFontSize(9);
        //      nomRemitente.SetFixedPosition(24.0f, 230.0f, 150.0f);
        //      document.Add(nomRemitente);

        //      // Destinatario
        //      Paragraph destinatario = new Paragraph("Destinatario").SetFontSize(9);
        //      destinatario.SetFixedPosition(24.0f, 215.0f, 150.0f);
        //      document.Add(destinatario);

        //      // nombre destinatario Av Benito Juarez 34 El Mirador 53050 Naucalpan de Juarez
        //      //limitamos los datos que mostraremos a 32 caracteres.
        //      string C32NDes = "";

        //      if (datosPDFGuia.NombreDestinatario.Length > 32)
        //      {
        //          C32NDes = datosPDFGuia.NombreDestinatario.Substring(0, 32);
        //      }
        //      else
        //      {
        //          C32NDes = datosPDFGuia.NombreDestinatario;
        //      }
        //      Paragraph nomDestinatario = new Paragraph(C32NDes).SetFontSize(9);
        //      nomDestinatario.SetFixedPosition(24.0f, 200.0f, 150.0f);
        //      document.Add(nomDestinatario);

        //      //Direccion
        //      //limitamos la direccion que mostraremos a 64 caracteres
        //      string C64Direccion = datosPDFGuia.Calle + " " + datosPDFGuia.NumeroExt + " " + datosPDFGuia.NumeroInt + " " +
        //          datosPDFGuia.Colonia + " " + datosPDFGuia.CodigoPostal;
        //      string cadena = "";
        //      if (C64Direccion.Length > 64)
        //      {
        //          cadena = C64Direccion.Substring(0, 64);
        //      }
        //      else
        //      {
        //          cadena = C64Direccion;
        //      }

        //      Paragraph direccion = new Paragraph(C64Direccion).SetFontSize(9);
        //      direccion.SetFixedPosition(24.0f, 170.0f, 150.0f);
        //      direccion.SetWidth(155.0f);
        //      direccion.SetHeight(30.0f);
        //      direccion.SetTextAlignment(TextAlignment.LEFT);
        //      document.Add(direccion);

        //      // Codigo de barras
        //      if(datosPDFGuia.guiaExterna!=null)
        //      {
        //          if (datosPDFGuia.guiaExterna.Length >= 10)
        //          {
        //              Barcode128 bar = new Barcode128(pdf);
        //              bar.SetCode(datosPDFGuia.guiaExterna.ToString());
        //              var prueba = bar.CreateFormXObject(pdf);
        //              var barcodeImg = new iText.Layout.Element.Image(prueba)
        //                .SetHorizontalAlignment(HorizontalAlignment.CENTER).SetFixedPosition(180.0f, 210.0f, 30.0f);
        //              barcodeImg.SetWidth(100f);
        //              barcodeImg.SetHeight(50f);
        //              barcodeImg.SetMaxHeight(30);

        //              document.Add(barcodeImg);
        //          }
        //      }

        //      //Logo de taimingo
        //      // seleccionamos el logo que corresponde a la paqueteria que entregara el paquete 
        //      string nomPaqueteria = "";
        //      if (datosPDFGuia.id_paqueteria == 1)
        //      {
        //          nomPaqueteria = "AccessPack";
        //          //   var pathLOGO = System.IO.Path.Combine(basePathPaqueteria, "Common", "imagenes", nomPaqueteria + ".png");
        //          var pathLOGO = System.AppDomain.CurrentDomain.BaseDirectory + "Common\\Imagenes\\" + nomPaqueteria + ".png";
        //          iText.Layout.Element.Image imgLogo = new iText.Layout.Element.Image(ImageDataFactory.Create(pathLOGO)).ScaleToFit(70.0f, 70.0f).SetFixedPosition(195.0f, 180.0f);
        //          document.Add(imgLogo);
        //      }
        //      else if (datosPDFGuia.id_paqueteria == 2)
        //      {
        //          nomPaqueteria = "DHL";
        //          //var pathLOGO = System.IO.Path.Combine(basePathPaqueteria, "Common", "imagenes", nomPaqueteria + ".png");
        //          var pathLOGO = System.AppDomain.CurrentDomain.BaseDirectory + "Common\\Imagenes\\" + nomPaqueteria + ".png";
        //          iText.Layout.Element.Image imgLogo = new iText.Layout.Element.Image(ImageDataFactory.Create(pathLOGO)).ScaleToFit(70.0f, 70.0f).SetFixedPosition(195.0f, 180.0f);
        //          document.Add(imgLogo);
        //      }
        //      else if (datosPDFGuia.id_paqueteria == 3)
        //      {
        //          nomPaqueteria = "RedPack";
        //          //var pathLOGO = System.IO.Path.Combine(basePathPaqueteria, "Common", "imagenes", nomPaqueteria + ".png");
        //          var pathLOGO = System.AppDomain.CurrentDomain.BaseDirectory + "Common\\Imagenes\\" + nomPaqueteria + ".png";
        //          iText.Layout.Element.Image imgLogo = new iText.Layout.Element.Image(ImageDataFactory.Create(pathLOGO)).ScaleToFit(70.0f, 70.0f).SetFixedPosition(195.0f, 180.0f);
        //          document.Add(imgLogo);
        //      }




        //      //qr
        //      IDictionary<EncodeHintType, Object> hints = new Dictionary<EncodeHintType, object>();

        //      //default character set (ISO-8859-1)
        //      hints[EncodeHintType.CHARACTER_SET] = "UTF-8";

        //      //Qrcode Error correction level L,M,Q,H
        //      //default ErrorCorrectionLevel.L
        //      hints[EncodeHintType.ERROR_CORRECTION] = ErrorCorrectionLevel.H;

        //      iText.Barcodes.BarcodeQRCode qrcode = new BarcodeQRCode(datosPDFGuia.NumeroGuia, hints);

        //      var codeQrImage = new iText.Layout.Element.Image(qrcode.CreateFormXObject(pdf))
        //         .SetHorizontalAlignment(HorizontalAlignment.CENTER).SetFixedPosition(2.0f, 30.0f, 100.0f);

        //      codeQrImage.SetHeight(160.0f);
        //      codeQrImage.SetWidth(160.0f);

        //      document.Add(codeQrImage);
        //      //agregamos imagen al qr
        //      //  var pathQR = System.IO.Path.Combine(basePathPaqueteria, "Common", "imagenes", "logo3.jpg");
        //      var pathQR = System.AppDomain.CurrentDomain.BaseDirectory + "Common\\Imagenes\\logo3.jpg";
        //      iText.Layout.Element.Image imgQR = new iText.Layout.Element.Image(ImageDataFactory.Create(pathQR)).
        //          ScaleToFit(50.0f, 50.0f).SetFixedPosition(60.0f, 100.0f);
        //      document.Add(imgQR);

        //      //agregamos los ultimos 5 caracteres de la guia 
        //      //extraemos los cinco ultimos datos de la guia
        //      string cadena1 = datosPDFGuia.NumeroGuia;
        //      string substr = cadena1.Substring(cadena1.Length - 5);
        //      Text cincoCaracteres = new Text(substr).AddStyle(bold);
        //      Paragraph to = new Paragraph().SetFontSize(12);

        //      to.Add(cincoCaracteres).SetFixedPosition(175.0f, 58.0f, 100.0f);
        //      to.SetRotationAngle(1.5708f);
        //      document.Add(to);



        //      //se agrega la barra 

        //      Barcode128 p = new Barcode128(pdf);
        //      p.SetCode(datosPDFGuia.NumeroGuia.ToString());
        //      var cf = p.CreateFormXObject(pdf);
        //      var bc = new iText.Layout.Element.Image(cf)
        //        .SetHorizontalAlignment(HorizontalAlignment.CENTER).SetFixedPosition(70.0f, 10.0f, 50.0f);
        //      bc.SetWidth(160f);
        //      document.Add(bc);



        //      //fecha de ingreso a bodega
        //      DateTime f_formato = DateTime.Now;
        //      Paragraph fecha = new Paragraph(f_formato.ToString("dd/MM/yyyy")).SetFontSize(12);
        //      fecha.SetFixedPosition(200.0f, 50.0f, 100.0f);
        //      document.Add(fecha);

        //      // var cuadrado = System.IO.Path.Combine(basePathPaqueteria, "Common", "imagenes", "cuadrado.png");
        //      var cuadrado = System.AppDomain.CurrentDomain.BaseDirectory + "Common\\Imagenes\\cuadrado.png";
        //      iText.Layout.Element.Image imgCua = new iText.Layout.Element.Image(ImageDataFactory.Create(cuadrado)).ScaleToFit(100.0f, 100.0f).SetFixedPosition(180.0f, 78.0f);
        //      document.Add(imgCua);

        //      //zona
        //      Style oStyle = new Style()
        //          .SetFontColor(ColorConstants.WHITE);

        //      //dimensiones del cuadro negro
        //      //agregar metodo para seleccionar la zona por medio del Codigo postal 
        //      Text lZona = new Text(datosPDFGuia.Zona);

        //      Paragraph zona = new Paragraph(lZona).SetFontSize(100).SetBold();

        //      zona.SetFixedPosition(180.0f, 50.0f, 50.0f).AddStyle(oStyle);
        //      zona.SetWidth(100);
        //      zona.SetTextAlignment(TextAlignment.CENTER);
        //      document.Add(zona);
        //      // Cerrar        
        //      pdf.Close();
        //      writer.Close();
        //      stream.Close();

        //      return stream.ToArray();

        //  }


    }
}
