using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Net.Http;
using Crossdock.Models;
using System.Text.Json;

namespace Crossdock.Context.Commands
{
    public class ImprimeEtiquetasComands
    {
        public async Task<bool> Imprime_Envio(EtiquetaIngresoBodega oGuia)
        {
            string url = "http://localhost/aPI_GoDex/api/Ingreso";
        
            var client = new HttpClient();
            //pasamos los parametros que vamos a imprimir 
            EtiquetaIngresoBodega oPost= new EtiquetaIngresoBodega
            {
              
                    Numero_Pedido=oGuia.Numero_Pedido,
                    Zona=oGuia.Zona,
                    Remitente=oGuia.Remitente,
                    Destinatario=oGuia.Destinatario,
                    Guia=oGuia.Guia,
                    Fecha_Recepcion=DateTime.Now,
                    Direccion=oGuia.Direccion,
                    Paqueteria=oGuia.Paqueteria,

            };
            //serializamos el objeto "oPost" en json para poder enviar la informacion 

            var data = JsonSerializer.Serialize <EtiquetaIngresoBodega>(oPost);
            //enviamos el json que acabamos de crear
            HttpContent content=new StringContent(data,System.Text.Encoding.UTF8,"application/json");
            //la siguiente variable cacha la respuesta que nos envia la peticion 

            var httpResponse= await client.PostAsync(url, content);

            if( httpResponse.IsSuccessStatusCode)
            {
                var result = await httpResponse.Content.ReadAsStringAsync();
                return true;
            }
            else
            {
                return false;
            }
                
        }
    }
}