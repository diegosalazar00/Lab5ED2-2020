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
            var result = new StringBuilder();
            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                while (reader.Peek() >= 0)
                    result.AppendLine(reader.ReadLine());
            }

            var method1 = method.ToUpper();
            switch (method1)
            {
                case "CESAR":
                    Cesar cesar = new Cesar(clave, nombre, ruta, server);//revisar archivos grandes
                    cesar.Cifrar();
         
                    break;
                case "CÉSAR":
                    Cesar cesar1 = new Cesar(clave, nombre, ruta, server);
                    cesar1.Cifrar();                    
                    break;
                case "ZIGZAG":
                    
                    ZigZag zigzagCipher = new ZigZag();
                    zigzagCipher.calculate(levels, result, nombre);
                   
                    break;
                case "RUTA":
                    Espiral espiral = new Espiral(rows, columns, nombre, ruta, server);
                    espiral.Cifrar();                     
                     
                    break;
                default:
                    break;
            }
            return Ok();
        }
        [HttpPost("decipher")]
        public ActionResult decipher([FromForm] IFormFile file, [FromForm] Llaves key)
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
            string extencion = Path.GetExtension(file.FileName);
            var result = new StringBuilder();
            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                while (reader.Peek() >= 0)
                    result.AppendLine(reader.ReadLine());
            }
            extencion = extencion.ToLower();
            switch (extencion)
            {
                case ".csr":
                    Cesar cesar = new Cesar(clave, nombre, ruta, server);//revisar archivos grandes
                    cesar.Descifrar();
                    break;
                case ".zz":
                    ZigZag zigzagCipher = new ZigZag();
                    zigzagCipher.decipher(levels,result , nombre);
                    break;
                case ".rt":
                    Espiral espiral = new Espiral(rows, columns, nombre, ruta, server);                    
                    espiral.Descifrar();
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
