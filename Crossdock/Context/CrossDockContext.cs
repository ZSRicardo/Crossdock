using System.IO;
using System.Net;

namespace Crossdock.Context
{
    /// <summary>
    /// Solo para las cadenas de conexion (Respetar el uso de tipo Writter(escritura) y Reader(lectura))
    /// </summary>
    public class CrossDockContext
    {
        public string Reader { set; get; }
        public string Writer { set; get; }
        public string Data_base { set; get; } = "database = crossdock_db; userid = admin; password=Jh4JDeFLPMwgrJB8jjaI;SSL Mode=None";

        // Obtener conexiones Writer y Reader de RDS
        public CrossDockContext GetRDSConections()
        {

            var url = $"https://www.apptaimingo.com/api_RDSConectionsAWS/api/RDS";
            var request = (HttpWebRequest)WebRequest.Create(url);
            string json = string.Empty;
            request.Method = "POST";
            request.Headers.Add("ApiKey", "abcd12345efgh6789");
            request.ContentType = "application/json";
            request.Accept = "application/json";
            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();
            }
            using (WebResponse response = request.GetResponse())
            {
                using (Stream strReader = response.GetResponseStream())
                {
                    using (StreamReader objReader = new StreamReader(strReader))
                    {
                        string responseBody = objReader.ReadToEnd();
                        // Do something with responseBody

                        return new CrossDockContext()
                        {
                            Writer = responseBody.Split(',', ':')[1].Replace("\"", "").Replace(":", "").Replace("{", "").Replace("Data", "").Replace("Reader", "")
                        ,
                            Reader = responseBody.Split(',', ':')[3].Replace("\"", "").Replace(":", "").Replace("{", "").Replace("Data", "").Replace("Writer", "").Replace("}", "")
                        };
                    }
                }
            }
        }


    }
}