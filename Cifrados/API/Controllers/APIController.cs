using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Cifrados;
using System.IO;
using API.Models;
using System.Text;

namespace API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class APIController : ControllerBase
    {

        [HttpPost("Cipher/{method}")]
        public ActionResult Cipher(string method,[FromForm] IFormFile file,[FromForm] Llaves key)
        {
            var pathActual = Environment.CurrentDirectory;
            Directory.CreateDirectory(pathActual + "\\temporal\\");
            ArchivoARuta(file);
            var nombre = file.FileName;
            var ruta = pathActual + "\\temporal\\" + nombre;
            var server = pathActual + "\\resultados";
            string clave = key.words;
            int levels = key.levels;
            int rows = key.rows;
            int columns = key.columns;
            var method1 = method.ToUpper();
            switch (method1)
            {
                case "CESAR":
                    Cesar cesar = new Cesar(clave, nombre, ruta, server);//revisar archivos grandes
                    cesar.Cifrar();
                    //cesar.Descifrar();
                    break;
                case "CÉSAR":
                    Cesar cesar1 = new Cesar(clave, nombre, ruta, server);
                    cesar1.Cifrar();
                    break;
                case "ZIGZAG":
                    ZigZag zigZag = new ZigZag(nombre,ruta,server,levels);//revisar error 
                    //zigZag.Cifrar();
                    zigZag.Descifrar();
                    break;
                case "RUTA":
                    Espiral espiral=new Espiral()//revisar parametros 
                       
                    break;
                default:
                    break;
            }
            return Ok();
        }

        public static void ArchivoARuta(IFormFile Archivo)
        {
            var resultado = new StringBuilder();
            using (var reader = new StreamReader(Archivo.OpenReadStream()))
            {
                while (reader.Peek() >= 0)
                    resultado.AppendLine(reader.ReadLine());
            }
            resultado.ToString();
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(Environment.CurrentDirectory + "\\Temporal\\" + Archivo.FileName, true))
            {
                file.Write(resultado);
            }
        }

    }
    
}
