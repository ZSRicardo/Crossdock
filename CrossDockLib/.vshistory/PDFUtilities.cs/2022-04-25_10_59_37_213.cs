using CrossDockLib.Models;
using iText.Barcodes;
using iText.IO.Font.Constants;
using iText.IO.Image;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            // Tamano
            PageSize pageSize = new PageSize(PageSize.A4.GetWidth(), PageSize.A4.GetHeight() / 2);
            pdf.SetDefaultPageSize(pageSize);

            Document document = new Document(pdf);
            document.SetMargins(0, 0, 0, 0);

            // Estilos
            Style bold = new Style();
            PdfFont boldFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
            bold.SetFont(boldFont);

            // Contenido

            // Imagenes

            // Logo Paqueteria a la izquiera
            var appDomainPaqueteria = AppDomain.CurrentDomain;
            var basePathPaqueteria = appDomainPaqueteria.RelativeSearchPath ?? appDomainPaqueteria.BaseDirectory;
            var pathPaqueteria = System.IO.Path.Combine(basePathPaqueteria, "Common", "imagenes", "LogoAccess.png");

            Image imgPaqueteria = new Image(ImageDataFactory
               .Create(pathPaqueteria))
                .ScaleToFit(140, 140);
            // .SetMarginLeft(35)
            // .SetMarginTop(15);

            // Logo Taimingo a la derecha
            var appDomainTai = AppDomain.CurrentDomain;
            var basePathTai = appDomainTai.RelativeSearchPath ?? appDomainTai.BaseDirectory;
            var pathTai = System.IO.Path.Combine(basePathTai, "Common", "imagenes", "LogoTaimingo.jpg");

            Image imgTai = new Image(ImageDataFactory
               .Create(pathTai))
                .ScaleToFit(120, 120);
            // .SetMarginRight(35)
            //.SetMarginTop(15);

            // Contenedor para centrar imagen SI imagen de la derecha es mas alta
            float imageHeight = imgTai.GetImageScaledHeight();

            var div = new Div()
                .Add(imgPaqueteria)
                    .SetHeight(imageHeight)
                    .SetVerticalAlignment(VerticalAlignment.MIDDLE);

            // Parrafo que contendra imagenes/logos
            Paragraph pImg = new Paragraph().SetMargin(0);

            pImg.Add(div);
            pImg.Add(new Tab());
            pImg.AddTabStops(new TabStop(1000, TabAlignment.RIGHT));

            pImg.Add(imgTai);

            pImg.SetMarginLeft(35).SetMarginTop(15).SetMarginBottom(15).SetMarginRight(35);

            document.Add(pImg);

            // Remitente
            Text remitente = new Text("Remitente: ").AddStyle(bold);
            Text remitenteData = new Text(datosPDFGuia.ClienteRazonSocial);

            Paragraph from = new Paragraph()
                .SetTextAlignment(TextAlignment.CENTER)
                .SetMargin(0);

            from.Add(remitente);
            from.Add(remitenteData);
            document.Add(from);

            // Destinatario
            Text destinatario = new Text("Destinatario: ").AddStyle(bold);
            Text destinatarioData = new Text(datosPDFGuia.NombreDestinatario);

            Paragraph to = new Paragraph()
                .SetTextAlignment(TextAlignment.CENTER)
                .SetMargin(0);

            to.Add(destinatario);
            to.Add(destinatarioData);
            document.Add(to);

            // Medida
            Text medida = new Text("Medida: ").AddStyle(bold);
            Text medidaData = new Text(datosPDFGuia.Medida);

            Paragraph paqueteMedida = new Paragraph()
                .SetTextAlignment(TextAlignment.CENTER)
                .SetMargin(0);

            paqueteMedida.Add(medida);
            paqueteMedida.Add(medidaData);
            document.Add(paqueteMedida);

            // Peso
            Text peso = new Text("Peso: ").AddStyle(bold);
            Text pesoData = new Text($"{datosPDFGuia.Peso} Kg");

            Paragraph paquetePeso = new Paragraph()
                .SetTextAlignment(TextAlignment.CENTER)
                .SetMargin(0);

            paquetePeso.Add(peso);
            paquetePeso.Add(pesoData);
            document.Add(paquetePeso);

            // Codigo de barras
            var bar = new BarcodeInter25(pdf);
            bar.SetCode(datosPDFGuia.NumeroGuia);

            //Here's how to add barcode to PDF with IText7
            var barcodeImg = new Image(bar.CreateFormXObject(pdf))
                .SetMarginTop(20)
                .SetWidth(300)
                .SetHorizontalAlignment(HorizontalAlignment.CENTER);

            document.Add(barcodeImg);

            // Cerrar        
            pdf.Close();
            writer.Close();
            stream.Close();

            return stream.ToArray();

        }

    }
}
