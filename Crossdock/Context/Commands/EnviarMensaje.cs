using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace Crossdock.Context.Commands
{
    public class EnviarMensaje
    {
        public void EnvioMensaje(string telefono, string mensaje)//Envia SMS
        {
            string usuario = "i_taimingo";
            string password = "t&a4iM62o";
            string llave = "637776006429782";

            var url = $"https://www.message-center.com.mx/webresources/Engine/SendMsg";
            var request = (HttpWebRequest)WebRequest.Create(url);

            string json = $"{{\"U\":\"{usuario}\",\"P\":\"{password}\",\"K\":\"{llave}\",\"T\":\"{telefono}\",\"M\":\"{mensaje}\"}}";
            request.Method = "POST";
            request.ContentType = "application/json";
            request.Accept = "application/json";
            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();
            }
            try
            {
                using (WebResponse response = request.GetResponse())
                {
                    using (Stream strReader = response.GetResponseStream())
                    {
                        using (StreamReader objReader = new StreamReader(strReader))
                        {
                            string responseBody = objReader.ReadToEnd();
                            // Do something with responseBody
                            Console.WriteLine(responseBody);

                        }

                    }
                }
            }
            catch (WebException ex)
            {
                // Handle error
                var error = ex;
            }
        }

        
    }
}