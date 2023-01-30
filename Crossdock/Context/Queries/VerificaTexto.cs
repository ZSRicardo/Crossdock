using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Crossdock.Context.Queries
{
    public class VerificaTexto
    {
        public string corrige_Nombres(string texto)
        {
            //Elimina acentos, quita espacios antes y despues del texto ingresado, espacios dobles o triples juntos
            //quita numeros y convierte a mayusculas 
            //preferentemente usar para nombres y apellidos 
            if(texto != null)
            {
                texto = texto.Trim();
                texto = Regex.Replace(texto, "\\s{2,}", " ");
                texto = Regex.Replace(texto.Normalize(NormalizationForm.FormD), @"[^a-zA-z ]+", "");
                texto = texto.ToUpper();
            }
           
            return texto;
        }

        public string corrige_direccion(string texto)
        {
            //Elimina acentos, quita espacios antes y despues del texto ingresado, espacios dobles o triples juntos
            //quita numeros y convierte a mayusculas 
            //preferentemente usar para nombres y apellidos 
            if(texto!=null)
            {
                texto = texto.Trim();
                texto = Regex.Replace(texto, "\\s{2,}", " ");
                texto = Regex.Replace(texto.Normalize(NormalizationForm.FormD), @"[^a-zA-z0-9]+ ", "");
                texto = texto.ToUpper();
            }
          
            return texto;
        }
        
        public string corrige_celular(string texto)
        {
            if(texto!=null)
            {
                texto = texto.Trim();
                texto = Regex.Replace(texto, "\\s{1,}", "");
                texto = Regex.Replace(texto.Normalize(NormalizationForm.FormD), @"[^0-9]+", "");
                texto = texto.ToUpper();
            }
          
            return texto;
        }

    }
}