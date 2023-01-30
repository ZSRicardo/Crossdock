using System.Net.Mail;
using System.Net;
using System;

namespace Crossdock.Models
{
    public class EnviarMail
    {
        //notificaciones
        public string EnviaMail(string to,string NameTitle, string asunto, string body)
        {

            string from = "notificaciones@taimingo.com";
            string password = "a1xzTaL#nMF#e5p";
            string displayName = NameTitle;

            string msge;
            try
            {
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(from, displayName);
                mail.To.Add(to);

                mail.Subject = asunto;
                mail.Body = body;
                mail.IsBodyHtml = true;

                SmtpClient client = new SmtpClient("mail.taimingo.com", 26);
                client.Credentials=new NetworkCredential(from, password);
                client.EnableSsl = true;

                client.Send(mail);
                msge="Correo Enviado Satisfactoriamente.";
            }

            catch (Exception ex)
            {
                msge=ex.Message+"Verifica tu conexión o los datos de envío.";
            }

            return msge;     
        }

        public string GeneraBody(string Nombre, string Usuario, string Contraseña)
        {
            return

                @"<table style='max-width: 800px; padding: 10px; margin:0 auto; border-collapse: collapse;'>

                        <tr>
                            <td>
                                <p>
                                <a href='https://www.apptaimingo.com/Crossdockv_1'><img src='https://i.postimg.cc/HLr8HPnR/Logo-Taimingo.jpg' align='right'></a>
                                <h2 style='color: #42B649; text-align: center; margin: 0 0 7px'>   ¡Hola, " + Nombre + @"!</h2>
                                <h2 style='color: #42B649; text-align: center; margin: 0 0 7px'> Bienvenido </h2>
                                </p>
                                
                            </td>
                        </tr>

                        <tr>
                            <td>
                                <p style='margin: 2px; font-size: 15px'>
                                </br>Estos son tus datos de acceso para la plataforma Crossdock Taimingo.</br>

                                    <h3 style='color: #3EB9CC; margin: 0 0 7px'> </br>Usuario: " + Usuario + @"</br>Contraseña: " + Contraseña + @"</br></h3>
                                    
                                   </br>Este es el link de la plataforma: " +

                                        @"<a style='text-decoration: none; border-radius: 5px; padding: 11px 23px; color: white; background-color: #3498db' href='https://www.apptaimingo.com/Crossdockv_1'>Taimingo - Crossdock</a></br>"

                                        + @"</br>Después de haber accedido al sitio, ingresa tus datos de acceso. </br> </br>Para cualquier duda con los permisos en la plataforma y su funcionamiento
                                           favor de ponerse en contacto con el área DEV o con los administradores de la misma.</br></br></p>

                                        <div style='width: 100%; text-align: center'>
                                        <a style='text-decoration: none; border-radius: 5px; padding: 11px 23px; color: white; background-color: #3498db' href='https://www.apptaimingo.com/Crossdockv_1'>Ir a la página</a>

                                    </div>
                                    <p style='color: #b3b3b3; font-size: 12px; text-align: center;margin: 30px 0 0'>Taimingo 2022 </br> Mensaje Generado Automáticante. </br>No responder.</p>
                                    </div>

                            </td>
                        </tr>

                    </table>";

        }

        public string GeneraBody_NuevasGuias(string rz)// usado para notificar  la creacion de nuevas guias
        {
            return

                @"<table style='max-width: 800px; padding: 10px; margin:0 auto; border-collapse: collapse;'>

                        <tr>
                            <td>
                                <p style='margin: 2px; font-size: 15px'>
                                </br>La siguiente empresa genero guias.</br>
</br>      </br>

                                    <h3 style='color: #3EB9CC; margin: 0 0 7px'> </br>Empresa: " + rz + @"</br></h3>
                        
                                    </br> Favor de ir a el menu Notificaciones en la aplicacion de CrossDock.</br>
</br>      </br>
                                    
         
                                    <p style='color: #b3b3b3; font-size: 12px; text-align: center;margin: 30px 0 0'>Taimingo 2022 </br> Mensaje Generado Automáticante. </br>No responder.</p>
                                    </div>

                            </td>
                        </tr>

                    </table>";

        }

    }
}