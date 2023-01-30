using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Crossdock.Models
{
    public class GeneraContraseña
    {
        //genera el password
        string _password;
        Random rdn = new Random();
        readonly string CARACTERES_MINUSCULAS = "abcdefghijklmnopqursuvwxyz";
        readonly string CARACTERES_MAYUSCULAS = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        public string Password { get => _password; set => _password=value; }

        public GeneraContraseña()
        {
            Random r = new Random();

            int k = CARACTERES_MINUSCULAS.Length;            
            string Minusculas = string.Empty;
            for (int i = 0; i<2; i++)
            {
                Minusculas += CARACTERES_MINUSCULAS[rdn.Next(k)].ToString();
            }

            k = CARACTERES_MAYUSCULAS.Length;
            string Mayusculas = string.Empty;
            for (int i = 0; i<1; i++)
            {
                Mayusculas += CARACTERES_MAYUSCULAS[rdn.Next(k)].ToString();
            }

            string Numeros = string.Empty;
            for (int i = 0; i < 2; i++)
            {
                Numeros += r.Next(0, 9);
            }

            string aux=Minusculas+Mayusculas+Numeros;
            var lista = from x in aux select x.ToString();
            List<string> lString = new List<string>(lista);

            IList<string> list = lString;
            int n = aux.Length;
            while (n > 1)
            {
                n--;
                int m = r.Next(n + 1);
                (list[n], list[m])=(list[m], list[n]);
            }

            foreach (string x in list)
            {
                Password+=x;
            }

        }

    }
}